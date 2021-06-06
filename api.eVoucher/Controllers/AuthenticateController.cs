using api.eVoucher.Authentication;
using api.eVoucher.Dtos;
using api.eVoucher.Entity;
using api.eVoucher.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace api.eVoucher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration _configuration;
        private readonly IDataContext _context;
        private readonly TokenValidationParameters _tokenValidationParameters;
        public AuthenticateController(UserManager<ApplicationUser> userManager, IConfiguration configuration,
            TokenValidationParameters tokenValidationParameters, IDataContext context)
        {
            this.userManager = userManager;
            _configuration = configuration;
            _tokenValidationParameters = tokenValidationParameters;
            _context = context;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                //var userRoles = await userManager.GetRolesAsync(user);

                //var authClaims = new List<Claim>
                //{
                //    new Claim(ClaimTypes.Name, user.UserName),
                //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                //};

                //foreach (var userRole in userRoles)
                //{
                //    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                //}

                //var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                //var token = new JwtSecurityToken(
                //    issuer: _configuration["JWT:ValidIssuer"],
                //    audience: _configuration["JWT:ValidAudience"],
                //    expires: DateTime.Now.AddHours(24),
                //    claims: authClaims,
                //    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                //    );

                var jwtTokenHandler = new JwtSecurityTokenHandler();

                var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = jwtTokenHandler.CreateToken(tokenDescriptor);

                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

                var refreshToken = new RefreshToken()
                {
                    JwtId = token.Id,
                    IsUsed = false,
                    UserId = user.Id,
                    AddedDate = DateTime.UtcNow,
                    ExpiryDate = DateTime.UtcNow.AddYears(1),
                    IsRevoked = false,
                    Token = RandomString(25) + Guid.NewGuid()
                };

                await _context.RefreshTokens.AddAsync(refreshToken);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    status = "0001",
                    message = "Token generated successfully.",
                    token = jwtToken,
                    expiration = token.ValidTo,
                    refresh_token = refreshToken.Token
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("refreshtoken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            if (ModelState.IsValid)
            {
                var res = await VerifyToken(tokenRequest);

                if (res == null)
                {
                    return BadRequest(new
                    {
                        Errors = new List<string>() {
                            "Invalid tokens"
                        },
                        Success = false
                    });
                }

                return Ok(res);
            }
            return BadRequest(new
            {
                Errors = new List<string>() {
                    "Invalid payload"
                },
                Success = false
            });
        }

        private async Task<IActionResult> VerifyToken(TokenRequest tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // This validation function will make sure that the token meets the validation parameters
                // and its an actual jwt token not just a random string
                var principal = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParameters, out var validatedToken);

                // Now we need to check if the token has a valid security algorithm
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (result == false)
                    {
                        return null;
                    }
                }

                // Will get the time stamp in unix time
                var utcExpiryDate = long.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                // we convert the expiry date from seconds to the date
                var expDate = UnixTimeStampToDateTime(utcExpiryDate);

                if (expDate > DateTime.UtcNow)
                {
                    return BadRequest(new
                    {
                        Errors = new List<string>() { "We cannot refresh this since the token has not expired" },
                        Success = false
                    });
                }

                // Check the token we got if its saved in the db
                var storedRefreshToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == tokenRequest.RefreshToken);

                if (storedRefreshToken == null)
                {
                    return BadRequest(new
                    {
                        Errors = new List<string>() { "refresh token doesnt exist" },
                        Success = false
                    });
                }

                // Check the date of the saved token if it has expired
                if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                {
                    return BadRequest(new
                    {
                        Errors = new List<string>() { "token has expired, user needs to relogin" },
                        Success = false
                    });
                }

                // check if the refresh token has been used
                if (storedRefreshToken.IsUsed)
                {
                    return BadRequest(new
                    {
                        Errors = new List<string>() { "token has been used" },
                        Success = false
                    });
                }

                // Check if the token is revoked
                if (storedRefreshToken.IsRevoked)
                {
                    return BadRequest(new
                    {
                        Errors = new List<string>() { "token has been revoked" },
                        Success = false
                    });
                }

                // we are getting here the jwt token id
                var jti = principal.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                // check the id that the recieved token has against the id saved in the db
                if (storedRefreshToken.JwtId != jti)
                {
                    return BadRequest(new
                    {
                        Errors = new List<string>() { "the token doenst mateched the saved token" },
                        Success = false
                    });
                }

                storedRefreshToken.IsUsed = true;
                _context.RefreshTokens.Update(storedRefreshToken);
                await _context.SaveChangesAsync();

                var dbUser = await userManager.FindByIdAsync(storedRefreshToken.UserId);

                return Ok(new
                {
                    status = "0001",
                    message = "Token generated successfully.",
                    token = GenerateJwtToken(dbUser)
                });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }

        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dtDateTime;
        }

        public string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "Error", message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "Error", message = "User creation failed! Please check user details and try again." });

            return Ok(new { status = "0001", Message = "User created successfully!" });
        }
    }
}

using api.eVoucher.Authentication;
using api.eVoucher.Dtos;
using api.eVoucher.Model;
using api.eVoucher.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.eVoucher.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PurchaseHistoryController : ControllerBase
    {
        private readonly IPurchaseHistoryRespository _purchaseHistoryRespository;
        private readonly IMapper _mapper;
        public PurchaseHistoryController(IPurchaseHistoryRespository purchaseHistoryRespository, IMapper mapper)
        {
            _purchaseHistoryRespository = purchaseHistoryRespository;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("verifypromo")]
        public async Task<ActionResult> VerifyPromoCode(PurchaseHistory model)
        {
            try
            {
                var purchaseHistory = await _purchaseHistoryRespository.GetPurchaseHistoryByPromoCode(model.promo_code);
                if (purchaseHistory != null)
                {
                    return Ok(new {
                        code = "0001",
                        message = "PromoCode verified.",
                        promo_code = purchaseHistory.promo_code,
                        is_used = purchaseHistory.is_used
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        code = "0003",
                        message = "PromoCode is not valid."
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPurchaseHistory()
        {
            try
            {
                var purchaseHistories = await _purchaseHistoryRespository.GetAll();
                purchaseHistories.Where(p => p.is_used).ToList().Select(ph => {
                        ph.promo_code = "";
                        ph.qr_code = null;
                        return ph;
                    }
                ).ToList();

                if (purchaseHistories == null)
                {
                    return BadRequest("No data found");
                }
                else
                {
                    return Ok(purchaseHistories);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPurchaseHistory(Guid id)
        {
            try
            {
                var purchaseHistories = await _purchaseHistoryRespository.GetAllByOrderId(id);
                if (purchaseHistories == null)
                {
                    return BadRequest("No data found");
                }
                else
                {
                    return Ok(purchaseHistories);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}

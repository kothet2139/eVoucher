using api.eVoucher.Dtos;
using api.eVoucher.Model;
using api.eVoucher.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public ProductsController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            try
            {
                var products = await _productRepository.GetAll();
                List<ProductViewModel> productViewModels = _mapper.Map<List<ProductViewModel>>(products);
                return Ok(productViewModels);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            try
            {
                var product = await _productRepository.Get(id);

                ProductViewModel productViewModel = _mapper.Map<ProductViewModel>(product);

                if (productViewModel == null)
                    return NotFound();

                return Ok(productViewModel);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct(ProductViewModel createProductDto)
        {
            try
            {
                Product product = _mapper.Map<Product>(createProductDto);

                await _productRepository.Add(product);
                return Ok(new
                {
                    code = "0001",
                    message = "Product created successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            try
            {
                await _productRepository.Delete(id);
                return Ok(new
                {
                    code = "0001",
                    message = "Product deleted successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(Guid id, ProductViewModel updateProductDto)
        {
            try
            {
                Product product = _mapper.Map<Product>(updateProductDto);

                await _productRepository.Update(id, product);
                return Ok(new
                {
                    code = "0001",
                    message = "Product updated successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

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
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        private readonly IMapper _mapper;
        public PaymentMethodController(IPaymentMethodRepository paymentMethodRepository, IMapper mapper)
        {
            _paymentMethodRepository = paymentMethodRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentMethod>>> GetPaymentMethods()
        {
            try
            {
                var paymentMethods = await _paymentMethodRepository.GetAll();
                List<PaymentMethodViewModel> productViewModels = _mapper.Map<List<PaymentMethodViewModel>>(paymentMethods);
                return Ok(productViewModels);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet("{productId}")]
        public async Task<ActionResult<IEnumerable<PaymentMethod>>> GetPaymentMethod(Guid productId)
        {
            try
            {
                var paymentMethods = await _paymentMethodRepository.GetAllByProductId(productId);
                List<PaymentMethodViewModel> productViewModels = _mapper.Map<List<PaymentMethodViewModel>>(paymentMethods);
                return Ok(productViewModels);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

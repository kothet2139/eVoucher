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
    public class OrdersController : ControllerBase
    {
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        public OrdersController(UserManager<ApplicationUser> userManager, IPaymentMethodRepository paymentMethodRepository,
            IOrderRepository orderRepository, IProductRepository productRepository, IMapper mapper)
        {
            _userManager = userManager;
            _paymentMethodRepository = paymentMethodRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("/checkout")]
        public async Task<ActionResult> Checkout(Order order)
        {
            try
            {
                var product = await _productRepository.GetActive(order.productid);                
                if(product != null)
                {
                    int quantity = product.quantity;

                    if (quantity > 0 && quantity >= order.quantity)
                    {
                        bool qtyIsOk = false;
                        if (product.is_onlyme_usage)
                        {
                            if (product.max_for_me < order.quantity)
                            {
                                return BadRequest(new
                                {
                                    code = "0003",
                                    message = "Order quantity is greater than maximun quantity to buy."
                                });
                            }
                            else
                            {
                                qtyIsOk = true;
                            }
                        }
                        else
                        {
                            if (product.max_to_gift < order.quantity && product.max_for_me < order.quantity)
                            {
                                return BadRequest(new
                                {
                                    code = "0003",
                                    message = "Order quantity is greater than maximun quantity to gift to use for other."
                                });
                            }
                            else
                            {
                                qtyIsOk = true;
                            }
                        }

                        if (qtyIsOk)
                        {
                            var paymentType = await _paymentMethodRepository.Get(order.payment_methodid);
                            decimal totalAmount = order.quantity * product.amount;
                            decimal paymentDiscount = 0;
                            if (paymentType != null)
                            {
                                paymentDiscount = paymentType.discount;
                            }

                            decimal totalDiscount = (totalAmount * paymentDiscount) / 100;
                        
                            order.discount_amount = totalDiscount;
                            order.total_amount = totalAmount;
                            string UserName = HttpContext.User.Identity.Name;
                            var user = await _userManager.FindByNameAsync(UserName);
                            order.user_id = user.Id;

                            await _orderRepository.Add(order);

                            product.quantity = product.quantity - order.quantity;

                            await _productRepository.Update(product.id , product);
                        }
                    }
                    else
                    {
                        return BadRequest(new
                        {
                            code = "0003",
                            message = "Stock quantity is not enough to buy."
                        });
                    }

                    return Ok(new
                    {
                        code = "0001",
                        message = "Order checkout successfully."
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        code = "0003",
                        message = "Product Not Found."
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("payment")]
        public async Task<ActionResult> Payment(Order order)
        {
            try
            {
                var getOrder = await _orderRepository.Get(order.id);

                if(getOrder != null)
                {
                    if(!String.IsNullOrEmpty(order.card_number) && !String.IsNullOrEmpty(order.cvv) && !String.IsNullOrEmpty(order.card_expiry_date))
                    {

                        await _orderRepository.Update(getOrder.id, order);

                        return Ok(new
                        {
                            code = "0001",
                            message = "Order payment successfully."
                        });
                    }
                    else
                    {
                        return BadRequest(new
                        {
                            code = "0003",
                            message = "Your payment information is not enough to pay."
                        });
                    }
                }
                else
                {
                    return BadRequest(new
                    {
                        code = "0003",
                        message = "Order Not Found."
                    });
                }
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

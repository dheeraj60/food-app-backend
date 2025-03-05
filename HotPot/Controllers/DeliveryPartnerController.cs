using HotPot.Exceptions;
using HotPot.Interfaces;
using HotPot.Models;
using HotPot.Models.DTO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotPot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("ReactPolicy")]
    public class DeliveryPartnerController : ControllerBase
    {
        private readonly IDeliveryPartnerServices _services;
        private readonly ILogger<DeliveryPartnerController> _logger;

        public DeliveryPartnerController(IDeliveryPartnerServices services, ILogger<DeliveryPartnerController> logger)
        {
            _services = services;
            _logger = logger;
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginUserDTO loginUser)
        {
            try
            {
                loginUser = await _services.LoginDeliveryPartner(loginUser);
                return Ok(loginUser);
            }
            catch (InvalidUserException e)
            {
                _logger.LogCritical(e.Message);
                return Unauthorized(e.Message);
            }
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDeliveryPartnerDTO registerDeliveryPartner)
        {
            try
            {
                var deliveryPartner = await _services.RegisterDeliveryPartner(registerDeliveryPartner);
                return Ok(deliveryPartner);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.Message);
                return BadRequest(e.Message);
            }
        }

        [Route("GetDetails")]
        [HttpGet]
        public async Task<IActionResult> GetDetails(int partnerId)
        {
            try
            {
                var deliveryPartner = await _services.GetDeliveryPartnerDetails(partnerId);
                return Ok(deliveryPartner);
            }
            catch (NoDeliveryPartnerFoundException e)
            {
                _logger.LogCritical(e.Message);
                return NotFound(e.Message);
            }
        }

        [Route("UpdateDetails")]
        [HttpPut]
        public async Task<IActionResult> UpdateDetails(DeliveryPartner deliveryPartner)
        {
            try
            {
                deliveryPartner = await _services.UpdateDeliveryPartnerDetails(deliveryPartner);
                return Ok(deliveryPartner);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.Message);
                return BadRequest(e.Message);
            }
        }

        [Route("ChangeOrderStatus")]
        [HttpPut]
        public async Task<IActionResult> ChangeOrderStatus(int orderId)
        {
            try
            {
                var order = await _services.ChangeOrderStatus(orderId);
                return Ok(order);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.Message);
                return BadRequest(e.Message);
            }
        }

        [Route("GetAllOrders")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrders(int partnerId)
        {
            try
            {
                var orders = await _services.GetAllOrders(partnerId);
                return Ok(orders);
            }
            catch (OrdersNotFoundException e)
            {
                _logger.LogCritical(e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}
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
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantUserServices _services;
        private readonly ILogger<RestaurantController> _logger;

        public RestaurantController(IRestaurantUserServices services, ILogger<RestaurantController> logger)
        {
            _services = services;
            _logger = logger;
        }

        //[Route("RegisterRestaurant")]
        //[HttpPost]
        //public async Task<LoginUserDTO> RegisterRestaurant(RegisterRestaurantDTO registerRestaurant)
        //{
        //    try
        //    {
        //        var newRestaurant=await _services.RegisterRestaurant
        //    }
        //}

        [Route("AddMenuItem")]
        [HttpPost]
        public async Task<ActionResult<Menu>> AddMenuItem(Menu newItem)
        {
            try
            {
                var menu = await _services.AddMenuItem(newItem);
                return menu;
            }
            catch (RestaurantNotFoundException e)
            {
                _logger.LogCritical(e.Message);
                return Unauthorized("Can't add the menu item");
            }
        }

        [Route("ChangeOrderStatus")]
        [HttpPut]
        public async Task<ActionResult<Order>> ChangeOrderStatus(int orderId, string newStatus)
        {
            try
            {
                var order = await _services.ChangeOrderStatus(orderId, newStatus);
                return order;
            }
            catch (OrdersNotFoundException e)
            {
                _logger.LogCritical(e.Message);
                return Unauthorized("Can't change the order status");
            }
        }

        [Route("AddRestaurant")]
        [HttpPost]
        public async Task<Restaurant> AddRestaurant(Restaurant restaurant)
        {
            restaurant = await _services.AddRestaurant(restaurant);
            return restaurant;
        }

        [Route("GetAllOrders")]
        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetALlOrders()
        {
            try
            {
                var result = await _services.GetAllOrders();
                return result;
            }
            catch (OrdersNotFoundException e)
            {
                _logger.LogCritical(e.Message);
                return NotFound("No order data found");
            }
        }

        [Route("GetAllOrdersByRestaurant")]
        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetALlOrders(int restaurantId)
        {
            try
            {
                var result = await _services.GetAllOrders(restaurantId);
                return result;
            }
            catch (OrdersNotFoundException e)
            {
                _logger.LogCritical(e.Message);
                return NotFound("No order data found");
            }
        }

        [Route("GetAllPayments")]
        [HttpGet]
        public async Task<ActionResult<List<Payment>>> GetPaymentsByRestaurants()
        {
            try
            {
                var payments = await _services.GetAllPayments();
                return payments;
            }
            catch (PaymentsNotFoundException e)
            {
                _logger.LogCritical(e.Message);
                return NotFound("No payment data found");
            }
        }

        [Route("GetAllPaymentsByRestaurants")]
        [HttpGet]
        public async Task<ActionResult<List<Payment>>> GetPaymentsByRestaurants(int restaurantId)
        {
            try
            {
                var payments = await _services.GetAllPayments(restaurantId);
                return payments;
            }
            catch (PaymentsNotFoundException e)
            {
                _logger.LogCritical(e.Message);
                return NotFound("No payment data found");
            }
        }

        [Route("GetSpecialities")]
        [HttpGet]
        public async Task<IActionResult> GetSpecialities()
        {
            try
            {
                var specialities = await _services.GetAllSpecialities();
                return Ok(specialities);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.Message);
                return BadRequest(e.Message);
            }
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginUserDTO loginUser)
        {
            try
            {
                loginUser = await _services.LogInRestaurant(loginUser);
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
        public async Task<IActionResult> Register(RegisterRestaurantDTO registerRestaurant)
        {
            try
            {
                var registeredRestaurant = await _services.RegisterRestaurant(registerRestaurant);
                return Ok(registeredRestaurant);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.Message);
                return BadRequest(e.Message);
            }
        }

        [Route("GetAllReviews")]
        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            try
            {
                var reviews = await _services.GetCustomerReviews();
                return Ok(reviews);
            }
            catch (NoCustomerReviewFoundException e)
            {
                _logger.LogCritical(e.Message);
                return NotFound(e.Message);
            }
        }

        [Route("DeleteMenuItem")]
        [HttpDelete]
        public async Task<IActionResult> DeleteMenuItem(int menuId)
        {
            try
            {
                var menuItem = await _services.DeleteMenuItem(menuId);
                return Ok(menuItem);
            }
            catch (NoMenuAvailableException e)
            {
                _logger.LogCritical(e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}
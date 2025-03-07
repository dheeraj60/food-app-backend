﻿using HotPot.Exceptions;
using HotPot.Interfaces;
using HotPot.Models;
using HotPot.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotPot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("ReactPolicy")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerServices _services;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ICustomerServices services, ILogger<CustomerController> logger)
        {
            _services = services;
            _logger = logger;
        }

        [Route("LogIn")]
        [HttpPost]
        public async Task<ActionResult<LoginUserDTO>> CustomerLogin(LoginUserDTO loginUser)
        {
            try
            {
                loginUser = await _services.LogIn(loginUser);
                return Ok(loginUser);
            }
            catch (InvalidUserException e)
            {
                _logger.LogWarning($"Login failed: {e.Message}");
                return Unauthorized(e.Message); // Return the exact reason for the failure
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error during login: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


        [Route("Register")]
        [HttpPost]
        public async Task<LoginUserDTO> CustomerRegistration(RegisterCustomerDTO registerCustomer)
        {
            var result = await _services.RegisterCustomer(registerCustomer);
            return result;
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDTO resetPassword)
        {
            try
            {
                await _services.ResetPassword(resetPassword);
                return Ok("Password reset successfully.");
            }
            catch (InvalidUserException e)
            {
                _logger.LogCritical(e.Message);
                return NotFound("User not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while resetting the password.");
                return StatusCode(500, "An error occurred while resetting the password.");
            }
        }

        //[Authorize(Roles ="Customer")]
        //[Authorize]
        [Route("GetRestaurantsByCity")]
        [HttpGet]
        public async Task<ActionResult<List<Restaurant>>> GetRestaurantsByCity(string city)
        {
            try
            {
                var result = await _services.GetRestaurantsByCity(city);
                return result;
            }
            catch (CityNotFoundException e)
            {
                _logger.LogCritical(e.Message);
                return NotFound(e.Message);
            }
            catch (RestaurantNotFoundException e)
            {
                _logger.LogCritical(e.Message);
                return NotFound(e.Message);
            }
        }

        [Route("GetRestaurantByName")]
        [HttpGet]
        public async Task<ActionResult<Restaurant>> GetRestaurantByName(string name)
        {
            try
            {
                var restaurant = await _services.GetRestaurantByName(name);
                return restaurant;
            }
            catch (RestaurantNotFoundException e)
            {
                _logger.LogCritical(e.Message);
                return NotFound("Can't find the restaurant you're looking for");
            }
        }

        [Route("GetMenuByRestaurant")]
        [HttpGet]
        public async Task<ActionResult<List<Menu>>> GetMenuByRestaurant(int restaurantId)
        {
            try
            {
                var result = await _services.GetMenuByRestaurant(restaurantId);
                return result;
            }
            catch (NoMenuAvailableException e)
            {
                _logger.LogCritical(e.Message);
                return NotFound("No Menu available to show at the moment");
            }
        }

        [Authorize(Roles = "Customer")]
        [Route("AddToCart")]
        [HttpPost]
        public async Task<CartMenuDTO> AddToCart(int userId, int menuItem)
        {
            var cart = await _services.AddToCart(userId, menuItem);
            return cart;
        }

        [Authorize(Roles = "Customer")]
        [Route("ViewCart")]
        [HttpGet]
        public async Task<ActionResult<List<CartMenuDTO>>> GetCarts(int userId)
        {
            try
            {
                var carts = await _services.GetCarts(userId);
                return carts;
            }
            catch (EmptyCartException e)
            {
                _logger.LogCritical(e.Message);
                return NotFound("Cart is empty");
            }
        }

        [Authorize(Roles = "Customer")]
        [Route("IncreaseCartItemQuantity")]
        [HttpPut]
        public async Task IncreaseCartItemQuantity(int cartId)
        {
            await _services.IncreaseCartItemQuantity(cartId);
        }

        [Authorize(Roles = "Customer")]
        [Route("DecreaseCartItemQuantity")]
        [HttpPut]
        public async Task DecreaseCartItemQuantity(int cartId)
        {
            await _services.DecreaseCartItemQuantity(cartId);
        }

        [Authorize(Roles = "Customer")]
        [Route("DeleteCartItem")]
        [HttpPut]
        public async Task DeleteCartItem(int cartId)
        {
            await _services.DeleteCartItem(cartId);
        }

        [Authorize(Roles = "Customer")]
        [Route("EmptyCart")]
        [HttpPut]
        public async Task EmptyCart(int customerId)
        {
            await _services.EmptyCart(customerId);
        }

        [Route("PlaceOrderForOne")]
        [HttpPost]
        public Task<OrderMenuDTO> PlaceOrderForOne(int cartId, string paymentMode)
        {
            var order = _services.PlaceOrderForOne(cartId, paymentMode);
            return order;
        }

        [Route("PlaceOrderForAll")]
        [HttpPost]
        public Task<OrderMenuDTO> PlaceOrderForAll(int customerId, string paymentMode)
        {
            var order = _services.PlaceOrder(customerId, paymentMode);
            return order;
        }

        [Authorize(Roles = "Customer")]
        [Route("ViewOrderStatus")]
        [HttpGet]
        public async Task<ActionResult<OrderMenuDTO>> ViewOrderStatus(int orderId)
        {
            try
            {
                var orderStatus = await _services.ViewOrderStatus(orderId);
                return orderStatus;
            }
            catch (OrdersNotFoundException e)
            {
                _logger.LogCritical(e.Message);
                return NotFound(e.Message);
            }
        }

        [Authorize(Roles = "Customer")]
        [HttpGet("ViewOrderHistoryForCustomer")]
        public async Task<ActionResult> ViewOrderHistoryForCustomer(int customerId)
        {
            try
            {
                var orderHistory = await _services.ViewOrderHistory(customerId);
                return Ok(orderHistory);
            }
            catch (OrdersNotFoundException e)
            {
                _logger.LogCritical(e.Message);
                return NotFound(e.Message);
            }
        }

        [Authorize(Roles = "Customer")]
        [Route("GetCustomerDetails")]
        [HttpGet]
        public async Task<IActionResult> GetCustomerDetails(int customerId)
        {
            try
            {
                var customer = await _services.GetCustomerDetails(customerId);
                return Ok(customer);
            }
            catch (NoUsersAvailableException e)
            {
                _logger.LogCritical(e.Message);
                return NotFound(e.Message);
            }
        }

        //[Authorize(Roles = "Customer")]
        [Route("UpdateCustomerDetails")]
        [HttpPut]
        public async Task<IActionResult> UpdateCustomerDetails(Customer customer)
        {
            try
            {
                var myCustomer = await _services.UpdateCustomerDetails(customer);
                return Ok(myCustomer);
            }
            catch (NoUsersAvailableException e)
            {
                _logger.LogCritical(e.Message);
                return NotFound(e.Message);
            }
        }

        [Route("GetAllCities")]
        [HttpGet]
        public async Task<IActionResult> GetAllCities()
        {
            try
            {
                var cities = await _services.GetAllCities();
                return Ok(cities);
            }
            catch (CityNotFoundException e)
            {
                _logger.LogCritical(e.Message);
                return NotFound(e.Message);
            }
        }

        [Route("CancelOrderFromCustomer")]
        [HttpPut]
        public async Task<IActionResult> CancelOrderFromCustomer(int orderId)
        {
            try
            {
                var order = await _services.CancelOrderFromCustomer(orderId);
                return Ok(order);
            }
            catch (OrdersNotFoundException e)
            {
                _logger.LogCritical(e.Message);
                return NotFound(e.Message);
            }
        }

        //Controllers set 2
        [Authorize(Roles = "Customer")]
        [HttpPost("address")]
        public async Task<IActionResult> AddCustomerAddress(CustomerAddress customerAddress)
        {
            try
            {
                var addedAddress = await _services.AddCustomerAddress(customerAddress);
                return Ok(addedAddress);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding customer address: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        //[Authorize(Roles = "Customer")]
        [HttpPut("address/{addressId}")]
        public async Task<IActionResult> UpdateCustomerAddress(int addressId, CustomerAddressUpdateDTO addressUpdateDto)
        {
            try
            {
                var updatedAddress = await _services.UpdateCustomerAddress(addressId, addressUpdateDto);
                return Ok(updatedAddress);
            }
            catch (NoCustomerAddressFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating customer address: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        //[Authorize(Roles = "RestaurantOwner")]
        [Authorize]
        [HttpGet("address/{customerId}")]
        public async Task<IActionResult> ViewCustomerAddressByCustomerId(int customerId)
        {
            try
            {
                var customerAddress = await _services.ViewCustomerAddressByCustomerId(customerId);
                return Ok(customerAddress);
            }
            catch (NoCustomerAddressFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching customer address for ID {customerId}: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("review")]
        public async Task<IActionResult> AddCustomerReview(CustomerReview customerReview)
        {
            try
            {
                var addedReview = await _services.AddCustomerReview(customerReview);
                return Ok(addedReview);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding customer review: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("review/{customerReviewId}")]
        public async Task<IActionResult> ViewCustomerReview(int customerReviewId)
        {
            try
            {
                var review = await _services.ViewCustomerReview(customerReviewId);
                return Ok(review);
            }
            catch (NoCustomerReviewFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving customer review: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("review")]
        public async Task<IActionResult> UpdateCustomerReviewText(CustomerReviewUpdateDTO reviewUpdateDTO)
        {
            try
            {
                var updatedReview = await _services.UpdateCustomerReviewText(reviewUpdateDTO);
                return Ok(updatedReview);
            }
            catch (NoCustomerReviewFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating customer review: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("review/{reviewId}")]
        public async Task<IActionResult> DeleteCustomerReview(int reviewId)
        {
            try
            {
                var deletedReview = await _services.DeleteCustomerReview(reviewId);
                return Ok(deletedReview);
            }
            catch (NoCustomerReviewFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting customer review: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        //Additional Menu controllers
        [HttpGet("menu/search")]
        public async Task<IActionResult> SearchMenu(int restaurantId, [FromQuery] string query)
        {
            try
            {
                var matchingMenuItems = await _services.SearchMenu(restaurantId, query);
                return Ok(matchingMenuItems);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error searching menu: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("menu/filter/price")]
        public async Task<IActionResult> FilterMenuByPriceRange(int restaurantId, [FromQuery] float minPrice, [FromQuery] float maxPrice)
        {
            try
            {
                var filteredMenuItems = await _services.FilterMenuByPriceRange(restaurantId, minPrice, maxPrice);
                return Ok(filteredMenuItems);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error filtering menu by price range: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("menu/filter/type")]
        public async Task<IActionResult> FilterMenuByType(int restaurantId, [FromQuery] string type)
        {
            try
            {
                var filteredMenuItems = await _services.FilterMenuByType(restaurantId, type);
                return Ok(filteredMenuItems);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error filtering menu by type: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("menu/filter/cuisine")]
        public async Task<IActionResult> FilterMenuByCuisine(int restaurantId, [FromQuery] string cuisine)
        {
            try
            {
                var filteredMenuItems = await _services.FilterMenuByCuisine(restaurantId, cuisine);
                return Ok(filteredMenuItems);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error filtering menu by cuisine: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        //Restaurant Endpoints
        [HttpGet("restaurants")]
        public async Task<IActionResult> ViewRestaurants()
        {
            try
            {
                var restaurants = await _services.ViewRestaurants();
                return Ok(restaurants);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving restaurants: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
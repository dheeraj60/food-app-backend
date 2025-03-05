
using HotPot.Exceptions;
using HotPot.Interfaces;
using HotPot.Models;
using HotPot.Models.DTO;
using HotPot.Repositories;
using HotPot.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotPot.Tests.Services
{
    [TestFixture]
    public class CustomerServicesTests
    {
        private CustomerServices _customerServices;
        private Mock<IRepository<int, string, Customer>> _custRepoMock;
        private Mock<IRepository<int, string, User>> _userRepoMock;
        private Mock<IRepository<int, string, Menu>> _menuRepoMock;
        private Mock<IRepository<int, string, Cart>> _cartRepoMock;
        private Mock<IRepository<int, string, Order>> _orderRepoMock;
        private Mock<IRepository<int, string, OrderItem>> _orderItemRepoMock;
        private Mock<IRepository<int, string, Payment>> _paymentRepoMock;
        private Mock<IRepository<int, string, Restaurant>> _restaurantRepoMock;
        private Mock<IRepository<int, string, City>> _cityRepoMock;
        private Mock<IRepository<int, string, CustomerAddress>> _custAddressRepoMock;
        private Mock<IRepository<int, string, CustomerReview>> _custReviewRepoMock;
        private Mock<IRepository<int, string, DeliveryPartner>> _deliveryPartnerRepoMock;
        private Mock<ITokenServices> _tokenServicesMock;
        private Mock<ILogger<CustomerServices>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _custRepoMock = new Mock<IRepository<int, string, Customer>>();
            _userRepoMock = new Mock<IRepository<int, string, User>>();
            _menuRepoMock = new Mock<IRepository<int, string, Menu>>();
            _cartRepoMock = new Mock<IRepository<int, string, Cart>>();
            _orderRepoMock = new Mock<IRepository<int, string, Order>>();
            _orderItemRepoMock = new Mock<IRepository<int, string, OrderItem>>();
            _paymentRepoMock = new Mock<IRepository<int, string, Payment>>();
            _restaurantRepoMock = new Mock<IRepository<int, string, Restaurant>>();
            _cityRepoMock = new Mock<IRepository<int, string, City>>();
            _custAddressRepoMock = new Mock<IRepository<int, string, CustomerAddress>>();
            _custReviewRepoMock = new Mock<IRepository<int, string, CustomerReview>>();
            _deliveryPartnerRepoMock = new Mock<IRepository<int, string, DeliveryPartner>>();
            _tokenServicesMock = new Mock<ITokenServices>();
            _loggerMock = new Mock<ILogger<CustomerServices>>();

            _customerServices = new CustomerServices(
                _custRepoMock.Object,
                _userRepoMock.Object,
                _menuRepoMock.Object,
                _cartRepoMock.Object,
                _orderRepoMock.Object,
                _orderItemRepoMock.Object,
                _paymentRepoMock.Object,
                _restaurantRepoMock.Object,
                _cityRepoMock.Object,
                _custAddressRepoMock.Object,
                _custReviewRepoMock.Object,
                _deliveryPartnerRepoMock.Object,
                _tokenServicesMock.Object,
                _loggerMock.Object);
        }

        //[Test]
        //public async Task LogIn_ValidUser_ReturnsLoginUserDTO()
        //{
        //    // Arrange
        //    var loginUser = new LoginUserDTO { UserName = "username", Password = "password" };
        //    var user = new User { UserName = "username", Password = Encoding.UTF8.GetBytes("password"), Key = Encoding.UTF8.GetBytes("key"), Role = "Customer" };
        //    var customer = new Customer { Id = 1, UserName = "username" };
        //    var token = "token";
        //    _userRepoMock.Setup(repo => repo.GetAsync("username")).ReturnsAsync(user);
        //    _custRepoMock.Setup(repo => repo.GetAsync()).ReturnsAsync(new List<Customer> { customer });
        //    _tokenServicesMock.Setup(service => service.GenerateToken(loginUser)).ReturnsAsync(token);

        //    // Act
        //    var result = await _customerServices.LogIn(loginUser);

        //    // Assert
        //    Assert.NotNull(result);
        //}

        [Test]
        public async Task RegisterCustomer_ValidInput_ReturnsLoginUserDTO()
        {
            // Arrange
            var registerCustomer = new RegisterCustomerDTO { UserName = "username", Password = "password" };
            var user = new User { UserName = "username", Password = Encoding.UTF8.GetBytes("password"), Role = "Customer" };
            var customer = new Customer { Id = 1, UserName = "username" };
            _userRepoMock.Setup(repo => repo.Add(It.IsAny<User>())).ReturnsAsync(user);
            _custRepoMock.Setup(repo => repo.Add(It.IsAny<Customer>())).ReturnsAsync(customer);
            _custAddressRepoMock.Setup(repo => repo.Add(It.IsAny<CustomerAddress>())).ReturnsAsync(new CustomerAddress());

            // Act
            var result = await _customerServices.RegisterCustomer(registerCustomer);

            // Assert
            Assert.NotNull(result);
        }


        [Test]
        public async Task GetMenuByRestaurant_ValidRestaurantId_ReturnsMenuList()
        {
            // Arrange
            var restaurantId = 1;
            var menus = new List<Menu>
            {
                new Menu { RestaurantId = restaurantId },
                new Menu { RestaurantId = 2 },
                new Menu { RestaurantId = restaurantId }
            };
            _menuRepoMock.Setup(repo => repo.GetAsync()).ReturnsAsync(menus);

            // Act
            var result = await _customerServices.GetMenuByRestaurant(restaurantId);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetMenuByRestaurant_NoMenuAvailable_ThrowsNoMenuAvailableException()
        {
            // Arrange
            var restaurantId = 1;
            _menuRepoMock.Setup(repo => repo.GetAsync()).ReturnsAsync(new List<Menu>());

            // Act & Assert
            Assert.ThrowsAsync<NoMenuAvailableException>(async () => await _customerServices.GetMenuByRestaurant(restaurantId));
        }

        [Test]
        public async Task GetRestaurantByName_ValidName_ReturnsRestaurant()
        {
            // Arrange
            var restaurantName = "Test Restaurant";
            var restaurant = new Restaurant { RestaurantName = restaurantName };
            _restaurantRepoMock.Setup(repo => repo.GetAsync(restaurantName)).ReturnsAsync(restaurant);

            // Act
            var result = await _customerServices.GetRestaurantByName(restaurantName);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetRestaurantByName_RestaurantNotFound_ThrowsRestaurantNotFoundException()
        {
            // Arrange
            var restaurantName = "Non-existent Restaurant";
            _restaurantRepoMock.Setup(repo => repo.GetAsync(restaurantName)).ReturnsAsync((Restaurant)null);

            // Act & Assert
            Assert.ThrowsAsync<RestaurantNotFoundException>(async () => await _customerServices.GetRestaurantByName(restaurantName));
        }

        [Test]
        public async Task GetRestaurantsByCity_ValidCity_ReturnsRestaurantList()
        {
            // Arrange
            var cityName = "Test City";
            var city = new City { Name = cityName };
            var restaurants = new List<Restaurant>
            {
                new Restaurant { CityId = city.CityId },
                new Restaurant { CityId = 2 },
                new Restaurant { CityId = city.CityId }
            };
            _cityRepoMock.Setup(repo => repo.GetAsync(cityName)).ReturnsAsync(city);
            _restaurantRepoMock.Setup(repo => repo.GetAsync()).ReturnsAsync(restaurants);

            // Act
            var result = await _customerServices.GetRestaurantsByCity(cityName);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetRestaurantsByCity_CityNotFound_ThrowsCityNotFoundException()
        {
            // Arrange
            var cityName = "Non-existent City";
            _cityRepoMock.Setup(repo => repo.GetAsync(cityName)).ReturnsAsync((City)null);

            // Act & Assert
            Assert.ThrowsAsync<CityNotFoundException>(async () => await _customerServices.GetRestaurantsByCity(cityName));
        }

        [Test]
        public async Task GetRestaurantsByCity_NoRestaurantsFound_ThrowsRestaurantNotFoundException()
        {
            // Arrange
            var cityName = "Test City";
            var city = new City { Name = cityName };
            _cityRepoMock.Setup(repo => repo.GetAsync(cityName)).ReturnsAsync(city);
            _restaurantRepoMock.Setup(repo => repo.GetAsync()).ReturnsAsync(new List<Restaurant>());

            // Act & Assert
            Assert.ThrowsAsync<RestaurantNotFoundException>(async () => await _customerServices.GetRestaurantsByCity(cityName));
        }

        //    
        //    [Test]
        //    public async Task PlaceOrder_ReturnsOrderMenuDTO()
        //    {
        //        // Arrange
        //        int customerId = 1;
        //        var carts = new List<Cart>
        //{
        //    new Cart { Id = 1, CustomerId = customerId, MenuItemId = 1, Quantity = 2, Status = "added", RestaurantId = 1 }
        //};
        //        var restaurant = new Restaurant { RestaurantId = 1, CityId = 1 };
        //        var menu = new Menu { MenuId = 1, Price = 10.0f, RestaurantId = 1 };
        //        var deliveryPartners = new List<DeliveryPartner> { new DeliveryPartner { PartnerId = 1 } };
        //        var payment = new Payment { PaymentId=1, Status = "successful" };
        //        _cartRepoMock.Setup(repo => repo.GetAsync()).ReturnsAsync(carts);
        //        _restaurantRepoMock.Setup(repo => repo.GetAsync(1)).ReturnsAsync(restaurant);
        //        _menuRepoMock.Setup(repo => repo.GetAsync(1)).ReturnsAsync(menu);
        //        _deliveryPartnerRepoMock.Setup(repo => repo.GetAsync()).ReturnsAsync(deliveryPartners);
        //        _paymentRepoMock.Setup(repo => repo.Update(It.IsAny<Payment>())).ReturnsAsync(payment);

        //        // Act
        //        var result = await _customerServices.PlaceOrder(customerId, "paymentMode");

        //        // Assert
        //        Assert.NotNull(result);
        //    }


        //[Test]
        //public async Task PlaceOrderForOne_ReturnsOrderMenuDTO()
        //{
        //    // Arrange
        //    int cartItemId = 1;
        //    var cartItem = new Cart { Id = 1, CustomerId = 1, MenuItemId = 1, Quantity = 2, Status = "added", RestaurantId = 1 };
        //    var menu = new Menu { MenuId = 1, Price = 10.0f, RestaurantId = 1 };
        //    var payment = new Payment { Status = "successful" };
        //    _cartRepoMock.Setup(repo => repo.GetAsync(cartItemId)).ReturnsAsync(cartItem);
        //    _menuRepoMock.Setup(repo => repo.GetAsync(1)).ReturnsAsync(menu);
        //    _paymentRepoMock.Setup(repo => repo.Update(It.IsAny<Payment>())).ReturnsAsync(payment);

        //    // Act
        //    var result = await _customerServices.PlaceOrderForOne(cartItemId, "paymentMode");

        //    // Assert
        //    Assert.NotNull(result);
        //}


        [Test]
        public async Task RecordPayment_ValidOrder_ReturnsPayment()
        {
            // Arrange
            var order = new Order { OrderId = 1, Amount = 100 };

            // Act
            var result = await _customerServices.RecordPayment(order);

            // Assert
            Assert.Null(result);
        }

        [Test]
        public async Task AddToCart_ValidUserIdAndMenuItemId_ReturnsCartMenuDTO()
        {
            // Arrange
            var userId = 1;
            var menuItemId = 1;
            var menuItem = new Menu { MenuId = menuItemId, RestaurantId = 1, Price = 10 };
            _menuRepoMock.Setup(r => r.GetAsync(menuItemId)).ReturnsAsync(menuItem);
            var cartItems = new List<Cart>
    {
        new Cart { Id = 1, CustomerId = userId, MenuItemId = 2, Quantity = 1, Status = "added" },
        new Cart { Id = 2, CustomerId = userId, MenuItemId = 3, Quantity = 2, Status = "added" }
    };
            _cartRepoMock.Setup(r => r.GetAsync()).ReturnsAsync(cartItems);
            _cartRepoMock.Setup(r => r.Add(It.IsAny<Cart>())).ReturnsAsync((Cart c) => c);

            // Act
            var result = await _customerServices.AddToCart(userId, menuItemId);

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task GetCarts_ReturnsCartMenuDTOList()
        {
            // Arrange
            int customerId = 1;
            var cartItems = new List<Cart>
        {
            new Cart { Id = 1, CustomerId = customerId, MenuItemId = 1, Quantity = 2, Status = "added", RestaurantId = 1 },
            new Cart { Id = 2, CustomerId = customerId, MenuItemId = 2, Quantity = 1, Status = "added", RestaurantId = 1 }
        };
            var restaurant = new Restaurant { RestaurantId = 1, RestaurantName = "Restaurant1", CityId = 1 };
            var menuItem1 = new Menu { MenuId = 1, Name = "Menu1", Price = 10.0f, RestaurantId = 1, ItemImage = "Image1" };
            var menuItem2 = new Menu { MenuId = 2, Name = "Menu2", Price = 20.0f, RestaurantId = 1, ItemImage = "Image2" };
            _cartRepoMock.Setup(repo => repo.GetAsync()).ReturnsAsync(cartItems);
            _restaurantRepoMock.Setup(repo => repo.GetAsync(1)).ReturnsAsync(restaurant);
            _menuRepoMock.Setup(repo => repo.GetAsync(1)).ReturnsAsync(menuItem1);
            _menuRepoMock.Setup(repo => repo.GetAsync(2)).ReturnsAsync(menuItem2);

            // Act
            var result = await _customerServices.GetCarts(customerId);

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task DeleteCartItem_ValidCartItemId_CallsCartRepoUpdate()
        {
            // Arrange
            var cartItemId = 1;
            var cartItem = new Cart { Id = cartItemId, Status = "added" };
            _cartRepoMock.Setup(r => r.GetAsync(cartItemId)).ReturnsAsync(cartItem);

            // Act
            await _customerServices.DeleteCartItem(cartItemId);

            // Assert
            _cartRepoMock.Verify(r => r.Update(cartItem), Times.Once);
        }

        [Test]
        public async Task EmptyCart_ValidCustomerId_CallsCartRepoUpdateForEachCartItem()
        {
            // Arrange
            var customerId = 1;
            var cartItems = new List<Cart>
    {
        new Cart { Id = 1, CustomerId = customerId, Status = "added" },
        new Cart { Id = 2, CustomerId = customerId, Status = "added" }
    };
            _cartRepoMock.Setup(r => r.GetAsync()).ReturnsAsync(cartItems);

            // Act
            await _customerServices.EmptyCart(customerId);

            // Assert
            _cartRepoMock.Verify(r => r.Update(It.IsAny<Cart>()), Times.Exactly(2));
        }

        [Test]
        public async Task IncreaseCartItemQuantity_ValidCartId_CallsCartRepoUpdate()
        {
            // Arrange
            var cartId = 1;
            var cartItem = new Cart { Id = cartId, Quantity = 1 };
            _cartRepoMock.Setup(r => r.GetAsync(cartId)).ReturnsAsync(cartItem);

            // Act
            await _customerServices.IncreaseCartItemQuantity(cartId);

            // Assert
            _cartRepoMock.Verify(r => r.Update(cartItem), Times.Once);
        }

        [Test]
        public async Task DecreaseCartItemQuantity_QuantityGreaterThan1_CallsCartRepoUpdate()
        {
            // Arrange
            var cartId = 1;
            var cartItem = new Cart { Id = cartId, Quantity = 2 };
            _cartRepoMock.Setup(r => r.GetAsync(cartId)).ReturnsAsync(cartItem);

            // Act
            await _customerServices.DecreaseCartItemQuantity(cartId);

            // Assert
            _cartRepoMock.Verify(r => r.Update(cartItem), Times.Once);
        }

        [Test]
        public async Task DecreaseCartItemQuantity_QuantityEquals1_CallsDeleteCartItem()
        {
            // Arrange
            var cartId = 1;
            var cartItem = new Cart { Id = cartId, Quantity = 1 };
            _cartRepoMock.Setup(r => r.GetAsync(cartId)).ReturnsAsync(cartItem);

            // Act
            await _customerServices.DecreaseCartItemQuantity(cartId);

            // Assert
            _cartRepoMock.Verify(r => r.Update(cartItem), Times.Once);
        }

        [Test]
        public async Task ViewOrderStatus_ExistingOrder_ReturnsOrderMenuDTO()
        {
            // Arrange
            int orderId = 1;
            var order = new Order { OrderId = orderId, RestaurantId = 1, CustomerId = 1, Status = "Pending", OrderDate = DateTime.Now };
            var orderItem = new OrderItem { OrderId = orderId, MenuId = 1, Quantity = 1, SubTotalPrice = 10.0f };
            var menu = new Menu { MenuId = 1, Name = "Menu Item" };
            var restaurant = new Restaurant { RestaurantId = 1, RestaurantName = "Restaurant", RestaurantImage = "Image" };

            _orderRepoMock.Setup(repo => repo.GetAsync(orderId)).ReturnsAsync(order);
            _restaurantRepoMock.Setup(repo => repo.GetAsync(order.RestaurantId)).ReturnsAsync(restaurant);
            _orderItemRepoMock.Setup(repo => repo.GetAsync()).ReturnsAsync(new List<OrderItem> { orderItem });
            _menuRepoMock.Setup(repo => repo.GetAsync(orderItem.MenuId)).ReturnsAsync(menu);

            // Act
            var result = await _customerServices.ViewOrderStatus(orderId);

            // Assert
            Assert.AreEqual(orderId, result.orderId);
        }

        [Test]
        public async Task ViewOrderHistory_ExistingOrders_ReturnsOrderMenuDTOList()
        {
            // Arrange
            int customerId = 1;
            var order1 = new Order { OrderId = 1, RestaurantId = 1, CustomerId = customerId, Status = "Pending", OrderDate = DateTime.Now };
            var order2 = new Order { OrderId = 2, RestaurantId = 2, CustomerId = customerId, Status = "Completed", OrderDate = DateTime.Now };
            var orderItem1 = new OrderItem { OrderId = 1, MenuId = 1, Quantity = 1, SubTotalPrice = 10.0f };
            var orderItem2 = new OrderItem { OrderId = 2, MenuId = 2, Quantity = 2, SubTotalPrice = 20.0f };
            var menu1 = new Menu { MenuId = 1, Name = "Menu Item 1" };
            var menu2 = new Menu { MenuId = 2, Name = "Menu Item 2" };
            var restaurant1 = new Restaurant { RestaurantId = 1, RestaurantName = "Restaurant 1", RestaurantImage = "Image 1" };
            var restaurant2 = new Restaurant { RestaurantId = 2, RestaurantName = "Restaurant 2", RestaurantImage = "Image 2" };

            _orderRepoMock.Setup(repo => repo.GetAsync()).ReturnsAsync(new List<Order> { order1, order2 });
            _restaurantRepoMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) => id == 1 ? restaurant1 : restaurant2);
            _orderItemRepoMock.Setup(repo => repo.GetAsync()).ReturnsAsync(new List<OrderItem> { orderItem1, orderItem2 });
            _menuRepoMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) => id == 1 ? menu1 : menu2);

            // Act
            var result = await _customerServices.ViewOrderHistory(customerId);

            // Assert
            Assert.AreEqual(2, result.Count);

        }



        [Test]
        public async Task GetCustomerDetails_ValidCustomerId_ReturnsCustomer()
        {
            // Arrange
            var customerId = 1;
            var customer = new Customer { Id = customerId, Name = "John Doe" };
            _custRepoMock.Setup(r => r.GetAsync(customerId)).ReturnsAsync(customer);

            // Act
            var result = await _customerServices.GetCustomerDetails(customerId);

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task UpdateCustomerDetails_ValidCustomer_ReturnsCustomer()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Jane Doe" };
            _custRepoMock.Setup(r => r.GetAsync(1)).ReturnsAsync(customer);
            _custRepoMock.Setup(r => r.Update(customer)).ReturnsAsync(customer);

            // Act
            var result = await _customerServices.UpdateCustomerDetails(customer);

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task GetAllCities_ReturnsListOfCities()
        {
            // Arrange
            var cities = new List<City>
    {
        new City { CityId = 1, Name = "City1" },
        new City { CityId = 2, Name = "City2" }
    };
            _cityRepoMock.Setup(r => r.GetAsync()).ReturnsAsync(cities);

            // Act
            var result = await _customerServices.GetAllCities();

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task CancelOrderFromCustomer_ValidOrderId_ReturnsCancelledOrder()
        {
            // Arrange
            var orderId = 1;
            var order = new Order { OrderId = orderId, Status = "placed" };
            _orderRepoMock.Setup(r => r.GetAsync(orderId)).ReturnsAsync(order);
            _orderRepoMock.Setup(r => r.Update(order)).ReturnsAsync(order);

            // Act
            var result = await _customerServices.CancelOrderFromCustomer(orderId);

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task AddCustomerAddress_ValidCustomerAddress_ReturnsAddedCustomerAddress()
        {
            // Arrange
            var customerAddress = new CustomerAddress { AddressId = 1 };
            _custAddressRepoMock.Setup(r => r.Add(customerAddress)).ReturnsAsync(customerAddress);

            // Act
            var result = await _customerServices.AddCustomerAddress(customerAddress);

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task UpdateCustomerAddress_ValidAddressIdAndAddressUpdateDto_ReturnsUpdatedCustomerAddress()
        {
            // Arrange
            var addressId = 1;
            var addressUpdateDto = new CustomerAddressUpdateDTO { HouseNumber = "123", BuildingName = "Building", Locality = "Locality", CityId = 1, LandMark = "Landmark" };
            var existingAddress = new CustomerAddress { AddressId = addressId };
            _custAddressRepoMock.Setup(r => r.GetAsync(addressId)).ReturnsAsync(existingAddress);
            _custAddressRepoMock.Setup(r => r.Update(existingAddress)).ReturnsAsync(existingAddress);

            // Act
            var result = await _customerServices.UpdateCustomerAddress(addressId, addressUpdateDto);

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task ViewCustomerAddressByCustomerId_ValidCustomerId_ReturnsCustomerAddress()
        {
            // Arrange
            var customerId = 1;
            var customerAddress = new CustomerAddress { CustomerId = customerId };
            _custAddressRepoMock.Setup(r => r.GetAsync(customerId)).ReturnsAsync(customerAddress);

            // Act
            var result = await _customerServices.ViewCustomerAddressByCustomerId(customerId);

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task AddCustomerReview_ValidCustomerReview_ReturnsAddedCustomerReview()
        {
            // Arrange
            var customerReview = new CustomerReview { ReviewId = 1 };
            _custReviewRepoMock.Setup(r => r.Add(customerReview)).ReturnsAsync(customerReview);

            // Act
            var result = await _customerServices.AddCustomerReview(customerReview);

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task ViewCustomerReview_ExistingReview_ReturnsReview()
        {
            // Arrange
            int customerReviewId = 1;
            var expectedReview = new CustomerReview { ReviewId = customerReviewId };
            _custReviewRepoMock.Setup(repo => repo.GetAsync(customerReviewId)).ReturnsAsync(expectedReview);

            // Act
            var result = await _customerServices.ViewCustomerReview(customerReviewId);

            // Assert
            Assert.AreEqual(expectedReview, result);
        }

        [Test]
        public void ViewCustomerReview_NonExistingReview_ThrowsException()
        {
            // Arrange
            int customerReviewId = 1;
            _custReviewRepoMock.Setup(repo => repo.GetAsync(customerReviewId)).ReturnsAsync((CustomerReview)null);

            // Assert
            Assert.ThrowsAsync<NoCustomerReviewFoundException>(async () => await _customerServices.ViewCustomerReview(customerReviewId));
        }

        [Test]
        public async Task UpdateCustomerReviewText_ExistingReview_ReturnsUpdatedReview()
        {
            // Arrange
            var reviewUpdateDTO = new CustomerReviewUpdateDTO { ReviewId = 1, TextReview = "Updated Text" };
            var existingReview = new CustomerReview { ReviewId = reviewUpdateDTO.ReviewId, TextReview = "Existing Text" };
            _custReviewRepoMock.Setup(repo => repo.GetAsync(reviewUpdateDTO.ReviewId)).ReturnsAsync(existingReview);
            _custReviewRepoMock.Setup(repo => repo.Update(existingReview)).ReturnsAsync(existingReview);

            // Act
            var result = await _customerServices.UpdateCustomerReviewText(reviewUpdateDTO);

            // Assert
            Assert.AreEqual(reviewUpdateDTO.TextReview, result.TextReview);
        }

        [Test]
        public void UpdateCustomerReviewText_NonExistingReview_ThrowsException()
        {
            // Arrange
            var reviewUpdateDTO = new CustomerReviewUpdateDTO { ReviewId = 1, TextReview = "Updated Text" };
            _custReviewRepoMock.Setup(repo => repo.GetAsync(reviewUpdateDTO.ReviewId)).ReturnsAsync((CustomerReview)null);

            // Assert
            Assert.ThrowsAsync<NoCustomerReviewFoundException>(async () => await _customerServices.UpdateCustomerReviewText(reviewUpdateDTO));
        }

        [Test]
        public async Task DeleteCustomerReview_ExistingReview_ReturnsDeletedReview()
        {
            // Arrange
            int reviewId = 1;
            var existingReview = new CustomerReview { ReviewId = reviewId };
            _custReviewRepoMock.Setup(repo => repo.GetAsync(reviewId)).ReturnsAsync(existingReview);
            _custReviewRepoMock.Setup(repo => repo.Delete(reviewId)).ReturnsAsync(existingReview);

            // Act
            var result = await _customerServices.DeleteCustomerReview(reviewId);

            // Assert
            Assert.AreEqual(existingReview, result);
        }

        [Test]
        public void DeleteCustomerReview_NonExistingReview_ThrowsException()
        {
            // Arrange
            int reviewId = 1;
            _custReviewRepoMock.Setup(repo => repo.GetAsync(reviewId)).ReturnsAsync((CustomerReview)null);

            // Assert
            Assert.ThrowsAsync<NoCustomerReviewFoundException>(async () => await _customerServices.DeleteCustomerReview(reviewId));
        }

        [Test]
        public async Task SearchMenu_ReturnsMatchingMenuItems()
        {
            // Arrange
            int restaurantId = 1;
            string query = "Pizza";
            var allMenus = new List<Menu>
        {
            new Menu { RestaurantId = restaurantId, Name = "Margherita Pizza" },
            new Menu { RestaurantId = restaurantId, Name = "Pepperoni Pizza" }
        };
            _menuRepoMock.Setup(repo => repo.GetAsync()).ReturnsAsync(allMenus);

            // Act
            var result = await _customerServices.SearchMenu(restaurantId, query);

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task FilterMenuByPriceRange_ReturnsFilteredMenuItems()
        {
            // Arrange
            int restaurantId = 1;
            float minPrice = 10.0f;
            float maxPrice = 20.0f;
            var allMenus = new List<Menu>
        {
            new Menu { RestaurantId = restaurantId, Price = 15.0f },
            new Menu { RestaurantId = restaurantId, Price = 25.0f }
        };
            _menuRepoMock.Setup(repo => repo.GetAsync()).ReturnsAsync(allMenus);

            // Act
            var result = await _customerServices.FilterMenuByPriceRange(restaurantId, minPrice, maxPrice);

            // Assert
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public async Task FilterMenuByType_ReturnsFilteredMenuItems()
        {
            // Arrange
            int restaurantId = 1;
            string type = "Main Course";
            var allMenus = new List<Menu>
        {
            new Menu { RestaurantId = restaurantId, Type = "Main Course" },
            new Menu { RestaurantId = restaurantId, Type = "Dessert" }
        };
            _menuRepoMock.Setup(repo => repo.GetAsync()).ReturnsAsync(allMenus);

            // Act
            var result = await _customerServices.FilterMenuByType(restaurantId, type);

            // Assert
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public async Task FilterMenuByCuisine_ReturnsFilteredMenuItems()
        {
            // Arrange
            int restaurantId = 1;
            string cuisine = "Italian";
            var allMenus = new List<Menu>
        {
            new Menu { RestaurantId = restaurantId, Cuisine = "Italian" },
            new Menu { RestaurantId = restaurantId, Cuisine = "Indian" }
        };
            _menuRepoMock.Setup(repo => repo.GetAsync()).ReturnsAsync(allMenus);

            // Act
            var result = await _customerServices.FilterMenuByCuisine(restaurantId, cuisine);

            // Assert
            Assert.AreEqual(1, result.Count);
        }
    }
}
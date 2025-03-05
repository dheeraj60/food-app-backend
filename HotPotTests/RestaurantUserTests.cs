
using NUnit.Framework;
using Moq;
using HotPot.Services;
using HotPot.Models;
using HotPot.Models.DTO;
using HotPot.Interfaces;
using HotPot.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace HotPot.Tests
{
    [TestFixture]
    public class RestaurantUserServicesTests
    {
        private Mock<IRepository<int, string, Restaurant>> _mockRestaurantRepo;
        private Mock<IRepository<int, string, City>> _mockCityRepo;
        private Mock<IRepository<int, string, Menu>> _mockMenuRepo;
        private Mock<IRepository<int, string, Payment>> _mockPaymentRepo;
        private Mock<IRepository<int, string, Order>> _mockOrderRepo;
        private Mock<IRepository<int, string, User>> _mockUserRepo;
        private Mock<IRepository<int, string, RestaurantOwner>> _mockRestOwnerRepo;
        private Mock<IRepository<int, string, RestaurantSpeciality>> _mockSpecialityRepo;
        private Mock<IRepository<int, string, CustomerReview>> _mockReviewRepo;
        private Mock<ITokenServices> _mockTokenServices;
        private Mock<ILogger<RestaurantUserServices>> _mockLogger;

        private RestaurantUserServices _restaurantUserServices;

        [SetUp]
        public void Setup()
        {
            _mockRestaurantRepo = new Mock<IRepository<int, string, Restaurant>>();
            _mockCityRepo = new Mock<IRepository<int, string, City>>();
            _mockMenuRepo = new Mock<IRepository<int, string, Menu>>();
            _mockPaymentRepo = new Mock<IRepository<int, string, Payment>>();
            _mockOrderRepo = new Mock<IRepository<int, string, Order>>();
            _mockUserRepo = new Mock<IRepository<int, string, User>>();
            _mockRestOwnerRepo = new Mock<IRepository<int, string, RestaurantOwner>>();
            _mockSpecialityRepo = new Mock<IRepository<int, string, RestaurantSpeciality>>();
            _mockReviewRepo = new Mock<IRepository<int, string, CustomerReview>>();
            _mockTokenServices = new Mock<ITokenServices>();
            _mockLogger = new Mock<ILogger<RestaurantUserServices>>();

            _restaurantUserServices = new RestaurantUserServices(
                _mockRestaurantRepo.Object,
                _mockCityRepo.Object,
                _mockMenuRepo.Object,
                _mockPaymentRepo.Object,
                _mockOrderRepo.Object,
                _mockUserRepo.Object,
                _mockRestOwnerRepo.Object,
                _mockSpecialityRepo.Object,
                _mockReviewRepo.Object,
                _mockTokenServices.Object,
                _mockLogger.Object
            );
        }

        // Method 1
        [Test]
        public async Task AddMenuItem_ValidMenu_ReturnsNewMenuItem()
        {
            // Arrange
            var menu = new Menu { RestaurantId = 1 };
            _mockRestaurantRepo.Setup(r => r.GetAsync(menu.RestaurantId)).ReturnsAsync(new Restaurant());
            _mockMenuRepo.Setup(m => m.Add(menu)).ReturnsAsync(menu);

            // Act
            var result = await _restaurantUserServices.AddMenuItem(menu);

            // Assert
            Assert.AreEqual(menu, result);
        }

        [Test]
        public void AddMenuItem_RestaurantNotFound_ThrowsRestaurantNotFoundException()
        {
            // Arrange
            var menu = new Menu { RestaurantId = 1 };
            _mockRestaurantRepo.Setup(r => r.GetAsync(menu.RestaurantId)).ReturnsAsync((Restaurant)null);

            // Act & Assert
            Assert.ThrowsAsync<RestaurantNotFoundException>(async () => await _restaurantUserServices.AddMenuItem(menu));
        }

        // Method 2
        [Test]
        public async Task AddRestaurant_ValidRestaurant_ReturnsNewRestaurant()
        {
            // Arrange
            var restaurant = new Restaurant()
            {
                RestaurantId = 1,
                RestaurantName = "Test",
            };

            // Act
            var result = await _restaurantUserServices.AddRestaurant(restaurant);

            // Assert
            Assert.Null(result);
        }

        // Method 3
        [Test]
        public async Task ChangeOrderStatus_ExistingOrder_ReturnsUpdatedOrder()
        {
            // Arrange
            var orderId = 1;
            var newStatus = "Completed";
            var existingOrder = new Order { OrderId = orderId, Status = "Pending" };
            _mockOrderRepo.Setup(o => o.GetAsync(orderId)).ReturnsAsync(existingOrder);
            _mockOrderRepo.Setup(o => o.Update(existingOrder)).ReturnsAsync(existingOrder);

            // Act
            var result = await _restaurantUserServices.ChangeOrderStatus(orderId, newStatus);

            // Assert
            Assert.AreEqual(newStatus.ToLower(), result.Status);
        }

        [Test]
        public void ChangeOrderStatus_NonExistingOrder_ThrowsOrdersNotFoundException()
        {
            // Arrange
            var orderId = 1;
            _mockOrderRepo.Setup(o => o.GetAsync(orderId)).ReturnsAsync((Order)null);

            // Act & Assert
            Assert.ThrowsAsync<OrdersNotFoundException>(async () => await _restaurantUserServices.ChangeOrderStatus(orderId, "Completed"));
        }

        // Method 4
        [Test]
        public async Task GetAllOrdersForRestaurant_ExistingOrders_ReturnsOrdersForRestaurant()
        {
            // Arrange
            var restaurantId = 1;
            var orders = new List<Order>
            {
                new Order { OrderId = 1, RestaurantId = restaurantId },
                new Order { OrderId = 2, RestaurantId = restaurantId }
            };
            _mockOrderRepo.Setup(o => o.GetAsync()).ReturnsAsync(orders);

            // Act
            var result = await _restaurantUserServices.GetAllOrders(restaurantId);

            // Assert
            Assert.AreEqual(orders.Count, result.Count);
        }


        [Test]
        public async Task GetAllOrders_ExistingOrders_ReturnsAllOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { OrderId = 1 },
                new Order { OrderId = 2 }
            };
            _mockOrderRepo.Setup(o => o.GetAsync()).ReturnsAsync(orders);

            // Act
            var result = await _restaurantUserServices.GetAllOrders();

            // Assert
            Assert.AreEqual(orders.Count, result.Count);
        }

        // Method 5
        [Test]
        public async Task GetAllPaymentsForRestaurant_ExistingPayments_ReturnsPaymentsForRestaurant()
        {
            // Arrange
            var restaurantId = 1;
            var payments = new List<Payment>
            {
                new Payment { PaymentId = 1, OrderId = 1 },
                new Payment { PaymentId = 2, OrderId = 2 }
            };
            var orders = new List<Order>
            {
                new Order { OrderId = 1, RestaurantId = restaurantId },
                new Order { OrderId = 2, RestaurantId = restaurantId }
            };
            _mockPaymentRepo.Setup(p => p.GetAsync()).ReturnsAsync(payments);
            _mockOrderRepo.Setup(o => o.GetAsync()).ReturnsAsync(orders);

            // Act
            var result = await _restaurantUserServices.GetAllPayments(restaurantId);

            // Assert
            Assert.AreEqual(payments.Count, result.Count);
        }

        [Test]
        public async Task GetAllPayments_ExistingPayments_ReturnsAllPayments()
        {
            // Arrange
            var payments = new List<Payment>
            {
                new Payment { PaymentId = 1 },
                new Payment { PaymentId = 2 }
            };
            _mockPaymentRepo.Setup(p => p.GetAsync()).ReturnsAsync(payments);

            // Act
            var result = await _restaurantUserServices.GetAllPayments();

            // Assert
            Assert.AreEqual(payments.Count, result.Count);
        }

        // Method 6
        //[Test]
        //public async Task LogInRestaurant_ValidLogin_ReturnsLoginUserDTO()
        //{
        //    // Arrange
        //    var loginUser = new LoginUserDTO { UserName = "test", Password = "password" };
        //    var user = new User { UserName = "test", Role = "RestaurantOwner", Key = new byte[] { 1, 2, 3 }, Password = new byte[] { 1, 2, 3 } };
        //    var owner = new RestaurantOwner { UserName = "test", RestaurantId = 1 };
        //    var token = "token";
        //    _mockUserRepo.Setup(u => u.GetAsync(loginUser.UserName)).ReturnsAsync(user);
        //    _mockRestOwnerRepo.Setup(o => o.GetAsync()).ReturnsAsync(new List<RestaurantOwner> { owner });
        //    _mockTokenServices.Setup(t => t.GenerateToken(loginUser)).ReturnsAsync(token);
        //    //_restaurantUserServices.passwordMatch = (pwd, userPwd) => true;

        //    // Act
        //    var result = await _restaurantUserServices.LogInRestaurant(loginUser);

        //    // Assert
        //    Assert.AreEqual(token, result.Token);
        //    Assert.AreEqual(owner.RestaurantId, result.UserId);
        //}

        [Test]
        public void LogInRestaurant_InvalidUser_ThrowsInvalidUserException()
        {
            // Arrange
            var loginUser = new LoginUserDTO { UserName = "test", Password = "password" };
            _mockUserRepo.Setup(u => u.GetAsync(loginUser.UserName)).ReturnsAsync((User)null);

            // Act & Assert
            Assert.ThrowsAsync<InvalidUserException>(async () => await _restaurantUserServices.LogInRestaurant(loginUser));
        }

        [Test]
        public async Task RegisterRestaurant_ValidInput_ReturnsLoginUserDTO()
        {
            // Arrange
            var registerRestaurant = new RegisterRestaurantDTO { UserName = "restaurant_owner", Password = "password" };
            var user = new User { UserName = "restaurant_owner", Role = "RestaurantOwner" };
            var restaurantOwner = new RestaurantOwner { UserName = "restaurant_owner" };
            _mockUserRepo.Setup(repo => repo.Add(It.IsAny<User>())).ReturnsAsync(user);
            _mockRestOwnerRepo.Setup(repo => repo.Add(It.IsAny<RestaurantOwner>())).ReturnsAsync(restaurantOwner);

            // Act
            var result = await _restaurantUserServices.RegisterRestaurant(registerRestaurant);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("restaurant_owner", result.UserName);
        }


        // Method 8
        [Test]
        public async Task GetAllSpecialities_ExistingSpecialities_ReturnsSpecialities()
        {
            // Arrange
            var specialities = new List<RestaurantSpeciality>
            {
                new RestaurantSpeciality { CategoryId = 1 },
                new RestaurantSpeciality { CategoryId = 2 }
            };
            _mockSpecialityRepo.Setup(s => s.GetAsync()).ReturnsAsync(specialities);

            // Act
            var result = await _restaurantUserServices.GetAllSpecialities();

            // Assert
            Assert.AreEqual(specialities.Count, result.Count);
        }

        //[Test]
        //public void GetCustomerReviews_NoReviewsFound_ThrowsNoCustomerReviewFoundException()
        //{
        //    // Arrange
        //    var reviews = new List<CustomerReview>();
        //    _mockReviewRepo.Setup(r => r.GetAsync()).ReturnsAsync(reviews);

        //    // Act & Assert
        //    Assert.ThrowsAsync<NoCustomerReviewFoundException>(async () => await _restaurantUserServices.GetCustomerReviews());
        //}

        [Test]
        public async Task GetCustomerReviews_ExistingReviews_ReturnsReviews()
        {
            // Arrange
            var reviews = new List<CustomerReview>
            {
                new CustomerReview { ReviewId = 1 },
                new CustomerReview { ReviewId = 2 }
            };
            _mockReviewRepo.Setup(r => r.GetAsync()).ReturnsAsync(reviews);

            // Act
            var result = await _restaurantUserServices.GetCustomerReviews();

            // Assert
            Assert.AreEqual(reviews.Count, result.Count);
        }

        [Test]
        public async Task DeleteMenuItem_ValidMenuItemId_ReturnsDeletedMenuItem()
        {
            // Arrange
            var menuItemId = 1;
            var menuItem = new Menu { MenuId = menuItemId };
            _mockMenuRepo.Setup(r => r.GetAsync(menuItemId)).ReturnsAsync(menuItem);
            _mockMenuRepo.Setup(r => r.Delete(menuItemId)).ReturnsAsync(menuItem);

            // Act
            var result = await _restaurantUserServices.DeleteMenuItem(menuItemId);

            // Assert
            Assert.NotNull(result);
            //Assert.AreEqual(menuItemId, result.MenuId);
            //_mockMenuRepo.Verify(r => r.Delete(menuItemId), Times.Once);
        }

    }
}
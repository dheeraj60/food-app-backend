using HotPot.Models;
using HotPot.Models.DTO;

namespace HotPot.Interfaces
{
    public interface IRestaurantOwnerServices : IRestaurantAdminServices
    {
        public Task<LoginUserDTO> RegisterRestaurant(RegisterRestaurantDTO registerRestaurant);
        public Task<LoginUserDTO> LogInRestaurant(LoginUserDTO loginRestaurant);
        public Task<Menu> AddMenuItem(Menu menu);
        public Task<List<Payment>> GetAllPayments(int RestaurantId);
        public Task<List<Order>> GetAllOrders(int RestaurantId);
        public Task<Order> ChangeOrderStatus(int orderId, string newStatus);
        public Task<Menu> DeleteMenuItem(int menuItemId);
    }
}
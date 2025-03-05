using HotPot.Models;
using HotPot.Models.DTO;

namespace HotPot.Interfaces
{
    public interface IDeliveryPartnerServices
    {
        public Task<DeliveryPartner> GetDeliveryPartnerDetails(int partnerId);
        public Task<DeliveryPartner> UpdateDeliveryPartnerDetails(DeliveryPartner deliveryPartner);
        public Task<Order> ChangeOrderStatus(int orderId);
        public Task<LoginUserDTO> RegisterDeliveryPartner(RegisterDeliveryPartnerDTO deliveryPartner);
        public Task<LoginUserDTO> LoginDeliveryPartner(LoginUserDTO loginUser);
        public Task<List<Order>> GetAllOrders(int partnerId);
    }
}
using HotPot.Models.DTO;
using HotPot.Models;

namespace HotPot.Mappers
{
    public class RegisterToRestaurant
    {
        RestaurantOwner restaurantOwner;
        public RegisterToRestaurant(RegisterRestaurantDTO registerRestaurant)
        {
            restaurantOwner = new RestaurantOwner();
            restaurantOwner.Name = registerRestaurant.Name;
            restaurantOwner.UserName = registerRestaurant.UserName;
            restaurantOwner.RestaurantId = registerRestaurant.RestaurantId;
        }
        public RestaurantOwner GetRestaurantOwner()
        {
            return restaurantOwner;
        }
    }
}
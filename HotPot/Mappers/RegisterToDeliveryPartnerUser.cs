using HotPot.Models;
using HotPot.Models.DTO;
using System.Security.Cryptography;
using System.Text;

namespace HotPot.Mappers
{
    public class RegisterToDeliveryPartnerUser
    {
        User newUser;

        public RegisterToDeliveryPartnerUser(RegisterDeliveryPartnerDTO deliveryPartner)
        {
            newUser = new User();
            newUser.UserName = deliveryPartner.UserName;
            newUser.Role = "DeliveryPartner";
            generatePassword(deliveryPartner.Password);
        }

        void generatePassword(string password)
        {
            HMACSHA512 hmac = new HMACSHA512();
            newUser.Key = hmac.Key;
            newUser.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public User GetUser()
        {
            return newUser;
        }
    }
}
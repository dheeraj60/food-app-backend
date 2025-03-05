using HotPot.Models;
using HotPot.Models.DTO;
using System.Security.Cryptography;
using System.Text;

namespace HotPot.Mappers
{
    public class RegisterToUser
    {
        User user;

        public RegisterToUser(RegisterCustomerDTO registerCustomer)
        {
            user = new User();
            user.UserName = registerCustomer.UserName;
            user.Role = registerCustomer.Role;
            generatePassword(registerCustomer.Password);
        }

        void generatePassword(string password)
        {
            HMACSHA512 hmac = new HMACSHA512();
            user.Key = hmac.Key;
            user.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public User getUser()
        {
            return user;
        }
    }
}
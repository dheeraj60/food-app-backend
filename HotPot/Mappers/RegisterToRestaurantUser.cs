﻿using HotPot.Models.DTO;
using HotPot.Models;
using System.Security.Cryptography;
using System.Text;

namespace HotPot.Mappers
{
    public class RegisterToRestaurantUser
    {
        User user;
        public RegisterToRestaurantUser(RegisterRestaurantDTO registerRestaurant)
        {
            user = new User();
            user.UserName = registerRestaurant.UserName;
            user.Role = registerRestaurant.Role;
            generatePassword(registerRestaurant.Password);
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
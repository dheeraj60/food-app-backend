﻿using HotPot.Models;
using HotPot.Models.DTO;

namespace HotPot.Mappers
{
    public class RegisterToCustomer
    {
        Customer customer;

        public RegisterToCustomer(RegisterCustomerDTO registerCustomer)
        {
            customer = new Customer();
            customer.Name = registerCustomer.Name;
            customer.Email = registerCustomer.Email;
            customer.Phone = registerCustomer.Phone;
            customer.UserName = registerCustomer.UserName;
        }

        public Customer GetCustomer()
        {
            return customer;
        }
    }
}
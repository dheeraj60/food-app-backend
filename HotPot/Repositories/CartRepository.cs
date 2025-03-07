﻿using HotPot.Contexts;
using HotPot.Interfaces;
using HotPot.Models;
using Microsoft.EntityFrameworkCore;

namespace HotPot.Repositories
{
    public class CartRepository : IRepository<int, string, Cart>
    {
        RequestTrackerContext _context;

        public CartRepository(RequestTrackerContext context)
        {
            _context = context;
        }

        public async Task<Cart> Add(Cart item)
        {
            _context.Add(item);
            _context.SaveChanges();
            return item;
        }

        public async Task<Cart> Delete(int key)
        {
            var cartItem = await GetAsync(key);
            _context.Carts.Remove(cartItem);
            _context.SaveChanges();
            return cartItem;
        }

        public async Task<Cart> GetAsync(int key)
        {
            var items = await GetAsync();
            var cartItem = items.FirstOrDefault(c => c.Id == key);
            return cartItem;
        }

        public async Task<List<Cart>> GetAsync()
        {
            var cartItems = _context.Carts.ToList();
            return cartItems;
        }

        public Task<Cart> GetAsync(string key)
        {
            throw new NotImplementedException();
        }

        public async Task<Cart> Update(Cart item)
        {
            _context.Entry<Cart>(item).State = EntityState.Modified;
            _context.SaveChanges();
            return item;
        }
    }
}
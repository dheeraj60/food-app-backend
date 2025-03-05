namespace HotPot.Models.DTO
{
    public class CartMenuDTO
    {
        public int CartId { get; set; }
        public int CustomerId { get; set; }
        public int RestaurantId { get; set; }
        public int MenuItemId { get; set; }
        public string? MenuTitle { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public string? MenuImage { get; set; }
        public string? RestaurantName { get; set; }
        public int RestaurantCityId { get; set; }
    }
}
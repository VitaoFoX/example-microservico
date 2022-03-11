using GeekShopping.Web.Models;

namespace GeekShopping.Web.Services
{
    public interface ICartService
    {
        Task<CartViewModel> FindCartByUserID(string userID, string token);
        Task<CartViewModel> AddItemToCart(CartViewModel cart, string token);
        Task<CartViewModel> UpdateCart(CartViewModel cart, string token);
        Task<bool> RemoveFromCart(long cartId, string token);


        Task<bool> ApplyCoupon(CartViewModel cart, string token);  
        Task<bool>RemoveCoupon(string userID, string token);  
        Task<bool> ClearCart(string userID, string token);  

        Task<CartViewModel> Checkout(CartHeaderViewModel cartHeader, string token);  

    }
}

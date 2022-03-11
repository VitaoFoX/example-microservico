using GeekShopping.Web.Models;

namespace GeekShopping.Web.Services
{
    public interface ICouponService
    {
        Task<CouponViewModel> GetCoupon(string code, string token);
       

    }
}

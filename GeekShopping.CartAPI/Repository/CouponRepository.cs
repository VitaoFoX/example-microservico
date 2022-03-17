using GeekShopping.CartAPI.Data.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace GeekShopping.CartAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly HttpClient _client;

        public CouponRepository(HttpClient client)
        {
            _client = client;
        }

        public async Task<CouponVO> GetCoupon(string couponCode, string token)
        {
            //Base path "api/v1/coupon" replace para tirar a parlavra Bearer
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("Bearer",""));
            var response = await _client.GetAsync($"api/v1/coupon/{couponCode}");
            var content = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK) return new CouponVO();

            return JsonSerializer.Deserialize<CouponVO>(content,
              new JsonSerializerOptions
              { PropertyNameCaseInsensitive = true });
        }
    }
}

using GeekShopping.OrderAPI.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.OrderAPI.Model
{
    [Table("order_header")]
    public class OrderHeader : BaseEntity
    {
        [Column("user_id")]
        public string UserId { get; set; }

        [Column("coupon_code")]
        public string? CouponCode { get; set; }
        public decimal PurchaseAmount { get; set; }

        public decimal DiscountAmount { get; set; }
        public string? FristName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateTime { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public string ExpiryMothYear { get; set; }

        public int CartTotalItens { get; set; }
        public IEnumerable<OrderDetailVO>? OrderDetails { get; set; }

    }
}

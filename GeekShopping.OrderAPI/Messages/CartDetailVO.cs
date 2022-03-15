using GeekShopping.OrderAPI.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.OrderAPI.Data.ValueObjects
{
   
    public class CartDetailVO
    {
        public long Id { get; set; }
        public long CartHeaderId { get; set; } 
        public long ProductId { get; set; }
        public virtual ProductVO Product { get; set; }
        public int Count { get; set; }
    }
}

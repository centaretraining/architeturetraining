using System;
using System.Collections.Generic;
using System.Linq;

namespace LegacyApp.Web.Domain.Ordering
{
    public class Cart
    {
        public Cart()
        {
            Items = new List<CartItem>();
            CreateDate = DateTime.UtcNow;
            LastUpdate = DateTime.UtcNow;
        }

        public int CartId { get; protected set; }

        public DateTime CreateDate { get; protected set; }

        public DateTime LastUpdate { get; protected set; }

        public virtual ICollection<CartItem> Items { get; protected set; }

        public CartItem AddItem(int productId)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
            {
                item = new CartItem(this, productId);
                item.Quantity = 1;
                Items.Add(item);
            }
            else
            {
                item.Quantity++;
            }

            LastUpdate = DateTime.UtcNow;

            return item;
        }
    }
}

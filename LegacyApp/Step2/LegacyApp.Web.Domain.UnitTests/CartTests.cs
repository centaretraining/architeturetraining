using System.Linq;
using LegacyApp.Web.Domain.Ordering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LegacyApp.Web.Domain.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void AddItem_should_add_item_with_quantity_of_one_if_product_is_not_in_cart()
        {
            var cart = new Cart();
            var item1 = cart.AddItem(1);
            var item2 = cart.AddItem(2);

            Assert.AreNotEqual(item1, item2);
            Assert.AreEqual(2, cart.Items.Count);
            Assert.AreEqual(item1, cart.Items.First());
            Assert.AreEqual(item2, cart.Items.Skip(1).First());
            Assert.AreEqual(item1.Quantity, 1);
            Assert.AreEqual(item2.Quantity, 1);
        }

        [TestMethod]
        public void AddItem_should_update_quantity_if_product_is_already_in_cart()
        {
            var cart = new Cart();
            var item1 = cart.AddItem(1);
            var item2 = cart.AddItem(1);

            Assert.AreEqual(1, cart.Items.Count);
            Assert.AreEqual(item1, cart.Items.First());
            Assert.AreEqual(item1, item2);
            Assert.AreEqual(item1.Quantity, 2);
        }
    }
}

using System;
using System.Configuration;
using System.Data.SqlClient;
using LegacyApp.Ordering.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LegacyApp.Ordering.Application.IntegrationTests
{
    [TestClass]
    public class CartServiceTests
    {
        [TestMethod]
        public void CreateCart_should_return_ID_of_new_cart()
        {
            var service = CreateCartService();
            var cartId = service.CreateCart();
            Assert.AreNotEqual(0, cartId);
        }

        [TestMethod]
        public void AddItemToCart_should_increment_quantity_if_product_is_already_in_cart()
        {
            var service = CreateCartService();
            var cartId = service.CreateCart();
            var productId = GetTestProductId();

            service.AddItemToCart(cartId, productId);
            service.AddItemToCart(cartId, productId);

            var qty = GetItemQuantity(cartId, productId);
            var cnt = GetItemCount(cartId, productId);

            Assert.AreEqual(2, qty);
            Assert.AreEqual(1, cnt);
        }

        [TestMethod]
        public void AddItemToCart_should_add_new_cart_item_with_quantity_of_one_if_product_is_not_in_cart()
        {
            var service = CreateCartService();
            var cartId = service.CreateCart();
            var productId = GetTestProductId();

            service.AddItemToCart(cartId, productId);

            var qty = GetItemQuantity(cartId, productId);
            var cnt = GetItemCount(cartId, productId);

            Assert.AreEqual(1, qty);
            Assert.AreEqual(1, cnt);
        }

        private CartService CreateCartService()
        {
            var dbContext = new LegacyAppDbContext();
            var uow = new UnitOfWork(dbContext);
            var service = new CartService(
                new CartRepository(dbContext),
                new GetCartQuery(dbContext),
                uow);
            return service;
        }

        private int GetTestProductId()
        {
            return ExecuteInt32("SELECT TOP 1 ProductId FROM dbo.Product").Value;
        }

        private int GetItemQuantity(int cartId, int productId)
        {
            return ExecuteInt32($"SELECT SUM(Quantity) FROM dbo.CartItem WHERE CartId = {cartId} AND ProductId = {productId}").Value;
        }

        private int GetItemCount(int cartId, int productId)
        {
            return ExecuteInt32($"SELECT COUNT(*) FROM dbo.CartItem WHERE CartId = {cartId} AND ProductId = {productId}").Value;
        }

        private int? ExecuteInt32(string sql)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
            {
                con.Open();

                using (var cmd = new SqlCommand(sql, con))
                {
                    var obj = cmd.ExecuteScalar();
                    return obj == null ? null : (int?) Convert.ToInt32(obj);
                }
            }
        }
    }
}

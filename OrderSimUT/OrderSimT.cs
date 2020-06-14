using NUnit.Framework;
using OrderSimulation.Config;
using OrderSimulation.Handler;
using OrderSimulation.Model;

using System.Collections.Generic;

namespace OrderSimUT
{
    public class OrderSimT
    {
        List<Order> _orders;
        OrderConfig _config;

        Order _order;

        [SetUp]
        public void Setup()
        {
            _orders = OrderConfig.LoadOrders("./config/orders.json");
            _config = OrderConfig.LoadConfig("./config/config.json");
        }

        [Test]
        public void TestOrder()
        {
            Assert.IsTrue(_orders.Count > 0);

            Assert.Pass();
        }


        [Test]
        public void PlaceToShelf()
        {
            var order = new Order
            {
                ID = "9012736d-777b-4f5b-a12d-982e302fefa1",
                Name = "Mixed Greens",
                ShelfType = ShelfType.Cold,
                ShelfLife = 252,
                DecayRate = (decimal)0.26
            };

            var handler = new KitchenHandler(null);
            handler.Process(order);

            Assert.Pass();
        }
    }
}
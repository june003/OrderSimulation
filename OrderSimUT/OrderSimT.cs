using NUnit.Framework;
using OrderSimulation.Config;
using OrderSimulation.Model;
using System.Collections.Generic;

namespace OrderSimUT
{
    public class OrderSimT
    {
        List<Order> _orders;
        OrderConfig _config;

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
    }
}
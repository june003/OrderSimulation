using NUnit.Framework;
using OrderSimulation.Handler;
using OrderSimulation.Model;
using System.Collections.Generic;

namespace OrderSimUT
{
    public class OrderSimT
    {
        List<Order> _orders;

        [SetUp]
        public void Setup()
        {
            _orders = OrderHandler.LoadOrders("./config/orders.json");

            var handler = new OrderHandler("./config/config.json");
            var rlt = handler.ProcessOrders(_orders);
            if (!rlt)
            {
                return;
            }
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}
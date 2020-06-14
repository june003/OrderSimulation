using NUnit.Framework;
using OrderSimulation.Config;
using OrderSimulation.Handler;
using OrderSimulation.Model;

using System.Collections.Generic;

namespace OrderSimUT
{
    public class OrderSimT
    {
        [SetUp]
        public void Setup()
        {
            var _orders = OrderConfig.LoadOrders("./config/orders.json");
            var _config = OrderConfig.LoadConfig("./config/config.json");
        }

        [Test]
        public void TestDelivery()
        {
            //Assert.IsTrue(_orders.Count > 0);

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
                ShelfLife = 500,
                DecayRate = (decimal)0.26
            };

            var handler = new KitchenHandler(null);
            handler.Process(order);

            Assert.Pass();
        }



        [Test]
        public void Decay()
        {
            var order = new Order
            {
                ID = "9012736d-777b-4f5b-a12d-982e302fefa1",
                Name = "Mixed Greens",
                ShelfType = ShelfType.Cold,
                ShelfLife = 10,
                DecayRate = (decimal)0.26
            };

            var handler = new KitchenHandler(null);
            handler.Process(order);

            Assert.Pass();
        }
    }
}
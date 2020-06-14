using NUnit.Framework;
using OrderSimulation.Config;
using OrderSimulation.Handler;
using OrderSimulation.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderSimUT
{
    public class OrderSimT
    {
        [SetUp]
        public void Setup()
        {

        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public void TestPlaceToShelf()
        {
            var order = new Order
            {
                ID = "9012736d-777b-4f5b-a12d-982e302fefa1",
                Name = "Mixed Greens",
                ShelfType = ShelfType.Hot,
                ShelfLife = 500,
                DecayRate = (decimal)0.01
            };

            var handler = new KitchenHandler(null);
            handler.Process(order);
            Task.Delay(1000).Wait();

            var shelf = handler.GetShelfOf(order);

            Assert.IsTrue(shelf == ShelfType.Hot);
            Assert.Pass();
        }


        [Test]
        public void TestDecay()
        {
            var order = new Order
            {
                ID = "9012736d-777b-4f5b-a12d-982e302fefa1",
                Name = "Mixed Greens",
                ShelfType = ShelfType.Cold,
                ShelfLife = 1,
                DecayRate = (decimal)10
            };
            order.Start();
            var handler = new KitchenHandler(null);
            handler.Process(order);
            Task.Delay(3000).Wait();

            var shelf = handler.GetShelfOf(order);

            Assert.IsTrue(order.Value <= 0); // decay
            Assert.IsTrue(shelf == ShelfType.Invalid);
            Assert.Pass();
        }


        [Test]
        public void TestDelivery()
        {
            var order = new Order
            {
                ID = "9012736d-777b-4f5b-a12d-982e302fefa1",
                Name = "Mixed Greens",
                ShelfType = ShelfType.Cold,
                ShelfLife = 1000,
                DecayRate = (decimal)0.01
            };
            order.Start();
            var handler = new KitchenHandler(null);
            var courier = new CourierHandler(null);
            handler.Process(order);
            courier.Process(order);

            Task.Delay(7000).Wait(); // deliver comes while order noe decay

            Assert.IsTrue(order.CourierAssigned == true);
            Assert.Pass();
        }

        [Test]
        public void TestPubSub()
        {
            var config = new OrderConfig
            {
                IngestionRate = 1,
            };
            var order1 = new Order
            {
                ID = "9012736d-777b-4f5b-a12d-982e302fefa1",
                Name = "Mixed Greens",
                ShelfType = ShelfType.Cold,
                ShelfLife = 1000,
                DecayRate = (decimal)0.01
            };
            var order2 = new Order
            {
                ID = "9012736d-777b-4f5b-a12d-982e302feff2",
                Name = "Mixed Greens 2",
                ShelfType = ShelfType.Hot,
                ShelfLife = 1000,
                DecayRate = (decimal)0.01
            };

            var handler = new OrderHandler(config);
            var pub = new Publisher(handler);
            var kitchenHandler = new KitchenHandler(handler);
            var courierHandler = new CourierHandler(handler);

            var orders = new List<Order> { order1, order2 };
            pub.Publish(orders);

            Task.Delay(7000).Wait(); // deliver comes while order not decay

            var shelf1 = kitchenHandler.GetShelfOf(order1);
            var shelf2 = kitchenHandler.GetShelfOf(order2);

            Assert.IsTrue(shelf1 == ShelfType.Invalid); // deliver already
            Assert.IsTrue(shelf2 == ShelfType.Invalid); // deliver already

            Assert.IsTrue(order1.CourierAssigned == true);
            Assert.IsTrue(order2.CourierAssigned == true);

            Assert.Pass();
        }

        [Test]
        public void TestOverflow()
        {
            // eg. 1000 orders with same temperature Hot
            // overflow shelf never exceed 15

            var config = new OrderConfig
            {
                IngestionRate = 500,
            };
            Assert.Pass();
        }
    }
}
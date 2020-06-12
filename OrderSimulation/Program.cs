using OrderSimulation.Model;
using System;
using System.Threading.Tasks;

namespace OrderSimulation
{
    class Program
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            Logger.Info("Start...");

            var orders = Kitchen.GetOrders();

            var orderHandler = new Kitchen();
            var ran = new Random();

            int ordersPerSec = 2;   //ingest 2 orders per sec
            foreach (var ord in orders)
            {
                orderHandler.ReceiveOrder(ord);

                Courier.Pickup(ord, ran.Next(2, 6));  // 2-6 seconds later, arrives

                Task.Delay(1000 / ordersPerSec).Wait();
            }

            Logger.Info("Done!");
        }
    }
}

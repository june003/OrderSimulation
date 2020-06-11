using System;

namespace OrderSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start...");

            var orderHandler = new OrderHandler();

            var oders = orderHandler.GetOrders();

            orderHandler.IngestOrder(oders);

            Console.WriteLine("Done!");
        }
    }
}

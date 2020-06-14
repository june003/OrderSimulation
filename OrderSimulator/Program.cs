//-----------------------------------------------------------------------------
// File Name   : Program
// Author      : junlei
// Date        : 6/13/2020 6:12:12 PM
// Description : 
// Version     : 1.0.0
// Updated     : 
//
//-----------------------------------------------------------------------------
using OrderSimulation.Config;
using OrderSimulation.Handler;
using System;

namespace OrderSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = OrderConfig.LoadConfig("./config/config.json");

            var handler = new OrderHandler(config);
            var pub = new Publisher(handler);
            var kitchenHandler = new KitchenHandler(handler);  // subscriber 1
            var courierHandler = new CourierHandler(handler);  // subscriber 2

            var orders = OrderConfig.LoadOrders("./config/orders.json");
            pub.Publish(orders);

            Console.ReadKey();
        }
    }
}

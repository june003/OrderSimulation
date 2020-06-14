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
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            Logger.Info("Start...");
            try
            {
                var config = OrderConfig.LoadConfig("./config/config.json");

                var handler = new OrderHandler(config);
                var pub = new Publisher(handler);
                var kitchenHandler = new KitchenHandler(handler);
                var courierHandler = new CourierHandler(handler);

                var orders = OrderConfig.LoadOrders("./config/orders.json");
                pub.Publish(orders);

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return;
            }
        }
    }
}

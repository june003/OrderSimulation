//-----------------------------------------------------------------------------
// File Name   : Kitchencs
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
                var orders = OrderHandler.LoadOrders("./config/orders.json");
                var config = OrderConfig.LoadConfig("./config/config.json");

                var eve = new EventAggregator(config);
                var pub = new Publisher(eve);
                var kitchenHandler = new KitchenHandler(eve);
                var courierHandler = new CourierHandler(eve);

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

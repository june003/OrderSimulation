//-----------------------------------------------------------------------------
// File Name   : Kitchencs
// Author      : junlei
// Date        : 6/13/2020 6:12:12 PM
// Description : 
// Version     : 1.0.0
// Updated     : 
//
//-----------------------------------------------------------------------------
using OrderSimulation.Handler;
using System;
using System.Threading.Tasks;

namespace OrderSimulation
{
    class Program
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        static  void  Main(string[] args)
        {
            Logger.Info("Start...");
            try
            {
                var orders = OrderHandler.LoadOrders("./config/orders.json");

                var handler = new OrderHandler("./config/config.json");
                var rlt =  handler.ProcessOrders(orders);
                if (!rlt)
                {
                    Logger.Error("Error happens when processing orders");
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return;
            }

            Logger.Info("Done!");
            Console.ReadKey();
        }
    }
}

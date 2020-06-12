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

                var handler = new OrderHandler();
                if (!handler.ProcessOrders(orders, out string result))
                {
                    Logger.Error(result);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return;
            }

            Logger.Info("Done!");
        }
    }
}

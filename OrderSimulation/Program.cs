using OrderSimulation.Handler;
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
            try
            {
                var orders = OrderHandler.LoadOrders("./config/orders.json");

                var handler = new OrderHandler();
                var rlt = handler.ProcessOrders(orders);
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
        }
    }
}

namespace OrderSimulation
{
    class Program
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            Logger.Info("Start...");

            var orderHandler = new OrderHandler();

            var oders = OrderHandler.GetOrders();

            orderHandler.IngestOrder(oders);

            Logger.Info("Done!");
        }
    }
}

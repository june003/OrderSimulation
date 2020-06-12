//-----------------------------------------------------------------------------
// File Name   : OrderHandler
// Author      : junlei
// Date        : 6/12/2020 3:43:05 PM
// Description : 
// Version     : 1.0.0      
// Updated     : 
//
//-----------------------------------------------------------------------------
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace OrderSimulation.Handler
{
    class OrderHandler
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private OrderConfig _orderConfig;
        private readonly Kitchen _kitchen = new Kitchen();
        public OrderHandler()
        {

        }

        internal bool ProcessOrders(List<Order> orders, out string result)
        {
            if (!LoadConfig("./config/config.json", out result))
            {
                result = "Failed to load config file.";
                return false;
            }

            foreach (var ord in orders)
            {
                ord.Start(); // order starts from here

                Task.Run(() => _kitchen.ReceiveOrder(ord)).ConfigureAwait(false); // new thread for kitchen

                Task.Run(() => Pickup(ord)).ConfigureAwait(false);  // new thread for Courier

                Task.Delay(1000 / _orderConfig.IngestionRate).Wait();
            }

            return true;
        }

        private void Pickup(Order order)
        {
            var courier = new Courier();
            courier.Pickup(order);
        }

        private bool LoadConfig(string path, out string result)
        {
            result = string.Empty;
            try
            {
                _orderConfig = JsonConvert.DeserializeObject<OrderConfig>(File.ReadAllText(path));

                if (_orderConfig.IngestionRate <= 0)
                {
                    result = "Please specify a valid ingestion rate.";
                    return false;
                }

                if (_orderConfig.IngestionRate > 1000)
                {
                    result = "Please specify a valid ingestion rate(cannot process to many orders).";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                result = $"Failed to load config file :{ex.Message}";
                return false;
            }
        }

        public static List<Order> LoadOrders(string orderPath)
        {
            try
            {
                return JsonConvert.DeserializeObject<List<Order>>(File.ReadAllText(orderPath));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

    }
}

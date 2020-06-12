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
using OrderSimulation.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace OrderSimulation
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
            result = string.Empty;
            if (!LoadConfig())
            {
                result = "Failed to load config file.";
                return false;
            }

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

            foreach (var ord in orders)
            {
                Task.Run(() => _kitchen.ReceiveOrder(ord)).ConfigureAwait(false); // new thread for kitchen

                Task.Run(() => Courier.Pickup(ord)).ConfigureAwait(false);  // new thread for Courier

                Task.Delay(1000 / _orderConfig.IngestionRate).Wait();
            }

            return true;
        }

        private bool LoadConfig()
        {
            try
            {
                _orderConfig = JsonConvert.DeserializeObject<OrderConfig>(File.ReadAllText("./config/config.json"));
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }

        public static List<Order> GetOrders(string orderPath = "./config/orders.json")
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

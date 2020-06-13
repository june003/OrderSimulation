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

        private static readonly Random _rand = new Random();

        public OrderHandler()
        {

        }

        internal bool ProcessOrders(List<Order> orders)
        {
            if (!string.IsNullOrEmpty(LoadConfig("./config/config.json")))
            {
                Logger.Error("Failed to load config file.");
                return false;
            }

            foreach (var ord in orders)
            {
                ord.Start(); // order starts from here

                Task.Run(() => ProcessOrders(ord)).ConfigureAwait(false); // new thread for kitchen
                Task.Run(() => AssignCourier(ord)).ConfigureAwait(false);  // new thread for Courier

                Task.Delay(1000 / _orderConfig.IngestionRate).Wait();
            }

            return true;
        }

        private void ProcessOrders(Order order)  // received and processed
        {
            _kitchen.ReceiveOrder(order);// new thread for kitchen
        }

        private void AssignCourier(Order order)
        {
            var delay = _rand.Next(2, 6);
            Logger.Info($"Courier comes {delay}' later");
            Task.Delay(delay * 1000).Wait(); // courier comes 2~6s later
            _kitchen.AssignCourier(order);
        }

        private string LoadConfig(string path)
        {
            try
            {
                Logger.Info("Loading configuration...");
                _orderConfig = JsonConvert.DeserializeObject<OrderConfig>(File.ReadAllText(path));

                if (_orderConfig.IngestionRate <= 0)
                {
                    return "Please specify a valid ingestion rate.";
                }

                if (_orderConfig.IngestionRate > 1000)
                {
                    return "Please specify a valid ingestion rate(cannot process to many orders).";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return $"Failed to load config file :{ex.Message}";
            }
        }

        public static List<Order> LoadOrders(string orderPath)
        {
            try
            {
                Logger.Info("Loading orders...");
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

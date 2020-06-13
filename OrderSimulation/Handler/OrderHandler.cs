//-----------------------------------------------------------------------------
// File Name   : OrderHandler
// Author      : junlei
// Date        : 6/13/2020 3:43:05 PM
// Description : 
// Version     : 1.0.0      
// Updated     : 
//
//-----------------------------------------------------------------------------
using Newtonsoft.Json;
using OrderSimulation.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace OrderSimulation.Handler
{
    public class OrderHandler
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly Random _rand = new Random();

        private readonly string _configPath;
        private OrderConfig _orderConfig;
        private readonly Kitchen _kitchen = new Kitchen();
        public OrderHandler(string configPath)
        {
            _configPath = configPath;
        }

        public bool ProcessOrders(List<Order> orders)
        {
            if (!LoadConfig(_configPath))
            {
                Logger.Error("Failed to load config file.");
                return false;
            }

            foreach (var ord in orders)
            {
                ord.Start(); // order is born

                ProcessOrder(ord);     // new thread for kitchen
                CallCourier(ord);  // new thread for Courier

                Task.Delay(1000 / _orderConfig.IngestionRate).Wait();
            }

            while (!_kitchen.IsEmpty())  // all orders processed
            {
                Task.Delay(100).Wait();
            }

            return true;
        }

        private void ProcessOrder(Order order)
        {
            Task.Run(() =>
            {
                _kitchen.OnOrderReady2Delivery += Kitchen_OrderReady2Delivery;
                _kitchen.ReceiveOrder(order);
            });
        }

        private void Kitchen_OrderReady2Delivery(Order order)
        {
            while (!order.CourierAssigned)  // waiting for courier 
            {
                Task.Delay(20).Wait();
            }

            if (order.IsLive)
            {
                _kitchen.Deliver(order);
            }
        }

        private void CallCourier(Order order)
        {
            Task.Run(() =>
            {
                var delay = _rand.Next(2, 7);
                Logger.Info($"Courier comes {delay}\" later."); // 2~6" later
                Task.Delay(delay * 1000).Wait();

                order.CourierAssigned = true;
            });
        }

        private bool LoadConfig(string path)
        {
            try
            {
                Logger.Info("Loading configuration...");
                _orderConfig = JsonConvert.DeserializeObject<OrderConfig>(File.ReadAllText(path));

                if (_orderConfig.IngestionRate <= 0)
                {
                    Logger.Error("Please specify a valid ingestion rate: (0, 1000].");
                    return false;
                }

                if (_orderConfig.IngestionRate > 1000)
                {
                    Logger.Error("Please specify a valid ingestion rate(cannot process to many orders): (0, 1000].");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to load config file :{ex.Message}");
                return false;
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

//-----------------------------------------------------------------------------
// File Name   : OrderHandler
// Author      : junlei
// Date        : 6/11/2020 6:26:18 PM
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
using System.Text;
using System.Threading.Tasks;

namespace OrderSimulation
{
    class OrderHandler
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public bool IngestOrder(List<Order> orders)
        {
            foreach (var order in orders)
            {
                HandleOrder(order);
                Task.Delay(500).Wait();
            }

            return true;
        }

        private void HandleOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public static List<Order> GetOrders(string path = "orders.json")
        {
            try
            {
                return JsonConvert.DeserializeObject<List<Order>>(File.ReadAllText(path));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }
    }
}

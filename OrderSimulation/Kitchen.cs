//-----------------------------------------------------------------------------
// File Name   : Kitchencs
// Author      : junlei
// Date        : 6/11/2020 6:20:32 PM
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

namespace OrderSimulation
{
    class Kitchen
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly Dictionary<Temperature, Shelf> _shelves = new Dictionary<Temperature, Shelf>();

        public Kitchen()
        {
            _shelves.Add(Temperature.Hot, new Shelf(10));
            _shelves.Add(Temperature.Cold, new Shelf(10));
            _shelves.Add(Temperature.Frozen, new Shelf(10));
            _shelves.Add(Temperature.Otehrs, new Shelf(15));
        }

        internal void ReceiveOrder(Order order)
        {
            while (!HandleOrder(order))
            {

            }

            Logger.Info($"Order {order.ID}:{order.Value} was processed.");
        }

        private bool HandleOrder(Order order)
        {
            if (!_shelves.ContainsKey(order.Temperature))
            {
                Logger.Warn($"Order temperature is not valid: {order.ID}:{order.Value}");
                return true;
            }

            var oriShelf = _shelves[order.Temperature];
            if (oriShelf.Place(order))
            {
                Logger.Info($"Placed order: {order.ID}:{order.Value}");
                return true;
            }

            var overflowShelf = _shelves[Temperature.Otehrs];
            if (overflowShelf.Place(order))
            {
                Logger.Info($"Placed order: {order.ID}:{order.Value}");
                return true;
            }

            return false;
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

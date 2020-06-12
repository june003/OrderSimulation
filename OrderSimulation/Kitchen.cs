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
using System.Threading.Tasks;

namespace OrderSimulation
{
    class Kitchen
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly Dictionary<Temperature, Shelf> _shelfDic = new Dictionary<Temperature, Shelf> {
            {Temperature.Hot,      new Shelf(10) },
            {Temperature.Cold,     new Shelf(10) },
            {Temperature.Frozen,   new Shelf(10) },
            {Temperature.Otehrs,   new Shelf(15) },
        };

        public Kitchen()
        {
        }

        internal void ReceiveOrder(Order order)
        {
            HandleOrder(order);

            Logger.Info($"Order {order.ID}:{order.Value} was processed.");
        }

        private bool HandleOrder(Order order)
        {
            if (!_shelfDic.ContainsKey(order.Temperature))
            {
                Logger.Warn($"Order temperature is not valid: {order.ID}:{order.Value}");
                return true;
            }

            var oriShelf = _shelfDic[order.Temperature];
            if (oriShelf.Place(order))
            {
                Logger.Info($"Placed order: {order.ID}:{order.Value}");
                return true;
            }

            var overflowShelf = _shelfDic[Temperature.Otehrs];
            if (overflowShelf.Place(order))
            {
                Logger.Info($"Placed order: {order.ID}:{order.Value}");
                return true;
            }

            return false;
        }

    }
}

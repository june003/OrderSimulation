//-----------------------------------------------------------------------------
// File Name   : Kitchencs
// Author      : junlei
// Date        : 6/11/2020 6:20:32 PM
// Description : 
// Version     : 1.0.0      
// Updated     : 
//
//-----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderSimulation.Handler
{
    class Kitchen
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly Dictionary<Temperature, Shelf> _shelfDic = new Dictionary<Temperature, Shelf> {
            {Temperature.Hot,    new Shelf("Hot",10) },
            {Temperature.Cold,   new Shelf("Cold", 10) },
            {Temperature.Frozen, new Shelf("Frozen", 10) },
            {Temperature.Otehrs, new Shelf("Overflow", 15) },
        };

        public Kitchen()
        {
        }

        internal void ReceiveOrder(Order order)
        {
            Logger.Info($"Order received: {order.Name}-{order.Value}.");
            order.OnDie += Order_OnDie;
            if (HandleOrder(order))
            {
                Logger.Info($"Order processed: {order.Name}-{order.Value}.");
            }
            else
            {
                Logger.Warn($"Order discarded: {order.Name}-{order.Value}.");
            }
        }

        private bool HandleOrder(Order order)
        {
            if (!_shelfDic.ContainsKey(order.Temperature))
            {
                Logger.Warn($"Order not valid: {order.Name}-{order.Temperature}-{order.ID}");
                return false;
            }

            if (_shelfDic[order.Temperature].Place(order))
            {
                Console.WriteLine("Shelves' info: \r\n" + GetShelvesInfo());
                return true;
            }

            //overflow shelf
            if (_shelfDic[Temperature.Otehrs].Place(order, true))
            {
                Console.WriteLine("Shelves' info: \r\n" + GetShelvesInfo());
                return true;
            }

            return false;
        }

        internal bool AssignCourier(Order order) // and delivery
        {
            if (order.Value <= 0) // order die
            {
                return false;
            }

            order.OnDie -= Order_OnDie;
            _shelfDic[order.Temperature].Remove(order);
            Logger.Info($"Order delivered: {order.Name}-{order.Value}");

            return true;
        }

        private void Order_OnDie(Order order)
        {
            Logger.Warn($"Order dies: {order.Name}-{order.Value}");
            _shelfDic[order.Temperature].Remove(order);
        }

        private string GetShelvesInfo()
        {
            var builder = new StringBuilder();
            foreach (var shf in _shelfDic.Values)
            {
                builder.AppendLine($"  {shf.GetInfo()}");
            }

            return builder.ToString();
        }
    }
}

//-----------------------------------------------------------------------------
// File Name   : CourierHandler
// Author      : junlei
// Date        : 6/13/2020 7:17:54 PM
// Description : 
// Version     : 1.0.0      
// Updated     : 
//
//-----------------------------------------------------------------------------
using OrderSimulation.Model;
using System;
using System.Threading.Tasks;

namespace OrderSimulation.Handler
{
    public class CourierHandler : ISubscriber
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private static readonly Random _rand = new Random();
        public CourierHandler(EventAggregator eventAggregator)
        {
            eventAggregator.Subscribe<Order>(ProcessOrder);
        }

        public void ProcessOrder(Order order)
        {
            Task.Run(() =>
            {
                var delay = _rand.Next(2, 7);
                Logger.Info($"Courier comes {delay}\" later for order {order.Name}-{order.Name}."); // 2~6" later                
                Task.Delay(delay * 1000).Wait();

                order.CourierAssigned = true;
            });

            //_eventAggregator.UnSbscribe(_myMessageToken);
        }
    }
}

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
    /// <summary>
    /// the simlified courier, which comes later(order is ready) to wait for the cooked orders
    /// </summary>
    public class CourierHandler : ISubscriber
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private static readonly Random _rand = new Random();
        public CourierHandler(OrderHandler orderHandler)
        {
            orderHandler?.Subscribe(this);
        }

        // create thread in the thread pool
        public async void Process(Order order)
        {
            await Task.Run(async () =>
            {
                var delay = _rand.Next(2, 7);
                Logger.Info($"Courier comes {delay}s later for order {order.Name}-{order.Value}.");
                await Task.Delay(delay * 1000);  // 2~6" later

                order.CourierAssigned = true;
            });
        }
    }
}

//-----------------------------------------------------------------------------
// File Name   : Publisher
// Author      : junlei
// Date        : 6/13/2020 5:47:45 PM
// Description : 
// Version     : 1.0.0      
// Updated     : 
//
//-----------------------------------------------------------------------------
using OrderSimulation.Model;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace OrderSimulation.Handler
{
    /// <summary>
    /// publisher: get orders....
    /// </summary>
    public class Publisher
    {
        private readonly OrderHandler _orderHandler;
        public Publisher(OrderHandler handler)
        {
            _orderHandler = handler;
        }

        public void Publish(List<Order> orders)
        {
            var ingestRate = _orderHandler.Config.IngestionRate;
            foreach (var ord in orders)
            {
                ord.Start();
                _orderHandler.Publish(ord);

                Task.Delay(1000 / ingestRate).Wait();
            }
        }
    }
}

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
    public class Publisher
    {
        private readonly EventAggregator _eventAggregator;
        public Publisher(EventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void Publish(List<Order> orders)
        {
            var ingestRate = _eventAggregator.Config.IngestionRate;
            foreach (var ord in orders)
            {
                ord.Start();
                _eventAggregator.Publish(ord);

                Task.Delay(1000 / ingestRate).Wait();
            }
        }
    }
}

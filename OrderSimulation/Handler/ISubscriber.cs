//-----------------------------------------------------------------------------
// File Name   : Subscriber
// Author      : junlei
// Date        : 6/13/2020 5:48:31 PM
// Description : 
// Version     : 1.0.0      
// Updated     : 
//
//-----------------------------------------------------------------------------
using OrderSimulation.Model;

namespace OrderSimulation.Handler
{
    public interface ISubscriber
    {
        void ProcessOrder(Order order);
    }
}

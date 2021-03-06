﻿//-----------------------------------------------------------------------------
// File Name   : ISubscriber
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
    /// <summary>
    /// interface: may has many subscibers to handle the orders
    /// </summary>
    public interface ISubscriber
    {
        void Process(Order order);
    }
}

//-----------------------------------------------------------------------------
// File Name   : Courier
// Author      : junlei
// Date        : 6/11/2020 6:21:35 PM
// Description : 
// Version     : 1.0.0      
// Updated     : 
//
//-----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderSimulation.Model
{
    class Courier
    {
        public bool Pickup(Order order)
        {
            return Deliver(order);
        }

        private bool Deliver(Order order)
        {
            return true;
        }
    }
}

﻿//-----------------------------------------------------------------------------
// File Name   : Shelf
// Author      : junlei
// Date        : 6/11/2020 6:21:06 PM
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
    public class Shelf
    {
        private int _capacity;

        public Shelf(int capacity)
        {
            _capacity = capacity;
        }

        internal bool Place(Order order)
        {
            throw new NotImplementedException();
        }

        internal Order Pickup()
        {
            return null;
        }
    }
}

﻿//-----------------------------------------------------------------------------
// File Name   : Order
// Author      : junlei
// Date        : 6/13/2020 6:20:51 PM
// Description : 
// Version     : 1.0.0      
// Updated     : 
//
//-----------------------------------------------------------------------------
using Newtonsoft.Json;
using System;
using System.Timers;

namespace OrderSimulation.Model
{
    /// <summary>
    /// order information
    /// </summary>
    public class Order
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("shelfLife")]
        public int ShelfLife { get; set; }
        [JsonProperty("decayRate")]
        public decimal DecayRate { get; set; }

        [JsonProperty("temp")]
        public ShelfType ShelfType { get; set; }

        /// <summary>
        /// if the courier is ready
        /// </summary>
        public bool CourierAssigned { get; set; } = false;

        /// <summary>
        /// order deteriorates with time, and Value becomes less
        /// </summary>
        public decimal Value
        {
            get
            {
                if (ShelfLife <= 0)
                {
                    return 0;
                }

                return (ShelfLife - DecayRate * _age * _shelfDecayModifier) / ShelfLife;
            }
        }

        private int _shelfDecayModifier = 0;
        public void SetShelfDecayModifier(ShelfType shelfType)
        {
            _shelfDecayModifier = (shelfType == ShelfType.Cold
             || shelfType == ShelfType.Hot
             || shelfType == ShelfType.Frozen) ? 1 : 2;
        }

        public event Action<Order, bool> OnFinish;  // decay? delivered? or discarded randomly?

        private int _age = 0; // by seconds
        private readonly Timer _growTimer = new Timer();
        public void Start()  // order is born an starts to grow
        {
            _growTimer.Elapsed += GrowUp;
            _growTimer.Interval = 1000;
            _growTimer.Start();
        }

        private void GrowUp(object sender, ElapsedEventArgs e)
        {
            ++_age;
            if (decimal.Compare(Value, 0) <= 0) // decayed
            {
                _growTimer.Stop();
                _growTimer.Elapsed -= GrowUp;

                OnFinish?.Invoke(this, false);
            }
            else if (CourierAssigned)  // courier comes for delivery
            {
                _growTimer.Stop();
                _growTimer.Elapsed -= GrowUp;

                OnFinish?.Invoke(this, true);
            }
        }
    }
}

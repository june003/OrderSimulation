//-----------------------------------------------------------------------------
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

        private ShelfType _shelfType;
        [JsonProperty("temp")]
        public ShelfType ShelfType
        {
            get { return _shelfType; }
            set
            {
                _shelfType = value;
                _shelfDecayModifier = (_shelfType == ShelfType.Cold
                    || ShelfType == ShelfType.Hot
                    || ShelfType == ShelfType.Frozen) ? 1 : 2;
            }
        }

        public bool CourierAssigned { get; set; } = false;

        private int _shelfDecayModifier;

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

        public event Action<Order, bool> OnFinish;

        private int _age = 0; // by seconds
        private readonly Timer _growTimer = new Timer();
        internal void Start()  // order is born an starts to grow
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
            else if (CourierAssigned)  // delivered
            {
                _growTimer.Stop();
                _growTimer.Elapsed -= GrowUp;

                OnFinish?.Invoke(this, true);
            }
        }
    }
}

//-----------------------------------------------------------------------------
// File Name   : Order
// Author      : junlei
// Date        : 6/11/2020 6:20:51 PM
// Description : 
// Version     : 1.0.0      
// Updated     : 
//
//-----------------------------------------------------------------------------
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Runtime.Serialization;
using System.Timers;

namespace OrderSimulation.Handler
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Temperature
    {
        [EnumMember(Value = "others")]
        Otehrs = 0,
        [EnumMember(Value = "hot")]
        Hot = 1,
        [EnumMember(Value = "cold")]
        Cold = 2,
        [EnumMember(Value = "frozen")]
        Frozen = 3,
    };

    public class Order
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("temp")]
        public Temperature Temperature { get; set; }
        [JsonProperty("shelfLife")]
        public int ShelfLife { get; set; }
        [JsonProperty("decayRate")]
        public decimal DecayRate { get; set; }

        public decimal Value
        {
            get
            {
                if (ShelfLife <= 0)
                {
                    return 0;
                }

                return (ShelfLife - DecayRate * _age * ShelfDecayModifier) / ShelfLife;
            }
        }

        private int ShelfDecayModifier => (Temperature == Temperature.Cold || Temperature == Temperature.Hot
                                        || Temperature == Temperature.Frozen) ? 1 : 2;

        public event Action<Order> OnDie;

        private int _age = 0; // by seconds
        internal void Start()
        {
            var timer = new Timer();
            timer.Elapsed += GrowUp;
            timer.Interval = 1000;
            timer.Start();
        }

        private void GrowUp(object sender, ElapsedEventArgs e)
        {
            ++_age;
            if (Value <= 0) // die
            {
                OnDie?.Invoke(this);
            }
        }
    }
}

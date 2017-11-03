using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace samochod
{
    class Car
    {
        public String Date { get; set; }
        public float FuelLiters { get; set; }
        public float FuelPrice { get; set; }
        public float AdditionalFuelLiters { get; set; }
        public float AdditionalFuelPrice { get; set; }
        public float Kilometers { get; set; }

        public Car(string d,float a,float b,float c,float e,float f)
        {
            Date = d;
            FuelLiters = a;
            FuelPrice = b;
            AdditionalFuelLiters = c;
            AdditionalFuelPrice = e;
            Kilometers = f;
        }
        public Car()
        {

        }
    }
}

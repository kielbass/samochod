using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace samochod
{
    class Year
    {
        public string Name { get; set; }
        public ObservableCollection<Month> months { get; set; }
        public int NameInt {
            get
            {
                return _nameInt;
            } private set
            {

            }
        }
        private int _nameInt;

        public Year(string s)
        {
            months = new ObservableCollection<Month>();
            Name = s;
            for (int i = 0; i < 12; i++)
            {
                months.Add(new Month(i));
            }
            int.TryParse(Name,out _nameInt);
        }
        public override string ToString()
        {
            return Name;
        }
    }
    class Month
    {
        public int Name { get;set;}
        public ObservableCollection<Car> cars { get; private set; }

        /// <summary>
        /// Średnie spalanie benzyny na 100km
        /// </summary>
        public float AverageFuel { get; private set; }
        /// <summary>
        /// Średnie spalanie LPG na 100km
        /// </summary>
        public float AverageAdditionalFuel { get; private set; }
        /// <summary>
        /// Suma kilometrów przejechanych
        /// </summary>
        public float TotalKilometers { get; private set; }

        /// <summary>
        /// Średni koszt benzyny na 100km
        /// </summary>
        public float AverageFuelPrice { get; private set; }
        /// <summary>
        /// Średni koszt LPG na 100km
        /// </summary>
        public float AverageAdditionalFuelPrice { get; private set; }
        public void Add(Car c)
        {
            TotalKilometers =0f;
            AverageAdditionalFuel = 0f;
            AverageAdditionalFuelPrice = 0f;
            AverageFuel = 0f;
            AverageFuelPrice = 0f;
            cars.Add(c);
            foreach(Car car in cars)
            {
                TotalKilometers += car.Kilometers;
                AverageAdditionalFuel += car.AdditionalFuelLiters;
                AverageAdditionalFuelPrice += car.AdditionalFuelPrice;
                AverageFuel += car.FuelLiters;
                AverageFuelPrice += car.FuelPrice;
            }
            AverageAdditionalFuel = (AverageAdditionalFuel / TotalKilometers) * 100;
            AverageAdditionalFuelPrice = (AverageAdditionalFuelPrice / TotalKilometers) *100;
            AverageFuel = (AverageFuel / TotalKilometers)*100;
            AverageFuelPrice = (AverageFuelPrice / TotalKilometers)*100;
        }
        public Month(int s)
        {
            cars = new ObservableCollection<Car>();
            Name = s;
            TotalKilometers =0f;
            AverageAdditionalFuel = 0f;
            AverageAdditionalFuelPrice = 0f;
            AverageFuel = 0f;
            AverageFuelPrice = 0f;
        }
        public Month()
        { }
        public override string ToString()
        {
            string s = "";
            switch (Name)
            {
                case 0:
                    {
                        s = "Styczeń";
                        break;
                    }
                case 1:
                    {
                        s = "Luty";
                        break;
                    }
                case 2:
                    {
                        s = "Marzec";
                        break;
                    }
                case 3:
                    {
                        s = "Kwiecień";
                        break;
                    }
                case 4:
                    {
                        s = "Maj";
                        break;
                    }
                case 5:
                    {
                        s = "Czerwiec";
                        break;
                    }
                case 6:
                    {
                        s = "Lipiec";
                        break;
                    }
                case 7:
                    {
                        s = "Sierpień";
                        break;
                    }
                case 8:
                    {
                        s = "Wrzesień";
                        break;
                    }
                case 9:
                    {
                        s = "Paździenik";
                        break;
                    }
                case 10:
                    {
                        s = "Listopad";
                        break;
                    }
                case 11:
                    {
                        s = "Grudzień";
                        break;
                    }
            }
            s += "(";
            s += cars.Count.ToString();
            s += ")";
            return s;
        }
    }
}

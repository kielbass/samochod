using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Linq;


namespace samochod
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Car> tab = new List<Car>();
        private string filePath = @"..\..\Data\car.xml";
        private XElement element;
        private ObservableCollection<Year> years = new ObservableCollection<Year>();

        public MainWindow()
        {
            InitializeComponent();
            InitializeCollections();
        }
        private void InitializeCollections()
        {
            if(File.Exists(filePath))
            {
                if (new FileInfo(filePath) != null)
                {
                    element = XElement.Load(filePath);
                }
                else
                {
                    CreateXml();
                }
            }
            else
            {
                
                CreateXml();
            }
            dataGrid.DataContext= element;
            cmbYears.ItemsSource = years;
        }
        private void CreateXml()
        {

            File.Create(filePath).Dispose();

            element = new XElement("Cars",
                new XElement("Car",
                new XElement("Date", "06.01.2009"),
                new XElement("FuelLiters", 1),
                new XElement("FuelPrice", 1),
                new XElement("AdditionalFuelLiters", 1),
                new XElement("AdditionalFuelPrice", 1),
                new XElement("Kilometers", 1)));
            element.Save(filePath);

        }
        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
                float a, b, c, f, g;
                bool ba, bb, bc, bd, be, bf;
                ba = float.TryParse(txtBenzL.Text, out a);
                bb = float.TryParse(txtBenzZ.Text, out b);
                bc = float.TryParse(txtLpgL.Text, out c);
                bd = float.TryParse(txtLpgZ.Text, out f);
                bf = datePicker.SelectedDate != null;
                be = float.TryParse(txtKm.Text, out g);
            if (ba && bb && bc && bd && be && bf)
            {
                DateTime d = (DateTime)datePicker.SelectedDate;
                element.Add(new XElement("Car",
                new XElement("Date", d.ToShortDateString()),
                new XElement("FuelLiters", a),
                new XElement("FuelPrice", b),
                new XElement("AdditionalFuelLiters", c),
                new XElement("AdditionalFuelPrice", f),
                new XElement("Kilometers", g)));
            }
            else
            {
                element.Add(new XElement("Car",
                new XElement("Date", "06.01.2009"),
                new XElement("FuelLiters", 1),
                new XElement("FuelPrice", 1),
                new XElement("AdditionalFuelLiters", 1),
                new XElement("AdditionalFuelPrice", 1),
                new XElement("Kilometers", 1)));
            }
            CollectionViewSource.GetDefaultView(dataGrid.ItemsSource).Refresh();
        }


        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                
                float a, b, c, f,g;
                bool ba, bb, bc, bd, be, bf;
                ba = float.TryParse(txtBenzL.Text, out a);
                bb = float.TryParse(txtBenzZ.Text, out b);
                bc = float.TryParse(txtLpgL.Text, out c);
                bd = float.TryParse(txtLpgZ.Text, out f);
                bf = datePicker.SelectedDate != null;
                be = float.TryParse(txtKm.Text, out g);
                if (ba && bb && bc && bd && be && bf)
                {
                    XElement temp = (XElement)dataGrid.SelectedItem;

                    DateTime d = (DateTime)datePicker.SelectedDate;
                    temp.SetElementValue("Date", d.ToShortDateString());
                    temp.SetElementValue("FuelPrice", b.ToString());
                    temp.SetElementValue("FuelLiters", a.ToString());
                    temp.SetElementValue("AdditionalFuelLiters", c.ToString());
                    temp.SetElementValue("AdditionalFuelPrice", f.ToString());
                    temp.SetElementValue("Kilometers", g.ToString());

                    CollectionViewSource.GetDefaultView(dataGrid.ItemsSource).Refresh();

                }
                else
                {
                    MessageBox.Show("WPISZ POPRAWNE DANE! KUR ZAPIAŁ!");
                    return;
                }
                


            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if(dataGrid.SelectedItem !=null)
            {
                XElement temp = (XElement)dataGrid.SelectedItem;
                MessageBoxResult m = MessageBox.Show("Czy na pewno wykasować pozycję z dnia " + temp.Element("Date").Value + " ?", "Pytanie", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (m == MessageBoxResult.Yes)
                {
                    temp.Remove();
                }
            }
        }
        private void plikExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void saveXML_Click(object sender, RoutedEventArgs e)
        {
            element.Save(filePath);
        }
        private void TabStatsInitiate()
        {
            tab.Clear();
            years.Clear();
            //z Xelemeny do listy
            tab = element.Elements("Car").Select(sv => new Car()
            {
                Date = (string)sv.Element("Date"),
                FuelLiters = (float)sv.Element("FuelLiters"),
                FuelPrice = (float)sv.Element("FuelPrice"),
                AdditionalFuelLiters = (float)sv.Element("AdditionalFuelLiters"),
                AdditionalFuelPrice = (float)sv.Element("AdditionalFuelPrice"),
                Kilometers = (float)sv.Element("Kilometers"),
            }).ToList();
            //Tworze liste years 
            //przeszukuje aby stworzyc nowe jesli nie istnieja
            //na koncu dodaje do odpowiednich miesiecy
            foreach (Car c in tab)
            {
                DateTime d = DateTime.Parse(c.Date);
                string y = d.Year.ToString();
                string m = d.Month.ToString();
                int.TryParse(m, out int k);
                k -= 1;//bo 12 miesiac jest 11 w kolekcji
                bool found = false;
                if(years.Count!=0)
                {
                    for(int i=0;i<years.Count;i++)
                    {
                        if (y == years[i].Name)
                        {
                            found = true;
                            years[i].months[k].Add(c);
                            break;
                        }
                    }
                    if(!found)
                    {
                        int j = years.Count;
                        years.Add(new Year(y));
                        years[j].months[k].Add(c);
                    }
                }
                else
                {
                    years.Add(new Year(y));
                    years[0].months[k].Add(c);
                }
                
            }
            //brzydko, może i mozna lepiej, ale działa ;)
            ObservableCollection<Year> yeye = new ObservableCollection<Year>(years.OrderBy(Year => Year.NameInt));
            years.Clear();
            foreach (Year y in yeye)
            {
                years.Add(y);
            }
        }

        private void tabStats_Selected(object sender, RoutedEventArgs e)
        {
            TabStatsInitiate();

        }

        private void cmbYears_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Year temp = (Year)cmbYears.SelectedItem;
            if(temp!=null)
            {
                cmbMonths.ItemsSource = temp.months;
            }
            
        }

        private void cmbMonths_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cmbMonths.SelectedItem!=null)
            {
                Month temp = (Month)cmbMonths.SelectedItem;
                if (temp != null)
                {
                    string s = temp.AverageFuel.ToString("0.00");
                    s += " [l/km]";
                    txtAvarageFuel.Text = s;
                    s = temp.AverageAdditionalFuel.ToString("0.00");
                    s += " [l/km]";
                    txtAvarageAdditionalFuel.Text = s;

                    s = temp.AverageFuelPrice.ToString("0.00");
                    s += " [zł/km]";
                    txtAvarageFuelPrice.Text = s;
                    s = temp.AverageAdditionalFuelPrice.ToString("0.00");
                    s += " [zł/km]";
                    txtAvarageAdditionalFuelPrice.Text = s;

                    s = temp.TotalKilometers.ToString("0.00");
                    s += " [km]";
                    txtTotalKilometers.Text = s;
                }
                Month prev = new Month();
                if (temp.Name != 0)
                {
                    Year y = (Year)cmbYears.SelectedItem;
                    prev = y.months[temp.Name - 1];
                    if (prev.cars.Count == 0)
                        prev = null;
                }
                else
                {
                    Year y = (Year)cmbYears.SelectedItem;
                    int.TryParse(y.Name, out int a);
                    a--;
                    foreach (Year ye in years)
                    {
                        if (a.ToString() == ye.Name)
                        {
                            prev = ye.months[11];
                            break;
                        }
                        else
                        {
                            prev = null;
                        }
                    }
                }
                if (prev != null)
                {
                    //srednia paliwa litry
                    float p = prev.AverageFuel;
                    float change = ((p - temp.AverageFuel)/p) * 100;
                    string s = change.ToString("0.00");
                    s += " %";
                    
                    if (change >= 0)
                    {
                        txtFuelChange.Foreground = Brushes.Green;
                        string t = "-";
                        t += s;
                        s = t;
                    }
                    else
                    {
                        txtFuelChange.Foreground = Brushes.Red;
                        s=s.Replace('-', '+');
                    }
                    txtFuelChange.Text = s;
                    //srednia litra gazu
                    p = prev.AverageAdditionalFuel;
                    change = ((p - temp.AverageAdditionalFuel) / p) * 100;
                    s = change.ToString("0.00");
                    s += " %";

                    if (change >= 0)
                    {
                        txtAdditionalFuelChange.Foreground = Brushes.Green;
                        string t = "-";
                        t += s;
                        s = t;
                    }
                    else
                    {
                        txtAdditionalFuelChange.Foreground = Brushes.Red;
                        s = s.Replace('-', '+');
                    }
                    txtAdditionalFuelChange.Text = s;
                    //srednia kosztu benzynowego
                    p = prev.AverageFuelPrice;
                    change = ((p - temp.AverageFuelPrice) / p) * 100;
                    s = change.ToString("0.00");
                    s += " %";

                    if (change >= 0)
                    {
                        txtFuelChangePrice.Foreground = Brushes.Green;
                        string t = "-";
                        t += s;
                        s = t;
                    }
                    else
                    {
                        txtFuelChangePrice.Foreground = Brushes.Red;
                        s = s.Replace('-', '+');
                    }
                    txtFuelChangePrice.Text = s;
                    //srednia kosztu LPG
                    p = prev.AverageAdditionalFuelPrice;
                    change = ((p - temp.AverageAdditionalFuelPrice) / p) * 100;
                    s = change.ToString("0.00");
                    s += " %";

                    if (change >= 0)
                    {
                        txtAdditionalFuelChangePrice.Foreground = Brushes.Green;
                        string t = "-";
                        t += s;
                        s = t;
                    }
                    else
                    {
                        txtAdditionalFuelChangePrice.Foreground = Brushes.Red;
                        s = s.Replace('-', '+');
                    }
                    txtAdditionalFuelChangePrice.Text = s;
                    //kilometrazu
                    p = prev.TotalKilometers;
                    change = ((p - temp.TotalKilometers) / p) * 100;
                    s = change.ToString("0.00");
                    s += " %";

                    if (change <= 0)
                    {
                        txtTotalKilometersChange.Foreground = Brushes.Green;
                        s = s.Replace('-', '+');
                    }
                    else
                    {
                        txtTotalKilometersChange.Foreground = Brushes.Red;
                        string t = "-";
                        t += s;
                        s = t;
                    }
                    txtTotalKilometersChange.Text = s;
                }
                else
                {
                    string s = "NaN";
                    txtFuelChange.Text = s;
                    txtFuelChange.Foreground = Brushes.Green;
                    txtAdditionalFuelChange.Text = s;
                    txtAdditionalFuelChange.Foreground = Brushes.Green;
                    txtFuelChangePrice.Text = s;
                    txtFuelChangePrice.Foreground = Brushes.Green;
                    txtAdditionalFuelChangePrice.Text = s;
                    txtAdditionalFuelChangePrice.Foreground = Brushes.Green;
                    txtTotalKilometersChange.Text = s;
                    txtTotalKilometersChange.Foreground = Brushes.Green;
                }
            }
        }

        private void tabEdit_Selected(object sender, RoutedEventArgs e)
        {
            cmbMonths.SelectedItem = null;
            cmbYears.SelectedItem = null;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            string s = "Aplikacja może nie najpiękniejsza ale robiłem ją dla siebie :) \n\n\n\n Pytania itp (albo oferty pracy:) ) patryk.atlas@hotmail.com ";
            MessageBox.Show(s, "O mnie");
        }
    }
}

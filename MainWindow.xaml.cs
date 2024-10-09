﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CafeBillingSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<ItemFromMenu> itemsFromMenu = new List<ItemFromMenu>
        {
            new ItemFromMenu("Latte", 5.50m, "Drink"),
            new ItemFromMenu("Espresso", 4m, "Drink"),
            new ItemFromMenu("Mocha", 7.90m, "Drink"),
            new ItemFromMenu("Cappucino", 8m, "Drink"),
            new ItemFromMenu("Cold Coffee", 10.50m, "Drink"),
            new ItemFromMenu("Coffee Cake", 15m, "Cake"),
            new ItemFromMenu("Red Valvet Cake", 18m, "Cake"),
            new ItemFromMenu("Cheese Cake", 20.49m, "Cake"),
            new ItemFromMenu("Rainbow Cake", 20m, "Cake")
        };

        private List<ItemFromMenu> orderItems = new List<ItemFromMenu>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            orderItems.Clear();
            LatteCheckBox.IsChecked = false;
            EspressoCheckBox.IsChecked = false;
            MochaCheckBox.IsChecked = false;
            CappucinoCheckBox.IsChecked = false;
            ColdCoffeeCheckBox.IsChecked = false;
            CoffeeCakeCheckBox.IsChecked = false;
            RedValvetCakeCheckBox.IsChecked = false;
            CheeseCakeCheckBox.IsChecked = false;
            RainbowCakeCheckBox.IsChecked = false;
            CostOfCakesTextBox.Text = "";
            CostOfDrinksTextBox.Text = "";
            ServiceChargeTextBox.Text = "";
            TotalTextBox.Text = "";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            orderItems.Clear();

            foreach (var checkBox in FindVisualChildren<CheckBox>(this))
                if (checkBox.IsChecked == true)
                    orderItems.Add(itemsFromMenu.FirstOrDefault(x => x.Name == checkBox.Content.ToString()));

            UpdatePrices();
            UpdateReceipt();
        }

        private void UpdatePrices()
        {
            decimal costOfDrinks = decimal.Zero;
            decimal costOfCakes = decimal.Zero;
            decimal serviceCharge = decimal.Zero;
            decimal total = decimal.Zero;

            foreach (var orderItem in orderItems)
            {
                if(orderItem.Type == "Drink")
                    costOfDrinks += orderItem.Price;
                else if(orderItem.Type == "Cake")
                    costOfCakes += orderItem.Price;
            }

            serviceCharge = (costOfDrinks + costOfCakes) * 0.05m;
            total = costOfDrinks + costOfCakes + serviceCharge;

            CostOfDrinksTextBox.Text = costOfDrinks.ToString();
            CostOfCakesTextBox.Text = costOfCakes.ToString();
            ServiceChargeTextBox.Text = serviceCharge.ToString();
            TotalTextBox.Text = total.ToString();
        }

        private void UpdateReceipt()
        {
            string textInReceipt = "Cafe & Cakes INC."
                + Environment.NewLine
                + "Item | Quantity | Price"
                + Environment.NewLine;

            foreach (var orderItem in orderItems)
                textInReceipt += $"{orderItem.Name} | 1 | {orderItem.Price}" + Environment.NewLine;

            ReceiptTextBox.Text = textInReceipt;
        }

        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private StackPanel FindParentStackPanel(DependencyObject child)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            while (parent != null && !(parent is StackPanel))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as StackPanel;
        }
    }
}
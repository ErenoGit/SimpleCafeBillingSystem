using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Printing;

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
        private DispatcherTimer timeCount;

        public MainWindow()
        {
            InitializeComponent();

            timeCount = new DispatcherTimer();
            timeCount.Interval = TimeSpan.FromSeconds(1);
            timeCount.Tick += TimerCount;
            timeCount.Start();

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

            serviceCharge = Decimal.Round(((costOfDrinks + costOfCakes) * 0.05m),2);
            total = Decimal.Round((costOfDrinks + costOfCakes + serviceCharge), 2);

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
                textInReceipt += $"{orderItem.Name} | 1 | {Decimal.Round(orderItem.Price, 2)}" + Environment.NewLine;

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

        private void ReceiptTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void DateTimeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }
        private void TimerCount(object sender, EventArgs e)
        {
            DateTimeBox.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if(printDialog.ShowDialog() == true)
            {
                FlowDocument document = new FlowDocument();
                Paragraph paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run($"Current time: {DateTimeBox.Text}\n"));
                paragraph.Inlines.Add(new Run($"Your receipt:\n"));
                paragraph.Inlines.Add(new Run(ReceiptTextBox.Text));
                paragraph.Inlines.Add(new Run($"\nTotal price:\n{TotalTextBox.Text}"));
                document.Blocks.Add(paragraph);

                // Ustawienie dla drukowania w trybie na całą stronę
                document.PageHeight = printDialog.PrintableAreaHeight;
                document.PageWidth = printDialog.PrintableAreaWidth;
                document.PagePadding = new Thickness(50);
                document.ColumnGap = 0;
                document.ColumnWidth = printDialog.PrintableAreaWidth;

                // Wydrukowanie dokumentu
                printDialog.PrintDocument(((IDocumentPaginatorSource)document).DocumentPaginator, "Receipt printing"); ;

            }

        }

    }
}
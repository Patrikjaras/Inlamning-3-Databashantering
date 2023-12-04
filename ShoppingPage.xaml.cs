using Inlamning_3_Databashantering.Data;
using Inlamning_3_Databashantering.Models;
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
using System.Windows.Shapes;

namespace Inlamning_3_Databashantering
{
    /// <summary>
    /// Interaction logic for ShoppingPage.xaml
    /// </summary>
    public partial class ShoppingPage : Window
    {
        private CustomerModel currentCustomer;
        static MongoCRUD db = new MongoCRUD("CustomerDB");

        public ShoppingPage(CustomerModel activeCustomer)
        {

            InitializeComponent();
            currentCustomer = activeCustomer;
            LabelGoShopping.Content = $"Welcome {currentCustomer.Name} go shopping!";
            LoadProducts();
            LoadCustomerCart();
           

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        

        private void BtnRemoveProductFromCart_Click(object sender, RoutedEventArgs e)
        {

            if (ListBoxCustomerCart.SelectedItem != null)
            {
                var product = ListBoxCustomerCart.SelectedItem as ProductModel;
                db.RemoveProductAtIndexFromCustomer("CustomerDB", currentCustomer, ListBoxCustomerCart.SelectedIndex);
                
                LoadCustomerCart();
            }
            else
            {
                MessageBox.Show("A product must be selected");
            }

        }

        private void BtnAddProductToCart_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxAllProdcuts.SelectedItem != null)
            {
                var product = ListBoxAllProdcuts.SelectedItem as ProductModel;
                db.UpdateCustomerCart("CustomerDB", currentCustomer, product);
                currentCustomer.Cart.Add(product);
                MessageBox.Show($"{product.Name} has been added to your cart.");
                LoadCustomerCart();
            }
            else
            {
                MessageBox.Show("A product must be selected");
            }
        }
        private void LoadProducts()
        {
            ListBoxAllProdcuts.ItemsSource = db.LoadRecord<ProductModel>("ProductsDB");
        }
        private void LoadCustomerCart()
        {                    
            ListBoxCustomerCart.ItemsSource = currentCustomer.Cart.ToList();

        }

        private void BtnCheckOut_Click(object sender, RoutedEventArgs e)
        {
            db.CheckOutAndClearCart("CustomerDB", currentCustomer);
            currentCustomer.Cart.Clear();
            LoadCustomerCart();

        }
    }
}

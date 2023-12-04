using Inlamning_3_Databashantering.Data;
using Inlamning_3_Databashantering.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
using static System.Net.Mime.MediaTypeNames;

namespace Inlamning_3_Databashantering
{
    /// <summary>
    /// Interaction logic for Productmanager.xaml
    /// </summary>
    public partial class Productmanager : Window
    {
        static MongoCRUD db = new MongoCRUD("CustomerDB");
        public Productmanager()
        {
            InitializeComponent();

            LoadProductListBox();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private async void AddProductBtn_Click(object sender, RoutedEventArgs e)
        {
            var products = db.LoadRecord<ProductModel>("ProductsDB").ToList();
            ProductModel newProduct = new ProductModel
            {
                Name = ProductNameTbx.Text,
                Price = int.Parse(ProductPriceTbx.Text)
            };


            if (ProductNameTbx.Text != null)
            {
                if(int.TryParse(ProductPriceTbx.Text, out int price))                 
                {

                  
                    if (!products.Any(x => x.Name.ToLower() == newProduct.Name.ToLower()))
                    { 
                    db.CreateRecord("ProductsDB", newProduct);
                    MessageBox.Show($"{newProduct.Name} has been added to inventory");
                    await LoadProductListBox();
                    ClearTextBoxes();
                    }
                    else
                    {
                        MessageBox.Show("Product name already exists");
                    }
                }
                else
                {
                    MessageBox.Show("Price must be a number");
                    ProductPriceTbx.Clear();
                }
            }
            else
            {
                MessageBox.Show("A name must be entered");
            }
        }

        private async void DeleteProductBtn_Click(object sender, RoutedEventArgs e)
        {
            if(AllProductsListBox.SelectedItems != null)
            {
                var productToDelete = AllProductsListBox.SelectedItem as ProductModel;
                db.DeleteRecord("ProductsDB", productToDelete);
                await LoadProductListBox();
                ClearTextBoxes();
            }
        }

        private void AllProductsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AllProductsListBox.SelectedItem != null)
            {
                var product = AllProductsListBox.SelectedItem as ProductModel;
                ProductNameTbx.Text = product.Name;
                ProductPriceTbx.Text = product.Price.ToString();
                
            }
        }

        private async void EditProductBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
               if (AllProductsListBox.SelectedItem != null)
               {
                   var product = AllProductsListBox.SelectedItem as ProductModel;
                   product.Name = ProductNameTbx.Text;
                   product.Price = int.Parse(ProductPriceTbx.Text);                                                  
                    db.UpdateProduct("ProductsDB", product);
                   
                   ClearTextBoxes();
                   await LoadProductListBox();
                   MessageBox.Show("Prodcut has been updated");
               }
               else
               {
                   MessageBox.Show("A product must be selected");
               }
            }
            catch (Exception)
            { 

            }

        }
        public void ClearTextBoxes()
        {
            ProductNameTbx.Clear(); 
            ProductPriceTbx.Clear();    
        }
        public Task LoadProductListBox()
        {
            AllProductsListBox.ItemsSource = db.GetAllItems<ProductModel>("ProductsDB");
            return Task.CompletedTask;
        }
    }
}

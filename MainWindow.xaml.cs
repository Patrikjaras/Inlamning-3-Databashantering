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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Inlamning_3_Databashantering
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static MongoCRUD db = new MongoCRUD("CustomerDB");
        static List <CustomerModel> models = new List <CustomerModel> ();   

        public MainWindow()
        {
            InitializeComponent();
            models = db.LoadRecord<CustomerModel>("CustomerDB");
        }       
        private void ProductManagerBtn_Click(object sender, RoutedEventArgs e)
        {
            Productmanager productmanager = new Productmanager();
            productmanager.Show();
            this.Close();
        }

        

        private void SignUpUserBtn_Click(object sender, RoutedEventArgs e)
        {
            
            CustomerModel newCustomer = new CustomerModel
            {
                Name = UserNameTXB.Text,
                Password = PasswordTXB.Text,    
                Cart = new List <ProductModel>()
                
            };
              if (!models.Any(model => model.Name.ToLower() == newCustomer.Name.ToLower()))
              {

              db.CreateRecord("CustomerDB", newCustomer);
              MessageBox.Show("Customer added");
              UserNameTXB.Clear();
              PasswordTXB.Clear();    
              }
              else
              {
                  MessageBox.Show("Username already exist");
              }
        }

        private void LogInBtn_Click(object sender, RoutedEventArgs e)
        {
            var customer = db.LoadRecord<CustomerModel>("CustomerDB").ToList();
            var currentCustomer = customer.FirstOrDefault(model => model.Name == UserNameTXB.Text);
           
            if (currentCustomer != null)
            {
                
                if (currentCustomer.Password == PasswordTXB.Text)
                {

                ShoppingPage shoppingPage = new ShoppingPage(currentCustomer);
                shoppingPage.Show();
                this.Close();
                }
                else
                {
                    MessageBox.Show("Wrong password try again");
                }

            }
            else
            {
                MessageBox.Show("Username does not exist");
            }

            
        }
    }
}

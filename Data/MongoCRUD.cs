using Inlamning_3_Databashantering.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace Inlamning_3_Databashantering.Data
{
    internal class MongoCRUD
    {

        private IMongoDatabase db;

        public MongoCRUD(string database)
        {
            var client = new MongoClient();
            db = client.GetDatabase(database);
        }
        public void CreateRecord<T>(string table, T record)
        {
            var collection = db.GetCollection<T>(table);
            collection.InsertOne(record);
        }
        public List <T> LoadRecord<T>(string table)
        {
            var collection = db.GetCollection<T>(table);
            return collection.Find(_ => true).ToList();
        }
        public List<T> GetAllItems<T> (string table)
        {
            var collection = db.GetCollection<T>(table);
            return collection.Find(x => true).ToList(); 
        }
        public void DeleteRecord(string table, ProductModel record)
        {
            var collection = db.GetCollection<ProductModel>(table);
            collection.DeleteOne(x => x.Id == record.Id);
        }
        public void RemoveProductAtIndexFromCustomer(string table, CustomerModel customer, int index)
        {
            var collection = db.GetCollection<CustomerModel>(table);

            
            if (index >= 0 && index < customer.Cart.Count)
            {
                
                customer.Cart.RemoveAt(index);
                                
                var filter = Builders<CustomerModel>.Filter.Eq(x => x.Id, customer.Id);
                var update = Builders<CustomerModel>.Update.Set(x => x.Cart, customer.Cart);

                collection.UpdateOne(filter, update);
            }
            
        }
 
       
        public Task UpdateProductInAllCarts(string table, Guid productId, string newName, int newPrice)
        {
            var collection = db.GetCollection<CustomerModel>(table);

            var filter = Builders<CustomerModel>.Filter.ElemMatch(x => x.Cart, p => p.Id == productId);
            var customersToUpdate = collection.Find(filter).ToList();

            foreach (var customer in customersToUpdate)
            {
                for (int i = 0; i < customer.Cart.Count; i++)
                {
                    if (customer.Cart[i].Id == productId)
                    {
                        customer.Cart[i].Name = newName;
                        customer.Cart[i].Price = newPrice;
                    }
                }
            }
                        
            foreach (var customer in customersToUpdate)
            {
                var updateFilter = Builders<CustomerModel>.Filter.Eq(x => x.Id, customer.Id);
                var update = Builders<CustomerModel>.Update.Set(x => x.Cart, customer.Cart);
                collection.UpdateOne(updateFilter, update);
            }
            return Task.CompletedTask;  
        }

        public async void UpdateProduct(string table, ProductModel product)
        {
            var collection = db.GetCollection<ProductModel>(table);
            
            collection.ReplaceOne(x => x.Id == product.Id, product, new ReplaceOptions { IsUpsert = true});
            await UpdateProductInAllCarts("CustomerDB", product.Id, product.Name, product.Price);
        }
        public void UpdateCustomerCart(string table, CustomerModel customer, ProductModel product)
        {
            var collection = db.GetCollection<CustomerModel>(table);
            var filter = Builders<CustomerModel>.Filter.Eq(x => x.Id, customer.Id);
            var updateCart = Builders<CustomerModel>.Update.Push(x => x.Cart, product);

            collection.UpdateOne(filter, updateCart);
        }
        public void CheckOutAndClearCart(string table, CustomerModel customer)
        {
            var collection = db.GetCollection<CustomerModel>(table);
            var filter = Builders<CustomerModel>.Filter.Eq(x => x.Id, customer.Id);
            var update = Builders<CustomerModel>.Update.Set(x => x.Cart, new List<ProductModel>());
            collection.UpdateOne(filter, update);
            MessageBox.Show("Thank you for shopping here, welcome back again");
        }
    }
}

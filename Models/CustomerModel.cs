using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlamning_3_Databashantering.Models
{
    public class CustomerModel
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Name { get; set; }    
        public string Password { get; set; }
        public List<ProductModel> Cart { get; set; }    
      // public CustomerModel(string name, string password) 
      // { 
      //     Name = name;
      //     Password = password;
      //     Cart = new List<ProductModel>();
      // 
      // }
    }
}

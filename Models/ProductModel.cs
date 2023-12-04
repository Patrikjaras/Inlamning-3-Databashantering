using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlamning_3_Databashantering.Models
{
    public class ProductModel
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
             
    }
}

using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreAWSExamen2.Models
{
    public class Foto
    {
        [DynamoDBProperty("titulo")]
        public String Titulo { get; set; }
        [DynamoDBProperty("imageb")]
        public String Imagen { get; set; }
    }
}

using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreAWSExamen2.Models
{
    [DynamoDBTable("usuarios")]
    public class Usuario
    {
        [DynamoDBProperty("idusuario")]
        [DynamoDBHashKey]
        public int IdUsuario { get; set; }
        [DynamoDBProperty("nombre")]
        public String Nombre { get; set; }
        [DynamoDBProperty("descripcion")]
        public String Descripcion { get; set; }
        [DynamoDBProperty("fechaalta")]
        public DateTime FechaAlta { get; set; }
        [DynamoDBProperty("foto")]
        public List<Foto> Fotos { get; set; }
    }
}

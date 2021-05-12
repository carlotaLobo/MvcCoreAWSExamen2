using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using MvcCoreAWSExamen2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreAWSExamen2.Services
{


    public class ServiceDynamoDb
    {
        private DynamoDBContext context;
        public ServiceDynamoDb()
        {
            AmazonDynamoDBClient client = new AmazonDynamoDBClient();
            this.context = new DynamoDBContext(client);
        }
        public async Task Create(Usuario usu)
        {
            await this.context.SaveAsync<Usuario>(usu);
        }
        public async Task<List<Usuario>> GetUsuarios()
        {
            var tabla = this.context.GetTargetTable<Usuario>();
            var scanoptions = new ScanOperationConfig();
            var result = tabla.Scan(scanoptions);
            List<Document> data = await result.GetNextSetAsync();
            IEnumerable<Usuario> u = this.context.FromDocuments<Usuario>(data);
            return u.ToList();
        }
        public async Task<Usuario> GetUsuario(int idusuario)
        {
            return await this.context.LoadAsync<Usuario>(idusuario);
        }
        public async Task<List<Usuario>> GetByNombr(String nombre)
        {
            var tabla = this.context.GetTargetTable<Usuario>();
            ScanFilter scan = new ScanFilter();
            scan.AddCondition("nombre", ScanOperator.Equal, nombre);

            var result = tabla.Scan(scan);

            List<Document> data = await result.GetNextSetAsync();
            IEnumerable<Usuario> p = this.context.FromDocuments<Usuario>(data);
            return p.ToList();
        }
        public async Task Delete(int idusuario)
        {
            await this.context.DeleteAsync<Usuario>(idusuario);
        }
    }
}

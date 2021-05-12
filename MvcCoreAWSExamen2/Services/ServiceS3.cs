using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using MvcCoreAWSExamen2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreAWSExamen2.Services
{
    public class ServiceS3
    {
        private String bucketName;
        private IAmazonS3 awsClient;
        private String url;

        public ServiceS3(IAmazonS3 awsClient, IConfiguration configuration)
        {

            this.awsClient = awsClient;
            this.bucketName = configuration["AWSS3:BucketName"];
            this.url = configuration["AWSS3:urlbucket"];
        }

        public async Task<bool> UploadFile(Stream stream, String filename)
        {
            PutObjectRequest request = new PutObjectRequest()
            {
                InputStream = stream,
                Key = filename,
                BucketName = this.bucketName
            };
            PutObjectResponse response = await this.awsClient.PutObjectAsync(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }
        public async Task<List<String>> GetNameFiles()
        {
            ListVersionsResponse response = await this.awsClient.ListVersionsAsync(this.bucketName);
            return response.Versions.Select(x => x.Key).ToList();
        }
        public async Task<bool> DeleteFile(String filename)
        {
            DeleteObjectResponse response = await this.awsClient.DeleteObjectAsync(this.bucketName, filename);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.NoContent) return true;
            return false;
        }
        public async Task<Stream> GetFile(String filename)
        {
            GetObjectResponse response = await this.awsClient.GetObjectAsync(this.bucketName, filename);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {

                return response.ResponseStream;
            }

            return null;

        }
        public String GetUrlFile(String filename)
        {
            return this.url + filename;
        }
    }
}

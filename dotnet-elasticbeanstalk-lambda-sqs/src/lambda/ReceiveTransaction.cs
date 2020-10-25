using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.Util;
using Amazon.Lambda;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using System.Text.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Demo.Aws.LambdaFunction
{
    public class Function
    {

        public async Task FunctionHandler(SQSEvent invocationEvent, ILambdaContext context)
        {
            LambdaLogger.Log("ENVIRONMENT VARIABLES: " + JsonSerializer.Serialize(System.Environment.GetEnvironmentVariables()));
            LambdaLogger.Log("CONTEXT: " + JsonSerializer.Serialize(context));
            LambdaLogger.Log("EVENT: " + JsonSerializer.Serialize(invocationEvent));


            var bucketName = Environment.GetEnvironmentVariable("S3Bucket");
            var bucketRegionSystemName = Environment.GetEnvironmentVariable("S3BucketRegion");
            RegionEndpoint bucketRegion = RegionEndpoint.USEast1;
            var client = new AmazonS3Client(bucketRegion);
            LambdaLogger.Log("bucketRegion.DisplayName:" + bucketRegion.DisplayName);
            foreach (var trans in invocationEvent.Records)
            {
                var transaction = JsonSerializer.Deserialize<Transaction>(trans.Body);
                LambdaLogger.Log("Transaction: " + transaction);
                var putRequest1 = new PutObjectRequest()
                {
                    BucketName = bucketName,
                    Key = Guid.NewGuid().ToString(),
                    ContentBody = trans.Body,
                };
                var response = await client.PutObjectAsync(putRequest1);
            }
            return;
        }
    }
}
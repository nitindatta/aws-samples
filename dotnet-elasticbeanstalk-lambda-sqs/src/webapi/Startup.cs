using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Demo.Aws.Entities;
using Demo.Aws.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Demo.Aws.Services.Interfaces;
using Demo.Aws.Services;
using Microsoft.Extensions.Configuration;
using Amazon.SQS;
using Amazon;
using System.IO;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using System;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace Demo.Aws
{
    public class Startup
    {
        private readonly IConfiguration Configuration;
        public IWebHostEnvironment HostingEnvironment { get; }

        public Startup(IConfiguration configuration,IWebHostEnvironment environment)
        {
            Configuration = configuration;
            HostingEnvironment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen();
            services.AddControllers();

            services
                .AddScoped<ITransactionService, TransactionService>()
                .AddScoped(typeof(IAsyncRepository<>), typeof(Repository<>))
                .AddSingleton<IAmazonSQS, AmazonSQSClient>();

            if (HostingEnvironment.IsDevelopment())
            {
                services.AddDbContext<TransactionContext>(c =>
                    c.UseSqlServer(Configuration.GetConnectionString("DemoDb")),
                    ServiceLifetime.Scoped);
            }
            else {
                // Example of how to set credentials security from Secrets Manager
                SqlConnectionStringBuilder builder =
                new SqlConnectionStringBuilder(Configuration.GetConnectionString("DemoDb"));

                var dbParam = JsonSerializer.Deserialize<DBSecret>(GetSecret());
                builder.UserID=dbParam.username;
                builder.Password=dbParam.password;
                Console.WriteLine("connection string:" + builder.ConnectionString);
                services.AddDbContext<TransactionContext>(c =>
                    c.UseSqlServer(builder.ConnectionString),
                    ServiceLifetime.Scoped);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TransactionContext transactionContext)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            //Pick from config :)
            AWSConfigs.AWSRegion = Configuration.GetValue<string>("AWSRegion");

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
                endpoints.MapControllers();
            });
            transactionContext.Database.Migrate();

        }
        public string GetSecret()
        {
            string secretName = "SqlUserSecret";
            string region = Configuration.GetValue<string>("AWSRegion");
            string secret = "";

            MemoryStream memoryStream = new MemoryStream();

            IAmazonSecretsManager client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));

            GetSecretValueRequest request = new GetSecretValueRequest();
            request.SecretId = secretName;
            request.VersionStage = "AWSCURRENT"; // VersionStage defaults to AWSCURRENT if unspecified.

            GetSecretValueResponse response = null;

            // In this sample we only handle the specific exceptions for the 'GetSecretValue' API.
            // See https://docs.aws.amazon.com/secretsmanager/latest/apireference/API_GetSecretValue.html
            // We rethrow the exception by default.

            try
            {
                response = client.GetSecretValueAsync(request).Result;                
            }
            catch (DecryptionFailureException)
            {
                // Secrets Manager can't decrypt the protected secret text using the provided KMS key.
                // Deal with the exception here, and/or rethrow at your discretion.
                throw;
            }
            catch (InternalServiceErrorException)
            {
                // An error occurred on the server side.
                // Deal with the exception here, and/or rethrow at your discretion.
                throw;
            }
            catch (InvalidParameterException)
            {
                // You provided an invalid value for a parameter.
                // Deal with the exception here, and/or rethrow at your discretion
                throw;
            }
            catch (InvalidRequestException)
            {
                // You provided a parameter value that is not valid for the current state of the resource.
                // Deal with the exception here, and/or rethrow at your discretion.
                throw;
            }
            catch (ResourceNotFoundException)
            {
                // We can't find the resource that you asked for.
                // Deal with the exception here, and/or rethrow at your discretion.
                throw;
            }
            catch (System.AggregateException)
            {
                // More than one of the above exceptions were triggered.
                // Deal with the exception here, and/or rethrow at your discretion.
                throw;
            }

            // Decrypts secret using the associated KMS CMK.
            // Depending on whether the secret is a string or binary, one of these fields will be populated.
            if (response.SecretString != null)
            {
                secret = response.SecretString;
            }
            else
            {
                memoryStream = response.SecretBinary;
                StreamReader reader = new StreamReader(memoryStream);
                string decodedBinarySecret = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
            }
            return secret;

            // Your code goes here.
        }
    }
    class DBSecret
    {
        public string username {get; set;}
        public string password {get; set;}
    }
}

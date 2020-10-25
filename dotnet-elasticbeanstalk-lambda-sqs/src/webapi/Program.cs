using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Demo.Aws
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostincontext,config)=>
                {        
                    Console.WriteLine("ASPNETCORE_ENVIRONMENT:" + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")) ;  
                    Console.WriteLine("ConnectionStrings__DemoDb:" + Environment.GetEnvironmentVariable("ConnectionStrings__DemoDb")) ;   
                    Console.WriteLine("SQS:" + Environment.GetEnvironmentVariable("sqsqueue")) ;           
        
                    config.AddJsonFile($"appsettings.json", true, true);
                    config.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .UseIIS();
                });
    }
}

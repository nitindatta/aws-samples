using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.Lambda.Core;
using Amazon.Lambda.CloudWatchEvents;
using System.Text.Json;


namespace Demo.Aws.LambdaFunction
{
    //Invoked through cloud watch event
    public class ScheduleJob
    {

        public async Task FunctionHandler(CloudWatchEvent<EventDetails> invocationEvent, ILambdaContext context)
        {
            LambdaLogger.Log("ENVIRONMENT VARIABLES: " + JsonSerializer.Serialize(System.Environment.GetEnvironmentVariables()));
            LambdaLogger.Log("CONTEXT: " + JsonSerializer.Serialize(context));
            LambdaLogger.Log("EVENT: " + JsonSerializer.Serialize(invocationEvent));

            //Do what is required for processing
            
            
            return;
        }
    }
    public class EventDetails {

        //--- Properties ---
        public string Message { get; set; }
    }
}

namespace Amazon.Lambda.CloudWatchEvents {

    public class MyCloudWatchEvent : CloudWatchEvent<Demo.Aws.LambdaFunction.EventDetails> { }
}
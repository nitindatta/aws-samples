import json;
import boto3;
import os;

jobqueue = os.environ['BATCH_JOBQUEUE']
jobdefinition = os.environ['BATCH_JOBDEFINITION']

def lambda_handler(event, context):
    inputFileName = ""
    bucketName = ""
    
    for record in event['Records']:
        bucketName = record['s3']['bucket']['name']
        inputFileName = record['s3']['object']['key']

 
    batch = boto3.client('batch')
    region = batch.meta.region_name
    batchCommand = "--bucketName " + bucketName  + " --fileName " + inputFileName + " --region " + region
    out = "inputFileName - " + bucketName + "/" + inputFileName + " Region " + region
    out = out + "  " + batchCommand
    print(out)
    #TODO: Move jobqueue and JobDefinitaion to input environment variables
    response = batch.submit_job(jobName='JsonImport', 
                                jobQueue=jobqueue, 
                                jobDefinition=jobdefinition, 
                                containerOverrides={
                                    "command": [ f"--files={inputFileName}"  ],
                                    "environment": [ 
                                        {"name": "S3Settings__BucketName", "value": bucketName},
                                        {"name": "FileName", "value": inputFileName},
                                        {"name": "Region", "value": region}
                                    ]
                                })
    print("Job ID is {}.".format(response['jobId']))
    return {
        'statusCode': 200,
        'body': json.dumps("Job ID is {}.".format(response['jobId']))
    }

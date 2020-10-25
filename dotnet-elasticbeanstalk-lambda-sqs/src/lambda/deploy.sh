#!/bin/bash

aws cloudformation package --template-file lambda.yml --s3-bucket dotnet-helloworld-us-east-1 --output-template-file dotnet-deploy-out.yml
aws cloudformation deploy --template-file dotnet-deploy-out.yml --stack-name dotnet-helloworld-lambda --capabilities CAPABILITY_NAMED_IAM
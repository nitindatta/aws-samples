#!/bin/bash

#======================================================================
# Deploys RDS Aurora Serverless and related resources
#======================================================================

# Sample invoke:
# ./deploy.sh dev

set -e

function error() {
    echo "Error: $1"
    exit -1
}
[[ -n "$1" ]] || error "Missing environment name (eg, dev, qa, prod)"
env_type=$1

. "./${env_type}-env.sh"

echo ${app_name}
#exit -1

aws cloudformation deploy \
    --template-file "${rds_cfn_template}" \
    --stack-name $rds_stack_name \
    --s3-bucket aws-sam-ticketappdeployment 
    --s3-prefix rds-ticketapp
    --region eu-central-1
    --profile erick
    --parameter-overrides \
        ParameterKey="AppName",ParameterValue="$app_name" \
        ParameterKey="EnvType",ParameterValue="$env_type" \
        ParameterKey="DBClusterName",ParameterValue="$db_cluster_name" \
        ParameterKey="DatabaseName",ParameterValue="$db_name" \
        ParameterKey="DBMasterUserName",ParameterValue="$db_master_username" \
        ParameterKey="DBSubnetList",ParameterValue="\"${db_subnet_1},${db_subnet_2},${db_subnet_3}\"" \
    --capabilities \
        CAPABILITY_IAM

# TODO: wait stack creation/update completion
sleep 180

# Enable the Data API
aws rds modify-db-cluster --db-cluster-identifier $db_cluster_name --enable-http-endpoint

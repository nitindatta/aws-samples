#!/bin/bash


# CHANGE THESE VALUES FOR YOUR AWS ACCOUNT  --------------------

# All resources deployed (eg, API, Lambdas) will be prefix w/ the env type (eg, dev-register-ami-lambda)

# ----- General Parameters ----- #

# Prefix to use to name provisioned resources
export env_type="stage"
# S3 bucket to store packaged Lambdas
export s3_bucket_deployment_artifacts="aws-sam-ticketappdeployment"

# ----- RDS Stack ----- #
# RDS database name (a-zA-Z0-9_)
export db_name="ticketapp"
# RDS Aurora Serverless Cluster Name (a-zA-Z0-9-)
export db_cluster_name="${env_type}-aurora-tickeapp-cluster"
# RDS Master Username
export db_master_username="db_user" # password will be create on-the-fly and associtated w/ this user
# RDS Aurora Serverless Cluster Subnets
export db_subnet_1="subnet-01502fa97260f9076"
export db_subnet_2="subnet-0933c1d03afa50a20"
export db_subnet_3="subnet-0cb87a26ada6e8c8f"

# ----- API Stack ----- #
export api_stage_name="stage"
export log_level="DEBUG"  # debug/info/error

# ---------------------------------------------------------------

# You probably don't need to change these values
export app_name="ticketapp"
export rds_cfn_template="aurora_cfn_template.yaml"
export api_cfn_template="api_cfn_template.yaml"
export gen_api_cfn_template="generated-${api_cfn_template}"
export sam_build_dir=".aws-sam"
export lambdas_dir="lambdas"
export rds_stack_name="${env_type}-${app_name}-database-stack"
export api_stack_name="${env_type}-${app_name}-api-stack"
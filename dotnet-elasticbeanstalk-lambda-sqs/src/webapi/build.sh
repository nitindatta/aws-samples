 #!/bin/bash

 rm -r -f ./deploy
 mkdir ./deploy/
 mkdir ./deploy/publish
 dotnet publish . -c Release -o ./deploy/publish
 mkdir ./deploy/sampledotnet-deploy
 zip a  ./deploy/sampledotnet-deploy/sampledotnet.zip  ./deploy/publish/*
 cp ./aws-windows-deployment-manifest.json ./deploy/sampledotnet-deploy
 zip a  ./deploy/dotnet-helloworld-deploy.zip  ./deploy/sampledotnet-deploy/*.*
 #rm -r -f ./deploy
Content-Type: multipart/mixed; boundary="//"
MIME-Version: 1.0

--//
Content-Type: text/cloud-config; charset="us-ascii"
MIME-Version: 1.0
Content-Transfer-Encoding: 7bit
Content-Disposition: attachment; filename="cloud-config.txt"

#cloud-config
cloud_final_modules:
- [scripts-user, always]

--//
Content-Type: text/x-shellscript; charset="us-ascii"
MIME-Version: 1.0
Content-Transfer-Encoding: 7bit
Content-Disposition: attachment; filename="userdata.txt"


#!/bin/bash
db_endpoint='db_endpoint1'
db_name='db_name1'
db_user='db_user1'
db_password='db_password1'
GetQueue="getquequeurl1"
AddQueue="addquequeurl1"



yum install -y git
mkdir /var/src
mkdir /var/out
cd /var/src
git clone https://github.com/ngiovin/AwsDecoupledDemo.git

cp AwsDecoupledDemo/helloapp.conf /etc/httpd/conf.d/helloapp.conf



sed -i "s/GetQueue/${GetQueue}/g" AwsDecoupledDemo/StorageLayer/StorageLayer/appsettings.Development.json
sed -i "s/AddQueue/${AddQueue}/g" AwsDecoupledDemo/StorageLayer/StorageLayer/appsettings.Development.json
sed -i "s/db_endpoint/${db_endpoint}/g" AwsDecoupledDemo/StorageLayer/StorageLayer/appsettings.Development.json
sed -i "s/db_name/${db_name}/g" AwsDecoupledDemo/StorageLayer/StorageLayer/appsettings.Development.json
sed -i "s/db_user/${db_user}/g" AwsDecoupledDemo/StorageLayer/StorageLayer/appsettings.Development.json
sed -i "s/db_password/${db_password}/g" AwsDecoupledDemo/StorageLayer/StorageLayer/appsettings.Development.json


rm AwsDecoupledDemo/StorageLayer/StorageLayer/appsettings.json
cp AwsDecoupledDemo/StorageLayer/StorageLayer/appsettings.Development.json AwsDecoupledDemo/StorageLayer/StorageLayer/appsettings.json 





cd AwsDecoupledDemo/StorageLayer/StorageLayer


export DOTNET_CLI_HOME=/temp


dotnet restore
dotnet publish -o /var/out

cp /var/src/AwsDecoupledDemo/StorageLayer/net-core.service /etc/systemd/system/net-core.service
systemctl enable net-core.service
systemctl start net-core.service





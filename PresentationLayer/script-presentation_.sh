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
GetQueue="getquequeurl1"
AddQueue="addquequeurl1"



yum install -y git httpd
mkdir /var/src
mkdir /var/out
cd /var/src
git clone https://github.com/ngiovin/AwsDecoupledDemo.git

cp AwsDecoupledDemo/helloapp.conf /etc/httpd/conf.d/helloapp.conf
systemctl restart httpd
systemctl enable httpd



sed -i "s/GetQueue/${GetQueue}/g" AwsDecoupledDemo/PresentationLayer/PresentationLayer/appsettings.Development.json
sed -i "s/AddQueue/${AddQueue}/g" AwsDecoupledDemo/PresentationLayer/PresentationLayer/appsettings.Development.json
rm AwsDecoupledDemo/PresentationLayer/PresentationLayer/appsettings.json
cp AwsDecoupledDemo/PresentationLayer/PresentationLayer/appsettings.Development.json AwsDecoupledDemo/PresentationLayer/PresentationLayer/appsettings.json 





cd AwsDecoupledDemo/PresentationLayer/PresentationLayer


export DOTNET_CLI_HOME=/temp


dotnet restore
dotnet publish -o /var/out

cp /var/src/AwsDecoupledDemo/PresentationLayer/net-core.service /etc/systemd/system/net-core.service
systemctl enable net-core.service
systemctl start net-core.service





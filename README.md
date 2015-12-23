# hubic-asp-net-sample
# asp.net sample of integration with hubiC API
hubiC is a cloud storage system and it has an API so you can make integration from your own programs - [API description](https://api.hubic.com/)

First thing to do is to login to your hubiC account (or create a new account - you get 25GB storage for free) and create an application.
Click your name and choose My account. Then go to the Developers tab and create a new application:

![alt tag](Images/hubiCapp.png)

Make a note of the values in Client ID and Secret Client - you will be using them later. You must register your application with a domain but you can use a fictional domain if you just want to test the sample on your local computer.

Next you must map this domain temporarely to 127.0.0.1 in your hosts file (C:\Windows\System32\drivers\etc\hosts) so hubiC can redirect back to your local computer.

Download and unzip the source files to a folder on your computer.

hubiC requires that your domain uses SSL/HTTPS so you must setup a secure site in IIS. Open IIS Manager and create a self-signed certificate (or import a certificate if you have one for your domain).

Create a new website and choose the physical path of the source files. Under Bindings choose HTTPS and select the certificate.

Open the project in Visual Studio and configure ClientID, SecretClient and RedirectionDomain in web.config with the values from your hubiC application.

Run and test the sample.

In the sample you can:

1. Get a request token
 
2. Get an access token from the request token
 
3. Refresh the access token
 
4. List account and usage information
 
5. Get an endpoint and token for the file storage
 
6. List, create and delete containers
 
7. List, upload, download and delete files
 

 

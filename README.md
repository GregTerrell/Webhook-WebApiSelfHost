# Webhook-WebApiSelfHost
LooUQ iotQi has established patterns for both commands (to device) and event webhooks (from device). Webhooks are a lightweight HTTP pattern providing a simple pub/sub model for wiring together Web APIs and SaaS services like LooUQ iotQi; many services such as GitHub, Twitter, SendGrid and Stripe use webhooks to callback into your application when service events happen.

LooUQ iotQi utilizes the webhook pattern to relay priority events back to customer systems.  This sample application demonstrates all of the key components in building a webhook receiver (consumer logic).  All of this is application is open-source and available for you to add to your project (even if it is not iotQi related).

When an event happens on a iotQi device, your device can send a priority alert event to the LooUQ cloud.  Based on rules you setup for your subscription, a notification is sent to designated recipients.  One form of notification is a webhook. For this type of notification, your application will receive a HTTP POST request with a request body containing JSON data originating from your device’s alert.  

## Using Webhooks with iotQi 
With iotQi using webhooks is easy.  With just a couple steps your webhooks will be flowing.
To start receiving webhook notifications you will setup one or more notification rules in iotQi Setup.  Notification rules apply to your subscription, not individual devices, so with only a handful of notification rules you can be receiving webhooks, text messages, and email notifications.
 
How exactly you build your webhook receiver will depend primarily on how you intend to use it and exactly what your application will do with the notification information you receive.  Regardless of the type of application, you will need to follow a basic set of tasks.  The sample webhook receiver on the LooUQ GitHub site has all of the required parts, with numerous comments on how to migrate each function to your application.  

**In summary, the webhook receiver must:**
* Listen for incoming HTTP POST requests.
* Route these requests to logic to unpack the request contents (if your familiar with ASP.NET WebAPI, this is a controller action).
* Perform validation on the received content. iotQi signs (optional, but strongly recommended) the outgoing request using a secret you provide.
* Execute your business logic to do whatever you intended the webhook to trigger.
  
If you need it, we have a detailed development guide available on the LooUQ Support Center site (see links at the end of the README).  We cover the topics above in more detail on that guide.

## Building Your Webhook Receiver
Webhook functionality can be integrated into numerous enterprise application models.  For large scale, enterprise you are likely to already be using web-based RESTful API services, which is exactly what a webhook is.

For lighter weight applications you may wish to build your application on workstation based platforms like a Windows console application to write incoming events to a disk log, or a WPF application to serve as a dashboard.  For these lighter applications, self-hosting is an option with Microsoft's Katana implementation of OWIN.  Self-hosting in this context refers to the ability to run a RESTful web API without the use of a full-fledged web server such as IIS (Internet Information Server). The webhook sample found in LooUQ's GitHub collection is an example of a self-hosted Windows console application.

Use code from this repository to enable webhook receive functionality in your application.  Use the classes as required (all for console self-host, to a couple if your application is ASP.NET WebAPI already.

## Obstacles You May Encounter
Anytime you are receiving unsolicited information you are likely to be impacted by firewall settings.  For server based implementations IIS will do most of the work to register your web applications appropriately with the supporting system's required resources and configure the necessary settings.  If you are building your webhook receiver logic as a self-hosted application you will have to manually complete the short-list of configuration tasks below in order to have a functioning webhook receiver application. The tasks below are for a Windows system.

**_Firewall Exceptions_**
One of the tricky things to address with remote access is the correct firewall rule to allow the request traffic in.  The unique issue is that the actual process on your PC listening for requests is not your application, it is the HTTP.SYS windows process; this is addressed in the rule below by designating "This program path: SYSTEM".  In summary setup the firewall rule as follows:
In Windows Firewall with Advanced Security
Inbound Rules
New Rule
Rule Type: Custom 
Program: This program path: SYSTEM
Protocol: TCP, Local Port (range suggested like: 8080-8089)
Scope: Any IP address (unless you want to be more specific)
Action: Allow the connection
Profile: Leave all checked (unless you are aware of a more restrictive policy)
Name: Enter a descriptive name and optional description paragraph
Special thanks to Pieter for this post on Stack Overflow for information on the firewall rule.

**_HTTP.SYS Listener Settings_**
When you use IIS, it automatically sets HTTP.SYS to be a listener for the interfaces (addresses) bound to your site applications.  If you self-host you may need to adjust settings for your computer as follows: 
Start by opening an elevated command prompt (run as administrator)
Review existing IP listen policies with: netsh http show iplisten
Set a new IP listen policy with: netsh http add iplisten ipaddress=<ip_address> (use 0.0.0.0 as the address for all address on your computer)

**_HTTP.SYS URL ACL Reservations_**
To receive incoming HTTP traffic, your application must be either 1) running as "administrator", or 2) register with HTTP.  I recommend the registration approach. 

To setup a HTTP URL ACL registration use the following recommended command line from a elevated command prompt.

netsh http add urlacl http://+:<portNumber>/ user=<yourUserName>

<portNumber> is the port number your application is listening for webhook requests on.  The + is the strong-wildcard match for any IP address or host name you are listening on.
<yourUserName> is your windows sign-on name.  If your application will be running under a different local account than yours, use that account username instead.
  
 ## Links
[LooUQ Support Center guide on webhooks](https://loouq.zendesk.com/knowledge/articles/115001639053/en-us?brand_id=658247)

[Microsoft: Use OWIN to Self-Host ASP.NET Web API 2, Great backgrounder on self-hosting OWIN](https://docs.microsoft.com/en-us/aspnet/web-api/overview/hosting-aspnet-web-api/use-owin-to-self-host-web-api) 

### OWIN Self-Host Issues
[Microsoft: Article about WCF over HTTP, many relevant comparisons to OWIN self-host](https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/configuring-http-and-https)

[Stack Overflow: Discusses http listening process with code examples](https://stackoverflow.com/questions/4019466/httplistener-access-denied)

[Stack Overflow: Discusses Nancy environment, which closely mirrors OWIN self-host](https://stackoverflow.com/questions/33559157/how-do-i-remotely-access-self-hosted-nancy-service)

[Stack Overflow: HTTP Listener Firewall Exception](https://stackoverflow.com/questions/17863294/c-sharp-httplistener-and-windows-firewall/21364604#21364604)


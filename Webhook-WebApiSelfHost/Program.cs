using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;

namespace WebHook_WebApiSelfHost
{
    public class Program
    {
        /* If you provide a webhook shared secret in your Setup notification rule, the LooUQ cloud will automatically add a signature header
         * the the webhook POST request.  The "shared secret" is used as salt for the hash of the webhook payload.  You can consider moving this
         * to the app.config, currently the validation logic looks for the Program.WebhookCredentials variable by name.
        */
        public const string WebhookCredentials = "myWebhookSecretSalt";

        public static WebhookDispatcher dispatcher = new WebhookDispatcher();

        static void Main(string[] args)
        {
            /* A key issue if your are targeting your webhooks to a workstation or server not running IIS is
             * ensuring the Windows OS will route requests to your webhook application.
             * 
             * To allow webhook requests in...
             *   > use http://localhost:port/ in VisualStudio without a netsh http addacl
             *   > use http://+:port/ and run VisualStudio or your executable as Run as Administrator
             * 
             *   > use http://+:port/ and add an addacl entry (recommended)
             * 
             * To test locally your can use localhost http://localhost:9000/webhook/alerts
             * 
             * To addacl reservation
             *   > netsh http add urlacl url=http://+:port/ user=<yourWinUserId>
             *   
             *   The LooUQ blog has an post about self-hosting
            */

            string baseAddress = "http://+:9000";

            dispatcher.Alert += OnAlertReceived;

            // Start OWIN host 
            WebApp.Start<Startup>(baseAddress);

            Console.WriteLine("Waiting for WebAPI requests using URL reservation: {0}, press <Enter> to exit.", baseAddress);
            Console.ReadLine();
        }


        static void OnAlertReceived(object sender, IotqiAlert e)
        {
            Console.WriteLine();
            Console.WriteLine("InEventReceiver() - AlertName: {0}, Value: {1}, Data: {2}", e.AlertName, e.AlertValue, e.AlertData);
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}

using Owin;
using System.Web.Http;

/* This class creates the configuration for OWIN and WebAPI routing to your controller.  It, along with your controller
 * define the application side of the URL.  This demo uses a controller named IotqiController and you can see below that
 * the route definition prefixes that with "webhook".  
 * 
 * Your webhook URL would be http://<yourHostNameOrIp>/webhook/iotqi
 * 
 * The route below is a universal matching route with an optional Id parameter (not used in this demo)
 */

namespace WebHook_WebApiSelfHost
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "webhook/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            appBuilder.UseWebApi(config);
        }
    }
}
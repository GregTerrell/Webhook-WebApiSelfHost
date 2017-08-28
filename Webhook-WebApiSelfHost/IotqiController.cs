using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;



namespace WebHook_WebApiSelfHost
{
    public class IotqiController : ApiController
    {
        // GET webhook/iotqi                                                            // left in source for demo\testing, delete if not used
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        // POST webhook/Iotqi 
        public HttpResponseMessage Post([FromBody]IotqiAlert alertContent)
        {
            if (!IotqiWebhook.ValidateWebhookObject(Request, alertContent))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest); ;
            }
            //process actions to be taken here
            Program.dispatcher.Dispatch(alertContent);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }

}

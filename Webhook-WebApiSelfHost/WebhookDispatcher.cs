using System;

namespace WebHook_WebApiSelfHost
{
    /* This class defines an event to transport your device alert data to a .NET code block that subscribes to it. */

    public class WebhookDispatcher
    {
        public event EventHandler<IotqiAlert> Alert;

        public void Dispatch(IotqiAlert alertContent)
        {
            Alert?.Invoke(this, alertContent);
        }
    }

}


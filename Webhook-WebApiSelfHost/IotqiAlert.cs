namespace WebHook_WebApiSelfHost
{
    /* This is the schema for device alerts delivered via webhooks.  All iotQi alerts implement this.  The actual data
     * contained in the POST body is the serialized JSON representation of this class.  The IotqiController action will 
     * automatically deserialize into this object.  Once deserialized, this is just like any other .NET class.
     */

    public class IotqiAlert
    {
        public string DeviceName { get; set; }
        public string DeviceId { get; set; }
        public string DeviceLocation { get; set; }
        public string AlertName { get; set; }
        public string AlertValue { get; set; }
        public object AlertData { get; set; }
    }
}

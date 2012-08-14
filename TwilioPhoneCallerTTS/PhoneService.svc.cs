using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Twilio;
using System.Xml;




namespace TwilioPhoneCallerTTS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class PhoneService : IPhoneService
    {
        public string MakeCall(string number, string message)
        {
            // Set our Account SID and AuthToken
            string accountSid = "AC955c4ca6365eb7628db4829d6a04966f";
            string authToken = "bb63e780836aa018a012fd97f64f7b5f";

            // A phone number you have previously validated with Twilio
            string phonenumber = "+16085127391";

            // Instantiate a new Twilio Rest Client
            var client = new TwilioRestClient(accountSid, authToken);

            // Initiate a new outbound call
            client.InitiateOutboundCall(
                phonenumber, // The number of the phone initiating the call
                "+1" + number.ToString(), // The number of the phone receiving call
                call => {
                    WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
                    var x = new XmlDocument();
                    x.LoadXml("<x>SpecialMessage</x>");
                    return x;
                }
                //"http://demo.twilio.com/welcome/voice/" // The URL Twilio will request when the call is answered
            );

            if (call.RestException == null)
            {
                return string.Format("Started call: {0}", call.Sid);
            }
            else
            {
                return string.Format("Error: {0}", call.RestException.Message);
            }
        }

        //              "<?xml version="1.0" encoding="utf-8" ?> "
        //- <Response>
        //  <Say>Thanks for the call. Configure your number's voice U R L to change this message.</Say> 
        //  <Pause length="1" /> 
        //  <Say voice="woman">Let us know if we can help you in any way during your development.</Say> 
        //  </Response>

        public static Action<string> WriteMessageResponse()
        {
            return delegate(string value)
            {
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
                var x = new XmlDocument();
                x.LoadXml("<x>SpecialMessage</x>");

            };
        }

        public XmlElement GetMessageText(Guid token)
        {
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            var x = new XmlDocument();
            x.LoadXml("<x>" + GetMessageFromDictionary(token) + "</x>");
            return x.DocumentElement;
        }

        private string GetMessageFromDictionary(Guid token)
        {
            return "Special Message";
        }
    }
}

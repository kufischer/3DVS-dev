using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json.Linq;
using RESTServicePlugin;
using FIVES;
using System.IO;

namespace CustomizedAnimationPlugin
{
    class CustomizedAnimationRequestHandler : RequestHandler
    {

        private string path;
        private string contentType;
        private bool debugMode;

        public CustomizedAnimationRequestHandler(string path, string contentType)
        {
            this.debugMode = false;
            this.path = path;
            this.contentType = contentType;
        }

        public override string Path
        {
            get
            {
                return this.path;
            }
        }

        public override string ContentType
        {
            get
            {
                return this.contentType;
            }
        }

        private JObject JsonParser(string json)
        {
            JObject o = JObject.Parse(json);
            if (this.debugMode)
            {
                foreach (JProperty property in o.Properties())
                {
                    Console.WriteLine(property.Name + " - " + property.Value);
                }
            }
            return o;
        }


        protected override RequestResponse HandlePOST(string requestPath, string content)
        {
            //Console.WriteLine("handle post");

            RequestResponse response = new RequestResponse();

            //Console.WriteLine(requestPath);

            //Console.WriteLine(content);

            if (requestPath.Equals("/SetAnimation"))
            {
                CustomizedAnimationManager.Instance.SetAnimation(content); //Set Animation

                response.ReturnCode = 200;
            }
            else if (requestPath.Equals("/PlaybackAnimation"))
            {
                CustomizedAnimationManager.Instance.PlaybackAnimation(content); //Play Animation

                response.ReturnCode = 200;
            }
            else if (requestPath.Equals("/StopAnimation"))
            {
                CustomizedAnimationManager.Instance.StopAnimation(content); //Play Animation

                response.ReturnCode = 200;
            }
            else
            {
                Console.WriteLine("HandlePOST: No corresponding processing method");
            }

            return response;
        }

        protected override RequestResponse HandleDELETE(string requestPath)
        {
            throw new NotImplementedException();
        }

        protected override RequestResponse HandleGET(string requestPath)
        {
            throw new NotImplementedException();
        }

        protected override RequestResponse HandlePUT(string requestPath, string content)
        {
            throw new NotImplementedException();
        }
    }
}

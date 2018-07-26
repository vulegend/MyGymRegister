using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyGymRegister
{
    public class WCFHandler
    {
        private static WCFHandler _instance;
        public static WCFHandler Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new WCFHandler();

                return _instance;
            }
        }

        public string SendPOSTRequest(string address, string jsonContent)
        {
            var webAddr = "http://localhost:9000/"+address;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(jsonContent);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                return result;
            }
        }

        public void UploadPicture(byte[] buffer, string imageName)
        {
            var webAddr = "http://localhost:9000/uploadphoto/" + imageName;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "text/plain";
            httpWebRequest.ContentLength = buffer.Length;

            using (Stream requestStream = httpWebRequest.GetRequestStream())
            {
                // Send the file as body request. 
                requestStream.Write(buffer, 0, buffer.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse())
                Console.WriteLine("HTTP/{0} {1} {2}", response.ProtocolVersion, (int)response.StatusCode, response.StatusDescription);
        }
    }
}

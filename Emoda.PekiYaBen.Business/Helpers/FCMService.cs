using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Emoda.PekiYaBen.Business.Helpers
{
    public static class FCMService
    {
        public static void SendNotification(List<string> deviceRegIds, string message, string title)
        {
            foreach (string token in deviceRegIds)
            {
                var retVal = Notify(token, message, title);
            }

        }

        private static string Notify(string regId, string message, string title)
        {
            string SERVER_API_KEY = "AAAAvR4-03w:APA91bENTOeJ7mGIelxfSgN6cP5s2yS556o03XCeEfkx04EHAYs9PLw9c1B8eCeiERbo3EUOICpKR4Ftkh46fEo2j5JDML9gVSWAi9mici5SP7U7SpejKmIPxSvfL08mExLKxYmF81LC";
            var SENDER_ID = "812256252796";
            string sResponseFromServer = "";

            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            //serverKey - Key from Firebase cloud messaging server  
            tRequest.Headers.Add(string.Format("Authorization: key={0}", SERVER_API_KEY));
            //Sender Id - From firebase project setting  
            tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));
            tRequest.ContentType = "application/json";
            var payload = new
            {
                to = regId,
                priority = "high",
                content_available = true,
                notification = new
                {
                    body = message,
                    title,
                    badge = 1,
                    sound = "default"
                },
            };

            string postbody = JsonConvert.SerializeObject(payload).ToString();
            Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
            tRequest.ContentLength = byteArray.Length;
            using (Stream dataStream = tRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
                using (WebResponse tResponse = tRequest.GetResponse())
                {
                    using (Stream dataStreamResponse = tResponse.GetResponseStream())
                    {
                        if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                sResponseFromServer = tReader.ReadToEnd();
                            }
                    }
                }
            }
            return sResponseFromServer;
        }
    }
}

﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utilities.Log;

namespace Utilities.FB
{
    public class Facebook
    {
        public static FacebookAccount GetAccount(string access_token)
        {
            string response = RequestResponse("https://graph.facebook.com/v2.12/me?fields=id,name,picture,email&access_token=" + access_token);

            if (string.IsNullOrEmpty(response))
                return null;

            FacebookAccount account = JsonConvert.DeserializeObject<FacebookAccount>(response);

            if (account == null || account.id <= 0)
                return null;

            return account;
        }
        public static string RequestResponse(string url)
        {
            string pageContent = string.Empty;
            try
            {
                System.Net.HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
                myRequest.Credentials = CredentialCache.DefaultCredentials;
                //// Get the response
                WebResponse webResponse = myRequest.GetResponse();
                Stream respStream = webResponse.GetResponseStream();
                if (respStream != null)
                {
                    ////
                    StreamReader ioStream = new StreamReader(respStream);
                    pageContent = ioStream.ReadToEnd();
                    //// Close streams
                    ioStream.Close();
                    respStream.Close();
                    return pageContent;
                }
                return string.Empty;
            }
            catch (WebException webEx)
            {
                if (webEx.Response != null)
                {
                    using (HttpWebResponse exResponse = (HttpWebResponse)webEx.Response)
                    {
                        using (StreamReader sr = new StreamReader(exResponse.GetResponseStream()))
                        {
                            pageContent = sr.ReadToEnd();
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }

            return pageContent;
        }
    }

    public class Data
    {
        public int height { get; set; }
        public bool is_silhouette { get; set; }
        public string url { get; set; }
        public int width { get; set; }
    }

    public class Picture
    {
        public Data data { get; set; }
    }

    public class FacebookAccount
    {
        public long id { get; set; }
        public string name { get; set; }
        public Picture picture { get; set; }
    }
}

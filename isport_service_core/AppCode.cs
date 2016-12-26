using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using Google;
using Google.Apis.Services;
using Google.Apis.Urlshortener.v1;
using Google.Apis.Urlshortener.v1.Data;

namespace isport_service_core
{
    public class AppCode
    {
        #region Google API ShoterURL
        public static string GenGoogleAPI_ShoterURL(string url)
        {
            string resultURL = "";
            if (!string.IsNullOrEmpty(url))
            {
                BaseClientService.Initializer initializer = new BaseClientService.Initializer();
                // You can enter your developer key for services requiring a developer key.
                initializer.ApiKey = "AIzaSyBVEcI59w8iUqBInxCuT2-RpSwYjyHZ0cA";
                UrlshortenerService service = new UrlshortenerService(initializer);
                Url toInsert = new Url { LongUrl = url };
                toInsert = service.Url.Insert(toInsert).Execute();
                resultURL = toInsert.Id;
            }
            return resultURL;
        }

        #endregion
    }
}
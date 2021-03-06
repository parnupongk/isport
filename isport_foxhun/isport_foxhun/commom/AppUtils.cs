﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Configuration;

namespace isport_foxhun.commom
{
    public class AppUtils
    {
        public class Session
        {
            private static HttpSessionState CurrentSession
            {
                get
                {
                    return HttpContext.Current.Session;
                }
            }

            private static T GetSessionValue<T>(string name)
            {
                return (T)CurrentSession[name];
            }

            private static List<T> GetSessionList<T>(string name)
            {
                return (List<T>)CurrentSession[name];
            }

            private static string GetSessionValueString(string name)
            {
                return GetSessionValue<string>(name);
            }

            private static void SetSessionValue(string name, object value)
            {
                CurrentSession[name] = value;
            }

            public static void ClearSession()
            {
                CurrentSession.Clear();
            }
      

            public static foxhun_users User
            {
                get
                {
                    var obj = GetSessionValue<foxhun_users>("USER");
                    
                    return obj;
                }
                set
                {
                    SetSessionValue("USER", value);
                }
            }

        }
    }
}
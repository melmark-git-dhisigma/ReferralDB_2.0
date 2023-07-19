using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


    public static class Helpers
    {
        public static bool GetBool(this bool? data)
        {
            if(data==null)
            {
                return false;
            }
            else
            {
                return (bool)data;
            }
        }
    }
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace libZoi
{
    public static class NetClass
    {
        public static bool ConnectionAvailable(string strServer)
        {
            try
            {                
                if (HttpStatusCode.OK == getResponseStatus(strServer))
                {
                    // HTTP = 200 - Internet connection available, server online
                    
                    return true;
                }
                else
                {
                    // Other status - Server or connection not available
                    return false;
                }
            }
            catch (WebException)
            {
                // Exception - connection not available
                return false;
            }
        }

        public static HttpStatusCode getResponseStatus(string strServer)
        {
            HttpStatusCode retValue;
            HttpWebRequest reqFP;
            HttpWebResponse rspFP = null;
            try
            {
                reqFP = (HttpWebRequest)HttpWebRequest.Create(strServer);
                rspFP = (HttpWebResponse)reqFP.GetResponse();
                retValue = rspFP.StatusCode;
            }
            catch (Exception)
            {
                retValue = HttpStatusCode.NotFound;
            }
            finally
            {
                if (rspFP != null)
                {
                    rspFP.Close();
                }
            }
            return retValue;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Launcher
{
    static public class Helpers
    {
        static public void DownloadFile(string subFolder, string fileName)
        {
            string remoteUri = "http://www.bituser.com/nathan/Floor_Zero/";
            using (WebClient myWebClient = new WebClient())
            {
                myWebClient.DownloadFile(remoteUri + subFolder + fileName, fileName);
            }
        }
    }
}

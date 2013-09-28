using System.Net;

namespace Launcher
{
    public static class Helpers
    {
        public static void DownloadFile(string subFolder, string fileName)
        {
            string remoteUri = "http://www.bituser.com/nathan/Floor_Zero/";
            using (var myWebClient = new WebClient())
            {
                myWebClient.DownloadFile(remoteUri + subFolder + fileName, fileName);
            }
        }
    }
}
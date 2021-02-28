using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WindowsExtensions.Services
{
    /// <summary>
    /// This is only used to grab the amount of doges in the wallet
    /// </summary>
    public class HttpService
    {
        public static async Task<string> ReadDogeAmount()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://dogechain.info/chain/Dogecoin/q/addressbalance/D6VB4FVDQa4B6EdXZTXTNpx5LjDD2eGrFb");
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }

    }
}

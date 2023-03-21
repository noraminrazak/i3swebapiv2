using I3SwebAPIv2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace I3SwebAPIv2.Class
{
    public class MOcean
    {
        public async Task<string> Send_OTP(string otp, string mobile_number)
        {
            string message = string.Empty;
            DateTime date = DateTime.Now;
            message = "I3S: " + otp + " is your OTP to verify your account on " + date.ToString("dd MMM yyyy") + ". Did not request? Call: +6011 5774 6255";

            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("mocean-api-key", "9b0dce88"),
                    new KeyValuePair<string, string>("mocean-api-secret", "62c8382a"),
                    new KeyValuePair<string, string>("mocean-to", mobile_number),
                    new KeyValuePair<string, string>("mocean-from", "i-3S Admin"),
                    new KeyValuePair<string, string>("mocean-text", message),
                    new KeyValuePair<string, string>("mocean-resp-format", "json"),
                    new KeyValuePair<string, string>("mocean-charset", "UTF-8")
                });
                var result = await client.PostAsync("https://rest-api.moceansms.com/rest/1/sms", content);
                return await result.Content.ReadAsStringAsync();
            }
        }
    }
}
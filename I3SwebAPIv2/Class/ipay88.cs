using I3SwebAPIv2.Models;
using I3SwebAPIv2.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static Google.Protobuf.WellKnownTypes.Field;

namespace I3SwebAPIv2.Class
{
    public class ipay88
    {
        public string _requestUrl = "https://payment.ipay88.com.my/epayment/entry.asp";
        public string _requeryUrl = "https://payment.ipay88.com.my/epayment/webservice/TxInquiryCardDetails/TxDetailsInquiry.asmx?op=TxDetailsInquiryCardInfo";
        //demo account
        //public string _MerchantCode = "M27493";
        //public string _MerchantKey = "F2Kvbe459A";
        public string _responseUrl = "http://giis.myedutech.my/Response.aspx";
        public string _backendUrl = "http://giis.myedutech.my/api/v2/payment/response";
        //public string _responseUrl = "http://192.168.68.121/Response.aspx";
        //public string _backendUrl = "http://192.168.68.121/api/v2/payment/response";
        //production account
        public string _MerchantCode = "M38509_S0001";
        public string _MerchantKey = "ErRmyri14b";
        //public string _responseUrl = "http://gviis.myedutech.my/Response.aspx";
        //public string _backendUrl = "http://gviis.myedutech.my/api/v2/payment/response";
        public string timeStamp = "";
        public string signature = "";
        public async Task<string> PostPaymentRequest(PaymentRequestParam data)
        {
            DateTime localDate = DateTime.Now;
            timeStamp = localDate.ToString("yyyyMMddHHmmss");

            signature = GetSha256Hash(string.Concat(_MerchantKey, _MerchantCode, data.RefNo, String.Format("{0:0}", data.Amount * 100), data.Currency, data.Xfield1));
            Console.WriteLine(signature);
            using (var client = new HttpClient())
            {
                try
                {
                    //client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
                    client.DefaultRequestHeaders.Add("Referer", "http://giis.myedutech.my");
                    client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                    client.DefaultRequestHeaders.Add("Accept", "application/x-www-form-urlencoded");

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("MerchantCode", _MerchantCode),
                        new KeyValuePair<string, string>("PaymentId", data.PaymentId), //let user select from ipay88 page
                        new KeyValuePair<string, string>("RefNo", data.RefNo),
                        new KeyValuePair<string, string>("Amount", data.Amount.ToString("F")),
                        new KeyValuePair<string, string>("Currency", data.Currency),
                        new KeyValuePair<string, string>("ProdDesc", data.ProdDesc),
                        new KeyValuePair<string, string>("UserName", data.UserName),
                        new KeyValuePair<string, string>("UserEmail", data.UserEmail),
                        new KeyValuePair<string, string>("UserContact", data.UserContact),
                        new KeyValuePair<string, string>("Remark", data.Remark),
                        new KeyValuePair<string, string>("Lang", data.Lang),
                        new KeyValuePair<string, string>("SignatureType", data.SignatureType),
                        new KeyValuePair<string, string>("Signature", signature),
                        new KeyValuePair<string, string>("ResponseURL", _responseUrl),
                        new KeyValuePair<string, string>("BackendURL", _backendUrl),
                        new KeyValuePair<string, string>("appdeeplink", data.appdeeplink),
                        new KeyValuePair<string, string>("Xfield1", data.Xfield1)
                    });
                    var response = await client.PostAsync(_requestUrl, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        ExceptionUtility.LogException(ex, "ipay88/payment " + timeStamp);
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostRequeryPaymentStatus(PaymentRequeryParam data)
        {
            DateTime localDate = DateTime.Now;
            timeStamp = localDate.ToString("yyyyMMddHHmmss");

            using (var client = new HttpClient())
            {
                try
                {
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("MerchantCode", _MerchantCode),
                        new KeyValuePair<string, string>("ReferenceNo", data.ReferenceNo),
                        new KeyValuePair<string, string>("Amount", data.Amount),
                        new KeyValuePair<string, string>("Version", data.Version)
                    });
                    var response = await client.PostAsync(_requeryUrl, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        ExceptionUtility.LogException(ex, "ipay88/requery " + timeStamp);
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        public static string GetSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public string ResponseMessage(HttpStatusCode StatusCode)
        {
            string message = string.Empty;
            ResponseProperty prop = new ResponseProperty();
            prop.Success = false;
            if (StatusCode == HttpStatusCode.BadRequest)
            {
                prop.Message = WebApiResources.BadRequestText;
            }
            else if (StatusCode == HttpStatusCode.Unauthorized)
            {
                prop.Message = WebApiResources.UnauthorizedText;
            }
            else if (StatusCode == HttpStatusCode.Forbidden)
            {
                prop.Message = WebApiResources.ForbiddenText;
            }
            else if (StatusCode == HttpStatusCode.NotFound)
            {
                prop.Message = WebApiResources.NotFoundText;
            }
            else if (StatusCode == HttpStatusCode.RequestTimeout)
            {
                prop.Message = WebApiResources.RequestTimeoutText;
            }
            else if (StatusCode == HttpStatusCode.InternalServerError)
            {
                prop.Message = WebApiResources.InternalServerErrorText;
            }
            else if (StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                prop.Message = WebApiResources.ServiceUnavailableText;
            }
            return JsonConvert.SerializeObject(prop);
        }
    }
}
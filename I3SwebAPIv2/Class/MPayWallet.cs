using I3SwebAPIv2.Models;
using I3SwebAPIv2.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace I3SwebAPIv2.Class
{
    public class MPayWallet
    {
        ////uat
        //public string PID = "100000119002620";
        //public string PartnerKey = "EN35PR90NWDVV2XG3YUAABCDUH8W1ABJIHKFD8MH1H2ZXJ8EZH";
        //public string requestUrl = "https://uat.mpay.my/mpayCZ/tpwalletapi";
        //public string url = "https://staging.i-3s.com.my";
        //production
        public string PID = "100000119012320";
        public string PartnerKey = "EN35PR90NWDVV2XG3YUAAAHNUH8W1ABJIHKFD8MH1H2ZXJ8EZH";
        public string requestUrl = "https://mpay.my/mpay/tpwalletapi";
        public string url = "https://program.i-3s.com.my";
        public string authToken = "";
        public string timeStamp = "";
        public string messageBody = "";
        public String LRC = "";
        public char del = (char)30;

        private static DataTable table;

        private DataTable CreateData()
        {
            table = new DataTable();
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Number", typeof(int));
            table.Rows.Add(new object[] { "one", 1 });
            table.Rows.Add(new object[] { "two", 2 });
            table.Rows.Add(new object[] { "three", 3 });
            table.Rows.Add(new object[] { "four", 4 });
            return table;
        }

        public DataView SelectMethod(string sortExpression)
        {
            table = CreateData();

            DataView dv = new DataView(table);
            dv.Sort = sortExpression;
            return dv;
        }

        public async Task<string> PostAccountRegistration(RegisterAccParam data)
        {
            DateTime localDate = DateTime.Now;
            timeStamp = localDate.ToString("yyyyMMddHHmmss");

            authToken = GetSha256Hash(string.Concat(PID, PartnerKey, timeStamp, data.address, "1", data.city, data.dob, data.email, data.idno, data.loginid,
                        data.marketingflag, data.mobileno, data.mothermaidenname, data.name, data.nationality, data.occupation, 
                        data.parentemail, data.parentidimagefilename,data.parentidimagestring, data.parentidno,data.parentmobileno,
                        data.parentname, "1", PID ,data.postalcode,"1", data.state, data.useridimagefilename.Split('/').Last(), ConvertImageURLToBase64(url + data.useridimagestring),
                        data.userselfieimagefilename.Split('/').Last(), ConvertImageURLToBase64(url + data.userselfieimagestring)));

            LRC = authToken.ToUpper() + del + PID + del + timeStamp + del + data.address + del + "1" + del + data.city + del + data.dob + del + data.email + del + data.idno + del + data.loginid + del +
            data.marketingflag + del + data.mobileno + del + data.mothermaidenname + del + data.name + del + data.nationality + del + data.occupation + del +
            data.parentemail + del + data.parentidimagefilename + del + data.parentidimagestring + del + data.parentidno + del + data.parentmobileno + del +
            data.parentname + del + "1" + del + PID + del + data.postalcode + del + "1" + del + data.state + del + data.useridimagefilename.Split('/').Last() + del + ConvertImageURLToBase64(url + data.useridimagestring) + del +
            data.userselfieimagefilename.Split('/').Last() + del + ConvertImageURLToBase64(url + data.userselfieimagestring);

            using (var client = new HttpClient())
            {
                try
                {
                    var content = new MyFormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("authtoken", authToken.ToUpper()),
                        new KeyValuePair<string, string>("timestamp", timeStamp),
                        new KeyValuePair<string, string>("PID", PID),
                        new KeyValuePair<string, string>("registerType", "1"),
                        new KeyValuePair<string, string>("name", data.name),
                        new KeyValuePair<string, string>("nationality", data.nationality),
                        new KeyValuePair<string, string>("idno", data.idno),
                        new KeyValuePair<string, string>("email", data.email),
                        new KeyValuePair<string, string>("mobileno", data.mobileno),
                        new KeyValuePair<string, string>("dob", data.dob),
                        new KeyValuePair<string, string>("loginid", data.loginid),
                        new KeyValuePair<string, string>("address", data.address),
                        new KeyValuePair<string, string>("state", data.state),
                        new KeyValuePair<string, string>("province", data.province),
                        new KeyValuePair<string, string>("addCountry", data.addCountry),
                        new KeyValuePair<string, string>("city", data.city),
                        new KeyValuePair<string, string>("postalcode", data.postalcode),
                        new KeyValuePair<string, string>("mothermaidenname", data.mothermaidenname),
                        new KeyValuePair<string, string>("occupation", data.occupation),
                        new KeyValuePair<string, string>("employer_name", data.employer_name),
                        new KeyValuePair<string, string>("useridimagefilename", data.useridimagefilename.Split('/').Last()),
                        new KeyValuePair<string, string>("useridimagestring", ConvertImageURLToBase64(url + data.useridimagestring)),
                        new KeyValuePair<string, string>("userselfieimagefilename", data.userselfieimagefilename.Split('/').Last()),
                        new KeyValuePair<string, string>("userselfieimagestring", ConvertImageURLToBase64(url + data.userselfieimagestring)),
                        new KeyValuePair<string, string>("parentname", data.parentname),
                        new KeyValuePair<string, string>("parentemail", data.parentemail),
                        new KeyValuePair<string, string>("parentmobileno", data.parentmobileno),
                        new KeyValuePair<string, string>("parentidno", data.parentidno),
                        new KeyValuePair<string, string>("parentidimagefilename", data.parentidimagefilename),
                        new KeyValuePair<string, string>("parentidimagestring", data.parentidimagestring),
                        new KeyValuePair<string, string>("agreementflag", "1"),
                        new KeyValuePair<string, string>("pdpaFlag", "1"),
                        new KeyValuePair<string, string>("marketingFlag", data.marketingflag),
                        new KeyValuePair<string, string>("lrc", GetLRC(LRC)),
                    });

                    var response = await client.PostAsync(requestUrl + "/account/registeracc", content);
                    string result = await response.Content.ReadAsStringAsync();


                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result;
                    }

                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        ExceptionUtility.LogException(ex, "mpay/registeracc " + timeStamp);
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);

                    }
                    throw;
                }
            }
        }

        public async Task<string> PostChangePIN(ChangePINParam data)
        {
            DateTime localDate = DateTime.Now;
            timeStamp = localDate.ToString("yyyyMMddHHmmss");

            authToken = GetSha256Hash(string.Concat(PID, PartnerKey, timeStamp, data.cardtoken, data.newPin, data.oldPin, data.uid));

            LRC = authToken.ToUpper() + del + timeStamp + del + PID + del + data.cardtoken + del + data.newPin + del + data.oldPin + del + data.uid + del;

            using (var client = new HttpClient())
            {
                try
                {
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("authtoken", authToken.ToUpper()),
                        new KeyValuePair<string, string>("timestamp", timeStamp),
                        new KeyValuePair<string, string>("PID", PID),
                        new KeyValuePair<string, string>("uid", data.uid),
                        new KeyValuePair<string, string>("oldPin", data.oldPin),
                        new KeyValuePair<string, string>("newPin", data.newPin),
                        new KeyValuePair<string, string>("cardtoken", data.cardtoken),
                        new KeyValuePair<string, string>("lrc", GetLRC(LRC)),
                    });
                    var response = await client.PostAsync(requestUrl + "/account/PINChange", content);
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
                        ExceptionUtility.LogException(ex, "mpay/PINChange " + timeStamp);
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostResubmitKYC(ResubmitKYCParam data)
        {
            DateTime localDate = DateTime.Now;
            timeStamp = localDate.ToString("yyyyMMddHHmmss");

            authToken = GetSha256Hash(string.Concat(PID, PartnerKey, timeStamp, data.parentidimagefilename, data.parentidimagestring,
                        data.uid, data.useridimagefilename.Split('/').Last(), ConvertImageURLToBase64(url + data.useridimagestring), 
                        data.userselfieimagefilename.Split('/').Last(), ConvertImageURLToBase64(url + data.userselfieimagestring)));

            LRC = authToken.ToUpper() + del + PID + del + timeStamp + del + data.parentidimagefilename + del + data.parentidimagestring + del +
            data.uid + del + data.useridimagefilename.Split('/').Last() + del + ConvertImageURLToBase64(url + data.useridimagestring) + del + 
            data.userselfieimagefilename.Split('/').Last() + del + ConvertImageURLToBase64(url + data.userselfieimagestring) + del;

            using (var client = new HttpClient())
            {
                try
                {
                    var content = new MyFormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("authtoken", authToken.ToUpper()),
                        new KeyValuePair<string, string>("timestamp", timeStamp),
                        new KeyValuePair<string, string>("PID", PID),
                        new KeyValuePair<string, string>("uid", data.uid),
                        new KeyValuePair<string, string>("mothermaidenname", data.mothermaidenname),
                        new KeyValuePair<string, string>("useridimagefilename", data.useridimagefilename.Split('/').Last()),
                        new KeyValuePair<string, string>("useridimagestring", ConvertImageURLToBase64(url + data.useridimagestring)),
                        new KeyValuePair<string, string>("userselfieimagefilename", data.userselfieimagefilename.Split('/').Last()),
                        new KeyValuePair<string, string>("userselfieimagestring", ConvertImageURLToBase64(url + data.userselfieimagestring)),
                        new KeyValuePair<string, string>("parentidimagefilename", data.parentidimagefilename),
                        new KeyValuePair<string, string>("parentidimagestring", data.parentidimagestring),
                        new KeyValuePair<string, string>("lrc", GetLRC(LRC)),
                    });

                    var response = await client.PostAsync(requestUrl + "/account/reuploadoc", content);
                    string result = await response.Content.ReadAsStringAsync();


                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result;
                    }

                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        ExceptionUtility.LogException(ex, "mpay/reuploadoc " + timeStamp);
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostAccountTopup(AccountTopupParam data)
        {
            DateTime localDate = DateTime.Now;
            timeStamp = localDate.ToString("yyyyMMddHHmmss");

            authToken = GetSha256Hash(string.Concat(PID, PartnerKey, timeStamp, data.amount, data.cardtoken, data.channel, data.uid));

            LRC = authToken.ToUpper() + del + timeStamp + del + PID + del + data.amount + del + data.cardtoken + del + data.channel + del + data.uid + del;

            using (var client = new HttpClient())
            {
                try
                {
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("authtoken", authToken.ToUpper()),
                        new KeyValuePair<string, string>("timestamp", timeStamp),
                        new KeyValuePair<string, string>("PID", PID),
                        new KeyValuePair<string, string>("uid", data.uid),
                        new KeyValuePair<string, string>("channel", data.channel),
                        new KeyValuePair<string, string>("cardtoken", data.cardtoken),
                        new KeyValuePair<string, string>("amount", data.amount),
                        new KeyValuePair<string, string>("lrc", GetLRC(LRC)),
                    });

                    var response = await client.PostAsync(requestUrl + "/account/topupaccount", content);
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
                        ExceptionUtility.LogException(ex, "mpay/topupaccount " + timeStamp);
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostCardBalance(MPayBalanceParam data)
        {
            DateTime localDate = DateTime.Now;
            timeStamp = localDate.ToString("yyyyMMddHHmmss");

            authToken = GetSha256Hash(string.Concat(PID, PartnerKey, timeStamp, data.cardtoken, data.uid));

            LRC = authToken.ToUpper() + del + timeStamp + del + PID + del + data.cardtoken + del +  data.uid + del;

            using (var client = new HttpClient())
            {
                try
                {
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("authtoken", authToken.ToUpper()),
                        new KeyValuePair<string, string>("timestamp", timeStamp),
                        new KeyValuePair<string, string>("PID", PID),
                        new KeyValuePair<string, string>("uid", data.uid),
                        new KeyValuePair<string, string>("cardtoken", data.cardtoken),
                        new KeyValuePair<string, string>("lrc", GetLRC(LRC)),
                    });
                    var response = await client.PostAsync(requestUrl + "/account/getcardbalance", content);
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
                        ExceptionUtility.LogException(ex, "mpay/getcardbalance " + timeStamp);
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostDoPayment(DoPaymentParam data)
        {
            DateTime localDate = DateTime.Now;
            timeStamp = localDate.ToString("yyyyMMddHHmmss");

            authToken = GetSha256Hash(string.Concat(PID, PartnerKey, timeStamp, data.amount, data.cardpin, data.cardtoken, data.productdescription,
                data.referenceno, data.uid));

            LRC = authToken.ToUpper() + del + timeStamp + del + PID + del + data.amount + del + data.cardpin + del + data.cardtoken + 
                del + data.productdescription + del + data.referenceno + del + data.uid + del;

            using (var client = new HttpClient())
            {
                try
                {
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("authtoken", authToken.ToUpper()),
                        new KeyValuePair<string, string>("timestamp", timeStamp),
                        new KeyValuePair<string, string>("PID", PID),
                        new KeyValuePair<string, string>("uid", data.uid),
                        new KeyValuePair<string, string>("amount", data.amount),
                        new KeyValuePair<string, string>("productdescription", data.productdescription),
                        new KeyValuePair<string, string>("referenceno", data.referenceno),
                        new KeyValuePair<string, string>("cardtoken", data.cardtoken),
                        new KeyValuePair<string, string>("cardpin", data.cardpin),
                        new KeyValuePair<string, string>("lrc", GetLRC(LRC)),
                    });
                    var response = await client.PostAsync(requestUrl + "/account/dopayment", content);
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
                        ExceptionUtility.LogException(ex, "mpay/dopayment " + timeStamp);
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostDoPaymentWithMID(DoPaymentWithMIDParam data)
        {
            DateTime localDate = DateTime.Now;
            timeStamp = localDate.ToString("yyyyMMddHHmmss");

            authToken = GetSha256Hash(string.Concat(PID, PartnerKey, timeStamp, data.amount, data.cardpin, data.cardtoken, data.mpay_mid,
                data.productdescription, data.referenceno, data.uid));

            LRC = authToken.ToUpper() + del + timeStamp + del + PID + del + data.amount + del + data.cardpin + del + data.cardtoken +
                del + data.mpay_mid + del + data.productdescription + del + data.referenceno + del + data.uid + del;

            using (var client = new HttpClient())
            {
                try
                {
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("authtoken", authToken.ToUpper()),
                        new KeyValuePair<string, string>("timestamp", timeStamp),
                        new KeyValuePair<string, string>("PID", PID),
                        new KeyValuePair<string, string>("uid", data.uid),
                        new KeyValuePair<string, string>("amount", data.amount),
                        new KeyValuePair<string, string>("productdescription", data.productdescription),
                        new KeyValuePair<string, string>("referenceno", data.referenceno),
                        new KeyValuePair<string, string>("cardtoken", data.cardtoken),
                        new KeyValuePair<string, string>("cardpin", data.cardpin),
                        new KeyValuePair<string, string>("lrc", GetLRC(LRC)),
                        new KeyValuePair<string, string>("mpay_mid", data.mpay_mid),
                    });
                    var response = await client.PostAsync(requestUrl + "/account/dopaymentWithMID", content);
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
                        ExceptionUtility.LogException(ex, "mpay/dopaymentWithMID " + timeStamp);
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostFundTransfer(FundTransferParam data)
        {
            DateTime localDate = DateTime.Now;
            timeStamp = localDate.ToString("yyyyMMddHHmmss");

            authToken = GetSha256Hash(string.Concat(PID, PartnerKey, timeStamp, data.amount, data.cardpin, data.destination_cardtoken, data.reference,
                data.remark, data.source_cardtoken, data.uid, data.recipient_type));

            LRC = authToken.ToUpper() + del + timeStamp + del + PID + del + data.amount + del + data.cardpin + del + data.destination_cardtoken +
                del + data.reference + del + data.remark + del + data.source_cardtoken + del + data.uid + del + data.recipient_type + del;

            using (var client = new HttpClient())
            {
                try
                {
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("authtoken", authToken.ToUpper()),
                        new KeyValuePair<string, string>("timestamp", timeStamp),
                        new KeyValuePair<string, string>("PID", PID),
                        new KeyValuePair<string, string>("uid", data.uid),
                        new KeyValuePair<string, string>("destination_cardtoken", data.destination_cardtoken),
                        new KeyValuePair<string, string>("source_cardtoken", data.source_cardtoken),
                        new KeyValuePair<string, string>("amount", data.amount),
                        new KeyValuePair<string, string>("reference", data.reference),
                        new KeyValuePair<string, string>("cardpin", data.cardpin),
                        new KeyValuePair<string, string>("recipient_type", data.recipient_type),
                        new KeyValuePair<string, string>("remark", data.remark),
                        new KeyValuePair<string, string>("ip_address", data.ip_address),
                        new KeyValuePair<string, string>("lrc", GetLRC(LRC)),
                    });
                    var response = await client.PostAsync(requestUrl + "/account/fundTransfer", content);
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
                        ExceptionUtility.LogException(ex, "mpay/fundTransfer " + timeStamp);
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostAddVirtualBalance(AddVirtualBalanceParam data)
        {
            DateTime localDate = DateTime.Now;
            timeStamp = localDate.ToString("yyyyMMddHHmmss");

            authToken = GetSha256Hash(string.Concat(PID, PartnerKey, timeStamp, data.card_type, data.uid));

            LRC = authToken.ToUpper() + del + timeStamp + del + PID + del + data.card_type + del + data.uid + del;

            using (var client = new HttpClient())
            {
                try
                {
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("authtoken", authToken.ToUpper()),
                        new KeyValuePair<string, string>("timestamp", timeStamp),
                        new KeyValuePair<string, string>("PID", PID),
                        new KeyValuePair<string, string>("uid", data.uid),
                        new KeyValuePair<string, string>("card_type", data.card_type),
                        new KeyValuePair<string, string>("lrc", GetLRC(LRC)),
                    });
                    var response = await client.PostAsync(requestUrl + "/account/addVirtualBalance", content);
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
                        ExceptionUtility.LogException(ex, "mpay/addVirtualBalance " + timeStamp);
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostGetAccountInfo(GetAccountInfoParam data)
        {
            DateTime localDate = DateTime.Now;
            timeStamp = localDate.ToString("yyyyMMddHHmmss");

            authToken = GetSha256Hash(string.Concat(PID, PartnerKey, timeStamp, data.uid));

            LRC = authToken.ToUpper() + del + timeStamp + del + PID + del + data.uid + del;

            using (var client = new HttpClient())
            {
                try
                {
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("authtoken", authToken.ToUpper()),
                        new KeyValuePair<string, string>("timestamp", timeStamp),
                        new KeyValuePair<string, string>("PID", PID),
                        new KeyValuePair<string, string>("uid", data.uid),
                        new KeyValuePair<string, string>("lrc", GetLRC(LRC)),
                    });
                    var response = await client.PostAsync(requestUrl + "/account/getaccountinfo", content);
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
                        ExceptionUtility.LogException(ex, "mpay/getaccountinfo " + timeStamp);
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostGetTrxHistory(GetTrxHistoryParam data)
        {
            DateTime localDate = DateTime.Now;
            timeStamp = localDate.ToString("yyyyMMddHHmmss");

            authToken = GetSha256Hash(string.Concat(PID, PartnerKey, timeStamp, data.uid, data.cardtoken, data.cardpin));

            LRC = authToken.ToUpper() + del + timeStamp + del + PID + del + data.uid + del + data.cardtoken + del + data.cardpin + del;

            using (var client = new HttpClient())
            {
                try
                {
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("authtoken", authToken.ToUpper()),
                        new KeyValuePair<string, string>("timestamp", timeStamp),
                        new KeyValuePair<string, string>("PID", PID),
                        new KeyValuePair<string, string>("uid", data.uid),
                        new KeyValuePair<string, string>("cardtoken", data.cardtoken),
                        new KeyValuePair<string, string>("cardpin", data.cardpin),
                        new KeyValuePair<string, string>("lrc", GetLRC(LRC)),
                    });
                    var response = await client.PostAsync(requestUrl + "/account/GetTrxHistoryWithPIN", content);
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
                        ExceptionUtility.LogException(ex, "mpay/GetTrxHistoryWithPIN " + timeStamp);
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public static string GetLRC(String data)
        {
            long lrc;
            string HexCode = string.Empty;

            char[] charStore = data.ToCharArray();
            lrc = charStore[0];

            for (int i = 1; i < charStore.Length; i++)
            {
                lrc = lrc ^ charStore[i];
            }

            HexCode = Convert.ToString(lrc, 16).ToUpper().ToString();

            if (HexCode.Length == 1)
                HexCode = "0" + HexCode;

            return HexCode;
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

        public string ConvertImageURLToBase64(string url)
        {
            StringBuilder _sb = new StringBuilder();

            Byte[] _byte = this.GetImage(url);

            _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));

            return _sb.ToString();
        }

        private byte[] GetImage(string url)
        {
            Stream stream = null;
            byte[] buf;

            try
            {
                WebProxy myProxy = new WebProxy();
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                stream = response.GetResponseStream();

                using (BinaryReader br = new BinaryReader(stream))
                {
                    int len = (int)(response.ContentLength);
                    buf = br.ReadBytes(len);
                    br.Close();
                }

                stream.Close();
                response.Close();
            }
            catch (Exception exp)
            {
                buf = null;
            }

            return (buf);
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
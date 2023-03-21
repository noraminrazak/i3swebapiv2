using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I3SwebAPIv2.Models
{
    public class PaymentDataModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class PaymentResponseParam
    {
        public string MerchantCode { get; set; }
        public int PaymentId { get; set; }
        public string RefNo { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Remark { get; set; }
        public string TransId { get; set; }
        public string AuthCode { get; set; }
        public string Status { get; set; }
        public string ErrDesc { get; set; }
        public string Signature { get; set; }
        public string CCName { get; set; }
        public string CCNo { get; set; }
        public string S_bankname { get; set; }
        public string S_country { get; set; }
        public DateTime TranDate { get; set; }
        public string Xfield1 { get; set; }
    }

    public class RequeryResponseParam
    {
        public string MerchantCode { get; set; }
        public int PaymentId { get; set; }
        public string RefNo { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Remark { get; set; }
        public string TransId { get; set; }
        public string AuthCode { get; set; }
        public string Status { get; set; }
        public string ErrDesc { get; set; }
        public string CCName { get; set; }
        public string Creditcardno { get; set; }
        public string CardType { get; set; }
        public string S_bankname { get; set; }
        public string S_country { get; set; }
        public string BankMID { get; set; }
        public DateTime TranDate { get; set; }
    }
    public class PaymentStatusParam
    {
        public int wallet_id { get; set; }
        public string reference_number { get; set; }
        public decimal amount { get; set; }
    }

    public class RequeryParam
    {
        public string MerchantCode { get; set; }
        public int PaymentId { get; set; }
        public string RefNo { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Remark { get; set; }
        public string TransId { get; set; }
        public string AuthCode { get; set; }
        public string Status { get; set; }
        public string ErrDesc { get; set; }
        public string CCName { get; set; }
        public string Creditcardno { get; set; }
        public string CardType { get; set; }
        public string S_bankname { get; set; }
        public string S_country { get; set; }
        public string BankMID { get; set; }
        public DateTime TranDate { get; set; }
    }

    public class RequestParam {
        public string platform_name { get; set; }
        public string nric { get; set; }
        public int wallet_id { get; set; }
        public string wallet_number { get; set; }
        public decimal topup_amount { get; set; }
        public DateTime topup_date { get; set; }
        public string create_by { get; set; }
    }

    public class PaymentRequeryParam
    {
        public string ReferenceNo { get; set; }
        public string Amount { get; set; }
        public string Version { get; set; } = "4";
    }

    public class PaymentRequestParam
    {
        public string PaymentId { get; set; } = "";
        public string RefNo { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "MYR";
        public string ProdDesc { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserContact { get; set; } = "";
        public string Remark { get; set; }
        public string Lang { get; set; } = "UTF-8";
        public string SignatureType { get; set; } = "SHA256";
        public string appdeeplink { get; set; }
        public string Xfield1 { get; set; } = "";
    }
}
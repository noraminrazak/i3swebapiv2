using System.Collections.Generic;

namespace I3SwebAPIv2.Models
{
    public class ResponseProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class DoEkycParam 
    {
        public string idImgString { get; set; }
        public string selfieImgString { get; set; }
        public string idType { get; set; }
        public string timeStamp { get; set; }
    }

    public class UpdateUserInfoParam
    {
        public string uid { get; set; }
        public string occupation { get; set; }
        public string employer_name { get; set; }
        public string address { get; set; }
        public string postcode { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string add_country { get; set; }
    }

    public class RegisterAccParam
    {
        public string name { get; set; }
        public string nationality { get; set; }
        public string idno { get; set; }
        public string email { get; set; }
        public string mobileno { get; set; }
        public string dob { get; set; }
        public string loginid { get; set; }
        public string address { get; set; } = "";
        public string state { get; set; } = "";
        public string province { get; set; } = "";
        public string addCountry { get; set; } = "";
        public string city { get; set; } = "";
        public string postalcode { get; set; } = "";
        public string mothermaidenname { get; set; } = "";
        public string occupation { get; set; }
        public string employer_name { get; set; } = "";
        public string useridimagefilename { get; set; }
        public string useridimagestring { get; set; }
        public string userselfieimagefilename { get; set; }
        public string userselfieimagestring { get; set; }
        public string parentname { get; set; } = "";
        public string parentemail { get; set; } = "";
        public string parentmobileno { get; set; } = "";
        public string parentidno { get; set; } = "";
        public string parentidimagefilename { get; set; } = "";
        public string parentidimagestring { get; set; } = "";
        public string marketingflag { get; set; }
    }

    public class ResubmitKYCParam
    {
        public string mothermaidenname { get; set; } = "";
        public string useridimagefilename { get; set; }
        public string useridimagestring { get; set; }
        public string userselfieimagefilename { get; set; }
        public string userselfieimagestring { get; set; }
        public string parentidimagefilename { get; set; } = "";
        public string parentidimagestring { get; set; } = "";
        public string uid { get; set; }
    }

    public class RegisterAccModel
    {
        public Header Header { get; set; }
        public BodyReg Body { get; set; }
    }

    public class ResubmitKYCModel
    {
        public Header Header { get; set; }
        public BodyResub Body { get; set; }
    }

    public class Header
    {
        public string status { get; set; }
        public string message { get; set; }
    }

    public class BodyResub
    {

    }

    public class BodyReg
    {
        public UserAccInfo useracc_info { get; set; }
        public List<CardInfo> cardinfo { get; set; }
    }
    public class UserAccInfo
    {
        public string uid { get; set; }
        public string name { get; set; }
        public string idno { get; set; }
        public string email { get; set; }
        public string mobileno { get; set; }
    }

    public class CardInfo
    {
        public string card_id { get; set; }
        public string mask_cardno { get; set; }
        public string cardtoken { get; set; }
        public string cardtype { get; set; }
        public string cardGroup { get; set; }
        public string card_temporary_pin { get; set; }
    }

    public class ChangPINModel
    {
        public Header Header { get; set; }
        public BodyPin Body { get; set; }
    }

    public class BodyPin
    {
        public string pin_change_status { get; set; }
        public string masked_card_no { get; set; }
    }

    public class ChangePINParam
    {
        public string wallet_id { get; set; }
        public string card_id { get; set; }
        public string uid { get; set; }
        public string oldPin { get; set; }
        public string newPin { get; set; }
        public string cardtoken { get; set; }
    }

    public class AccountTopupParam
    {
        public string amount { get; set; }
        public string cardtoken { get; set; }
        public string channel { get; set; }
        public string uid { get; set; }
    }

    public class AccountTopupModel
    {
        public Header Header { get; set; }
        public BodyTopup Body { get; set; }
    }

    public class FundTransferModel
    {
        public Header Header { get; set; }
        public BodyTransfer Body { get; set; }
    }
    public class BodyTransfer
    {
        public string sourceAccount { get; set; }
        public string destinationAccount { get; set; }
        public string transfer_out_reference_id { get; set; }
        public string transfer_in_reference_id { get; set; }
        public string amount { get; set; }
        public string trx_date { get; set; }

    }

    public class CardBalanceModel
    {
        public Header Header { get; set; }
        public BodyCard Body { get; set; }
    }

    public class BodyCard
    {
        public CardList cardlist { get; set; }
    }

    public class CardList
    {
        public string accountType { get; set; }
        public string accountGroup { get; set; }
        public string accountTypeName { get; set; }
        public string cardno { get; set; }
        public string balance { get; set; }
        public string name { get; set; }
        public string accStatus { get; set; }
    }

    public class BodyTopup
    {
        public TopupInfo topupinfo { get; set; }
    }
    public class TopupInfo
    {
        public string topupurl { get; set; }
        public string reference_id { get; set; }
    }
    public class AccountInfoParam
    {
        public string username { get; set; }
    }

    public class AccountStatusParam
    {
        public string wallet_number { get; set; }
        public int profile_id { get; set; }
    }

    public class AccountTransactionParam
    {
        public string authtoken { get; set; }
        public string timestamp { get; set; }
        public string PID { get; set; }
        public string uid { get; set; }
        public string masked_card_no { get; set; }
        public string amount { get; set; }
        public string master_trx_id { get; set; }
        public string payment_ref_id { get; set; }
        public string merchant_name { get; set; }
        public string trx_date { get; set; }
        public string notification_id { get; set; }
        public string lrc { get; set; }
    }

    public class AccountInfoModel
    {
        public Header Header { get; set; }
        public BodyAccInfo Body { get; set; }
    }
    public class BodyAccInfo
    {
        public AccInfo accountinfo { get; set; }
    }

    public class AccInfoDataModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<AccInfo> Data { get; set; }
    }

    public class AccInfo
    {
        public string countrylookup_id { get; set; }
        public string mobile_no { get; set; }
        public string id_no { get; set; }
        public string dob { get; set; }
        public string name { get; set; }
        public string userstatuslookup_id { get; set; }
        public string kycstatuslookup_id { get; set; }
        public string docstatuslookup_id { get; set; }
        public string occupation { get; set; }
        public string occupation_flag { get; set; }
        public string address_flag { get; set; }
    }

    public class AccStatusDataModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<AccStatus> Data { get; set; }
    }

    public class AccStatus
    {
        public string mpay_uid { get; set; }
        public string account_status_id { get; set; }
        public string account_status { get; set; }
        public string kyc_status_id { get; set; }
        public string kyc_status { get; set; }
    }

    public class AccBalanceDataModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<AccBalance> Data { get; set; }
    }

    public class AccBalance
    {
        public string balance { get; set; }
    }

    public class TrxHistoryDataModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<TrxHist> Data { get; set; }
    }

    public class TrxHistoryModel
    {
        public Header Header { get; set; }
        public BodyTrxHist Body { get; set; }
    }
    public class BodyTrxHist
    {
        public List<TrxHist> trxhist_info { get; set; }
    }

    public class TrxHist
    {
        public string datetimecreated { get; set; }
        public string trx_detail { get; set; }
        public string credit_amount { get; set; }
        public string debit_amount { get; set; }
        public string Currency { get; set; }
        public string masked_acc_no { get; set; }
    }

    public class AccountUpdateUserParam
    {
        public string authtoken { get; set; }
        public string timestamp { get; set; }
        public string UID { get; set; }
        public string dob { get; set; }
        public string email { get; set; }
        public string idno { get; set; }
        public string loginid { get; set; }
        public string mobileno { get; set; }
        public string name { get; set; }
        public string occupation { get; set; }
        public string lrc { get; set; }
    }

    public class UpdateKYCStatusParam
    {
        public string authtoken { get; set; }
        public string timestamp { get; set; }
        public string uid { get; set; }
        public string notification_id { get; set; }
        public string lrc { get; set; }
    }

    public class UpdateAccountStatusParam
    {
        public string authtoken { get; set; }
        public string timestamp { get; set; }
        public string PID { get; set; }
        public string cardtoken { get; set; }
        public string account_status { get; set; }
        public string UID { get; set; }
        public string lrc { get; set; }
    }

    public class DoPaymentModel
    {
        public Header Header { get; set; }
        public BodyPayAuth Body { get; set; }
    }

    public class BodyPayAuth
    {
        public string transactionstatus { get; set; }
        public string transactionstatusdesc { get; set; }
        public string responseauthtoken { get; set; }
        public string maskedcardno { get; set; }
        public string amount { get; set; }
        public string authcode { get; set; }
        public string mpayrefno { get; set; }
    }

    //public class PayAuth
    //{
    //    public string transactionstatus { get; set; }
    //    public string transactionstatusdesc { get; set; }
    //    public string responseauthtoken { get; set; }
    //    public string maskedcardno { get; set; }
    //    public string amount { get; set; }
    //    public string authcode { get; set; }
    //    public string mpayrefno { get; set; }
    //}
}
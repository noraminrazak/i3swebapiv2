using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I3SwebAPIv2.Models
{
    public class WalletDataModel
    {
        public bool Success { get; set; } = false;
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class WalletTopupParam
    {
        public int wallet_id { get; set; }
        public string wallet_number { get; set; }
        public decimal topup_amount { get; set; }
        public DateTime topup_date { get; set; }
        public string create_by { get; set; }
    }

    public class MPayTopupStatusParam
    {
        public string username { get; set; }
        public string reference_id { get; set; }
        public string update_by { get; set; }
    }

    public class PurchaseParam
    {
        public int wallet_id { get; set; }
        public string wallet_number { get; set; }
        public decimal amount { get; set; }

        public string create_by { get; set; }
    }

    public class CardBalanceParam
    {
        public string username { get; set; }
    }

    public class MPayBalanceParam
    {
        public string uid { get; set; }
        public string cardtoken { get; set; }
    }

    public class DoPaymentParam
    {
        public string uid { get; set; }
        public string amount { get; set; }
        public string productdescription { get; set; }
        public string referenceno { get; set; }
        public string cardtoken { get; set; }
        public string cardpin { get; set; }
    }

    public class DoPaymentWithMIDParam
    {
        public string uid { get; set; }
        public string amount { get; set; }
        public string productdescription { get; set; }
        public string referenceno { get; set; }
        public string cardtoken { get; set; }
        public string cardpin { get; set; }
        public string mpay_mid { get; set; }
    }

    public class AddVirtualBalanceParam
    {
        public string uid { get; set; }
        public string card_type { get; set; } = "1";
    }

    public class GetAccountInfoParam
    {
        public string uid { get; set; }
    }
    public class GetTrxHistoryParam
    {
        public string uid { get; set; }
        public string cardtoken { get; set; }
        public string cardpin { get; set; }
    }


    public class AddVirtualBalanceModel
    {
        public Header Header { get; set; }
        public BodyAddCard Body { get; set; }
    }

    public class BodyAddCard
    {
        public List<CardInfo> cardinfo { get; set; }
    }

    public class FundTransferParam
    {
        public string uid { get; set; }
        public string destination_cardtoken { get; set; }
        public string source_cardtoken { get; set; }
        public string amount { get; set; }
        public string reference { get; set; } = "Transfer Of Funds between persons Normal and Juridical";
        public string cardpin { get; set; }
        public string recipient_type { get; set; } = "5";
        public string remark { get; set; }
        public string ip_address { get; set; }
    }

    public class WalletTopupStatusParam
    {
        public int wallet_id { get; set; }
        public string reference_id { get; set; }
        public decimal topup_amount { get; set; }
        public int status_id { get; set; }
        public string update_by { get; set; }
    }

    public class WalletTransactionHistoryParam
    {
        public string wallet_number { get; set; }
    }

    public class SystemVersionParam
    {
        public string platform { get; set; }
    }

    public class WalletTransactionReferenceParam
    {
        public int transaction_id { get; set; }
        public string reference_number { get; set; }
    }

    public class WalletTransactionDetailParam
    {
        public string wallet_number { get; set; }
        public string reference_number { get; set; }
    }

    public class WalletTransferParam
    {
        public int wallet_id { get; set; }
        public string wallet_number { get; set; }
        public int recipient_id { get; set; }
        public string recipient_wallet { get; set; }
        public decimal transfer_amount { get; set; }
        public DateTime transfer_date { get; set; }
        public string ip_address { get; set; }
        public string create_by { get; set; }
    }

    public class WalletTransactionHistoryModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<WalletTransactionHistory> Data { get; set; }
    }

    public class WalletTransactionHistory
    {
        public int transaction_id { get; set; }
        public string reference_number { get; set; }
        public int transaction_type_id { get; set; }
        public string transaction_type { get; set; }
        public string transaction_type_bm { get; set; }
        public string wallet_number { get; set; }
        public string full_name { get; set; }
        public string wallet_number_reference { get; set; }
        public string full_name_reference { get; set; }
        public decimal amount { get; set; }
        public int status_id { get; set; }
        public string status_code { get; set; }
        public string status_code_bm { get; set; }
        public DateTime create_at { get; set; }
    }

    public class WalletTransactionReferenceModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<WalletTransactionReference> Data { get; set; }
    }

    public class WalletTransactionReference
    {
        public int transaction_id { get; set; }
        public string reference_number { get; set; }
        public int transaction_type_id { get; set; }
        public string transaction_type { get; set; }
        public string transaction_type_bm { get; set; }
        public string wallet_number { get; set; }
        public string full_name { get; set; }
        public string wallet_number_reference { get; set; }
        public string full_name_reference { get; set; }
        public decimal amount { get; set; }
        public int status_id { get; set; }
        public string status_code { get; set; }
        public string status_code_bm { get; set; }
        public DateTime create_at { get; set; }
    }

    public class WalletTransactionDetailModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<WalletTransactionDetail> Data { get; set; }
    }

    public class WalletTransactionDetail
    {
        public int rcpt_detail_id { get; set; }
        public int product_id { get; set; }
        public string product_name { get; set; }
        public int product_qty { get; set; }
        public decimal unit_price { get; set; }
        public decimal sub_total_amount { get; set; }
        public decimal discount_amount { get; set; }
        public decimal total_amount { get; set; }
    }

    public class WalletTransactionMasterModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<WalletTransactionMaster> Data { get; set; }
    }

    public class WalletTransactionMaster
    {
        public decimal sub_total_amount { get; set; }
        public string tax_rate { get; set; }
        public decimal tax_amount { get; set; }
        public decimal total_amount { get; set; }
    }
}
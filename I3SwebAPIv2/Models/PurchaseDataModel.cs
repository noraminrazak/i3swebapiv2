using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I3SwebAPIv2.Models
{
    public class PurchaseDataModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class CartCountDataModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public int Total { get; set; }
    }



    public class CartTotalModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public decimal Total { get; set; }
    }

    public class CartTotalDataModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<CartTotal> Data { get; set; }
    }

    public class CartTotal {
        public decimal sub_total_amount { get; set; }
        public int tax_rate { get; set; }
        public decimal tax_amount { get; set; }
        public decimal total_amount { get; set; }
    }

    public class CartTotalHistoryDataModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<CartTotalHistory> Data { get; set; }
    }

    public class CartTotalHistory
    {
        public decimal sub_total_amount { get; set; }
        public string tax_rate { get; set; }
        public decimal tax_amount { get; set; }
        public decimal total_amount { get; set; }
    }
    public class PurchaseModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }

        public List<Purchase> Data { get; set; }
    }

    public class DoPayment 
    {
        public int merchant_id { get; set; }
        public int profile_id { get; set; }
        public int school_id { get; set; }
        public string mpay_mid { get; set; }
        public decimal total_amount { get; set; }
        public string product_desc { get; set; }
    }

    public class Purchase
    {
        public int cart_id { get; set; }
        public int profile_id { get; set; }
        public int wallet_id { get; set; }
        public string wallet_number { get; set; }
        public string full_name { get; set; }
        public int merchant_id { get; set; }
        public string company_name { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int class_id { get; set; }
        public string class_name { get; set; }
        public int recipient_id { get; set; }
        public int recipient_role_id { get; set; }
        public string recipient_role { get; set; }
        public string recipient_name { get; set; }
        public DateTime pickup_date { get; set; }
        public TimeSpan pickup_time { get; set; }
        public int service_method_id { get; set; }
        public string delivery_location { get; set; }
        public int product_id { get; set; }
        public string product_photo_url { get; set; }
        public string product_name { get; set; }
        public string product_description { get; set; }
        public decimal unit_price { get; set; }
        public int product_qty { get; set; }
        public decimal sub_total_amount { get; set; }
        public decimal tax_amount { get; set; }
        public decimal total_amount { get; set; }
        public int order_status_id { get; set; }
        public string order_status { get; set; }
        public string order_status_bm { get; set; }
        public string create_by { get; set; }
    }

    public class PurchaseDateModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }

        public List<PurchaseDate> Data { get; set; }
    }
    public class PurchaseDate
    {
        public DateTime pickup_date { get; set; }
    }

    public class SettlementDateModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }

        public List<SettlementDate> Data { get; set; }
    }
    public class SettlementDate
    {
        public DateTime receipt_date { get; set; }
    }

    public class OrderHistoryMasterModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<OrderHistoryMaster> Data { get; set; }
    }
    public class OrderHistoryMaster
    {
        public int order_id { get; set; }
        public int profile_id { get; set; }
        public int wallet_id { get; set; }
        public string wallet_number { get; set; }
        public int user_role_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public string reference_number { get; set; }
        public decimal total_amount { get; set; }
        public decimal sub_total_amount { get; set; }
        public int order_status_id { get; set; }
        public string order_status { get; set; }
        public string order_status_bm { get; set; }
        public string payment_method { get; set; }
        public string payment_method_bm { get; set; }
        public DateTime create_at { get; set; }

    }

    public class OrderHistoryDetailModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<OrderHistoryDetail> Data { get; set; }
    }
    public class OrderHistoryDetail
    {
        public int order_detail_id { get; set; }
        public int order_id { get; set; }
        public int wallet_id { get; set; }
        public string wallet_number { get; set; }
        public string reference_number { get; set; }
        public int recipient_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int class_id { get; set; }
        public string class_name { get; set; }
        public DateTime pickup_date { get; set; }
        public TimeSpan pickup_time { get; set; }
        public int service_method_id { get; set; }
        public string delivery_location { get; set; }
        public int merchant_id { get; set; }
        public string company_name { get; set; }
        public int product_id { get; set; }
        public string product_name { get; set; }
        public string product_photo_url { get; set; }
        public decimal unit_price { get; set; }
        public int product_qty { get; set; }
        public decimal sub_total_amount { get; set; }
        public decimal discount_amount { get; set; }
        public decimal total_amount { get; set; }

    }
    public class OrderHistoryMasterParam
    {
        public int profile_id { get; set; }
        public int wallet_id { get; set; }
    }
    public class OrderHistoryDetailParam
    {
        public string wallet_number { get; set; }
        public string reference_number { get; set; }
    }

    public class OrderHistoryDetailGroupParam
    {
        public string wallet_number { get; set; }
        public string reference_number { get; set; }
    }

    public class OrderHistoryDetailDateParam
    {
        public string wallet_number { get; set; }
        public string reference_number { get; set; }
        public DateTime pickup_date { get; set; }
    }
}
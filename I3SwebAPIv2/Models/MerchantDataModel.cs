using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I3SwebAPIv2.Models
{
    public class MerchantParam
    {
        public int merchant_id { get; set; }

    }

    public class MerchantTerminalParam
    {
        public int merchant_id { get; set; }
        public int school_id { get; set; }
        public DateTime receipt_date { get; set; }
    }

    public class MerchantSalesParam
    {
        public int merchant_id { get; set; }
        public int school_id { get; set; }
    }

    public class MerchantSettlementParam
    {
        public int merchant_id { get; set; }
        public int school_id { get; set; }
        public DateTime receipt_date { get; set; }
    }
    public class MerchantSalesMethodParam
    {
        public int merchant_id { get; set; }
        public int school_id { get; set; }
        public DateTime receipt_date { get; set; }
    }

    public class MerchantTerminalReceiptParam
    {
        public int merchant_id { get; set; }
        public int school_id { get; set; }
        public int terminal_id { get; set; }
        public DateTime receipt_date { get; set; }
    }

    public class MerchantTerminalReceiptDetailParam
    {
        public int rcpt_id { get; set; }
        public string wallet_number { get; set; }
        public string reference_number { get; set; }
    }

    public class MerchantDataModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class MerchantTypeModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<MerchantType> Data { get; set; }
    }

    public class MerchantType
    {
        public int merchant_type_id { get; set; }
        public string merchant_type { get; set; }
        public string merchant_type_bm { get; set; }
    }

    public class MerchantSchoolModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<MerchantSchool> Data { get; set; }
    }

    public class MerchantSchool
    {
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int school_type_id { get; set; }
        public string school_type { get; set; }
        public string city { get; set; }
        public string state_name { get; set; }
        public string country_name { get; set; }
        public string status_code { get; set; }
    }

    public class MerchantModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<Merchant> Data { get; set; }
    }

    public class Merchant
    {
        public int merchant_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public string company_name { get; set; }
        public int merchant_type_id { get; set; }
        public string merchant_type { get; set; }
        public string registration_number { get; set; }
        public string contact_number { get; set; }
        public string person_in_charge { get; set; }
    }

    public class MerchantTerminalModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<MerchantTerminal> Data { get; set; }
    }

    public class MerchantTerminal
    {
        public int terminal_id { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public string school_type { get; set; }
        public string terminal_model { get; set; }
        public string tag_number { get; set; }
        public string serial_number { get; set; }
        public string hardware_status { get; set; }
        public decimal total_amount { get; set; }
        public int total_transaction { get; set; }
    }

    public class MerchantTerminalReceiptModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<MerchantTerminalReceipt> Data { get; set; }
    }

    public class MerchantTerminalReceipt
    {
        public int rcpt_id { get; set; }
        public int wallet_id { get; set; }
        public string wallet_number { get; set; }
        public string reference_number { get; set; }
        public string full_name { get; set; }
        public DateTime receipt_date { get; set; }
        public decimal total_amount { get; set; }
        public string payment_method { get; set; }
        public string payment_method_bm { get; set; }
        public string company_name { get; set; }
    }

    public class CreateProductCategoryParam
    {
        public int school_id { get; set; }
        public int merchant_id { get; set; }
        public string category_name { get; set; }
        public string category_description { get; set; }
        public string create_by { get; set; }
    }

    public class CreateProductDetailParam
    {
        public int merchant_id { get; set; }
        public int category_id { get; set; }
        public string product_name { get; set; }
        public string product_sku { get; set; }
        public string photo_base64 { get; set; }
        public string file_name { get; set; }
        public decimal unit_price { get; set; }
        public decimal cost_price { get; set; }
        public decimal discount_amount { get; set; }
        public string product_description { get; set; }
        public string product_ingredient { get; set; }
        public string product_weight { get; set; }
        public string text_color { get; set; }
        public string background_color { get; set; }
        public string create_by { get; set; }
        public string special_flag { get; set; }
        public string available_day { get; set; }
    }

    public class CreateProductNutritionParam
    {
        public int product_id { get; set; }
        public string nutrition_name { get; set; }
        public string per_gram { get; set; }
        public string per_serving { get; set; }
        public string create_by { get; set; }
    }

    public class UpdateProductCategoryParam
    {
        public int school_id { get; set; }
        public int merchant_id { get; set; }
        public int category_id { get; set; }
        public string category_name { get; set; }
        public string category_description { get; set; }
        public string update_by { get; set; }
    }

    public class UpdateOrderStatusParam
    {
        public int merchant_id { get; set; }
        public int school_id { get; set; }
        public string order_id { get; set; }
        public DateTime order_date { get; set; }
        public int order_status_id { get; set; }
        public string update_by { get; set; }
    }

    public class UpdateProductDetailParam
    {
        public int product_id { get; set; }
        public int merchant_id { get; set; }
        public int category_id { get; set; }
        public string product_name { get; set; }
        public string product_sku { get; set; }
        public string file_name { get; set; }
        public string photo_base64 { get; set; }
        public decimal unit_price { get; set; }
        public decimal cost_price { get; set; }
        public decimal discount_amount { get; set; }
        public string product_description { get; set; }
        public string product_ingredient { get; set; }
        public string product_weight { get; set; }
        public string text_color { get; set; }
        public string background_color { get; set; }
        public string update_by { get; set; }
        public string special_flag { get; set; }
        public string available_day { get; set; }
    }

    public class RemoveProductPhotoParam
    {
        public int product_id { get; set; }
        public int merchant_id { get; set; }
        public int category_id { get; set; }
        public string update_by { get; set; }
    }

    public class RemovePostPhotoParam
    {
        public int post_id { get; set; }
        public int school_id { get; set; }
        public int class_id { get; set; }
        public int club_id { get; set; }
        public int staff_id { get; set; }
        public int post_group_id { get; set; }
        public string update_by { get; set; }
    }

    public class UpdateProductNutritionParam
    {
        public int product_id { get; set; }
        public int info_id { get; set; }
        public string nutrition_name { get; set; }
        public string per_gram { get; set; }
        public string per_serving { get; set; }
        public string update_by { get; set; }
    }

    public class DeleteProductCategoryParam
    {
        public int school_id { get; set; }
        public int merchant_id { get; set; }
        public int category_id { get; set; }
        public string update_by { get; set; }
    }

    public class DeleteProductDetailParam
    {
        public int product_id { get; set; }
        public int merchant_id { get; set; }
        public int category_id { get; set; }
        public string update_by { get; set; }
    }

    public class DeleteProductNutritionParam
    {
        public int product_id { get; set; }
        public int info_id { get; set; }
        public string update_by { get; set; }
    }

    public class ProductCategoryModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ProductCategory> Data { get; set; }
    }

    public class ProductCategory
    {
        public int category_id { get; set; }
        public int merchant_id { get; set; }
        public int school_id { get; set; }
        public string category_name { get; set; }
        public string category_description { get; set; }
        public int total_product { get; set; }
    }

    public class ProductCategoryParam
    {
        public int school_id { get; set; }
        public int merchant_id { get; set; }
        public string special_flag { get; set; }
    }

    public class ProductDetailModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ProductDetail> Data { get; set; }
    }

    public class ProductDetail
    {
        public int product_id { get; set; }
        public int category_id { get; set; }
        public string category_name { get; set; }
        public int merchant_id { get; set; }
        public int school_id { get; set; }
        public string product_name { get; set; }
        public string product_sku { get; set; }
        public string photo_url { get; set; }
        public decimal unit_price { get; set; }
        public decimal cost_price { get; set; }
        public decimal discount_amount { get; set; }
        public string product_description { get; set; }
        public string product_weight { get; set; }
        public string product_ingredient { get; set; }
        public string special_flag { get; set; }
        public string available_day { get; set; }
        public string text_color { get; set; }
        public string background_color { get; set; }
    }

    public class ProductDetailParam
    {
        public int category_id { get; set; }
        public int merchant_id { get; set; }
        public int school_id { get; set; }
        public string special_flag { get; set; }
        public string available_day { get; set; }
    }

    public class ProductNutritionModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ProductNutrition> Data { get; set; }
    }

    public class ProductNutrition
    {
        public int info_id { get; set; }
        public int product_id { get; set; }
        public int merchant_id { get; set; }
        public int school_id { get; set; }
        public string nutrition_name { get; set; }
        public string per_gram { get; set; }
        public string per_serving { get; set; }
    }

    public class ProductNutritionParam
    {
        public int product_id { get; set; }
        public int merchant_id { get; set; }
        public int school_id { get; set; }
    }

    public class CreateOrderCartParam
    {
        public int profile_id { get; set; }
        public int wallet_id { get; set; }
        public int merchant_id { get; set; }
        public int school_id { get; set; }
        public int user_role_id { get; set; }
        public int recipient_id { get; set; }
        public int recipient_role_id { get; set; }
        public DateTime pickup_date { get; set; }
        public TimeSpan pickup_time { get; set; }
        public int service_method_id { get; set; }
        public string delivery_location { get; set; }
        public int product_id { get; set; }
        public int product_qty { get; set; }
        public string create_by { get; set; }
    }

    public class UpdateOrderCartParam
    {
        public int cart_id { get; set; }
        public int profile_id { get; set; }
        public int wallet_id { get; set; }
        public int merchant_id { get; set; }
        public int school_id { get; set; }
        public int user_role_id { get; set; }
        public int recipient_id { get; set; }
        public DateTime pickup_date { get; set; }
        public int product_id { get; set; }
        public int product_qty { get; set; }
        public string update_by { get; set; }
    }

    public class DeleteOrderCartParam
    {
        public int cart_id { get; set; }
        public int profile_id { get; set; }
        public int recipient_id { get; set; }
        public int product_id { get; set; }
        public string update_by { get; set; }
    }

    public class RemoveOrderCartParam
    {
        public int order_id { get; set; }
        public int profile_id { get; set; }
        public int wallet_id { get; set; }
        public string update_by { get; set; }
    }
    public class CartCountParam
    {
        public int profile_id { get; set; }
        public int wallet_id { get; set; }
    }
    public class OrderCartParam
    {
        public int profile_id { get; set; }
        public int wallet_id { get; set; }
        public int order_status_id { get; set; }
    }

    public class CheckOutParam
    {
        public int profile_id { get; set; }
        public int wallet_id { get; set; }
        public int order_status_id { get; set; }
        public int user_role_id { get; set; }
        public decimal sub_total_amount { get; set; }
        public decimal tax_amount { get; set; }
        public decimal total_amount { get; set; }
        public int payment_method_id { get; set; }
        public string create_by { get; set; }
    }

    public class OrderCartPickupParam
    {
        public int profile_id { get; set; }
        public int wallet_id { get; set; }
        public int order_status_id { get; set; }
        public DateTime pickup_date { get; set; }
    }
    public class OrderHistoryGroupParam
    {
        public int school_id { get; set; }
        public int merchant_id { get; set; }
    }
    public class OrderHistoryParam
    {
        public int school_id { get; set; }
        public int merchant_id { get; set; }
        public DateTime pickup_date { get; set; }
    }
    public class StudentOrderHistoryParam
    {
        public int merchant_id { get; set; }
        public int school_id { get; set; }
        public int class_id { get; set; }
        public DateTime pickup_date { get; set; }
    }
    public class StaffOrderHistoryParam
    {
        public int order_id { get; set; }
        public int school_id { get; set; }
        public DateTime pickup_date { get; set; }
    }
    public class DetailOrderHistoryParam
    {
        public int merchant_id { get; set; }
        public int school_id { get; set; }
        public int class_id { get; set; }
        public DateTime pickup_date { get; set; }
        public int profile_id { get; set; }
        public int product_id { get; set; }
    }
    public class OrderHistoryModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<OrderHistory> Data { get; set; }
    }
    public class OrderHistory
    {
        public int total_order { get; set; }
        public decimal total_amount { get; set; }
        public int merchant_id { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int class_id { get; set; }
        public string class_name { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public DateTime pickup_date { get; set; }
        public TimeSpan pickup_time { get; set; }
        public int service_method_id { get; set; }
        public string delivery_location { get; set; }
        public string order_id { get; set; }
        public int order_status_id { get; set; }
        public string order_status { get; set; }
        public string order_status_bm { get; set; }
    }

    public class StudentOrderHistoryModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<StudentOrderHistory> Data { get; set; }
    }
    public class StudentOrderHistory
    {
        public DateTime pickup_date { get; set; }
        public decimal total_amount { get; set; }
        public int recipient_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int class_id { get; set; }
        public string class_name { get; set; }
    }
    public class ProductOrderHistoryModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ProductOrderHistory> Data { get; set; }
    }
    public class ProductOrderHistory
    {
        public DateTime pickup_date { get; set; }
        public int product_id { get; set; }
        public string product_name { get; set; }
        public string product_photo_url { get; set; }
        public decimal unit_price { get; set; }
        public int product_qty { get; set; }
        public decimal total_amount { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int class_id { get; set; }
        public string class_name { get; set; }
    }

    public class ProductDetailOrderHistoryModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ProductDetailOrderHistory> Data { get; set; }
    }
    public class ProductDetailOrderHistory
    {
        public DateTime pickup_date { get; set; }
        public int recipient_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int class_id { get; set; }
        public string class_name { get; set; }
        public int product_id { get; set; }
        public string product_name { get; set; }
        public string product_photo_url { get; set; }
        public decimal unit_price { get; set; }
        public int product_qty { get; set; }
        public decimal sub_total_amount { get; set; }
        public decimal discount_amount { get; set; }
        public decimal total_amount { get; set; }
    }
    public class StudentDetailOrderHistoryModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<StudentDetailOrderHistory> Data { get; set; }
    }
    public class StudentDetailOrderHistory
    {
        public DateTime pickup_date { get; set; }
        public int recipient_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int class_id { get; set; }
        public string class_name { get; set; }
        public int product_id { get; set; }
        public string product_name { get; set; }
        public string product_photo_url { get; set; }
        public decimal unit_price { get; set; }
        public int product_qty { get; set; }
        public decimal sub_total_amount { get; set; }
        public decimal discount_amount { get; set; }
        public decimal total_amount { get; set; }

    }

    public class MerchantSalesModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<MerchantSales> Data { get; set; }
    }

    public class MerchantSales
    {
        public int merchant_id { get; set; }
        public DateTime receipt_date { get; set; }
        public decimal total_amount { get; set; }
    }

    public class MerchantSettlementModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<MerchantSettlement> Data { get; set; }
    }

    public class MerchantSettlement
    {
        public int merchant_id { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int sales_method_id { get; set; }
        public string sales_method { get; set; }
        public string sales_method_bm { get; set; }
        public DateTime receipt_date { get; set; }
        public decimal total_amount { get; set; }
        public decimal settlement_amount { get; set; }
        public decimal fee_amount { get; set; }
        public decimal net_amount { get; set; }
        public int status_id { get; set; }
        public string status { get; set; }
        public string status_bm { get; set; }
        public DateTime payment_date { get; set; }
        public string reference_number { get; set; }
    }

    public class SettlementReportModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<SettlementReport> Data { get; set; }
    }

    public class SettlementReport
    {
        public int merchant_id { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int sales_method_id { get; set; }
        public string sales_method { get; set; }
        public string sales_method_bm { get; set; }
        public DateTime receipt_date { get; set; }
        public decimal total_amount { get; set; }
        public decimal fee_amount { get; set; }
        public decimal net_amount { get; set; }
        public decimal settlement_amount { get; set; }
        public int total_receipt { get; set; }
        public string status { get; set; }
        public string status_description { get; set; }
    }

    public class MerchantSalesMethodModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<MerchantSalesMethod> Data { get; set; }
    }

    public class MerchantSalesMethod
    {
        public int merchant_id { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public DateTime receipt_date { get; set; }
        public decimal total_amount { get; set; }
        public string sales_method { get; set; }
        public string sales_method_bm { get; set; }
    }
}
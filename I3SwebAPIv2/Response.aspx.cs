using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace I3SwebAPIv2
{
    public partial class Response : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (Request.Form["MerchantCode"] != null)
                MerchantCode.Value = Request.Form["MerchantCode"];
            if (Request.Form["PaymenId"] != null)
                PaymenId.Value = Request.Form["PaymenId"];
            if (Request.Form["RefNo"] != null)
                RefNo.Value = Request.Form["RefNo"];
            if (Request.Form["Amount"] != null)
                Amount.Value = Request.Form["Amount"];
            if (Request.Form["Currency"] != null)
                Currency.Value = Request.Form["Currency"];
            if (Request.Form["Remark"] != null)
                Remark.Value = Request.Form["Remark"];
            if (Request.Form["TransId"] != null)
                TransId.Value = Request.Form["TransId"];
            if (Request.Form["AuthCode"] != null)
                AuthCode.Value = Request.Form["AuthCode"];
            if (Request.Form["Status"] != null)
                Status.Value = Request.Form["Status"];
            if (Request.Form["ErrDesc"] != null)
                ErrDesc.Value = Request.Form["ErrDesc"];
            if (Request.Form["Signature"] != null)
                Signature.Value = Request.Form["Signature"];
            if (Request.Form["CCName"] != null)
                CCName.Value = Request.Form["CCName"];
            if (Request.Form["CCNo"] != null)
                CCNo.Value = Request.Form["CCNo"];
            if (Request.Form["S_bankname"] != null)
                S_bankname.Value = Request.Form["S_bankname"];
            if (Request.Form["S_country"] != null)
                S_country.Value = Request.Form["S_country"];
            if (Request.Form["TranDate"] != null)
                TranDate.Value = Request.Form["TranDate"];
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Status.Value))
            {
                lblMerchantCode.InnerHtml = MerchantCode.Value;
                lblRefNo.InnerHtml = RefNo.Value;
                lblCurrency.InnerHtml = Currency.Value;
                lblAmount.InnerHtml = Amount.Value;
                lblPaymentId.InnerHtml = PaymenId.Value;
                lblStatus.InnerHtml = Status.Value;
                lblErrDesc.InnerHtml = ErrDesc.Value;
                lblRemark.InnerHtml = Remark.Value;
                lblSignature.InnerHtml = Signature.Value;
            }
            else 
            {
                lblStatus.InnerHtml = "Waiting for payment status";
            }
        }
    }
}
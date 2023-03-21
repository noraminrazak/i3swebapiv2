<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Response.aspx.cs" Inherits="I3SwebAPIv2.Response" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Payment</title>
    <script src="<%=ResolveUrl("http://giis.myedutech.my/PageJS/Response.js") %>"></script>
</head>
<body>
    <form id="form1" runat="server" action="api/v2/payment/response" method="post"  >
         <input runat="server" type="hidden" name="MerchantCode" id="MerchantCode" />
         <input runat="server" type="hidden" name="PaymenId" id="PaymenId"/>
         <input runat="server" type="hidden" name="RefNo" id="RefNo"/>
         <input runat="server" type="hidden" name="Amount" id="Amount"/>
         <input runat="server" type="hidden" name="Currency" id="Currency"/>
         <input runat="server" type="hidden" name="Remark" id="Remark"/>
         <input runat="server" type="hidden" name="TransId" id="TransId"/>
         <input runat="server" type="hidden" name="AuthCode" id="AuthCode"/>
         <input runat="server" type="hidden" name="Status" id="Status"/>
         <input runat="server" type="hidden" name="ErrDesc" id="ErrDesc"/>
         <input runat="server" type="hidden" name="Signature" id="Signature"/>
        <input runat="server" type="hidden" name="CCName" id="CCName"/>
        <input runat="server" type="hidden" name="CCNo" id="CCNo"/>
        <input runat="server" type="hidden" name="S_bankname" id="S_bankname"/>
        <input runat="server" type="hidden" name="S_country" id="S_country"/>
        <input runat="server" type="hidden" name="TranDate" id="TranDate"/>
        <table>
            <tr>
                <td>
                    <label runat="server" id="lblMerchantCode" name="status" style="text-align:center" text="Status"/>
                </td>
            </tr>
            <tr>
                <td>
                     <label runat="server" id="lblRefNo" name="status" style="text-align:center" text="Status"/>
                </td>
            </tr>
            <tr>
                <td>
                     <label runat="server" id="lblPaymentId" name="status" style="text-align:center" text="Status"/>
                </td>
            </tr>
            <tr>
                <td>
                    <label runat="server" id="lblCurrency" name="status" style="text-align:center" text="Status"/>
                     <label runat="server" id="lblAmount" name="status" style="text-align:center" text="Status"/>
                </td>
            </tr>
            <tr>
                <td>
                    <label runat="server" id="lblRemark" name="status" style="text-align:center" text="Status"/>
                </td>
            </tr>
            <tr>
                <td>
                    <label runat="server" id="lblStatus" name="status" style="text-align:center" text="Status"/>
                </td>
            </tr>
            <tr>
                <td>
                    <label runat="server" id="lblErrDesc" name="status" style="text-align:center" text="Status"/>
                </td>
            </tr>
            <tr>
                <td>
                    <label runat="server" id="lblSignature" name="status" style="text-align:center" text="Status"/>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>

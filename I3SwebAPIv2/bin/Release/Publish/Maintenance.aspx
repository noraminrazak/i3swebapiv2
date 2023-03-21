<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Maintenance.aspx.cs" Inherits="I3SwebAPIv2.Maintenance" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
    <title>SAPS | I-3S</title>
    <!-- Favicon-->
    <link rel="icon" href="favicon.ico" type="image/x-icon" />

    <!-- Google Fonts -->
    <link href="https://fonts.googleapis.com/css?family=Roboto:400,700&subset=latin,cyrillic-ext" rel="stylesheet" type="text/css" />
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet" type="text/css" />

    <!-- Bootstrap Core Css -->
    <link href="<%=ResolveUrl("~/AdminBSB/plugins/bootstrap/css/bootstrap.css") %>" rel="stylesheet" />

    <!-- Waves Effect Css -->
    <link href="<%=ResolveUrl("~/AdminBSB/plugins/node-waves/waves.css") %>" rel="stylesheet" />

    <!-- Animation Css -->
    <link href="<%=ResolveUrl("~/AdminBSB/plugins/animate-css/animate.css") %>" rel="stylesheet" />

    <!-- Custom Css -->
    <link href="<%=ResolveUrl("~/AdminBSB/css/style.css") %>" rel="stylesheet" />

    <!-- Jquery Core Js -->
    <script src="<%=ResolveUrl("~/AdminBSB/plugins/jquery/jquery.min.js") %>"></script>

    <!-- Bootstrap Core Js -->
    <script src="<%=ResolveUrl("~/AdminBSB/plugins/bootstrap/js/bootstrap.js") %>"></script>

    <!-- Waves Effect Plugin Js -->
    <script src="<%=ResolveUrl("~/AdminBSB/plugins/node-waves/waves.js") %>"></script>

    <!-- Validation Plugin Js -->
    <script src="<%=ResolveUrl("~/AdminBSB/plugins/jquery-validation/jquery.validate.js") %>"></script>

    <!-- Bootstrap Notify Plugin Js -->
    <script src="<%=ResolveUrl("~/AdminBSB/plugins/bootstrap-notify/bootstrap-notify.js") %>"></script>

    <!-- Custom Js -->
    <%--<script src="<%=ResolveUrl("~/AdminBSB/js/pages/ui/notifications.js") %>"></script>--%>
    <script src="<%=ResolveUrl("~/AdminBSB/js/admin.js") %>"></script>
    <script src="<%=ResolveUrl("~/PageJS/Common.js") %>"></script>
</head>
<body style="background-color: white" class="four-zero-four">
    <div class="four-zero-four-container center" style="margin-top:100px">
        <div class="logo">
            <a><img src="Images/ic_logo.png" alt="" style="width:30%;height:30%;" class="center"/></a><br/>
            <%--<small><b>SISTEM ANALISA PEPERIKSAAN SEKOLAH (SAPS)</b></small>--%>
        </div><br />
        <div class="font-15"><b>Maaf, sistem sedang dinaik taraf!</b></div>
        <div class="font-15"><b>Sila kembali semula.</b></div>
        <hr>
        <div class="font-15"><b>Sorry, system upgrade in progress!</b></div>
        <div class="font-15"><b>Please come back later.</b></div>
    </div>
</body>
</html>

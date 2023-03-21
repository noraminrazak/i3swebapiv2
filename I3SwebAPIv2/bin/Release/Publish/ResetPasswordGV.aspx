<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPasswordGV.aspx.cs" Inherits="I3SwebAPIv2.ResetPasswordGV" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
    <title>Reset Password | GVIIS</title>
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
    <script src="<%=ResolveUrl("~/PageJS/ResetPasswordGV.js") %>"></script>
</head>
<body class="signup-page">
    <div class="signup-box">
        <div class="logo">
            <a><img src="Images/gviis_banner.png" alt="" style="width:50%;height:50%;" class="center"/></a><br/>
            <%--<small><span style="color:black">GREENVIEW ISLAMIC INTERNATIONAL SCHOOL</span></small>--%>
        </div>
        <div class="card">
            <div class="body">
                <form id="sign_up" method="POST">
                    <div class="msg">Reset Password</div>
                    <div class="input-group" style="display:none">
                        <span class="input-group-addon">
                            <i class="material-icons">lock</i>
                        </span>
                        <div class="form-line">
                            <input type="text" id="uid" class="form-control" name="uid" placeholder="UID" />
                        </div>
                    </div>
                    <div class="input-group" style="display:none">
                        <span class="input-group-addon">
                            <i class="material-icons">lock</i>
                        </span>
                        <div class="form-line">
                            <input type="text" id="email" class="form-control" name="email" placeholder="Email" />
                        </div>
                    </div>
                    <div class="input-group" style="display:none">
                        <span class="input-group-addon">
                            <i class="material-icons">lock</i>
                        </span>
                        <div class="form-line">
                            <input type="text" id="token" class="form-control" name="token" placeholder="Token" />
                        </div>
                    </div>
                    <div class="input-group">
                        <span class="input-group-addon">
                            <i class="material-icons">lock</i>
                        </span>
                        <div class="form-line">
                            <input type="password" id="password" class="form-control" name="password" placeholder="Password" />
                        </div>
                    </div>
                    <div class="input-group">
                        <span class="input-group-addon">
                            <i class="material-icons">lock</i>
                        </span>
                        <div class="form-line">
                            <input type="password" id="confirm_password" class="form-control" name="confirm" placeholder="Confirm Password" />
                        </div>
                    </div>

                    <button class="btn btn-block btn-lg bg-pink waves-effect" type="submit" id="btnReset">Reset Password</button>
                </form>
            </div>
        </div>
    </div>
</body>
</html>
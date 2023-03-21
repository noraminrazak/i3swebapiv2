<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FAQ.aspx.cs" Inherits="I3SwebAPIv2.FAQ" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
    <title>FAQ | I-3S</title>
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
    <script src="<%=ResolveUrl("~/PageJS/FAQ.js") %>"></script>
</head>
<body style="background-color: white">
    <!-- Nav tabs -->
    <ul class="nav nav-tabs tab-col-deep" role="tablist">
        <li role="presentation" class="active"><a href="#general" data-toggle="tab">General</a></li>
        <li role="presentation"><a href="#student" data-toggle="tab">Students</a></li>
        <li role="presentation"><a href="#parent" data-toggle="tab">Parents / Guardians</a></li>
        <li role="presentation"><a href="#teacher" data-toggle="tab">Teachers</a></li>
        <li role="presentation"><a href="#staff" data-toggle="tab">Staff</a></li>
    </ul>
    <!-- Tab panes -->
    <div class="tab-content">
        <div role="tabpanel" class="tab-pane fade in active" id="general">
            <div class="col-xs-12 ol-sm-12 col-md-12 col-lg-12">
                <div class="panel-group" id="accordion_g1" role="tablist" aria-multiselectable="false">
                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingOne_g1">
                            <h4 class="panel-title" style="color:black">
                                <a role="button" data-toggle="collapse" href="#collapseOne_g1" aria-expanded="true" aria-controls="collapseOne_g1">What is i-3s & cashless student cards?                                          
                                </a>
                            </h4>
                        </div>
                        <div id="collapseOne_g1" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne_g1">
                            <div class="panel-body">
                                i-3s stands for Integrated School Support System. This is the latest system to use a centralised school attendance 
                                        for recording and automation using a student/teacher/staff ID card. Teachers can be relieved from their administrative 
                                        work and focus more time on teaching. Parents are able to view and be notified on their children’s attendance via i-3s mobile 
                                        application or web portal daily. In addition, the cardholders enjoy a safe and convenient way to make purchases in school using their prepaid e-wallet.                                        
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_g2">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_g2" aria-expanded="false" aria-controls="collapseFour_g2">Why this ID card is given?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_g2" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_g2">
                            <div class="panel-body">
                                This new ID card allows the student/teacher/staff to record their school attendance electronically and instantly by scanning their ID card when they
                                        arrive in school. In addition, this ID card allows safe, convenient & cashless transaction for food purchases in the canteen as well as stationery purchases 
                                        in the bookstore. Therefore, there is no need to carry cash to school for safety reasons especially involving students.                                        
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div role="tabpanel" class="tab-pane fade" id="student">
            <div class="col-xs-12 ol-sm-12 col-md-12 col-lg-12">

                <div class="panel-group" id="accordion_s1" role="tablist" aria-multiselectable="true">
                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingOne_s1">
                            <h4 class="panel-title" style="color:black">
                                <a role="button" data-toggle="collapse" href="#collapseOne_s1" aria-expanded="true" aria-controls="collapseOne_s1">How do I register my attendance?                                          
                                </a>
                            </h4>
                        </div>
                        <div id="collapseOne_s1" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne_s1">
                            <div class="panel-body">
                                When you first enter at school on each school day, you need to touch your student card to the card reader. The card reader is installed near the 
                                                    canteen and assembly point. Once the card reader successfully reads the card, you will hear a “beep”. Attendance is recorded for that day.
                                        
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_s2">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_s2" aria-expanded="false" aria-controls="collapseFour_s2">How many times do I need to touch the student card to the card reader?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_s2" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_s2">
                            <div class="panel-body">
                                You only need to touch once to record your attendance. You can touch the card reader many times, but the system will record your first touch as your attendance.                                        
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_s3">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_s3" aria-expanded="false" aria-controls="collapseFour_s3">Can I use any card reader in the school?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_s3" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_s3">
                            <div class="panel-body">
                                Yes, you can use any card reader in the school for the attendance recording purpose.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_s4">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_s4" aria-expanded="false" aria-controls="collapseFour_s4">How do I use the card to buy food in the canteen or buy stationery in the book store?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_s4" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_s4">
                            <div class="panel-body">
                                To make a payment, touch the sales terminal with your card in canteen or book store. You will hear a “beep” from the terminal after the amount 
                                        is successfully deducted from your card.
                            </div>
                        </div>
                    </div>


                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_s5">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_s5" aria-expanded="false" aria-controls="collapseFour_s5">What if I do not have enough money in the card?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_s5" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_s5">
                            <div class="panel-body">
                                If the card balance is insufficient, the transaction will not go through and no amount will be deducted. The sales terminal will display an “insufficient balance” message.
                            </div>
                        </div>
                    </div>


                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_s6">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_s6" aria-expanded="false" aria-controls="collapseFour_s6">How do I top-up the card?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_s6" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_s6">
                            <div class="panel-body">
                                Your parent can transfer money to top up the card using the i-3s mobile application and web portal.
                            </div>
                        </div>
                    </div>


                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_s7">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_s7" aria-expanded="false" aria-controls="collapseFour_s7">How do I check my card balance?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_s7" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_s7">
                            <div class="panel-body">
                                You can use any card reader in the school. Touch your card to the card reader. You will hear a “beep” and the card balance amount is displayed in the LED 
                                        display on the card reader.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_s8">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_s8" aria-expanded="false" aria-controls="collapseFour_s8">What if I forget to bring the card on that day?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_s8" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_s8">
                            <div class="panel-body">
                                Please contact the school administrator for a temporary card to be used immediately for your attendance & purchases in school.
                            </div>
                        </div>
                    </div>


                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_s9">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_s9" aria-expanded="false" aria-controls="collapseFour_s9">What if I lost my student card or it is not working?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_s9" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_s9">
                            <div class="panel-body">
                                Please report to us via the i-3s mobile application or web portal, or call our customer service to suspend your card. You may contact the school administrator for a replacement student 
                                        card request and to collect a temporary card for your attendance & purchases in the school. A new replacement card will be issued within 5 days and a RM12.00 replacement fee will be 
                                        charged or deducted from the card balance. The lost card balance will be transferred to the new card.
                            </div>
                        </div>
                    </div>


                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_s10">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_s10" aria-expanded="false" aria-controls="collapseFour_s10">How do I protect & take care of the student card?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_s10" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_s10">
                            <div class="panel-body">
                                The student card carries important information. Take good care of the card. Do not lend your card to others. If dirty, please wipe it with a damp towel. 
                                        Don’t wash or submerge in water. Avoid scratching the card surface which may damage the card to render it unreadable.
                            </div>
                        </div>
                    </div>


                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_s11">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_s11" aria-expanded="false" aria-controls="collapseFour_s11">My parents need receipts for book purchases.
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_s11" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_s11">
                            <div class="panel-body">
                                Parent can download from i-3s application.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_s12">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_s12" aria-expanded="false" aria-controls="collapseFour_s12">What do I do when I graduate from the school?.
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_s12" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_s12">
                            <div class="panel-body">
                                The card does not need to be returned to the school administrator when you graduate or when you are no longer a student in that school. You may keep it as a souvenir.
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>

        <div role="tabpanel" class="tab-pane fade" id="parent">
            <div class="col-xs-12 ol-sm-12 col-md-12 col-lg-12">
                <div class="panel-group" id="accordion_p1" role="tablist" aria-multiselectable="false">
                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingOne_p1">
                            <h4 class="panel-title" style="color:black">
                                <a role="button" data-toggle="collapse" href="#collapseOne_p1" aria-expanded="true" aria-controls="collapseOne_p1">How do I register an i-3s account?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseOne_p1" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne_p1">
                            <div class="panel-body">
                                Your registration is automatically created based on the school information under your child’s record. Download and install the i-3s mobile application from either Apple Store or Play Store.
                                        Please login, using your identity card number and follow instructions.Your e-Wallet will be created.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_p2">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_p2" aria-expanded="false" aria-controls="collapseFour_p2">How do I see my child’s attendance record?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_p2" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_p2">
                            <div class="panel-body">
                                Click student name inside student’s tab then click “Calendar icon” in the i-3s mobile application to view your child’s attendance record.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_p3">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_p3" aria-expanded="false" aria-controls="collapseFour_p3">I do not see my child’s record in the i-3s mobile application. How do I add my child in the application?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_p3" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_p3">
                            <div class="panel-body">
                                Please watch this video for further instructions <a href="https://www.youtube.com/channel/UCIzmykyHxx05NOT4XangY7Q">Click here</a>. If you have further problems, 
                                        please contact our customer service +6011-57746255 to report this issue or email to support@i-3s.com.my
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_p4">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_p4" aria-expanded="false" aria-controls="collapseFour_p4">What if my child does not register attendance that day?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_p4" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_p4">
                            <div class="panel-body">
                                If the student card is not scanned by 8 am (morning session) or 2 pm (evening session) during a school day, a notification will be sent to your smartphone via the i-3s 
                                        mobile application.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_p5">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_p5" aria-expanded="false" aria-controls="collapseFour_p5">Can I message to my child’s class teacher?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_p5" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_p5">
                            <div class="panel-body">
                                Yes, you can message to your child’s class teacher via the i-3s mobile application or web portal. This platform supports direct chat with the class teacher.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_p6">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_p6" aria-expanded="false" aria-controls="collapseFour_p6">How do I top-up my child’s student card?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_p6" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_p6">
                            <div class="panel-body">
                                In the i-3s mobile application or web portal’s “Wallet” section, you can top-up your e-wallet via credit cards, debit cards or online banking anytime. 
                                        Then, you can top-up your child’s student card by transferring any amount from the balance in your e-wallet to the student card in your child’s name.
                            </div>
                        </div>
                    </div>


                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_p7">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_p7" aria-expanded="false" aria-controls="collapseFour_p7">How do I top-up my e-wallet from the i-3s mobile App?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_p7" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_p7">
                            <div class="panel-body">
                                1. Login into your i-3S account on the mobile app from your smartphone.<br />
                                2. Click the icon ‘<img src="Content/images/plus_btn.png" style="width: 4%;" />’ on the top right corner to top-up your account.<br />
                                3. Enter/select the desired amount and click Top-up e-wallet. Select your preferred method of payment and click green button to confirm
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_p8">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_p8" aria-expanded="false" aria-controls="collapseFour_p8">How do I add money or transfer my balance to my child’s student card?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_p8" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_p8">
                            <div class="panel-body">
                                1. Login into your i-3S account on the mobile app from your smartphone.<br />
                                2. In the Wallet section, select your child’s name and click the icon ‘<img src="Content/images/transfer_btn.png" style="width: 4%;" />’ at the top right corner to transfer money from parent account.
                                <br />
                                3. Due to the current banking restrictions, you are not allowed to transfer credit from your child e-wallet back into your e-wallet.
                            </div>
                        </div>
                    </div>


                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_p9">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_p9" aria-expanded="false" aria-controls="collapseFour_p9">Note: Transfers between e-wallet accounts are FREE OF CHARGE for unlimited number of transactions.<br />

                                </a>
                            </h4>
                        </div>
                        <%-- <div id="collapseFour_p9" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_p9">
                                    <div class="panel-body">
                                        <i style="font-size: smaller;">Nota: Pemindahan antara akaun e-wallet adalah PERCUMA untuk jumlah 
                                        transaksi yang tidak terhad. </i>
                                    </div>
                                </div>--%>
                    </div>


                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_p10">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_p10" aria-expanded="false" aria-controls="collapseFour_p10">How do I deposit cash into my e-wallet?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_p10" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_p10">
                            <div class="panel-body">
                                You are unable to use cash to top-up your i-3s wallet. You may use credit cards, debit cards or online banking account to top-up your account.
                                        
                            </div>
                        </div>
                    </div>


                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_p11">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_p11" aria-expanded="false" aria-controls="collapseFour_p11">How do I get the balance back on my child’s student card?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_p11" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_p11">
                            <div class="panel-body">
                                When a card is terminated, the remaining credit balance is automatically transferred back to the specified bank account. 
                                        Please lookup the menu “Closed Account” in our i-3s mobile application or web portal to terminate your card. You or your child may keep the ID card.                                       
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div role="tabpanel" class="tab-pane fade" id="teacher">
            <div class="col-xs-12 ol-sm-12 col-md-12 col-lg-12">
                <div class="panel-group" id="accordion_t1" role="tablist" aria-multiselectable="false">

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingOne_t1">
                            <h4 class="panel-title" style="color:black">
                                <a role="button" data-toggle="collapse" href="#collapseOne_t1" aria-expanded="true" aria-controls="collapseOne_t1">How do I register an i-3s account?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseOne_t1" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne_t1">
                            <div class="panel-body">
                                Your registration is automatically created based on the school information under your record. Download and install the i-3s mobile application from either Apple Store or Play Store. 
                                        Please login, using your identity card number and follow instructions.Your e-Wallet will be created.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_t2">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_t2" aria-expanded="false" aria-controls="collapseFour_t2">Will teachers be given a teacher card?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_t2" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_t2">
                            <div class="panel-body">
                                Yes, teachers will be given a card for use in the school. Teachers can register attendance and make purchases in the school using the card.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_t3">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_t3" aria-expanded="false" aria-controls="collapseFour_t3">How do I register my attendance?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_t3" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_t3">
                            <div class="panel-body">
                                You will need to touch your card to the card reader. The card reader is installed near or inside the office. Once the card reader successfully reads the card, 
                                        you will hear a “beep”. Attendance is recorded for that day.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_t4">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_t4" aria-expanded="false" aria-controls="collapseFour_t4">How do I use the card to buy food in the canteen or buy stationery in the book store?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_t4" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_t4">
                            <div class="panel-body">
                                To make a payment, touch the sales terminal with your card in canteen or book store. You will hear a “beep” from the terminal after the amount is successfully 
                                        deducted from your card.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_t5">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_t5" aria-expanded="false" aria-controls="collapseFour_t5">How do I top-up the wallet account in my card?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_t5" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_t5">
                            <div class="panel-body">
                                In the i-3s mobile application or web portal’s “Wallet” section, you can top-up your e-wallet via credit cards, debit cards or online banking anytime.                                       
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_t6">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_t6" aria-expanded="false" aria-controls="collapseFour_t6">What if I do not have enough money in the card?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_t6" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_t6">
                            <div class="panel-body">
                                If the card balance is insufficient, the transaction will not go through and no amount will be deducted. The sales terminal will display an “insufficient balance” message.                                        
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_t7">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_t7" aria-expanded="false" aria-controls="collapseFour_t7">How do I check my card balance?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_t7" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_t7">
                            <div class="panel-body">
                                You can use any of the yellow card reader in the school. Touch your card to the card reader. You will hear a “beep” and the card balance amount is displayed in 
                                        the LED display on the card reader.                                        
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_t8">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_t8" aria-expanded="false" aria-controls="collapseFour_t8">What if I forget to bring the card on that day?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_t8" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_t8">
                            <div class="panel-body">
                                Please contact the school administrator for a temporary card to be used immediately for your attendance & purchases in school.                                       
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_t9">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_t9" aria-expanded="false" aria-controls="collapseFour_t9">What if I lost my card or it is not working? 
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_t9" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_t9">
                            <div class="panel-body">
                                Please report to us via the i-3s mobile application or web portal, or contact our customer service to suspend your card. You may contact the school 
                                        administrator to collect a temporary card for your attendance & purchases in the school. We will send you a new card as soon as possible and a 
                                        RM12.00 replacement fee will be charged or deducted from the card balance. The lost card balance will be transferred to the new card immediately.                                         
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_t10">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_t10" aria-expanded="false" aria-controls="collapseFour_t10">How do I monitor my class attendance? 
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_t10" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_t10">
                            <div class="panel-body">
                                You can monitor your class attendance by clicking the Class/Club tab in the i-3s mobile application.                                        
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_t11">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_t11" aria-expanded="false" aria-controls="collapseFour_t11">How do I use i-3s mobile application for class and club activities?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_t11" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_t11">
                            <div class="panel-body">
                                In the i-3s mobile application, click the Class/Club tab. You need first to identify and register the students as members of a club, association, etc. 
                                        You can post notices/pictures inside the class/club for parents to read, take attendance and chat with student’s parent.                                          
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_t12">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_t12" aria-expanded="false" aria-controls="collapseFour_t12">How do I protect & take care of the card? 
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_t12" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_t12">
                            <div class="panel-body">
                                The card carries important information. Take good care of the card. Do not lend your card to others. If dirty, please wipe it with a damp towel. 
                                        Don’t wash or submerge in water. Avoid scratching the card surface which may damage the card to render it unreadable.                                        
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div role="tabpanel" class="tab-pane fade" id="staff">
            <div class="col-xs-12 ol-sm-12 col-md-12 col-lg-12">
                <div class="panel-group" id="accordion_st1" role="tablist" aria-multiselectable="false">

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingOne_st1">
                            <h4 class="panel-title" style="color:black">
                                <a role="button" data-toggle="collapse" href="#collapseOne_st1" aria-expanded="true" aria-controls="collapseOne_st1">How do I register an i-3s account?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseOne_st1" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne_st1">
                            <div class="panel-body">
                                Your registration is automatically created based on the school information under your record. Download and install the i-3s mobile application from either Apple Store or Play Store. 
                                        Please login, using your identity card number and follow instructions.Your e-Wallet will be created.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_st2">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_st2" aria-expanded="false" aria-controls="collapseFour_st2">Will staffs be given a staff card?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_st2" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_st2">
                            <div class="panel-body">
                                Yes, staffs will be given a card for use in the school. Staffs can register attendance and make purchases in the school using the card.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_st3">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_st3" aria-expanded="false" aria-controls="collapseFour_st3">How do I register my attendance?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_st3" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_st3">
                            <div class="panel-body">
                                You will need to touch your card to the card reader. The card reader is installed near or inside the office. Once the card reader successfully reads the card, 
                                        you will hear a “beep”. Attendance is recorded for that day.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_st4">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_st4" aria-expanded="false" aria-controls="collapseFour_st4">How do I use the card to buy food in the canteen or buy stationery in the book store?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_st4" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_st4">
                            <div class="panel-body">
                                To make a payment, touch the sales terminal with your card in canteen or book store. You will hear a “beep” from the terminal after the 
                                        amount is successfully deducted from your card.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_st5">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_st5" aria-expanded="false" aria-controls="collapseFour_st5">How do I top-up the card?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_st5" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_st5">
                            <div class="panel-body">
                                In the i-3s mobile application or web portal’s “Wallet” section, you can top-up your e-wallet via credit cards, debit cards or online banking anytime.                                        
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_st6">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_st6" aria-expanded="false" aria-controls="collapseFour_st6">How do I top-up my e-wallet from the i-3s mobile App?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_st6" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_st6">
                            <div class="panel-body">
                                1.  Login into your i-3S account on the mobile app from your smartphone.<br />
                                2.	Click the icon ‘<img src="Content/images/plus_btn.png" style="width: 4%;" />’ on the top right corner to top-up your account.<br />
                                3.	Enter/select the desired amount and click Top-up e-wallet. Select your preferred method of payment and click green button to confirm 
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_st7">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_st7" aria-expanded="false" aria-controls="collapseFour_st7">What if I do not have enough money in the card?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_st7" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_st7">
                            <div class="panel-body">
                                If the card balance is insufficient, the transaction will not go through and no amount will be deducted. The sales terminal will display an “insufficient balance” message.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_st8">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_st8" aria-expanded="false" aria-controls="collapseFour_st8">How do I check my card balance?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_st8" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_st8">
                            <div class="panel-body">
                                You can use any card reader in the school. Touch your card to the card reader. You will hear a “beep” and the card balance amount is displayed in the 
                                        LED display on the card reader. 
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_st9">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_st9" aria-expanded="false" aria-controls="collapseFour_st9">What if I forget to bring the card on that day?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_st9" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_st9">
                            <div class="panel-body">
                                Please contact the school administrator for a temporary card to be used immediately for your attendance & purchases in school.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_st10">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_st10" aria-expanded="false" aria-controls="collapseFour_st10">What if I lost my card or it is not working? 
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_st10" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_st10">
                            <div class="panel-body">
                                Please report to us via the i-3s mobile application or web portal, or contact our customer service to suspend your card. You may contact the school administrator to 
                                        collect a temporary card for your attendance & purchases in the school. We will send you a new card as soon as possible and a RM12.00 replacement fee will be charged 
                                        or deducted from the card balance. The lost card balance will be transferred to the new card immediately
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-yellow">
                        <div class="panel-heading" role="tab" id="headingFour_st11">
                            <h4 class="panel-title" style="color:black">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_st11" aria-expanded="false" aria-controls="collapseFour_st11">How do I protect & take care of the card? 
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_st11" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_st11">
                            <div class="panel-body">
                                The card carries important information. Take good care of the card. Do not lend your card to others. If dirty, please wipe it with a damp towel. 
                                        Don’t wash or submerge in water. Avoid scratching the card surface which may damage the card to render it unreadable. 
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
</body>
</html>

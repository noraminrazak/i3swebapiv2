<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FAQ_bm.aspx.cs" Inherits="I3SwebAPIv2.FAQ" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
    <title>Soalan Lazim | I-3S</title>
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
        <!-- Page Loader -->
<%--    <div class="page-loader-wrapper">
        <div class="loader">
            <div class="preloader">
                <div class="spinner-layer pl-red">
                    <div class="circle-clipper left">
                        <div class="circle"></div>
                    </div>
                    <div class="circle-clipper right">
                        <div class="circle"></div>
                    </div>
                </div>
            </div>
            <p>Please wait...</p>
        </div>
    </div>--%>
    <!-- #END# Page Loader -->
    <!-- Nav tabs -->
    <ul class="nav nav-tabs tab-col-grey" role="tablist">
        <li role="presentation" class="active"><a href="#general" data-toggle="tab">Umum</a></li>
        <li role="presentation"><a href="#student" data-toggle="tab">Pelajar</a></li>
        <li role="presentation"><a href="#parent" data-toggle="tab">IbuBapa / Penjaga</a></li>
        <li role="presentation"><a href="#teacher" data-toggle="tab">Guru</a></li>
        <li role="presentation"><a href="#staff" data-toggle="tab">Kakitangan</a></li>
    </ul>
    <!-- Tab panes -->
    <div class="tab-content">
        <div role="tabpanel" class="tab-pane fade in active" id="general">
            <div class="col-xs-12 ol-sm-12 col-md-12 col-lg-12">
                <div class="panel-group" id="accordion_g1" role="tablist" aria-multiselectable="false">
                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingOne_g1">
                            <h4 class="panel-title">
                                <a role="button" data-toggle="collapse" href="#collapseOne_g1" aria-expanded="true" aria-controls="collapseOne_g1">Apakah itu i-3s & kad pelajar tanpa tunai?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseOne_g1" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne_g1">
                            <div class="panel-body" style="text-align:justify">
                                i-3s ialah Sistem Sokongan Sekolah Bersepadu. Ia adalah sistem terkini untuk merekod dan automasi kehadiran sekolah berpusat menggunakan 
                                kad pelajar / guru / kakitangan. Guru boleh dilepaskan dari kerja pentadbiran mereka dan memfokuskan lebih banyak masa untuk mengajar. 
                                Ibu bapa dapat melihat dan diberitahu mengenai kehadiran anak-anak mereka melalui aplikasi mudah alih i-3 atau portal web setiap hari. 
                                Di samping itu, pemegang kad dapat menikmati cara yang selamat dan selesa untuk membuat pembelian di sekolah menggunakan e-wallet prabayar mereka.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_g2">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_g2" aria-expanded="false" aria-controls="collapseFour_g2">Mengapa kad ID ini diberikan?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_g2" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_g2">
                            <div class="panel-body" style="text-align:justify">
                                Kad ID baru ini membolehkan pelajar / guru / kakitangan untuk merekod kehadiran sekolah mereka secara elektronik dan dengan serta-merta dengan mengimbas 
                                        kad ID mereka apabila mereka tiba di sekolah. Di samping itu, kad ID ini membolehkan urus niaga selamat, mudah & tanpa tunai untuk pembelian makanan di kantin 
                                        serta pembelian alat tulis di kedai buku. Oleh itu, pemegang kad tidak perlu lagi risau atas alasan keselamatan membawa wang tunai ke sekolah terutama para pelajar.
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div role="tabpanel" class="tab-pane fade" id="student">
            <div class="col-xs-12 ol-sm-12 col-md-12 col-lg-12">
                <div class="panel-group" id="accordion_s1" role="tablist" aria-multiselectable="true">
                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingOne_s1">
                            <h4 class="panel-title">
                                <a role="button" data-toggle="collapse" href="#collapseOne_s1" aria-expanded="true" aria-controls="collapseOne_s1">Bagaimana saya rekod kehadiran saya?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseOne_s1" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne_s1">
                            <div class="panel-body" style="text-align:justify">
                                Apabila anda memasuki sekolah pada setiap hari, anda perlu menyentuh kad pelajar anda ke pembaca kad. Pembaca kad dipasang di kantin dan tapak perhimpunan.
                                                    Apabila pembaca kad berjaya membaca kad, anda akan mendengar bunyi “bip”. Kehadiran anda telah direkodkan untuk hari itu.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_s2">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_s2" aria-expanded="false" aria-controls="collapseFour_s2">Berapa kali saya perlu menyentuh kad pelajar ke pembaca kad?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_s2" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_s2">
                            <div class="panel-body" style="text-align:justify">
                                Anda hanya perlu menyentuh sekali untuk merakam kehadiran anda. Anda boleh menyentuh pembaca kad berkali-kali, tetapi sistem akan merekod sentuhan pertama anda sebagai 
                                                    kehadiran anda.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_s3">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_s3" aria-expanded="false" aria-controls="collapseFour_s3">Bolehkah saya menggunakan mana-mana pembaca kad di sekolah?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_s3" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_s3">
                            <div class="panel-body" style="text-align:justify">
                                Ya, anda boleh menggunakan mana-mana pembaca kad di dalam sekolah untuk tujuan rekod kehadiran.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_s4">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_s4" aria-expanded="false" aria-controls="collapseFour_s4">Bagaimanakah saya boleh guna kad untuk membeli makanan di kantin atau membeli alat tulis di kedai buku?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_s4" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_s4">
                            <div class="panel-body" style="text-align:justify">
                                Untuk membuat pembayaran, sentuh terminal jualan dengan kad anda di kantin atau kedai buku. Anda akan mendengar bunyi “bip” dari terminal selepas 
                                        jumlah berjaya ditolak dari kad anda.
                            </div>
                        </div>
                    </div>


                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_s5">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_s5" aria-expanded="false" aria-controls="collapseFour_s5">Bagaimana jika saya tidak mempunyai wang yang mencukupi dalam kad?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_s5" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_s5">
                            <div class="panel-body" style="text-align:justify">
                                Jika baki kad tidak mencukupi, urus niaga tidak akan berlaku dan tiada jumlah akan ditolak. Terminal jualan akan memaparkan mesej “Baki tidak mencukupi”
                            </div>
                        </div>
                    </div>


                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_s6">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_s6" aria-expanded="false" aria-controls="collapseFour_s6">Bagaimana saya boleh menambah nilai baki kad?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_s6" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_s6">
                            <div class="panel-body" style="text-align:justify">
                                Ibu bapa anda boleh memindahkan wang untuk menambah nilai baki kad dengan menggunakan aplikasi mudah alih i-3s, dan portal web.
                            </div>
                        </div>
                    </div>


                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_s7">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_s7" aria-expanded="false" aria-controls="collapseFour_s7">Bagaimana saya boleh menyemak baki kad saya?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_s7" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_s7">
                            <div class="panel-body" style="text-align:justify">
                                Anda boleh menggunakan mana-mana pembaca kad di dalam sekolah. Sentuh kad anda ke pembaca kad. Anda akan mendengar “bip” dan jumlah baki kad akan 
                                        dipaparkan di paparan LED pada pembaca kad.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_s8">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_s8" aria-expanded="false" aria-controls="collapseFour_s8">Bagaimana jika saya terlupa membawa kad pada hari sekolah?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_s8" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_s8">
                            <div class="panel-body" style="text-align:justify">
                                Sila hubungi pentadbir sekolah anda untuk kad pelajar sementara yang boleh digunakan dengan serta-merta untuk kehadiran & pembelian anda di sekolah.
                            </div>
                        </div>
                    </div>


                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_s9">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_s9" aria-expanded="false" aria-controls="collapseFour_s9">Bagaimana jika saya kehilangan kad pelajar saya atau ia tidak berfungsi?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_s9" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_s9">
                            <div class="panel-body" style="text-align:justify">
                                Sila laporkan kepada kami melalui aplikasi mudah alih i-3s atau portal web, atau hubungi khidmat pelanggan kami untuk menggantung penggunaan kad anda.
                                        Anda boleh menghubungi pentadbir sekolah untuk mohon kad pelajar gantian dan mendapatkan kad pelajar sementara untuk kehadiran & pembelian anda di sekolah. Kad gantian baru akan 
                                        dikeluarkan dalam tempoh 5 hari dan yuran penggantian sebanyak RM12.00 akan dikenakan atau ditolak dari baki kad. Baki kad yang hilang akan dipindahkan ke kad baru.
                            </div>
                        </div>
                    </div>


                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_s10">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_s10" aria-expanded="false" aria-controls="collapseFour_s10">Bagaimana saya melindungi & menjaga kad pelajar?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_s10" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_s10">
                            <div class="panel-body" style="text-align:justify">
                                Kad pelajar anda mengandungi maklumat penting. Anda harus menjaga kad itu dengan baik. Jangan pinjamkan kad anda kepada orang lain. Sekiranya kotor, sila lap dengan tuala lembap. 
                                        Jangan basuh atau tenggelamkan ke dalam air. Elakkan menggaru permukaan kad kerana ia boleh merosakkan kad dan menjadikannya tidak boleh dibaca.
                            </div>
                        </div>
                    </div>


                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_s11">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_s11" aria-expanded="false" aria-controls="collapseFour_s11">Ibu bapa saya memerlukan resit untuk pembelian buku.
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_s11" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_s11">
                            <div class="panel-body" style="text-align:justify">
                                Ibubapa boleh memuat turun daripada aplikasi mudah alih i-3s.
                            </div>
                        </div>
                    </div>
                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_s12">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_s12" aria-expanded="false" aria-controls="collapseFour_s12">Apa yang saya lakukan apabila saya seorang graduan yang telah tamat sekolah?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_s12" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_s12">
                            <div class="panel-body" style="text-align:justify">
                                Kad tidak perlu dikembalikan kepada pentadbir sekolah apabila anda tamat atau tidak lagi menjadi pelajar di sekolah itu. Anda boleh menyimpannya sebagai cenderahati.
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div role="tabpanel" class="tab-pane fade" id="parent">
            <div class="col-xs-12 ol-sm-12 col-md-12 col-lg-12">
                <div class="panel-group" id="accordion_p1" role="tablist" aria-multiselectable="false">
                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingOne_p1">
                            <h4 class="panel-title">
                                <a role="button" data-toggle="collapse" href="#collapseOne_p1" aria-expanded="true" aria-controls="collapseOne_p1">Bagaimana cara mendaftar akaun i-3s?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseOne_p1" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne_p1">
                            <div class="panel-body" style="text-align:justify">
                                Pendaftaran anda dibuat secara automatik berdasarkan maklumat sekolah anak anda. Muat turun aplikasi i-3s dari sama ada Apple Store atau Play Store dari telefon pintar mudah alih anda.
                                        Sila log masuk, menggunakan nombor kad pengenalan anda dan ikuti arahan. e-Wallet anda akan dibuat.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_p2">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_p2" aria-expanded="false" aria-controls="collapseFour_p2">Bagaimana saya dapat lihat rekod kehadiran anak saya?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_p2" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_p2">
                            <div class="panel-body" style="text-align:justify">
                                Klik nama pelajar dalam tab pelajar, kemudian klik “ikon Kalendar” dalam aplikasi mudah alih i-3s untuk melihat rekod kehadiran anak anda.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_p3">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_p3" aria-expanded="false" aria-controls="collapseFour_p3">Saya tidak nampak rekod anak saya dalam aplikasi mudah alih i-3s. Bagaimana saya mahu masukkan anak saya ke dalam applikasi itu?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_p3" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_p3">
                            <div class="panel-body" style="text-align:justify">
                                Sila lihat video di chanel youtube ini <a href="https://www.youtube.com/channel/UCIzmykyHxx05NOT4XangY7Q">Klik disini</a>. 
                                        Sekiranya, masih ada problem sila hubungi talian  khidmat pelanggan kami +6011-57746255 untuk melaporkan isu ini atau email ke support@i-3s.com.my
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_p4">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_p4" aria-expanded="false" aria-controls="collapseFour_p4">Bagaimana jika anak saya tidak mendaftar kehadiran hari itu
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_p4" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_p4">
                            <div class="panel-body" style="text-align:justify">
                                Jika kad pelajar tidak diimbas pada jam 8 pagi (sesi pagi) atau 2 petang (sesi petang) pada hari sekolah, pemberitahuan akan dihantar ke telefon pintar anda melalui aplikasi mudah alih i-3s.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_p5">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_p5" aria-expanded="false" aria-controls="collapseFour_p5">Bolehkah saya menghantar mesej kepada guru kelas anak saya?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_p5" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_p5">
                            <div class="panel-body" style="text-align:justify">
                                Ya, anda boleh menghantar mesej kepada guru kelas anak anda melalui aplikasi mudah alih i-3s atau portal web. Platform ini menyokong perbualan secara langsung dengan guru kelas.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_p6">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_p6" aria-expanded="false" aria-controls="collapseFour_p6">Bagaimana saya boleh menambah nilai baki kad pelajar anak saya?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_p6" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_p6">
                            <div class="panel-body" style="text-align:justify">
                                Dalam aplikasi mudah alih i-3s atau portal web, klik di bahagian “Wallet”. Anda boleh tambah nilai  e-wallet anda melalui kad kredit, 
                                        kad debit atau perbankan atas talian pada bila-bila masa. Kemudian, anda boleh menambah nilai baki kad pelajar anak anda dengan memindahkan sebarang jumlah dari e-wallet anda ke kad pelajar anak anda. 
                            </div>
                        </div>
                    </div>


                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_p7">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_p7" aria-expanded="false" aria-controls="collapseFour_p7">Bagaimanakah saya boleh tambah nilai e-wallet saya dari aplikasi mudah alih i-3s?</a>
                            </h4>
                        </div>
                        <div id="collapseFour_p7" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_p7">
                            <div class="panel-body" style="text-align:justify">
                                1. Log masuk ke akaun i-3s pada aplikasi mudah alih dari telefon pintar anda.<br />
                                2. Klik icon ‘<img src="Content/images/plus_btn.png" style="width: 4%;" />’ di sudut kanan atas untuk menambah akaun anda.<br />
                                3. Masukkan / pilih jumlah yang dikehendaki dan klik butang ‘Top-up e-wallet’. Pilih kaedah pembayaran pilihan anda dan klik butang hijau untuk mengesahkan
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_p8">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_p8" aria-expanded="false" aria-controls="collapseFour_p8">Bagaimanakah cara menambah nilai atau memindahkan baki saya ke kad pelajar anak saya?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_p8" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_p8">
                            <div class="panel-body" style="text-align:justify">
                                1. Log masuk ke akaun i-3s pada aplikasi mudah alih dari telefon pintar anda.<br />
                                2. Di bahagian Wallet, pilih nama anak anda dan klik icon ‘<img src="Content/images/transfer_btn.png" style="width: 4%;" />’ di sudut kanan atas untuk pindahkan duit daripada akaun ibubapa.<br />
                                3. Oleh kerana sekatan perbankan semasa, anda tidak dibenarkan memindahkan kredit dari ewallet anak anda kembali ke dalam e-wallet anda.
                            </div>
                        </div>
                    </div>


                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_p9">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_p9" aria-expanded="false" aria-controls="collapseFour_p9">Nota: Pemindahan antara akaun e-wallet adalah PERCUMA untuk jumlah transaksi yang tidak terhad.                                          
                                </a>
                            </h4>
                        </div>
                        <%-- <div id="collapseFour_p9" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_p9">
                                    <div class="panel-body" style="text-align:justify">
                                        <i style="font-size: smaller;">Nota: Pemindahan antara akaun e-wallet adalah PERCUMA untuk jumlah 
                                        transaksi yang tidak terhad. </i>
                                    </div>
                                </div>--%>
                    </div>


                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_p10">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_p10" aria-expanded="false" aria-controls="collapseFour_p10">Bagaimanakah saya boleh deposit wang tunai ke dalam e-wallet saya?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_p10" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_p10">
                            <div class="panel-body" style="text-align:justify">
                                Anda tidak dapat menggunakan wang tunai untuk menambah dompet i-3s anda. Anda boleh menggunakan kad kredit, kad debit atau akaun perbankan dalam talian untuk 
                                        menambah nilai akaun anda.
                            </div>
                        </div>
                    </div>


                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_p11">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_p11" aria-expanded="false" aria-controls="collapseFour_p11">Bagaimanakah saya dapat pemulangan baki kad pelajar anak saya?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_p11" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_p11">
                            <div class="panel-body" style="text-align:justify">
                                Apabila kad ditamatkan, baki kredit yang tinggal akan dipindahkan semula ke akaun bank tertentu. 
                                            Sila lihat menu “Akaun Ditutup” dalam aplikasi mudah alih i-3s kami atau portal web untuk menamatkan kad anda. Anda atau anak anda boleh menyimpan kad ID.
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div role="tabpanel" class="tab-pane fade" id="teacher">
            <div class="col-xs-12 ol-sm-12 col-md-12 col-lg-12">
                <div class="panel-group" id="accordion_t1" role="tablist" aria-multiselectable="false">
                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingOne_t1">
                            <h4 class="panel-title">
                                <a role="button" data-toggle="collapse" href="#collapseOne_t1" aria-expanded="true" aria-controls="collapseOne_t1">Bagaimana cara mendaftar akaun i-3s?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseOne_t1" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne_t1">
                            <div class="panel-body" style="text-align:justify">
                                Pendaftaran anda dibuat secara automatik berdasarkan rekod anda dalam maklumat sekolah anda. Muat turun aplikasi i-3s dari sama ada Apple Store atau Play Store dari telefon pintar mudah alih anda. 
                                            Sila log masuk, menggunakan nombor kad pengenalan anda dan ikuti arahan. e-Wallet anda akan dibuat.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_t2">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_t2" aria-expanded="false" aria-controls="collapseFour_t2">Adakah guru diberi kad guru?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_t2" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_t2">
                            <div class="panel-body" style="text-align:justify">
                                Ya, guru akan diberi kad untuk digunakan di sekolah. Guru boleh merekod kehadiran dan membuat pembelian di sekolah menggunakan kad.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_t3">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_t3" aria-expanded="false" aria-controls="collapseFour_t3">Bagaimana saya rekod kehadiran saya?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_t3" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_t3">
                            <div class="panel-body" style="text-align:justify">
                                Anda perlu menyentuh kad anda ke pembaca kad. Pembaca kad dipasang di pintu atau di dalam pejabat. 
                                            Apabila pembaca kad berjaya membaca kad, anda akan mendengar bunyi “bip”. Kehadiran anda telah direkodkan untuk hari itu.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_t4">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_t4" aria-expanded="false" aria-controls="collapseFour_t4">Bagaimana saya boleh guna kad untuk membeli makanan di kantin atau membeli alat tulis di kedai buku?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_t4" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_t4">
                            <div class="panel-body" style="text-align:justify">
                                Untuk membuat pembayaran, sentuh terminal jualan dengan kad anda di kantin atau kedai buku. 
                                            Anda akan mendengar bunyi “bip” dari terminal selepas jumlah berjaya ditolak dari kad anda.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_t5">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_t5" aria-expanded="false" aria-controls="collapseFour_t5">Bagaimana saya boleh menambah nilai baki kad saya?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_t5" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_t5">
                            <div class="panel-body" style="text-align:justify">
                                Dalam aplikasi mudah alih i-3s atau portal web, klik di bahagian “Wallet”. Anda boleh tambah nilai  
                                            e-wallet anda melalui kad kredit, kad debit atau perbankan dalam talian pada bila-bila masa.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_t6">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_t6" aria-expanded="false" aria-controls="collapseFour_t6">Bagaimana jika saya tidak mempunyai wang yang mencukupi dalam kad
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_t6" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_t6">
                            <div class="panel-body" style="text-align:justify">
                                Jika baki kad tidak mencukupi, urus niaga tidak akan berlaku dan tiada jumlah akan ditolak. Terminal jualan akan memaparkan mesej “Baki tidak mencukupi”.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_t7">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_t7" aria-expanded="false" aria-controls="collapseFour_t7">Bagaimana saya boleh menyemak baki kad saya?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_t7" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_t7">
                            <div class="panel-body" style="text-align:justify">
                                Anda boleh menggunakan mana-mana pembaca kad di kelas di dalam sekolah. Sentuh kad anda ke pembaca kad. Anda akan mendengar “bip” dan jumlah baki kad akan dipaparkan di paparan LED pada pembaca kad.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_t8">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_t8" aria-expanded="false" aria-controls="collapseFour_t8">Bagaimana jika saya terlupa membawa kad pada hari sekolah?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_t8" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_t8">
                            <div class="panel-body" style="text-align:justify">
                                Sila hubungi pentadbir sekolah anda untuk kad pelajar sementara yang boleh digunakan dengan serta-merta untuk kehadiran & pembelian anda di sekolah
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_t9">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_t9" aria-expanded="false" aria-controls="collapseFour_t9">Bagaimana jika saya kehilangan kad saya atau ia tidak berfungsi?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_t9" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_t9">
                            <div class="panel-body" style="text-align:justify">
                                Sila laporkan kepada kami melalui aplikasi mudah alih i-3s atau portal web, atau hubungi khidmat pelanggan kami 
                                            untuk menggantung penggunaan kad anda.  Anda boleh menghubungi pentadbir sekolah untuk mohon kad gantian dan mendapatkan kad sementara untuk 
                                            kehadiran & pembelian anda di sekolah. Kad gantian baru akan dikeluarkan dalam tempoh 5 hari dan yuran penggantian sebanyak RM12.00 akan dikenakan 
                                            atau ditolak dari baki kad. Baki kad yang hilang akan dipindahkan ke kad baru.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_t10">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_t10" aria-expanded="false" aria-controls="collapseFour_t10">Bagaimana saya memantau kehadiran pelajar kelas saya?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_t10" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_t10">
                            <div class="panel-body" style="text-align:justify">
                                Anda boleh memantau kehadiran pelajar kelas anda dengan mengklik tab Kelas / Kelab dalam aplikasi mudah alih i-3s.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_t11">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_t11" aria-expanded="false" aria-controls="collapseFour_t11">Bagaimana saya menggunakan aplikasi mudah alih i-3s untuk aktiviti kelas dan kelab?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_t11" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_t11">
                            <div class="panel-body" style="text-align:justify">
                                Dalam aplikasi mudah alih i-3s, klik tab Kelas / Kelab. Anda perlu mengenal pasti dan mendaftarkan para pelajar 
                                            sebagai ahli kelab, persatuan, dan sebagainya.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_t12">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_t12" aria-expanded="false" aria-controls="collapseFour_t12">Bagaimana saya melindungi & menjaga kad?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_t12" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_t12">
                            <div class="panel-body" style="text-align:justify">
                                Jangan pinjamkan kad anda kepada orang lain. Sekiranya kotor, sila lap dengan tuala lembap. Jangan basuh atau tenggelamkan ke dalam air. 
                                            Elakkan menggaru permukaan kad kerana ia boleh merosakkan kad dan menjadikannya tidak boleh dibaca.
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div role="tabpanel" class="tab-pane fade" id="staff">
            <div class="col-xs-12 ol-sm-12 col-md-12 col-lg-12">
                <div class="panel-group" id="accordion_st1" role="tablist" aria-multiselectable="false">
                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingOne_st1">
                            <h4 class="panel-title">
                                <a role="button" data-toggle="collapse" href="#collapseOne_st1" aria-expanded="true" aria-controls="collapseOne_st1">Bagaimana cara mendaftar akaun i-3s?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseOne_st1" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne_st1">
                            <div class="panel-body" style="text-align:justify">
                                Pendaftaran anda dibuat secara automatik berdasarkan rekod anda dalam maklumat sekolah anda. Muat turun aplikasi i-3s dari sama ada Apple Store atau Play Store dari telefon pintar mudah alih anda. 
                                            Sila log masuk, menggunakan nombor kad pengenalan anda dan ikuti arahan. e-Wallet anda akan dibuat.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_st2">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_st2" aria-expanded="false" aria-controls="collapseFour_st2">Adakah kakitangan diberi kad kakitangan?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_st2" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_st2">
                            <div class="panel-body" style="text-align:justify">
                                Ya, kakitangan akan diberi kad untuk digunakan di sekolah. Kakitangan boleh merekod kehadiran dan membuat pembelian di sekolah menggunakan kad.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_st3">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_st3" aria-expanded="false" aria-controls="collapseFour_st3">Bagaimana saya rekod kehadiran saya?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_st3" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_st3">
                            <div class="panel-body" style="text-align:justify">
                                Anda perlu menyentuh kad anda ke pembaca kad. Pembaca kad dipasang di pintu atau di dalam pejabat. 
                                            Apabila pembaca kad berjaya membaca kad, anda akan mendengar bunyi “bip”. Kehadiran anda telah direkodkan untuk hari itu.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_st4">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_st4" aria-expanded="false" aria-controls="collapseFour_st4">Bagaimana saya boleh guna kad untuk membeli makanan di kantin atau membeli alat tulis di kedai buku?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_st4" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_st4">
                            <div class="panel-body" style="text-align:justify">
                                Untuk membuat pembayaran, sentuh terminal jualan dengan kad anda di kantin atau kedai buku. Anda akan mendengar bunyi “bip” dari terminal selepas jumlah berjaya ditolak dari kad anda.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_st5">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_st5" aria-expanded="false" aria-controls="collapseFour_st5">Bagaimana saya boleh menambah nilai baki kad saya?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_st5" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_st5">
                            <div class="panel-body" style="text-align:justify">
                                Dalam aplikasi mudah alih i-3s atau portal web, klik di bahagian “Wallet”. Anda boleh tambah nilai  
                                            e-wallet anda melalui kad kredit, kad debit atau perbankan dalam talian pada bila-bila masa.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_st6">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_st6" aria-expanded="false" aria-controls="collapseFour_st6">Bagaimanakah saya boleh tambah nilai e-wallet saya dari aplikasi mudah alih i-3s?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_st6" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_st6">
                            <div class="panel-body" style="text-align:justify">
                                1. Log masuk ke akaun i-3s pada aplikasi mudah alih dari telefon pintar anda.<br />
                                2.	Klik icon ‘<img src="Content/images/plus_btn.png" style="width: 4%;" />’ di sudut kanan atas untuk menambah akaun anda.<br />
                                3.	Masukkan / pilih jumlah yang dikehendaki dan klik butang ‘Top-up e-wallet’. Pilih kaedah pembayaran pilihan anda dan klik butang hijau untuk mengesahkan
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_st7">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_st7" aria-expanded="false" aria-controls="collapseFour_st7">Bagaimana jika saya tidak mempunyai wang yang mencukupi dalam kad?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_st7" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_st7">
                            <div class="panel-body" style="text-align:justify">
                                Jika baki kad tidak mencukupi, urus niaga tidak akan berlaku dan tiada jumlah akan ditolak. Terminal jualan akan memaparkan mesej “Baki tidak mencukupi”. 
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_st8">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_st8" aria-expanded="false" aria-controls="collapseFour_st8">Bagaimana saya boleh menyemak baki kad saya?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_st8" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_st8">
                            <div class="panel-body" style="text-align:justify">
                                Anda boleh menggunakan mana-mana pembaca kad di dalam sekolah. 
                                            Sentuh kad anda ke pembaca kad. Anda akan mendengar “bip” dan jumlah baki kad akan dipaparkan di paparan LED pada pembaca kad.
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_st9">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_st9" aria-expanded="false" aria-controls="collapseFour_st9">Bagaimana jika saya terlupa membawa kad pada hari sekolah?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_st9" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_st9">
                            <div class="panel-body" style="text-align:justify">
                                Sila hubungi pentadbir sekolah anda untuk kad pelajar sementara yang boleh digunakan dengan serta-merta untuk kehadiran & pembelian anda di sekolah. 
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_st10">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_st10" aria-expanded="false" aria-controls="collapseFour_st10">Bagaimana jika saya kehilangan kad saya atau ia tidak berfungsi?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_st10" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_st10">
                            <div class="panel-body" style="text-align:justify">
                                Sila laporkan kepada kami melalui aplikasi mudah alih i-3s atau portal web, atau hubungi khidmat pelanggan kami untuk menggantung 
                                            penggunaan kad anda.  Anda boleh menghubungi pentadbir sekolah untuk mohon kad gantian dan mendapatkan kad sementara untuk kehadiran & pembelian anda di sekolah. 
                                            Kad gantian baru akan dikeluarkan dalam tempoh 5 hari dan yuran penggantian sebanyak RM12.00 akan dikenakan atau ditolak dari baki kad. Baki kad yang hilang akan 
                                            dipindahkan ke kad baru. 
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-col-slate-grey">
                        <div class="panel-heading" role="tab" id="headingFour_st11">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFour_st11" aria-expanded="false" aria-controls="collapseFour_st11">Bagaimana saya melindungi & menjaga kad?
                                </a>
                            </h4>
                        </div>
                        <div id="collapseFour_st11" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour_st11">
                            <div class="panel-body" style="text-align:justify">
                                Kad anda mengandungi maklumat penting. Anda harus menjaga kad itu dengan baik. Jangan pinjamkan kad anda kepada orang lain. 
                                            Sekiranya kotor, sila lap dengan tuala lembap. Jangan basuh atau tenggelamkan ke dalam air. Elakkan menggaru permukaan kad kerana ia boleh merosakkan 
                                            kad dan menjadikannya tidak boleh dibaca.
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>

    </div>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SeatManageWebV2.Index" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>高校云选座平台</title>
    <link href="NewContent/css/framework-font.css" rel="stylesheet" />
    <link href="NewContent/css/framework-login.css" rel="stylesheet" />
    <script src="NewContent/js/jquery/jquery-2.1.1.min.js"></script>
    <script src="NewContent/js/cookie/jquery.cookie.js"></script>
    <script src="NewContent/js/md5/jquery.md5.js"></script>
    <!--[if lte IE 8]>
        <div id="errorie"><div>您还在使用老掉牙的IE，正常使用系统前请升级您的浏览器到 IE8以上版本 <a target="_blank" href="http://windows.microsoft.com/zh-cn/internet-explorer/ie-8-worldwide-languages">点击升级</a>&nbsp;&nbsp;强烈建议您更改换浏览器：<a href="http://down.tech.sina.com.cn/content/40975.html" target="_blank">谷歌 Chrome</a></div></div>
    <![endif]-->
        <meta name="vs_defaultClientScript" content="JavaScript" />

    <script src="Content/js/jquery/jquery-2.1.1.min.js" type="text/javascript"></script>
    <script src="Content/plugin/encrypt/aes.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        window.moveTo(0, 0);
        window.resizeTo(window.screen.availWidth, window.screen.availHeight);
        function SubmitExchange() {
            document.all.cmdOK.click();
        }
        function formReset() {
            From1.txtUserName.value = "";
            From1.txtPassword.value = "";

        }
        if (jQuery.browser.msie) {
            alert("为了保证功能的正常使用，建议使用FireFox或Chrome游览器，如果您使用的是360或搜狗游览器请打开急速模式！");
            //window.close();
        }


        function checkForm() {
            var $username = $("#txtUserName").val();
            var $password = $("#txtPassword").val();
            $("#txtUserName").val($username);
            $("#txtPassword").val($password);
           return true;
        }


    </script>
    <style type="text/css">

        .button
        {
            background-color:#1ab085;
            font-size:16px;
             text-align:center;
            color:white;
        }

    </style>
</head>
<body>
        
    <div class="wrapper">
        <div class="container">
            <div class="logo">
                <i class="fa fa-modx"></i>
                <h1><span>CW</span>高校云选座平台</h1>
            </div>
            <form id="From1" class="form" method="post" runat="server" action="Index.aspx">
                <div class="row">
                    
                    <asp:TextBox ID="txtUserName"  runat="server" placeholder="登录账号"></asp:TextBox>

                    <i class="fa fa-user"></i>
                </div>
                <div class="row">
                 <asp:TextBox ID="txtPassword" placeholder="登录密码"  runat="server" TextMode="password"
                                            onKeyUp="if (event.keyCode==13) cmdOK.onclick()"></asp:TextBox>
                    <i class="fa fa-key"></i>
                </div>
                <div class="row">
                    <asp:Button ID="cmdOK" CssClass="button" runat="server" OnClientClick="return checkForm();"  OnClick="cmdOK_Click"  PostBackUrl="Index.aspx" Text="登录" />
                </div>
                <div class="row">
                </div>
                    </form>
            <div class="login_tips"></div>
        </div>
        <ul class="bg-bubbles">
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
        </ul>
    </div>
    <div class="copyright">
        <a href="http://www.gxchuwei.com" style="text-decoration:none;color:#fff;">楚惟团队出品</a>
        <br>
       适用浏览器：IE8以上、360、FireFox、Chrome、Safari、Opera、傲游、搜狗、世界之窗.
    </div>
    <script type="text/javascript">
        (function ($) {
            $.login = {
                formMessage: function (msg) {
                    $('.login_tips').find('.tips_msg').remove();
                    $('.login_tips').append('<div class="tips_msg"><i class="fa fa-question-circle"></i>' + msg + '</div>');
                },
                loginClick: function () {
                    var $username = $("#txt_account");
                    var $password = $("#txt_password");
                    var $code = $("#txt_code");
                    if ($username.val() == "") {
                        $username.focus();
                        $.login.formMessage('请输入登录账号');
                        return false;
                    } else if ($password.val() == "") {
                        $password.focus();
                        $.login.formMessage('请输入登录密码。');
                        return false;
                    }
                    //else if ($code.val() == "") {
                    //    $code.focus();
                    //    $.login.formMessage('请输入验证码。');
                    //    return false;
                    //}
                    else {
                        $("#login_button").attr('disabled', 'disabled').find('span').html("loading...");
                        $.ajax({
                            url: "/Login/CheckLogin",
                            data: { username: $.trim($username.val()), password: $.md5($.trim($password.val())), code: $.trim($code.val()) },
                            type: "post",
                            dataType: "json",
                            success: function (data) {
                                if (data.state == "success") {
                                    $("#login_button").find('span').html("登录成功，正在跳转...");
                                    window.setTimeout(function () {
                                        window.location.href = "/Home/Index";
                                    }, 500);
                                } else {
                                    $("#login_button").removeAttr('disabled').find('span').html("登录");
                                    $("#switchCode").trigger("click");
                                    $code.val('');
                                    $.login.formMessage(data.message);
                                }
                            }
                        });
                    }
                },
                init: function () {
                    $('.wrapper').height($(window).height());
                    $(".container").css("margin-top", ($(window).height() - $(".container").height()) / 2 - 50);
                    $(window).resize(function (e) {
                        $('.wrapper').height($(window).height());
                        $(".container").css("margin-top", ($(window).height() - $(".container").height()) / 2 - 50);
                    });
                    $("#switchCode").click(function () {
                        $("#imgcode").attr("src", "/Login/GetAuthCode?time=" + Math.random());
                    });
                    var login_error = top.$.cookie('nfine_login_error');
                    if (login_error != null) {
                        switch (login_error) {
                            case "overdue":
                                $.login.formMessage("系统登录已超时,请重新登录");
                                break;
                            case "OnLine":
                                $.login.formMessage("您的帐号已在其它地方登录,请重新登录");
                                break;
                            case "-1":
                                $.login.formMessage("系统未知错误,请重新登录");
                                break;
                        }
                        top.$.cookie('nfine_login_error', '', { path: "/", expires: -1 });
                    }
                    $("#login_button").click(function () {
                        $.login.loginClick();
                    });
                    document.onkeydown = function (e) {
                        if (!e) e = window.event;
                        if ((e.keyCode || e.which) == 13) {
                            document.getElementById("login_button").focus();
                            document.getElementById("login_button").click();
                        }
                    }
                }
            };
            $(function () {
                $.login.init();
            });
        })(jQuery);
    </script>
        
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyQRCode.aspx.cs" Inherits="SeatManageWebV2.FunctionPages.WechatCode.MyQRCode" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <link href="../../Styles/main.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            font-size: small;
        }
        .style2
        {
            font-family: 黑体;
            font-weight: bold;
            font-size: x-large;
        }
    </style>
</head>
<body bgcolor="#d0ebe2">
    <form id="form1" runat="server">
    <table width="800" style="background: #d0ebe2;">
        <tr align="left">
            <td>
                <span class="style2">第一步：微信扫一扫，关注下方公众号</span>
            </td>
        </tr>
       <tr align="left">
            <td>
                <img src="../../Images/wechat.jpg" style="height: 242px; width: 243px" />
            </td>
        </tr>
                <tr align="left">
            <td>
                <span class="style2">第二步:在微信公众号菜单点击账号绑定，然后进行扫描下方二维码，绑定成功后即可开始使用微信端</span>
            </td>
        </tr>
         <tr align="left">
            <td>
                <asp:Image ID="Image1" runat="server"  style="height: 242px; width: 243px" />
            </td>
        </tr>
        </table>
    </form>
</body>
</html>

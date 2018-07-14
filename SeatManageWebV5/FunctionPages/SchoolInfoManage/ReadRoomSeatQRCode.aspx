<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReadRoomSeatQRCode.aspx.cs" Inherits="SeatManageWebV2.FunctionPages.SchoolInfoManage.ReadRoomSeatQRCode" %>

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
<body>
    <form id="form1" runat="server">
        <table width="99%" style="background: #d0ebe2;">
        <tr align="left">
            <td>
                <span class="style2">阅览室编号：<%=CurrentRomm.No %>&nbsp;&nbsp;&nbsp;&nbsp;阅览室名称：<%=CurrentRomm.Name %> </span>
                   <%if (System.IO.Directory.Exists(SubPath))
                       { %>
                     <asp:Button ID="btnDownLoad" runat="server" Text="打包下载" OnClick="btnDownLoad_Click" />
                    <%}
                        else
                    { %>
                   <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="请先生成座位二维码方可打包下载"></asp:Label>
&nbsp;<%} %></td>
        </tr>
        </table>
        <table width="99%" style="background: #d0ebe2;">
        <tr align="left">
            <td>座位编号</td>
            <td>二维码图片</td>
        </tr>
            <%foreach (SeatManage.ClassModel.Seat item in CurrentListSeat)
                {
                   // string imgUrl = "~/SeatQRCode/" + CurrentRomm.No + "/" + item.SeatNo + ".jpg";
                    %>
        <tr align="left">
            <td><%=item.SeatNo %></td>
            <td>
                <img width="100" height="100" alt="" src='/SeatQRCode/<%=CurrentRomm.No %>/<%=item.SeatNo %>.jpg' /></td>
        </tr>
               <% } %>


        </table>
    </form>
</body>
</html>

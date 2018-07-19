<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="SeatManageWebV2.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    	    <link href="../../Styles/main.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/SeatGraph.css" rel="stylesheet" type="text/css" />
    <title></title>
     <script type="text/javascript" src="../../Scripts/SeatGraphHandle.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery-1.4.1.js"></script>
        <script type="text/javascript">
        $(document).ready(
            function () {
                loadBespeakSeatNowDayLayout();
                intervalRun();
            })

        function intervalRun() {
            var interval;
            interval = setInterval(loadBespeakSeatNowDayLayout, '5000');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">

            <div>
        阅览室编号：
        <asp:TextBox id="hiddenRoomNum" runat="server"></asp:TextBox>
        日期：
        <asp:TextBox id="date" runat="server"></asp:TextBox>
        <asp:Button runat="server" OnClick="submit" Text="查询"></asp:Button>
    </div>
    <div class="mainDiv">
      <div id="divSeatGraphMain" class="SeatGraphMain">
             正在载入……
        </div>
    </div>

    </form>
</body>
</html>

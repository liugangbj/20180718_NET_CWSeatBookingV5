<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="SeatManageWebV5.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../../Styles/main.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/SeatGraph.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Scripts/SeatGraphHandle.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery-1.4.1.js"></script>
    <script type="text/javascript">
        $(document).ready(
            function () {
                loadBespeakSeatLayoutInterface();
                intervalRun();
            })

        function intervalRun() {
            var interval;
            interval = setInterval(loadBespeakSeatLayoutInterface, '5000');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager" runat="server" AutoSizePanelID="RegionPanel_1"
        HideScrollbar="true" />
    <ext:Window ID="bespeakHandleWindow" Title="预约确认" Popup="false" EnableIFrame="true"
        IFrameUrl="about:blank" EnableMaximize="true" Target="Self" EnableResize="false"
        AutoHeight="true" runat="server" IsModal="true" Width="300px" EnableConfirmOnClose="true" 
        Height="230px">
    </ext:Window>
    <div id="bub_box" onclick="tipHidden()" style="position: absolute; z-index: 2147483647;
        width: 200px; left: 63px; display: none; top: 140px;">
        <div class="ns_bub_box-arrow" style="border-top: transparent 15px dashed; border-left: #e6e6e6 15px solid;
            position: absolute; left: 15px;">
        </div>
        <div id="bub_Content" class="ns_bub_wrapper" style="position: absolute; top: 10px;
            box-shadow: 3px 3px 3px #ccc; padding: 4px; background: #e6e6e6; border-radius: 5px;">
        </div>
    </div>
    <div class="mainDiv">
      <div id="divSeatGraphMain" class="SeatGraphMain">

      </div>
    </div>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     </iframe></div></div></div></div><div class="x-window-bl x-panel-nofooter" id="ext-gen8"><div class="x-window-br"><div class="x-window-bc"></div></div></div></div><a href="#" class="x-dlg-focus" tabindex="-1" id="ext-gen13">&nbsp;</a></div></div>
    </form>
</body>
</html>

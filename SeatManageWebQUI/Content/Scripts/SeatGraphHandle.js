//透明层


var divTop;
var divleft;
function ThumbnailClick(objThunbnail, event, scaleX, scaleY, moveX, moveY) {
    //event.stopEvent();
    var asd = "";

    //半透明div移动的距离
    var divTransparent = document.getElementById("divTransparent");
    divTop = getY(objThunbnail, event) - divTransparent.offsetHeight / 2;
    divleft = getX(objThunbnail, event) - divTransparent.offsetWidth / 2;
    divTransparent.style.top = divTop + 'px';
    divTransparent.style.left = divleft + 'px';

    var seatLayoutTop, seatLayoutLeft;
    seatLayoutTop = -((divTop - moveY) * scaleY) + "px";
    seatLayoutLeft = -((divleft - moveX) * scaleX) + "px";
    $("#divSeatLayout").css("top", seatLayoutTop);
    $("#divSeatLayout").css("left", seatLayoutLeft);
}
//座位管理处理窗口
function seatClick(urlParameters) {
    if (urlParameters == "" || urlParameters == NaN) {
        return;
    }
    //new 

    var used = urlParameters.charAt(urlParameters.length - 1);
  
    var diag = new top.Dialog();
    diag.Title = "座位操作";
    diag.URL = "/SeatMonitor/SeatHandle" + urlParameters;
    diag.Width = 600;
    diag.Height = 400;
    diag.OkButtonText = "关闭";
    //顺序很重要，diag.show()之前添加确定按钮事件，show之后添加新按钮
    diag.OKEvent = function () {
        diag.close();
        //alert("关闭");
        //diag.innerFrame.contentWindow.submitHandler(0);
    };
    diag.show();

    if (used == '0')//seatUsed=0          空闲状态，不能暂离，释放，黑名单，只能分配
    {
        diag.addButton("next", " 分配座位 ", function () {
            var inputValue = diag.innerFrame.contentWindow.document.getElementById('txtCardNo').value;
           // alert(inputValue);//txtCardNo
            urlParameters += "&cardNo="+inputValue;
            //SeatMonitor
            $.ajax({
                url: "/SeatMonitor/SureAllotSeat" + urlParameters,
               // data: { username: $.trim($username.val()), password: $password.val() },
                type: "post",
                dataType: "json",
                success: function (data) {
                    if (data.status == "yes") {
                        top.Toast('showSuccessToast', data.message);
                        diag.close();
                    } else {
                        top.Toast('showErrorToast', data.message);
                    }
                }
            });
        });
    } else if (used == '2') {// seatUsed == "2"     已被预约，不能暂离，释放，黑名单，不能分配
        
    }
    else if (used == '3') {//seatUsed == "3"    座位停用状态，不能暂离，释放，黑名单，不能分配


    } else {  //其余的：可以加黑名单，可以释放，可以暂离，不能分配

        diag.addButton("next", " 暂离/取消暂离 ", function () {
            var inputValue = diag.innerFrame.contentWindow.document.getElementById('txtbtnShortLeave').value;
            if (inputValue == "暂离") {
                urlParameters += "&isShortLeave=y";
            } else {
                urlParameters += "&isShortLeave=n";
            }
            $.ajax({
                url: "/SeatMonitor/ShortLeave" + urlParameters,
                type: "post",
                dataType: "json",
                success: function (data) {
                    if (data.status == "yes") {
                        top.Toast('showSuccessToast', data.message);
                        diag.close();
                    } else {
                        top.Toast('showErrorToast', data.message);
                    }
                }
            });
        });
        diag.addButton("next", " 释放 ", function () {
            //Leave
            $.ajax({
                url: "/SeatMonitor/Leave" + urlParameters,
                type: "post",
                dataType: "json",
                success: function (data) {
                    if (data.status == "yes") {
                        top.Toast('showSuccessToast', data.message);
                        diag.close();
                    } else {
                        top.Toast('showErrorToast', data.message);
                    }
                }
            });
        });
        diag.addButton("next", " 加入黑名单 ", function () {
            //addBlackListRemark  SureAddBlacklist
            var addBlackListRemark = diag.innerFrame.contentWindow.document.getElementById('addBlackListRemark').value;
            var CardNo = diag.innerFrame.contentWindow.document.getElementById('CardNo').value;
            urlParameters += "&CardNo=" + CardNo + "&addBlackListRemark=" + addBlackListRemark;
            $.ajax({
                url: "/SeatMonitor/SureAddBlacklist" + urlParameters,
                type: "post",
                dataType: "json",
                success: function (data) {
                    if (data.status == "yes") {
                        top.Toast('showSuccessToast', data.message);
                        diag.close();
                    } else {
                        top.Toast('showErrorToast', data.message);
                    }
                }
            });
        });

    }
    //old
    //X("seatHandleWindow").box_show("/SeatMonitor/SeatHandle" + urlParameters, '座位操作');
   // X("seatHandleWindow").box_show("../SeatMonitor/SeatHandle.aspx" + urlParameters, '座位操作');
}
//座位预约确认窗口
function BespeakSeatClick(urlParameters) {
    if (urlParameters == "" || urlParameters == NaN) {
        return;
    }
    var diag = new top.Dialog();
    diag.Title = "座位操作";
    diag.URL = "/SeatBespeak/BespeakSubmitWindow?parm=" + urlParameters;
    diag.Width = 450;
    diag.Height = 300;
    diag.ShowButtonRow = true;
    diag.ShowOkButton = false;
    diag.CancelButtonText = " 关 闭 ";
    //顺序很重要，diag.show()之前添加确定按钮事件，show之后添加新按钮
    diag.show();
    diag.addButton("next", " 提交 ", function () {
         diag.innerFrame.contentWindow.submitForm();
    });
}
//座位预约确认窗口(当天)
function BespeakSeatNowDayClick(urlParameters) {
    if (urlParameters == "" || urlParameters == NaN) {
        return;
    }
  //  alert(urlParameters);
   
    var diag = new top.Dialog();
    diag.Title = "座位操作";
    diag.URL = "/SeatBespeak/BespeakNowDayHandle?parameters=" + urlParameters;
    diag.Width = 450;
    diag.Height = 300;
    diag.ShowButtonRow = true;
    diag.ShowOkButton = false;
    diag.CancelButtonText = " 关 闭 ";
    //顺序很重要，diag.show()之前添加确定按钮事件，show之后添加新按钮
    diag.show();
    diag.addButton("next", " 提交 ", function () {
        
   
        diag.innerFrame.contentWindow.submitFormNowDay();
    });
  //  X("bespeakHandleWindow").box_show("../SeatBespeak/BespeakNowDayHandle.aspx?parameters=" + urlParameters, '座位预约');
}
//预约座位设置窗口
function BespeakSeatSettingClick(seatNo, urlParameters) {
    //alert(seatNo + "$$" + urlParameters);
    if (urlParameters == "" || urlParameters == NaN) {
        return;
    }
    $.ajax({
        url: "/SchoolInfoManage/BespeakSeatSettingCanBook?" + urlParameters,
     //   data: { username: $.trim($username.val()), password: $password.val() },
        type: "post",
        dataType: "json",
        success: function (data) {
            if (data.status == "yes") {
                loadBespeakSeatSettingLayout();
            } else {
          
            }
        }
    });


    //X("seatBespeakSettingWindow").box_show("BespeakSeatSettingWindow.aspx?" + urlParameters, '预约座位设置');
    //var seatStatus = $("#seatStatus").val();
    //nobook表示座位未设置预约，canbook表示当前座位已设置预约
//    if (seatStatus == "nobook" || seatStatus == "" || seatStatus == NaN) {
//        $("#subCmd").val('setBook');
//        $("#seatNo").val(seatNo);
//        form1.submit();
//    }
//    else {
//        $("#subCmd").val('setNoBook');
//        $("#seatNo").val(seatNo);
//        form1.submit();
//    }
}

function trimStr(str) { return str.replace(/(^\s*)|(\s*$)/g, ""); }

function loadSeatLayout() {
    var roomNum = roomId;//trimStr($("#hiddenRoomNum").val());
   // alert(roomNum);
    if (roomNum.length != 6) {
        alert("传入的阅览室编号长度只能是6位！");
    }
    else {
        $.ajax({ //一个Ajax过程 
            type: "post", //使用get方法访问后台
            dataType: "html", //返回json格式的数据
            // dataType: "text",
            url: "/SeatMonitor/DrowSeatLayoutHtml", //要访问的后台地址
          //  url: "SeatLayout.ashx", //要访问的后台地址
            data: { "roomNum": roomNum, "divTransparentTop": divTop, "divTransparentLeft": divleft }, //要发送的数据

            // complete: function () { $("#load").hide(); }, //AJAX请求完成时隐藏loading提示
            success: function (msg) {//msg为返回的数据，在这里做数据绑定
                $("#divSeatGraphMain").html(msg);

            },
            error: function () {
                //alert("error");
            }
        });
    }

}
function loadBespeakSeatLayout() {
   // var roomNum = $("#hiddenRoomNum").val();
  //  var bespeakDate = $("#hiddenDate").val();

    $.ajax({ //一个Ajax过程 
        type: "post", //使用get方法访问后台
        dataType: "html", //返回json格式的数据 
       // url: "SeatLayoutHandle.ashx", //要访问的后台地址
        url: "/SeatBespeak/drowBespeakSeatLayOutHtml", //要访问的后台地址
        data: { "roomNum": roomNum, "date": bespeakDate, "divTransparentTop": divTop, "divTransparentLeft": divleft }, //要发送的数据

        // complete: function () { $("#load").hide(); }, //AJAX请求完成时隐藏loading提示
        success: function (msg) {//msg为返回的数据，在这里做数据绑定
            $("#divSeatGraphMain").html(msg);

        },
        error: function () {
            //alert("error");
        }
    });

}
function loadBespeakSeatNowDayLayout() {
    //var roomNum = $("#hiddenRoomNum").val();

    $.ajax({ //一个Ajax过程 
        type: "post", //使用get方法访问后台
        dataType: "html", //返回json格式的数据 
        url: "/SeatBespeak/NowBespeakSeatLayoutHTML",
        //url: "NowBespeakSeatLayout.ashx", //要访问的后台地址
        data: { "roomNum": roomNum, "divTransparentTop": divTop, "divTransparentLeft": divleft }, //要发送的数据

        // complete: function () { $("#load").hide(); }, //AJAX请求完成时隐藏loading提示
        success: function (msg) {//msg为返回的数据，在这里做数据绑定
            $("#divSeatGraphMain").html(msg);

        },
        error: function () {
            //alert("error");
        }
    });

}
function loadBespeakSeatSettingLayout() { //可预约座位
    // var roomNum = $("#hiddenRoomNum").val();
    var roomNum = roomId;
    $.ajax({ //一个Ajax过程 
        type: "post", //使用get方法访问后台
        dataType: "html", //返回json格式的数据 
        url: "/SchoolInfoManage/DrawBespeakSeatSettingLayout",//"BespeakSeatGraph.ashx", //要访问的后台地址
        data: { "roomNum": roomNum, "divTransparentTop": divTop, "divTransparentLeft": divleft }, //要发送的数据

        // complete: function () { $("#load").hide(); }, //AJAX请求完成时隐藏loading提示
        success: function (msg) {//msg为返回的数据，在这里做数据绑定
            $("#divSeatGraphMain").html(msg);

        },
        error: function () {
            //alert("error");
        }
    });

}
//获取鼠标点击时先对于点击对象的X坐标
function getX(obj, event) {
    var newleft = $(obj).offset().left;
    newleft = (event.clientX - newleft - 8 + document.body.scrollLeft);
    return newleft;
}
//获取鼠标点击时相对于点击对象的Y坐标
function getY(obj, event) {
    var newtop = $(obj).offset().top;
    newtop = (event.clientY - newtop + document.body.scrollLeft);
    return newtop;
}

function tipShow(object, tipContent) {

    var actualLeft = $(object).offset().left;
    var actualTop = $(object).offset().top;

    actualTop = actualTop + 42;
    actualLeft = actualLeft + 10;
    $("#bub_box").css("top", actualTop + "px");
    $("#bub_box").css("left", actualLeft + "px");
    $("#bub_box").css("display", "block");
    $("#bub_Content").html(tipContent);
}


function tipShowPad(object, tipContent) {

    var actualLeft = $(object).offset().left;
    var actualTop = $(object).offset().top;

    actualTop = actualTop + 42;
    if (($(window).width() - actualLeft) < 150) {

        if (object.className != "RealSeatFree") {
            actualLeft = actualLeft - 80;
            $("#bub_JanTou").css("left", "120px");
        }
        else {
            actualLeft = actualLeft + 10;
            $("#bub_JanTou").css("left", "15px");
        }
    }
    else {
        actualLeft = actualLeft + 10;
        $("#bub_JanTou").css("left", "15px");
    }
    $("#bub_box").css("top", actualTop + "px");
    $("#bub_box").css("left", actualLeft + "px");
    $("#bub_box").css("display", "block");
    $("#bub_Content").html(tipContent);
}
function tipHidden(clickObject) {
    $("#bub_box").css("display", "none");
}

/*
* 座位操作
*/
function showDiv(showDiv, hiddenDiv) {
    $("#" + showDiv).css("display", "block");
    $("#" + hiddenDiv).css("display", "none");
    return false;
}

function hiddenAll(showDiv, hiddenDiv) {
    $("#" + showDiv).css("display", "none");
    $("#" + hiddenDiv).css("display", "none");
    return false;
}
function loadBespeakSeatLayoutInterface() {
    var roomNo = $("#hiddenRoomNum").val();
    var Date = $("#hiddenDate").val();
    var schoolNo = $("#hiddenSchoolNo").val();
    var studentNo = $("#hiddenStudentNo").val();

    $.ajax({ //一个Ajax过程 
        type: "post", //使用get方法访问后台
        dataType: "html", //返回json格式的数据 
        url: "SeatLayoutInterface.asmx", //要访问的后台地址
        data: { "schoolNo": schoolNo, "studentNo": studentNo, "date": Date, "roomNum": roomNo, "divTransparentTop": divTop, "divTransparentLeft": divleft }, //要发送的数据

        // complete: function () { $("#load").hide(); }, //AJAX请求完成时隐藏loading提示
        success: function (msg) {//msg为返回的数据，在这里做数据绑定
            $("#divSeatGraphMain").html(msg);

        },
        error: function () {
            //alert("error");
        }
    });

}
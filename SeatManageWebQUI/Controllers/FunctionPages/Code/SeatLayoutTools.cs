﻿using SeatManage.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SeatManageWebQUI.Controllers.FunctionPages.Code
{
    public class SeatLayoutTools
    {
        /// <summary>
        /// 绘制Html的座位布局
        /// </summary>
        /// <param name="roomNum">房间编号</param>
        /// <param name="divTransparentTop">透明层距离顶部的位置</param>
        /// <param name="divTransparentLeft">透明层距离左边的位置</param>
        /// <returns></returns>
        public string drowSeatLayoutHtml(string roomNum, string divTransparentTop, string divTransparentLeft)
        {
            long tmp = 0;
            if (string.IsNullOrEmpty(roomNum))
            {
                return "阅览室编号不能为空";
            }
            else if (roomNum.Length != 6)
            {
                return "阅览室编号长度不符合规定";
            }
            else if (!long.TryParse(roomNum, out tmp))
            {
                return "传入的阅览室编号不符合系统要求";
            }
            try
            {
                SeatManage.ClassModel.SeatLayout _SeatLayout = EnterOutOperate.GetRoomSeatLayOut(roomNum);
                if (_SeatLayout == null)
                {
                    return "获取阅览室座位布局出错了";
                }
                StringBuilder seatLayoutHtml = new StringBuilder();
                //布局实际图
                //绘制实际图窗体大小
                //实际图的长宽

                double layoutWidth = (double)(_SeatLayout.SeatCol * 18);
                double layoutHeight = (double)(_SeatLayout.SeatRow * 18);
                //实际图和缩略图的比例
                double scaleX = (double)layoutWidth / 300;
                double scaleY = (double)layoutHeight / 300;
                double transparentScaleX = layoutWidth / 1245;
                double transparentScaleY = layoutHeight / 685;
                double moveX = 0;
                double moveY = 0;
                if (layoutWidth >= layoutHeight)
                {
                    scaleY = scaleX;
                    moveY = (layoutWidth - layoutHeight) / 2 / scaleY;
                }
                else
                {
                    scaleX = scaleY;
                    moveX = (layoutHeight - layoutWidth) / 2 / scaleX;
                }

                //实际图当前的坐标位置
                if (string.IsNullOrEmpty(divTransparentTop) || string.IsNullOrEmpty(divTransparentLeft))
                {
                    divTransparentTop = moveY.ToString();
                    divTransparentLeft = moveX.ToString();
                }
                double layoutTop = (moveY - double.Parse(divTransparentTop)) * scaleY;
                double layoutLeft = (moveX - double.Parse(divTransparentLeft)) * scaleX;
                seatLayoutHtml.Append("<div id='divSeatLayoutFrom' class='SeatLayoutFrom'>");
                seatLayoutHtml.AppendFormat("<div id='divSeatLayout' class='SeatLayout' style='height:{0}px; width:{1}px;top:{2}px;left:{3}px'>", layoutHeight, layoutWidth, layoutTop, layoutLeft);
                foreach (SeatManage.ClassModel.Seat seat in _SeatLayout.Seats.Values)
                {
                    string shortleaveimg = "/Content/Images/Node/note_blank.png";
                    string powerimh = "/Content/Images/Node/note_blank.png";
                    string readerimg = "/Content/Images/Node/note_blank.png";
                    string seatimg = "/Content/Images/SeatImage/ImgSeat.png";
                    //string seatStyle = "";//座位样式
                    string seatTop = (18 * seat.PositionY).ToString();//座位顶部位置
                    string seatLeft = (18 * seat.PositionX).ToString();//座位左边位置
                    string tipContent = "";
                    string used = "";
                    if (seat.HavePower)
                    {
                        powerimh = "/Content/Images/SeatImage/ImgPower.png";
                    }
                    switch (seat.SeatUsedState)
                    {
                        case SeatManage.EnumType.EnterOutLogType.Leave:
                            if (seat.IsSuspended)
                            {
                                seatimg = "/Content/Images/SeatImage/ImgSeatDisable.png";
                                readerimg = "/Content/Images/SeatImage/ImgStopUse.png";
                                tipContent = string.Format("暂停使用");
                                used = "3";
                            }
                            else
                            {
                                tipContent = string.Format("座位空闲");
                                used = "0";
                            }
                            break;
                        case SeatManage.EnumType.EnterOutLogType.SelectSeat:
                        case SeatManage.EnumType.EnterOutLogType.ReselectSeat:
                        case SeatManage.EnumType.EnterOutLogType.ComeBack:
                        case SeatManage.EnumType.EnterOutLogType.ContinuedTime:
                        case SeatManage.EnumType.EnterOutLogType.BookingConfirmation:
                        case SeatManage.EnumType.EnterOutLogType.WaitingSuccess:
                            readerimg = "/Content/Images/SeatImage/ImgReader.png";
                            seatimg = "/Content/Images/SeatImage/ImgSeatUsing.png";
                            string time = String.Format("{0:MM月dd日 HH:mm:ss}", seat.BeginUsedTime);
                            tipContent = string.Format("学号：{0}<br />姓名：{1}<br />入座时间：{2}", seat.UserCardNo, seat.UserName, time);
                            //TODO:无人管理模式事件 
                            used = "1";
                            break;
                        case SeatManage.EnumType.EnterOutLogType.ShortLeave:
                            seatimg = "/Content/Images/SeatImage/ImgSeatShortLeave.png";
                            shortleaveimg = "/Content/Images/SeatImage/ImgShortLeave.png";
                            readerimg = "/Content/Images/SeatImage/ImgShortLeaveReader.png";
                            time = String.Format("{0:MM月dd日 HH:mm:ss}", seat.BeginUsedTime);
                            tipContent = string.Format("学号：{0}<br />姓名：{1}<br />暂离时间：{2}", seat.UserCardNo, seat.UserName, time);
                            used = "1";
                            break;
                        case SeatManage.EnumType.EnterOutLogType.BespeakWaiting:
                            shortleaveimg = "/Content/Images/SeatImage/ImgBook.png";
                            tipContent = string.Format("已经被预约<br />学号：{0}", seat.UserCardNo);
                            used = "2";
                            break;
                    }
                    string urlParameters = string.Format("?seatNo={0}&seatShortNo={1}&used={2}", seat.SeatNo, seat.ShortSeatNo, used);
                    string seatdiv = "<div id='{0}' class='SeatBackground' style='left: {1}px; top: {2}px;width: 36px; height: 36px;transform: rotate({3}deg); -o-transform: rotate({3}deg); -webkit-transform: rotate({3}deg);-moz-transform: rotate({3}deg);'  onclick='seatClick(\"{4}\")' onmouseover='tipShow(this,\"{5}\")' onmouseout='tipHidden(this)'>" +
                                     "<img src='{10}' height='34px' width='34px' />" +
                                     "<div style='margin-top: -34px; margin-left: 24px; height: 12px; width: 12px'>" +
                                     "<img src='{6}' height='12px' width='12px' />" +
                                     "</div>" +
                                     "<div style='margin-top: 10px; margin-left: 0px; height: 12px; width: 12px'>" +
                                     "<img src='{7}' height='12px' width='12px' />" +
                                     "</div>" +
                                     "<div style='margin-top: -28px; margin-left: 6px; height: 24px; width: 24px'>" +
                                     "<img src='{8}' height='24px' width='24px' />" +
                                     "</div>" +
                                     /* 下方style='margin-top: -34px;这个是旧的，mvc qui版本的需要50px才能适应，备注一下 zdh20181117 */
                                     "<div style='margin-top: -50px; margin-left: 0px;color:white; transform: rotate(-{3}deg); -o-transform: rotate(-{3}deg);" +
                                     "-webkit-transform: rotate(-{3}deg); -moz-transform: rotate(-{3}deg);'>{9}</div></div>";
                    seatLayoutHtml.AppendFormat(seatdiv, seat.SeatNo, seatLeft, seatTop, seat.RotationAngle, urlParameters, tipContent, shortleaveimg, powerimh, readerimg, seat.ShortSeatNo, seatimg);
                }
                //备注
                int countNode = 0;
                foreach (SeatManage.ClassModel.Note note in _SeatLayout.Notes)
                {
                    countNode += 1;
                    string seatTop = (18 * note.PositionY).ToString();//座位顶部位置
                    string seatLeft = (18 * note.PositionX).ToString();//座位左边位置 
                    string noteImage = "note_blank";
                    switch (note.Type)
                    {
                        case SeatManage.EnumType.OrnamentType.AirConditioning:
                            noteImage = "note_AirConditioning";
                            break;
                        case SeatManage.EnumType.OrnamentType.Bookshelf:
                            noteImage = "note_Bookshelf";
                            break;
                        case SeatManage.EnumType.OrnamentType.Door:
                            noteImage = "note_Door";
                            break;
                        case SeatManage.EnumType.OrnamentType.PCTable:
                            noteImage = "note_PCTable";
                            break;
                        case SeatManage.EnumType.OrnamentType.Pillar:
                            noteImage = "note_Pillar";
                            break;
                        case SeatManage.EnumType.OrnamentType.Plant:
                            noteImage = "note_Plant";
                            break;
                        case SeatManage.EnumType.OrnamentType.Roundtable:
                            noteImage = "note_Roundtable";
                            break;
                        case SeatManage.EnumType.OrnamentType.Steps:
                            noteImage = "note_Steps";
                            break;
                        case SeatManage.EnumType.OrnamentType.Table:
                            noteImage = "note_Table";
                            break;
                        case SeatManage.EnumType.OrnamentType.Wall:
                            noteImage = "note_Wall";
                            break;
                        case SeatManage.EnumType.OrnamentType.Window:
                            noteImage = "note_Window";
                            break;
                        case SeatManage.EnumType.OrnamentType.Elevator:
                            noteImage = "note_Elevator";
                            break;
                        case SeatManage.EnumType.OrnamentType.Stairway:
                            noteImage = "note_Stairway";
                            break;
                    }
                    seatLayoutHtml.AppendFormat("<div id='{0}' class='note_blank' style='left: {1}px; top: {2}px;width: {3}px;height: {4}px;transform: rotate({6}deg);-o-transform: rotate({6}deg);-webkit-transform: rotate({6}deg);-moz-transform: rotate({6}deg); '><img  src='/Content/Images/Node/{7}.png' style='width: {3}px;height: {4}px;'/><div style='margin-top: {8}px;transform: rotate(-0deg); -o-transform: rotate(-0deg);-webkit-transform: rotate(-0deg); -moz-transform: rotate(-0deg);'>{5}</div></div>", countNode, seatLeft, seatTop, note.BaseWidth * 18, note.BaseHeight * 18, note.Remark, note.RotationAngle, noteImage, -note.BaseHeight * 9 - 18);
                }
                seatLayoutHtml.Append("</div>");
                seatLayoutHtml.Append("</div>");

                //缩略图
                seatLayoutHtml.AppendFormat("<div id='divThumbnail' class='Thumbnail' onclick='ThumbnailClick(this,event,{0},{1},{2},{3})' >", scaleX, scaleY, moveX, moveY);
                foreach (SeatManage.ClassModel.Seat seat in _SeatLayout.Seats.Values)
                {
                    double width = 24 / scaleX;
                    double height = 24 / scaleY;
                    string style = "";
                    switch (seat.SeatUsedState)
                    {
                        case SeatManage.EnumType.EnterOutLogType.Leave:
                            style = "ThumbnailSeatFree";
                            break;
                        case SeatManage.EnumType.EnterOutLogType.SelectSeat:
                        case SeatManage.EnumType.EnterOutLogType.ReselectSeat:
                        case SeatManage.EnumType.EnterOutLogType.ComeBack:
                        case SeatManage.EnumType.EnterOutLogType.ContinuedTime:
                        case SeatManage.EnumType.EnterOutLogType.ShortLeave:
                        case SeatManage.EnumType.EnterOutLogType.BespeakWaiting:
                        case SeatManage.EnumType.EnterOutLogType.BookingConfirmation:
                        case SeatManage.EnumType.EnterOutLogType.WaitingSuccess:
                            style = "ThumbnailSeatUsing";
                            break;
                    }
                    seatLayoutHtml.AppendFormat("<div id='t{0}' class='{1}' style='left: {2}px; top: {3}px; width: {4}px;height: {5}px;transform: rotate({6}deg);-o-transform: rotate({6}deg);-webkit-transform: rotate({6}deg);-moz-transform: rotate({6}deg); '></div>", seat.SeatNo, style, (18 * seat.PositionX + 3) / scaleX + moveX, (18 * seat.PositionY + 3) / scaleY + moveY, width, height, seat.RotationAngle);
                }

                foreach (SeatManage.ClassModel.Note note in _SeatLayout.Notes)
                {
                    double width = 18 * note.BaseWidth / scaleX;
                    double height = 18 * note.BaseHeight / scaleY;
                    string style = "";
                    switch (note.Type)
                    {
                        case SeatManage.EnumType.OrnamentType.Plant:
                        case SeatManage.EnumType.OrnamentType.Roundtable:
                            seatLayoutHtml.AppendFormat("<div id='note' class='ThumbnailNote' style='left: {1}px; top: {2}px; width: {3}px;height: {4}px; border:1px solid #F5BF36; -webkit-border-radius:{5}px; -moz-border-radius:{5}px; -o-border-radius:{5}px; border-radius:{5}px;'></div>", style, (18 / scaleX) * (note.PositionX) + moveX, (18 / scaleY) * (note.PositionY) + moveY, width - 1, height - 1, height);
                            continue;
                        case SeatManage.EnumType.OrnamentType.AirConditioning:
                        case SeatManage.EnumType.OrnamentType.Bookshelf:
                        case SeatManage.EnumType.OrnamentType.PCTable:
                        case SeatManage.EnumType.OrnamentType.Pillar:
                        case SeatManage.EnumType.OrnamentType.Steps:
                        case SeatManage.EnumType.OrnamentType.Table:
                        case SeatManage.EnumType.OrnamentType.Wall:
                        case SeatManage.EnumType.OrnamentType.Elevator:
                            style = "ThumbnailNote";
                            break;
                    }

                    seatLayoutHtml.AppendFormat("<div id='note' class='{0}' style='left: {1}px; top: {2}px; width: {3}px;height: {4}px;transform: rotate({5}deg);-o-transform: rotate({5}deg);-webkit-transform: rotate({5}deg);-moz-transform: rotate({5}deg); '></div>", style, (18 * note.PositionX) / scaleX + moveX, (18 * note.PositionY) / scaleY + moveY, width, height, note.RotationAngle);
                }
                seatLayoutHtml.AppendFormat("<div id='divTransparent' class='Transparent' style='left: {0}px; top: {1}px; width: {2}px; height: {3}px;'>", divTransparentLeft, divTransparentTop, (300 - 2 * moveX) / transparentScaleX, (300 - 2 * moveY) / transparentScaleY);
                seatLayoutHtml.Append("</div>");
                seatLayoutHtml.Append("</div>");
                return seatLayoutHtml.ToString();
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// 绘制可预约座位（隔天）座位图
        /// </summary>
        /// <param name="roomNum">房间编号</param>
        /// <param name="divTransparentTop">透明层距离顶部的位置</param>
        /// <param name="divTransparentLeft">透明层距离左边的位置</param>
        /// <returns></returns>
        public string drowBespeakSeatSettingHtml(string roomNum, string divTransparentTop, string divTransparentLeft)
        {
            if (string.IsNullOrEmpty(roomNum))
            {
                return "阅览室编号不能为空";
            }
            //if (string.IsNullOrEmpty(divTransparentTop) || string.IsNullOrEmpty(divTransparentLeft))
            //{
            //    divTransparentTop = "0";
            //    divTransparentLeft = "0";
            //}
            try
            {
                SeatManage.ClassModel.SeatLayout _SeatLayout = SeatManage.Bll.T_SM_SeatBespeak.GetBeseakSeatSettingLayout(roomNum);
                if (_SeatLayout == null)
                {
                    return "获取阅览室座位布局出错了";
                }
                StringBuilder seatLayoutHtml = new StringBuilder();
                //布局实际图
                //绘制实际图窗体大小
                //实际图的长宽
                double layoutWidth = (double)(_SeatLayout.SeatCol * 18);
                double layoutHeight = (double)(_SeatLayout.SeatRow * 18);
                //实际图和缩略图的比例
                double scaleX = (double)layoutWidth / 300;
                double scaleY = (double)layoutHeight / 300;
                double transparentScaleX = layoutWidth / 1245;
                double transparentScaleY = layoutHeight / 685;
                double moveX = 0;
                double moveY = 0;
                if (layoutWidth >= layoutHeight)
                {
                    scaleY = scaleX;
                    moveY = (layoutWidth - layoutHeight) / 2 / scaleY;
                }
                else
                {
                    scaleX = scaleY;
                    moveX = (layoutHeight - layoutWidth) / 2 / scaleX;
                }
                //实际图当前的坐标位置
                if (string.IsNullOrEmpty(divTransparentTop) || string.IsNullOrEmpty(divTransparentLeft))
                {
                    divTransparentTop = moveY.ToString();
                    divTransparentLeft = moveX.ToString();
                }

                double layoutTop = (moveY - double.Parse(divTransparentTop)) * scaleY;
                double layoutLeft = (moveX - double.Parse(divTransparentLeft)) * scaleX;
                seatLayoutHtml.Append("<div id='divSeatLayoutFrom' class='SeatLayoutFrom'>");
                seatLayoutHtml.AppendFormat("<div id='divSeatLayout' class='SeatLayout' style='height:{0}px; width:{1}px;top:{2}px;left:{3}px'>", layoutHeight, layoutWidth, layoutTop, layoutLeft);
                //布局座位
                foreach (SeatManage.ClassModel.Seat seat in _SeatLayout.Seats.Values)
                {
                    string shortleaveimg = "/Content/Images/Node/note_blank.png";
                    string powerimh = "/Content/Images/Node/note_blank.png";
                    string readerimg = "/Content/Images/Node/note_blank.png";
                    //string seatStyle = "";//座位样式
                    string seatTop = (18 * seat.PositionY).ToString();//座位顶部位置
                    string seatLeft = (18 * seat.PositionX).ToString();//座位左边位置
                    string urlParameters = "";
                    string tipContent = "";
                    if (seat.HavePower)
                    {
                        powerimh = "/Content/Images/SeatImage/ImgPower.png";
                    }
                    if (seat.IsSuspended)
                    {
                        readerimg = "/Content/Images/SeatImage/ImgStopUse.png";
                        tipContent = "座位暂停使用";
                        urlParameters = "";
                    }

                    else if (seat.CanBeBespeak)
                    {
                        readerimg = "/Content/Images/SeatImage/ImgBook.png";
                        tipContent = "点击设置座位不可预约";
                        urlParameters = "roomNo=" + roomNum + "&seatNo=" + seat.SeatNo + "&canBook=nobook";
                    }
                    else
                    {
                        tipContent = "点击设置座位可预约";
                        urlParameters = "roomNo=" + roomNum + "&seatNo=" + seat.SeatNo + "&canBook=canbook";
                    }
                    string seatdiv = "<div id='{0}' class='SeatBackground' style='left: {1}px; top: {2}px;width: 42px; height: 42px;transform: rotate({3}deg); -o-transform: rotate({3}deg); -webkit-transform: rotate({3}deg);-moz-transform: rotate({3}deg);'  onclick='BespeakSeatSettingClick(\"{0}\",\"{4}\")' onmouseover='tipShow(this,\"{5}\")' onmouseout='tipHidden(this)'>" +
                                     "<img src='/Content/Images/SeatImage/ImgSeat.png' height='34px' width='34px' />" +
                                     "<div style='margin-top: -34px; margin-left: 24px; height: 12px; width: 12px'>" +
                                     "<img src='{6}' height='12px' width='12px' />" +
                                     "</div>" +
                                     "<div style='margin-top: 12px; margin-left: 0px; height: 12px; width: 12px'>" +
                                     "<img src='{7}' height='12px' width='12px' />" +
                                     "</div>" +
                                     "<div style='margin-top: -28px; margin-left: 6px; height: 24px; width: 24px'>" +
                                     "<img src='{8}' height='24px' width='24px' />" +
                                     "</div>" +
                                     "<div style='margin-top: -50px; margin-left: 0px;color:white; transform: rotate(-{3}deg); -o-transform: rotate(-{3}deg);" +
                                     "-webkit-transform: rotate(-{3}deg); -moz-transform: rotate(-{3}deg);'>{9}</div></div>";
                    seatLayoutHtml.AppendFormat(seatdiv, seat.SeatNo, seatLeft, seatTop, seat.RotationAngle, urlParameters, tipContent, shortleaveimg, powerimh, readerimg, seat.ShortSeatNo);

                    //seatLayoutHtml.AppendFormat("<div id='{0}'   class='{1}' style='left: {2}px; top: {3}px;width: {4}px;height: {5}px;transform: rotate({10}deg);-o-transform: rotate({10}deg);-webkit-transform: rotate({10}deg);-moz-transform: rotate({10}deg)' onclick='BespeakSeatSettingClick(\"{6}\",\"{7}\")' onmouseover='tipShow(this,\"{8}\")' onmouseout='tipHidden(this)'>,<div style='transform: rotate(-{10}deg);-o-transform: rotate(-{10}deg);-webkit-transform: rotate(-{10}deg);-moz-transform: rotate(-{10}deg)'>{9}</div></div> ", seat.SeatNo, seatStyle, seatLeft, seatTop, 42, 42, seat.SeatNo, urlParameters, tipContent, seat.ShortSeatNo, seat.RotationAngle);
                }
                //备注
                int countNode = 0;
                foreach (SeatManage.ClassModel.Note note in _SeatLayout.Notes)
                {
                    countNode += 1;
                    string seatTop = (18 * note.PositionY).ToString();//座位顶部位置
                    string seatLeft = (18 * note.PositionX).ToString();//座位左边位置 
                    string noteImage = "note_blank";
                    switch (note.Type)
                    {
                        case SeatManage.EnumType.OrnamentType.AirConditioning:
                            noteImage = "note_AirConditioning";
                            break;
                        case SeatManage.EnumType.OrnamentType.Bookshelf:
                            noteImage = "note_Bookshelf";
                            break;
                        case SeatManage.EnumType.OrnamentType.Door:
                            noteImage = "note_Door";
                            break;
                        case SeatManage.EnumType.OrnamentType.PCTable:
                            noteImage = "note_PCTable";
                            break;
                        case SeatManage.EnumType.OrnamentType.Pillar:
                            noteImage = "note_Pillar";
                            break;
                        case SeatManage.EnumType.OrnamentType.Plant:
                            noteImage = "note_Plant";
                            break;
                        case SeatManage.EnumType.OrnamentType.Roundtable:
                            noteImage = "note_Roundtable";
                            break;
                        case SeatManage.EnumType.OrnamentType.Steps:
                            noteImage = "note_Steps";
                            break;
                        case SeatManage.EnumType.OrnamentType.Table:
                            noteImage = "note_Table";
                            break;
                        case SeatManage.EnumType.OrnamentType.Wall:
                            noteImage = "note_Wall";
                            break;
                        case SeatManage.EnumType.OrnamentType.Window:
                            noteImage = "note_Window";
                            break;
                        case SeatManage.EnumType.OrnamentType.Elevator:
                            noteImage = "note_Elevator";
                            break;
                        case SeatManage.EnumType.OrnamentType.Stairway:
                            noteImage = "note_Stairway";
                            break;
                    }
                    seatLayoutHtml.AppendFormat("<div id='{0}' class='note_blank' style='left: {1}px; top: {2}px;width: {3}px;height: {4}px;transform: rotate({6}deg);-o-transform: rotate({6}deg);-webkit-transform: rotate({6}deg);-moz-transform: rotate({6}deg); '><img  src='/Content/Images/Node/{7}.png' style='width: {3}px;height: {4}px;'/><div style='margin-top: {8}px;transform: rotate(-0deg); -o-transform: rotate(-0deg);-webkit-transform: rotate(-0deg); -moz-transform: rotate(-0deg);'>{5}</div></div>", countNode, seatLeft, seatTop, note.BaseWidth * 18, note.BaseHeight * 18, note.Remark, note.RotationAngle, noteImage, -note.BaseHeight * 9 - 18);
                }
                seatLayoutHtml.Append("</div>");
                seatLayoutHtml.Append("</div>");

                //缩略图
                seatLayoutHtml.AppendFormat("<div id='divThumbnail' class='Thumbnail' onclick='ThumbnailClick(this,event,{0},{1},{2},{3})' >", scaleX, scaleY, moveX, moveY);
                foreach (SeatManage.ClassModel.Seat seat in _SeatLayout.Seats.Values)
                {
                    double width = 36 / scaleX;
                    double height = 36 / scaleY;
                    string style = "";
                    switch (seat.CanBeBespeak)
                    {
                        case false:
                            style = "ThumbnailSeatFree";
                            break;
                        case true:
                            style = "ThumbnailSeatUsing";
                            break;
                    }
                    seatLayoutHtml.AppendFormat("<div id='t{0}' class='{1}' style='left: {2}px; top: {3}px; width: {4}px;height: {5}px;transform: rotate({6}deg);-o-transform: rotate({6}deg);-webkit-transform: rotate({6}deg);-moz-transform: rotate({6}deg)'></div>", seat.SeatNo, style, (18 * seat.PositionX + 3) / scaleX + moveX, (18 * seat.PositionY + 3) / scaleY + moveY, width, height, seat.RotationAngle);
                }
                foreach (SeatManage.ClassModel.Note note in _SeatLayout.Notes)
                {
                    double width = 18 * note.BaseWidth / scaleX;
                    double height = 18 * note.BaseHeight / scaleY;
                    string style = "";
                    switch (note.Type)
                    {
                        case SeatManage.EnumType.OrnamentType.Plant:
                        case SeatManage.EnumType.OrnamentType.Roundtable:
                            seatLayoutHtml.AppendFormat("<div id='note' class='ThumbnailNote' style='left: {1}px; top: {2}px; width: {3}px;height: {4}px; border:1px solid #F5BF36; -webkit-border-radius:{5}px; -moz-border-radius:{5}px; -o-border-radius:{5}px; border-radius:{5}px;'></div>", style, (double)(18 / scaleX * note.PositionX) + (double)moveX, (double)(18 / scaleY * note.PositionY) + (double)moveY, (double)width - 1, (double)height - 1, (double)height);
                            continue;
                        case SeatManage.EnumType.OrnamentType.AirConditioning:
                        case SeatManage.EnumType.OrnamentType.Bookshelf:
                        case SeatManage.EnumType.OrnamentType.PCTable:
                        case SeatManage.EnumType.OrnamentType.Pillar:
                        case SeatManage.EnumType.OrnamentType.Steps:
                        case SeatManage.EnumType.OrnamentType.Table:
                        case SeatManage.EnumType.OrnamentType.Wall:
                        case SeatManage.EnumType.OrnamentType.Elevator:
                            style = "ThumbnailNote";
                            break;
                    }

                    seatLayoutHtml.AppendFormat("<div id='note' class='{0}' style='left: {1}px; top: {2}px; width: {3}px;height: {4}px;transform: rotate({5}deg);-o-transform: rotate({5}deg);-webkit-transform: rotate({5}deg);-moz-transform: rotate({5}deg); '></div>", style, (double)(18 / scaleX * note.PositionX) + (double)moveX, (double)(18 / scaleY * note.PositionY) + (double)moveY, (double)width, (double)height, note.RotationAngle);
                }
                seatLayoutHtml.AppendFormat("<div id='divTransparent' class='Transparent' style='left: {0}px; top: {1}px; width: {2}px; height: {3}px;'>", divTransparentLeft, divTransparentTop, (300 - 2 * moveX) / transparentScaleX, (300 - 2 * moveY) / transparentScaleY);
                seatLayoutHtml.Append("</div>");
                seatLayoutHtml.Append("</div>");
                return seatLayoutHtml.ToString();
            }
            catch (Exception ex)
            {
                return string.Format("布局座位分布遇到异常：{0}", ex.Message);
            }
        }

    }
}
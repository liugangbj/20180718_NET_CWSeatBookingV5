using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatManageWebQUI.Controllers.FunctionPages.Model
{
    public class RoomSettingViewModel
    {
        #region 选座方式设置

        public string SeatSelectDefaultMode { get; set; }
        //public string SeatSelectDefaultMode1 { get; set; }
        //public string SeatSelectDefaultMode0 { get; set; }
        //public string SeatSelectDefaultMode2 { get; set; }

        public string SeatSelectPos { get; set; }
        public string SelectSeatPosTimes { get; set; }

        public string SelectSeatPosCount { get; set; } 
        #endregion

        #region 高级设置
        public string SeatSelectAdMode { get; set; }

        #region 周一
        public string SeatSelectAdModeDay1 { get; set; }

        public string SeatSelectAdModeDay1_Time1_StartH { get; set; }
        public string SeatSelectAdModeDay1_Time1_StartM { get; set; }
        public string SeatSelectAdModeDay1_Time1_EndH { get; set; }
        public string SeatSelectAdModeDay1_Time1_EndM { get; set; }

        public string SeatSelectAdModeDay1_Time1_SelectMode { get; set; }
        //public string SeatSelectAdModeDay1_Time1_SelectMode1 { get; set; }
        //public string SeatSelectAdModeDay1_Time1_SelectMode0 { get; set; }
        //public string SeatSelectAdModeDay1_Time1_SelectMode2 { get; set; }
        public string SeatSelectAdModeDay1_Time1_Error { get; set; }

        public string SeatSelectAdModeDay1_Time2_StartH { get; set; }
        public string SeatSelectAdModeDay1_Time2_StartM { get; set; }
        public string SeatSelectAdModeDay1_Time2_EndH { get; set; }
        public string SeatSelectAdModeDay1_Time2_EndM { get; set; }

        public string SeatSelectAdModeDay1_Time2_SelectMode { get; set; }
        //public string SeatSelectAdModeDay1_Time2_SelectMode1 { get; set; }
        //public string SeatSelectAdModeDay1_Time2_SelectMode0 { get; set; }
        //public string SeatSelectAdModeDay1_Time2_SelectMode2 { get; set; }
        public string SeatSelectAdModeDay1_Time2_Error { get; set; }

        public string SeatSelectAdModeDay1_Time3_StartH { get; set; }
        public string SeatSelectAdModeDay1_Time3_StartM { get; set; }
        public string SeatSelectAdModeDay1_Time3_EndH { get; set; }
        public string SeatSelectAdModeDay1_Time3_EndM { get; set; }

        public string SeatSelectAdModeDay1_Time3_SelectMode { get; set; }
        //public string SeatSelectAdModeDay1_Time3_SelectMode1 { get; set; }
        //public string SeatSelectAdModeDay1_Time3_SelectMode0 { get; set; }
        //public string SeatSelectAdModeDay1_Time3_SelectMode2 { get; set; }
        public string SeatSelectAdModeDay1_Time3_Error { get; set; }
        #endregion

        #region 周二
        public string SeatSelectAdModeDay2 { get; set; }

        public string SeatSelectAdModeDay2_Time1_StartH { get; set; }
        public string SeatSelectAdModeDay2_Time1_StartM { get; set; }
        public string SeatSelectAdModeDay2_Time1_EndH { get; set; }
        public string SeatSelectAdModeDay2_Time1_EndM { get; set; }

        public string SeatSelectAdModeDay2_Time1_SelectMode { get; set; }
        //public string SeatSelectAdModeDay2_Time1_SelectMode1 { get; set; }
        //public string SeatSelectAdModeDay2_Time1_SelectMode0 { get; set; }
        //public string SeatSelectAdModeDay2_Time1_SelectMode2 { get; set; }
        public string SeatSelectAdModeDay2_Time1_Error { get; set; }

        public string SeatSelectAdModeDay2_Time2_StartH { get; set; }
        public string SeatSelectAdModeDay2_Time2_StartM { get; set; }
        public string SeatSelectAdModeDay2_Time2_EndH { get; set; }
        public string SeatSelectAdModeDay2_Time2_EndM { get; set; }

        public string SeatSelectAdModeDay2_Time2_SelectMode { get; set; }
        //public string SeatSelectAdModeDay2_Time2_SelectMode1 { get; set; }
        //public string SeatSelectAdModeDay2_Time2_SelectMode0 { get; set; }
        //public string SeatSelectAdModeDay2_Time2_SelectMode2 { get; set; }
        public string SeatSelectAdModeDay2_Time2_Error { get; set; }

        public string SeatSelectAdModeDay2_Time3_StartH { get; set; }
        public string SeatSelectAdModeDay2_Time3_StartM { get; set; }
        public string SeatSelectAdModeDay2_Time3_EndH { get; set; }
        public string SeatSelectAdModeDay2_Time3_EndM { get; set; }

        public string SeatSelectAdModeDay2_Time3_SelectMode { get; set; }
        //public string SeatSelectAdModeDay2_Time3_SelectMode1 { get; set; }
        //public string SeatSelectAdModeDay2_Time3_SelectMode0 { get; set; }
        //public string SeatSelectAdModeDay2_Time3_SelectMode2 { get; set; }
        public string SeatSelectAdModeDay2_Time3_Error { get; set; }
        #endregion

        #region 周三
        public string SeatSelectAdModeDay3 { get; set; }

        public string SeatSelectAdModeDay3_Time1_StartH { get; set; }
        public string SeatSelectAdModeDay3_Time1_StartM { get; set; }
        public string SeatSelectAdModeDay3_Time1_EndH { get; set; }
        public string SeatSelectAdModeDay3_Time1_EndM { get; set; }

        public string SeatSelectAdModeDay3_Time1_SelectMode { get; set; }
        //public string SeatSelectAdModeDay3_Time1_SelectMode1 { get; set; }
        //public string SeatSelectAdModeDay3_Time1_SelectMode0 { get; set; }
        //public string SeatSelectAdModeDay3_Time1_SelectMode2 { get; set; }
        public string SeatSelectAdModeDay3_Time1_Error { get; set; }

        public string SeatSelectAdModeDay3_Time2_StartH { get; set; }
        public string SeatSelectAdModeDay3_Time2_StartM { get; set; }
        public string SeatSelectAdModeDay3_Time2_EndH { get; set; }
        public string SeatSelectAdModeDay3_Time2_EndM { get; set; }

        public string SeatSelectAdModeDay3_Time2_SelectMode { get; set; }
        //public string SeatSelectAdModeDay3_Time2_SelectMode1 { get; set; }
        //public string SeatSelectAdModeDay3_Time2_SelectMode0 { get; set; }
        //public string SeatSelectAdModeDay3_Time2_SelectMode2 { get; set; }
        public string SeatSelectAdModeDay3_Time2_Error { get; set; }

        public string SeatSelectAdModeDay3_Time3_StartH { get; set; }
        public string SeatSelectAdModeDay3_Time3_StartM { get; set; }
        public string SeatSelectAdModeDay3_Time3_EndH { get; set; }
        public string SeatSelectAdModeDay3_Time3_EndM { get; set; }

        public string SeatSelectAdModeDay3_Time3_SelectMode { get; set; }
        //public string SeatSelectAdModeDay3_Time3_SelectMode1 { get; set; }
        //public string SeatSelectAdModeDay3_Time3_SelectMode0 { get; set; }
        //public string SeatSelectAdModeDay3_Time3_SelectMode2 { get; set; }
        public string SeatSelectAdModeDay3_Time3_Error { get; set; }
        #endregion

        #region 周四
        public string SeatSelectAdModeDay4 { get; set; }

        public string SeatSelectAdModeDay4_Time1_StartH { get; set; }
        public string SeatSelectAdModeDay4_Time1_StartM { get; set; }
        public string SeatSelectAdModeDay4_Time1_EndH { get; set; }
        public string SeatSelectAdModeDay4_Time1_EndM { get; set; }

         public string SeatSelectAdModeDay4_Time1_SelectMode { get; set; }
        //public string SeatSelectAdModeDay4_Time1_SelectMode1 { get; set; }
        //public string SeatSelectAdModeDay4_Time1_SelectMode0 { get; set; }
        //public string SeatSelectAdModeDay4_Time1_SelectMode2 { get; set; }
        public string SeatSelectAdModeDay4_Time1_Error { get; set; }

        public string SeatSelectAdModeDay4_Time2_StartH { get; set; }
        public string SeatSelectAdModeDay4_Time2_StartM { get; set; }
        public string SeatSelectAdModeDay4_Time2_EndH { get; set; }
        public string SeatSelectAdModeDay4_Time2_EndM { get; set; }

        public string SeatSelectAdModeDay4_Time2_SelectMode { get; set; }
        //public string SeatSelectAdModeDay4_Time2_SelectMode1 { get; set; }
        //public string SeatSelectAdModeDay4_Time2_SelectMode0 { get; set; }
        //public string SeatSelectAdModeDay4_Time2_SelectMode2 { get; set; }
        public string SeatSelectAdModeDay4_Time2_Error { get; set; }

        public string SeatSelectAdModeDay4_Time3_StartH { get; set; }
        public string SeatSelectAdModeDay4_Time3_StartM { get; set; }
        public string SeatSelectAdModeDay4_Time3_EndH { get; set; }
        public string SeatSelectAdModeDay4_Time3_EndM { get; set; }

        public string SeatSelectAdModeDay4_Time3_SelectMode { get; set; }
        //public string SeatSelectAdModeDay4_Time3_SelectMode1 { get; set; }
        //public string SeatSelectAdModeDay4_Time3_SelectMode0 { get; set; }
        //public string SeatSelectAdModeDay4_Time3_SelectMode2 { get; set; }
        public string SeatSelectAdModeDay4_Time3_Error { get; set; }
        #endregion

        #region 周五
        public string SeatSelectAdModeDay5 { get; set; }

        public string SeatSelectAdModeDay5_Time1_StartH { get; set; }
        public string SeatSelectAdModeDay5_Time1_StartM { get; set; }
        public string SeatSelectAdModeDay5_Time1_EndH { get; set; }
        public string SeatSelectAdModeDay5_Time1_EndM { get; set; }

        public string SeatSelectAdModeDay5_Time1_SelectMode { get; set; }
        //public string SeatSelectAdModeDay5_Time1_SelectMode1 { get; set; }
        //public string SeatSelectAdModeDay5_Time1_SelectMode0 { get; set; }
        //public string SeatSelectAdModeDay5_Time1_SelectMode2 { get; set; }
        public string SeatSelectAdModeDay5_Time1_Error { get; set; }

        public string SeatSelectAdModeDay5_Time2_StartH { get; set; }
        public string SeatSelectAdModeDay5_Time2_StartM { get; set; }
        public string SeatSelectAdModeDay5_Time2_EndH { get; set; }
        public string SeatSelectAdModeDay5_Time2_EndM { get; set; }

        public string SeatSelectAdModeDay5_Time2_SelectMode { get; set; }
        //public string SeatSelectAdModeDay5_Time2_SelectMode1 { get; set; }
        //public string SeatSelectAdModeDay5_Time2_SelectMode0 { get; set; }
        //public string SeatSelectAdModeDay5_Time2_SelectMode2 { get; set; }
        public string SeatSelectAdModeDay5_Time2_Error { get; set; }

        public string SeatSelectAdModeDay5_Time3_StartH { get; set; }
        public string SeatSelectAdModeDay5_Time3_StartM { get; set; }
        public string SeatSelectAdModeDay5_Time3_EndH { get; set; }
        public string SeatSelectAdModeDay5_Time3_EndM { get; set; }

        public string SeatSelectAdModeDay5_Time3_SelectMode { get; set; }
        //public string SeatSelectAdModeDay5_Time3_SelectMode1 { get; set; }
        //public string SeatSelectAdModeDay5_Time3_SelectMode0 { get; set; }
        //public string SeatSelectAdModeDay5_Time3_SelectMode2 { get; set; }
        public string SeatSelectAdModeDay5_Time3_Error { get; set; }
        #endregion

        #region 周六
        public string SeatSelectAdModeDay6 { get; set; }

        public string SeatSelectAdModeDay6_Time1_StartH { get; set; }
        public string SeatSelectAdModeDay6_Time1_StartM { get; set; }
        public string SeatSelectAdModeDay6_Time1_EndH { get; set; }
        public string SeatSelectAdModeDay6_Time1_EndM { get; set; }

        public string SeatSelectAdModeDay6_Time1_SelectMode { get; set; }
        //public string SeatSelectAdModeDay6_Time1_SelectMode1 { get; set; }
        //public string SeatSelectAdModeDay6_Time1_SelectMode0 { get; set; }
        //public string SeatSelectAdModeDay6_Time1_SelectMode2 { get; set; }
        public string SeatSelectAdModeDay6_Time1_Error { get; set; }

        public string SeatSelectAdModeDay6_Time2_StartH { get; set; }
        public string SeatSelectAdModeDay6_Time2_StartM { get; set; }
        public string SeatSelectAdModeDay6_Time2_EndH { get; set; }
        public string SeatSelectAdModeDay6_Time2_EndM { get; set; }

        public string SeatSelectAdModeDay6_Time2_SelectMode { get; set; }
        //public string SeatSelectAdModeDay6_Time2_SelectMode1 { get; set; }
        //public string SeatSelectAdModeDay6_Time2_SelectMode0 { get; set; }
        //public string SeatSelectAdModeDay6_Time2_SelectMode2 { get; set; }
        public string SeatSelectAdModeDay6_Time2_Error { get; set; }

        public string SeatSelectAdModeDay6_Time3_StartH { get; set; }
        public string SeatSelectAdModeDay6_Time3_StartM { get; set; }
        public string SeatSelectAdModeDay6_Time3_EndH { get; set; }
        public string SeatSelectAdModeDay6_Time3_EndM { get; set; }

        public string SeatSelectAdModeDay6_Time3_SelectMode { get; set; }
        //public string SeatSelectAdModeDay6_Time3_SelectMode1 { get; set; }
        //public string SeatSelectAdModeDay6_Time3_SelectMode0 { get; set; }
        //public string SeatSelectAdModeDay6_Time3_SelectMode2 { get; set; }
        public string SeatSelectAdModeDay6_Time3_Error { get; set; }
        #endregion

        #region 周日
        public string SeatSelectAdModeDay0 { get; set; }

        public string SeatSelectAdModeDay0_Time1_StartH { get; set; }
        public string SeatSelectAdModeDay0_Time1_StartM { get; set; }
        public string SeatSelectAdModeDay0_Time1_EndH { get; set; }
        public string SeatSelectAdModeDay0_Time1_EndM { get; set; }

        public string SeatSelectAdModeDay0_Time1_SelectMode { get; set; }
        //public string SeatSelectAdModeDay0_Time1_SelectMode1 { get; set; }
        //public string SeatSelectAdModeDay0_Time1_SelectMode0 { get; set; }
        //public string SeatSelectAdModeDay0_Time1_SelectMode2 { get; set; }
        public string SeatSelectAdModeDay0_Time1_Error { get; set; }

        public string SeatSelectAdModeDay0_Time2_StartH { get; set; }
        public string SeatSelectAdModeDay0_Time2_StartM { get; set; }
        public string SeatSelectAdModeDay0_Time2_EndH { get; set; }
        public string SeatSelectAdModeDay0_Time2_EndM { get; set; }

        public string SeatSelectAdModeDay0_Time2_SelectMode { get; set; }
        //public string SeatSelectAdModeDay0_Time2_SelectMode1 { get; set; }
        //public string SeatSelectAdModeDay0_Time2_SelectMode0 { get; set; }
        //public string SeatSelectAdModeDay0_Time2_SelectMode2 { get; set; }
        public string SeatSelectAdModeDay0_Time2_Error { get; set; }

        public string SeatSelectAdModeDay0_Time3_StartH { get; set; }
        public string SeatSelectAdModeDay0_Time3_StartM { get; set; }
        public string SeatSelectAdModeDay0_Time3_EndH { get; set; }
        public string SeatSelectAdModeDay0_Time3_EndM { get; set; }

        public string SeatSelectAdModeDay0_Time3_SelectMode { get; set; }
        //public string SeatSelectAdModeDay0_Time3_SelectMode1 { get; set; }
        //public string SeatSelectAdModeDay0_Time3_SelectMode0 { get; set; }
        //public string SeatSelectAdModeDay0_Time3_SelectMode2 { get; set; }
        public string SeatSelectAdModeDay0_Time3_Error { get; set; }
        #endregion
        #endregion

        #region 暂离设置
        public string ShortLeaveDufaultTime { get; set; }

        public string ShortLeaveByAdmin { get; set; }
        public string ShortLeaveByAdmin_LeaveTime { get; set; }
        public string ShortLeaveByAdmin_Error { get; set; }


        public string ShortLeaveAdMode { get; set; }
        public string ShortLeaveAdMode_Time1 { get; set; }
        public string ShortLeaveAdMode_Time1_StartH { get; set; }
        public string ShortLeaveAdMode_Time1_StartM { get; set; }
        public string ShortLeaveAdMode_Time1_EndH { get; set; }
        public string ShortLeaveAdMode_Time1_EndM { get; set; }
        public string ShortLeaveAdMode_Time1_LeaveTime { get; set; }
        public string ShortLeaveAdMode_Time1_Error { get; set; }
        public string ShortLeaveAdMode_Time2 { get; set; }
        public string ShortLeaveAdMode_Time2_StartH { get; set; }
        public string ShortLeaveAdMode_Time2_StartM { get; set; }
        public string ShortLeaveAdMode_Time2_EndH { get; set; }
        public string ShortLeaveAdMode_Time2_EndM { get; set; }
        public string ShortLeaveAdMode_Time2_LeaveTime { get; set; }
        public string ShortLeaveAdMode_Time2_Error { get; set; }
        #endregion

        #region 开闭馆设置
        public string ReadingRoomDufaultOpenTimeH { get; set; }
        public string ReadingRoomDufaultOpenTimeM { get; set; }
        public string ReadingRoomBeforeOpenTime { get; set; }
        public string ReadingRoomDufaultCloseTimeH { get; set; }
        public string ReadingRoomDufaultCloseTimeM { get; set; }
        public string ReadingRoomBeforeCloseTime { get; set; }

        public string ReadingRoomOpen24H { get; set; }

        #region 开闭馆高级设置
        public string ReadingRoomOpenCloseAdMode { get; set; }
        #region 周1
        public string ReadingRoomAdOpenTime_Day1 { get; set; }
        public string ReadingRoomAdOpenTime_Day1_Time1_OpenH { get; set; }
        public string ReadingRoomAdOpenTime_Day1_Time1_OpenM { get; set; }
        public string ReadingRoomAdOpenTime_Day1_Time1_CloseH { get; set; }
        public string ReadingRoomAdOpenTime_Day1_Time1_CloseM { get; set; }
        public string ReadingRoomAdOpenTime_Day1_Time2_OpenH { get; set; }
        public string ReadingRoomAdOpenTime_Day1_Time2_OpenM { get; set; }
        public string ReadingRoomAdOpenTime_Day1_Time2_CloseH { get; set; }
        public string ReadingRoomAdOpenTime_Day1_Time2_CloseM { get; set; }
        public string ReadingRoomAdOpenTime_Day1_Time3_OpenH { get; set; }
        public string ReadingRoomAdOpenTime_Day1_Time3_OpenM { get; set; }
        public string ReadingRoomAdOpenTime_Day1_Time3_CloseH { get; set; }
        public string ReadingRoomAdOpenTime_Day1_Time3_CloseM { get; set; }
        public string ReadingRoomAdOpenTime_Day1_Time1_Error { get; set; }
        public string ReadingRoomAdOpenTime_Day1_Time2_Error { get; set; }
        public string ReadingRoomAdOpenTime_Day1_Time3_Error { get; set; }
        #endregion

        #region 周2
        public string ReadingRoomAdOpenTime_Day2 { get; set; }
        public string ReadingRoomAdOpenTime_Day2_Time1_OpenH { get; set; }
        public string ReadingRoomAdOpenTime_Day2_Time1_OpenM { get; set; }
        public string ReadingRoomAdOpenTime_Day2_Time1_CloseH { get; set; }
        public string ReadingRoomAdOpenTime_Day2_Time1_CloseM { get; set; }
        public string ReadingRoomAdOpenTime_Day2_Time2_OpenH { get; set; }
        public string ReadingRoomAdOpenTime_Day2_Time2_OpenM { get; set; }
        public string ReadingRoomAdOpenTime_Day2_Time2_CloseH { get; set; }
        public string ReadingRoomAdOpenTime_Day2_Time2_CloseM { get; set; }
        public string ReadingRoomAdOpenTime_Day2_Time3_OpenH { get; set; }
        public string ReadingRoomAdOpenTime_Day2_Time3_OpenM { get; set; }
        public string ReadingRoomAdOpenTime_Day2_Time3_CloseH { get; set; }
        public string ReadingRoomAdOpenTime_Day2_Time3_CloseM { get; set; }
        public string ReadingRoomAdOpenTime_Day2_Time1_Error { get; set; }
        public string ReadingRoomAdOpenTime_Day2_Time2_Error { get; set; }
        public string ReadingRoomAdOpenTime_Day2_Time3_Error { get; set; }
        #endregion

        #region 周3
        public string ReadingRoomAdOpenTime_Day3 { get; set; }
        public string ReadingRoomAdOpenTime_Day3_Time1_OpenH { get; set; }
        public string ReadingRoomAdOpenTime_Day3_Time1_OpenM { get; set; }
        public string ReadingRoomAdOpenTime_Day3_Time1_CloseH { get; set; }
        public string ReadingRoomAdOpenTime_Day3_Time1_CloseM { get; set; }
        public string ReadingRoomAdOpenTime_Day3_Time2_OpenH { get; set; }
        public string ReadingRoomAdOpenTime_Day3_Time2_OpenM { get; set; }
        public string ReadingRoomAdOpenTime_Day3_Time2_CloseH { get; set; }
        public string ReadingRoomAdOpenTime_Day3_Time2_CloseM { get; set; }
        public string ReadingRoomAdOpenTime_Day3_Time3_OpenH { get; set; }
        public string ReadingRoomAdOpenTime_Day3_Time3_OpenM { get; set; }
        public string ReadingRoomAdOpenTime_Day3_Time3_CloseH { get; set; }
        public string ReadingRoomAdOpenTime_Day3_Time3_CloseM { get; set; }
        public string ReadingRoomAdOpenTime_Day3_Time1_Error { get; set; }
        public string ReadingRoomAdOpenTime_Day3_Time2_Error { get; set; }
        public string ReadingRoomAdOpenTime_Day3_Time3_Error { get; set; }
        #endregion

        #region 周4
        public string ReadingRoomAdOpenTime_Day4 { get; set; }
        public string ReadingRoomAdOpenTime_Day4_Time1_OpenH { get; set; }
        public string ReadingRoomAdOpenTime_Day4_Time1_OpenM { get; set; }
        public string ReadingRoomAdOpenTime_Day4_Time1_CloseH { get; set; }
        public string ReadingRoomAdOpenTime_Day4_Time1_CloseM { get; set; }
        public string ReadingRoomAdOpenTime_Day4_Time2_OpenH { get; set; }
        public string ReadingRoomAdOpenTime_Day4_Time2_OpenM { get; set; }
        public string ReadingRoomAdOpenTime_Day4_Time2_CloseH { get; set; }
        public string ReadingRoomAdOpenTime_Day4_Time2_CloseM { get; set; }
        public string ReadingRoomAdOpenTime_Day4_Time3_OpenH { get; set; }
        public string ReadingRoomAdOpenTime_Day4_Time3_OpenM { get; set; }
        public string ReadingRoomAdOpenTime_Day4_Time3_CloseH { get; set; }
        public string ReadingRoomAdOpenTime_Day4_Time3_CloseM { get; set; }
        public string ReadingRoomAdOpenTime_Day4_Time1_Error { get; set; }
        public string ReadingRoomAdOpenTime_Day4_Time2_Error { get; set; }
        public string ReadingRoomAdOpenTime_Day4_Time3_Error { get; set; }
        #endregion

        #region 周5
        public string ReadingRoomAdOpenTime_Day5 { get; set; }
        public string ReadingRoomAdOpenTime_Day5_Time1_OpenH { get; set; }
        public string ReadingRoomAdOpenTime_Day5_Time1_OpenM { get; set; }
        public string ReadingRoomAdOpenTime_Day5_Time1_CloseH { get; set; }
        public string ReadingRoomAdOpenTime_Day5_Time1_CloseM { get; set; }
        public string ReadingRoomAdOpenTime_Day5_Time2_OpenH { get; set; }
        public string ReadingRoomAdOpenTime_Day5_Time2_OpenM { get; set; }
        public string ReadingRoomAdOpenTime_Day5_Time2_CloseH { get; set; }
        public string ReadingRoomAdOpenTime_Day5_Time2_CloseM { get; set; }
        public string ReadingRoomAdOpenTime_Day5_Time3_OpenH { get; set; }
        public string ReadingRoomAdOpenTime_Day5_Time3_OpenM { get; set; }
        public string ReadingRoomAdOpenTime_Day5_Time3_CloseH { get; set; }
        public string ReadingRoomAdOpenTime_Day5_Time3_CloseM { get; set; }
        public string ReadingRoomAdOpenTime_Day5_Time1_Error { get; set; }
        public string ReadingRoomAdOpenTime_Day5_Time2_Error { get; set; }
        public string ReadingRoomAdOpenTime_Day5_Time3_Error { get; set; }
        #endregion

        #region 周6
        public string ReadingRoomAdOpenTime_Day6 { get; set; }
        public string ReadingRoomAdOpenTime_Day6_Time1_OpenH { get; set; }
        public string ReadingRoomAdOpenTime_Day6_Time1_OpenM { get; set; }
        public string ReadingRoomAdOpenTime_Day6_Time1_CloseH { get; set; }
        public string ReadingRoomAdOpenTime_Day6_Time1_CloseM { get; set; }
        public string ReadingRoomAdOpenTime_Day6_Time2_OpenH { get; set; }
        public string ReadingRoomAdOpenTime_Day6_Time2_OpenM { get; set; }
        public string ReadingRoomAdOpenTime_Day6_Time2_CloseH { get; set; }
        public string ReadingRoomAdOpenTime_Day6_Time2_CloseM { get; set; }
        public string ReadingRoomAdOpenTime_Day6_Time3_OpenH { get; set; }
        public string ReadingRoomAdOpenTime_Day6_Time3_OpenM { get; set; }
        public string ReadingRoomAdOpenTime_Day6_Time3_CloseH { get; set; }
        public string ReadingRoomAdOpenTime_Day6_Time3_CloseM { get; set; }
        public string ReadingRoomAdOpenTime_Day6_Time1_Error { get; set; }
        public string ReadingRoomAdOpenTime_Day6_Time2_Error { get; set; }
        public string ReadingRoomAdOpenTime_Day6_Time3_Error { get; set; }
        #endregion

        #region 周日
        public string ReadingRoomAdOpenTime_Day0 { get; set; }
        public string ReadingRoomAdOpenTime_Day0_Time1_OpenH { get; set; }
        public string ReadingRoomAdOpenTime_Day0_Time1_OpenM { get; set; }
        public string ReadingRoomAdOpenTime_Day0_Time1_CloseH { get; set; }
        public string ReadingRoomAdOpenTime_Day0_Time1_CloseM { get; set; }
        public string ReadingRoomAdOpenTime_Day0_Time2_OpenH { get; set; }
        public string ReadingRoomAdOpenTime_Day0_Time2_OpenM { get; set; }
        public string ReadingRoomAdOpenTime_Day0_Time2_CloseH { get; set; }
        public string ReadingRoomAdOpenTime_Day0_Time2_CloseM { get; set; }
        public string ReadingRoomAdOpenTime_Day0_Time3_OpenH { get; set; }
        public string ReadingRoomAdOpenTime_Day0_Time3_OpenM { get; set; }
        public string ReadingRoomAdOpenTime_Day0_Time3_CloseH { get; set; }
        public string ReadingRoomAdOpenTime_Day0_Time3_CloseM { get; set; }
        public string ReadingRoomAdOpenTime_Day0_Time1_Error { get; set; }
        public string ReadingRoomAdOpenTime_Day0_Time2_Error { get; set; }
        public string ReadingRoomAdOpenTime_Day0_Time3_Error { get; set; }
        #endregion
        #endregion 
        #endregion

        #region 在座限时模式设置

        public string SeatTime { get; set; }

        public string SeatTime_Mode { get; set; }
        //public string SeatTime_ModeFree { get; set; }
        //public string SeatTime_ModeFixed { get; set; }
        public string SeatTime_SeatTime { get; set; }
        public string SeatTime_TimeSpanList { get; set; }

        public string SeatTime_OverTime_Mode { get; set; }
        //public string SeatTime_OverTime_Mode8 { get; set; }
        //public string SeatTime_OverTime_Mode0 { get; set; }
        public string SeatTime_ContinueTime { get; set; }
        public string SeatTime_ContinueTime_Time { get; set; }
        public string SeatTime_ContinueTime_ContinueCount { get; set; }
        public string SeatTime_ContinueTime_BeforeTime { get; set; }


        #endregion

        #region 预约网站设置

        public string ckbShortLeave { get; set; }
        public string ckbDelayTime { get; set; }
        public string ckbLeave { get; set; }

        #endregion

        #region 预约功能设置

        #region 预约模式
        public string SeatBook { get; set; }
        public string cbNowDayBook { get; set; }
        public string cbSpecifiedBook { get; set; }
        #endregion

        #region 隔天预约设置
        public string SeatBook_BeforeBookDay { get; set; }
        public string SeatBook_BeforeBookDay_Error { get; set; }
        public string SeatBook_BookTime_StartH { get; set; }
        public string SeatBook_BookTime_StartM { get; set; }
        public string SeatBook_BookTime_EndH { get; set; }
        public string SeatBook_BookTime_EndM { get; set; }
        public string SeatBook_BookTime_Error { get; set; }
        public string SeatBook_CanNotSeatBookDate { get; set; }
        public string SeatBook_CanNotSeatBookDate_Error { get; set; }
        #endregion

        #region 隔天预约设置

        public string SeatBook_SpecifiedTime { get; set; }
        public string SeatBook_SpecifiedTime_Error { get; set; }
        public string SeatBook_SpecifiedTimeList { get; set; }
        public string SeatBook_SpecifiedTimeList_Error { get; set; }

        #endregion

        #region 预约范围设置（隔天）

        public string SeatBook_SeatBookRadioSetted { get; set; }
        public string btnSetBespeakSeat { get; set; }

        public string SeatBespeak { get; set; }
        public string SeatBook_SeatBookRadioPercent { get; set; }
        public string SeatBook_SeatBookRadioPercent_Percent { get; set; }
        public string SeatBook_SeatBookRadioPercent_Percent_Error { get; set; }

        #endregion

        #region 当天预约设置

        public string SeatBook_BespeakSeatCount { get; set; }
        public string SeatBook_BespeakSeatCount_Error { get; set; }
        public string SeatBook_SelectBespeakSeat { get; set; }
        public string SeatBook_BespeakSeatOnSeat { get; set; }
        public string SeatBook_SelectBespeakSeat_Error { get; set; }

        #endregion

        #region 预约签到设置

        public string SeatBook_SubmitBeforeTime { get; set; }
        public string SeatBook_SubmitLateTime { get; set; }
        public string SeatBook_SubmitTime_Error { get; set; }
        public string NowDayBookTime { get; set; }
        public string NowDayBookTime_Error { get; set; }
        #endregion

        #endregion

        #region 黑名单设置
        public string IsRecordViolate { get; set; }
        public string UseBlacklist { get; set; }
        public string UseBlacklistSetting { get; set; }


        public string RecordViolateCount { get; set; }
        public string RecordViolateCount_Error { get; set; }

        public string leaveblacklist { get; set; }
        public string AutoLeave { get; set; }
        public string HardLeave { get; set; }

        public string LeaveBlackDays { get; set; }
        public string LeaveBlackDays_Error { get; set; }
        public string LeaveRecordViolateDays { get; set; }
        public string LeaveRecordViolateDays_Error { get; set; }
        public string RecordViolate_LeaveByAdmin { get; set; }
        public string RecordViolate_ShortLeaveByAdmin { get; set; }
        public string RecordViolate_SeatOverTime { get; set; }
        public string RecordViolate_ShortLeaveOverTime { get; set; }
        public string RecordViolate_ShortLeaveByReader { get; set; }
        public string RecordViolate_BookOverTime { get; set; }
        #endregion

        #region 其他功能设置
        public string ShowSeatNumberCount { get; set; }
        public string ShowSeatNumberCount_Error { get; set; }
        public string NoManMode { get; set; }
        public string NoManMode_WaitTime { get; set; }
        public string NoManMode_WaitTime_Error { get; set; }
        public string ReaderLimit { get; set; }
        public string ReaderLimit_LimitMode_Writelist { get; set; }
        public string ReaderLimit_LimitMode_Blacklist { get; set; }

        public string limitReader { get; set; }
        #endregion

        #region 应用到其他阅览室
        public string SelectAllRR { get; set; }
        #endregion
    }


}
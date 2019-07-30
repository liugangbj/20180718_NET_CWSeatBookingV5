using SeatClientV3.MyUserControl;
using SeatManage.EnumType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace SeatClientV3.Code
{
    /// <summary>
    /// 座位控件缓存
    /// </summary>
    public class SeatCache
    {
        public static List<SeatElement> SeatList { get; set; }

        public static List<NoteElement> NoteList { get; set; }
        
        public static List<ThumbElement> ThumbList { get; set; }
    }

    public class SeatElement
    {
        public string ReadingRoomNum { get; set; }
        
        public UC_Seat seatUC { get; set; }
        
        public float seatLeft { get; set; }
        
        public float seatTop { get; set; }
        
        public Rectangle rec { get; set; }
        
        public float recLeft { get; set; }
        
        public float recTop { get; set; }
    }

    public class NoteElement
    {
        public string ReadingRoomNum { get; set; }
        
        public UC_Note noteUC { get; set; }
        
        public float noteLeft { get; set; }
        
        public float noteTop { get; set; }
        
        public OrnamentType noteType { get; set; }
        
        public Border br { get; set; }
        
        public Rectangle rec { get; set; }
        
        public float recLeft { get; set; }
        
        public float recTop { get; set; }
    }

    public class ThumbElement
    {
        public string ReadingRoomNum { get; set; }
        
        public string SeatNo { get; set; }
        
        public Rectangle rec { get; set; }
        
        public float recLeft { get; set; }
        
        public float recTop { get; set; }
    }
}

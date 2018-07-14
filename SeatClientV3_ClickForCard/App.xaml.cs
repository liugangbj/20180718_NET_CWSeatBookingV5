using System;
using System.Windows;
using SeatClientV3.WindowObject;
using SeatClientV3.OperateResult;

namespace SeatClientV3
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {

        protected override void OnExit(ExitEventArgs e)　　　　　//该重写函数实现在程序退出时关闭某个进程
        {
            // MessageBoxResult result = MessageBox.Show("确定是退出吗？", "询问", MessageBoxButton.YesNo, MessageBoxImage.Question);

            //if (SystemObject.GetInstance().ObjCardReader != null)
            //{
            //    SystemObject.GetInstance().ObjCardReader.Stop();

            //    bool isTrue = SystemObject.GetInstance().ObjCardReader.Close();

            //    if (isTrue)
            //    {
            //        SeatManage.SeatManageComm.WriteLog.Write("读卡器已经关闭");
            //    }
            //    else
            //    {
            //        SeatManage.SeatManageComm.WriteLog.Write("读卡器没关闭");
            //    }
            //}
            //else
            //{
            //    SeatManage.SeatManageComm.WriteLog.Write("读卡器没在工作");
            //}
        }
    

    //private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    //    {
    //        MessageBoxResult result = MessageBox.Show("确定是退出吗？", "询问", MessageBoxButton.YesNo, MessageBoxImage.Question);

    //        //关闭窗口
    //        if (result == MessageBoxResult.Yes)
    //            e.Cancel = false;

    //        //不关闭窗口
    //        if (result == MessageBoxResult.No)
    //            e.Cancel = true;
    //    }

        //[STAThread]
        //public static void Main()
        //{
        //    var w = new AppLoadingWindow();
        //    var app = new App();
        //    app.Run(w);
        //}
        //private AppLoadingWindowObject _wappLoadingWindowObject;
        //private KeyboardWindowObject _wkeyboardWindowObject;
        //private LeaveWindowObject _wleaveWindowObject;
        //private MainWindowObject _wmainWindowObject;
        //private PopupWindowsObject _wpopupWindowsObject;
        //private ReaderNoteWindowObject _wreaderNoteWindowObject;
        //private ReadingRoomWindowObject _wreadingRoomWindowObject;
        //private RecordTheQueryWindowObject _wrecordTheQueryWindowObject;
        //private RoomSeatWindowObject _wroomSeatWindowObject;
        //private UsuallySeatWindowObject _wusuallySeatWindowObject;

        //public AppLoadingWindowObject WappLoadingWindowObject
        //{
        //    get { return _wappLoadingWindowObject; }
        //    set { _wappLoadingWindowObject = value; }
        //}

        //public KeyboardWindowObject WkeyboardWindowObject
        //{
        //    get { return _wkeyboardWindowObject; }
        //    set { _wkeyboardWindowObject = value; }
        //}

        //public LeaveWindowObject WleaveWindowObject
        //{
        //    get { return _wleaveWindowObject; }
        //    set { _wleaveWindowObject = value; }
        //}

        //public MainWindowObject WmainWindowObject
        //{
        //    get { return _wmainWindowObject; }
        //    set { _wmainWindowObject = value; }
        //}

        //public PopupWindowsObject WpopupWindowsObject
        //{
        //    get { return _wpopupWindowsObject; }
        //    set { _wpopupWindowsObject = value; }
        //}

        //public ReaderNoteWindowObject WreaderNoteWindowObject
        //{
        //    get { return _wreaderNoteWindowObject; }
        //    set { _wreaderNoteWindowObject = value; }
        //}

        //public ReadingRoomWindowObject WreadingRoomWindowObject
        //{
        //    get { return _wreadingRoomWindowObject; }
        //    set { _wreadingRoomWindowObject = value; }
        //}

        //public RecordTheQueryWindowObject WrecordTheQueryWindowObject
        //{
        //    get { return _wrecordTheQueryWindowObject; }
        //    set { _wrecordTheQueryWindowObject = value; }
        //}

        //public RoomSeatWindowObject WroomSeatWindowObject
        //{
        //    get { return _wroomSeatWindowObject; }
        //    set { _wroomSeatWindowObject = value; }
        //}

        //public UsuallySeatWindowObject WusuallySeatWindowObject
        //{
        //    get { return _wusuallySeatWindowObject; }
        //    set { _wusuallySeatWindowObject = value; }
        //}
    }
}

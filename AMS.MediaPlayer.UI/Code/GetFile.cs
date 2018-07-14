﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Timers;
using System.IO;
using System.Configuration;
using System.Drawing;
using System.Windows.Media.Imaging;
using SeatManage.ClassModel;

namespace AMS.MediaPlayer.Code
{
    public delegate void PlayVideoEventHandler(object sender, string message);
    /// <summary>
    /// 
    /// </summary>
    public class GetVideoFile
    {
        #region 事件
        /// <summary>
        /// 通知播放事件
        /// </summary>
        public event PlayVideoEventHandler PlayVideo;
        /// <summary>
        /// 下载事件
        /// </summary>
        public event PlayVideoEventHandler PlayListHandleEvent;
        /// <summary>
        /// 初始化结束
        /// </summary>
        public event EventHandler PlayListHandleOver;
        #endregion

        public GetVideoFile()
        {
        }

        #region 成员变量
        /// <summary>
        /// 定时器，定时扫描播放列表
        /// </summary>
        private Timer timer = new Timer(300);

        /// <summary>
        /// 定时器，计重试时长；
        /// </summary>
        private Timer timer1;
        /// <summary>
        /// 当前要播放的视频文件地址
        /// </summary>
        private string _videoPath = "";
        /// <summary>
        /// 文件播放列表
        /// </summary>
        private List<AMS_VideoMd5Item> plists = new List<AMS_VideoMd5Item>();
        /// <summary>
        /// 错误日志文件夹路径
        /// </summary>
        private DirectoryInfo errorDir;
        #endregion

        /// <summary>
        /// 获取当前应该播放的媒体信息
        /// </summary>
        /// <param name="isOffline">是否脱机运行</param>
        public void Run()
        {
            if (PlayerSetting.IsOffline == "1")
            {

                //从服务器上获取新的播放列表以及媒体文件并且保存到本地
                // get.Run();
                SeatManage.ClassModel.AMS_PlayListMd5 playlistModel = new AMS_PlayListMd5();
                //从服务器上获取新的播放列表
                timer1 = new Timer(1000);
                timer1.Elapsed += new ElapsedEventHandler(timer1_Elapsed);
                timer1.Start();
                try
                {
                    if (DownloadPlaylist(ref playlistModel))
                    {
                        //把新获取的播放列表写入文件
                        if (WritePlayListToFile(playlistModel))
                        {
                            SeatManage.SeatManageComm.WriteLog.Write("播放列表文件写入成功，准备下载……");
                            if (string.IsNullOrEmpty(PlayerSetting.DeviceNo))
                            {
                                SeatManage.SeatManageComm.WriteLog.Write("终端编号为空");
                            }
                            else
                            {
                                TerminalInfoV2 terminal = SeatManage.Bll.TerminalOperatorService.GetTeminalSetting(PlayerSetting.DeviceNo);
                                if (terminal != null)
                                {
                                    terminal.IsUpdatePlayList = false;
                                    SeatManage.Bll.TerminalOperatorService.UpdateTeminalSetting(terminal);
                                }
                            }
                            List<string> lists = GetDownloadVideoFile(playlistModel);
                            //删除无用的文件
                            DeleteNullFile();
                            DownloadFile(lists);

                        }
                        else
                        {
                            SeatManage.SeatManageComm.WriteLog.Write("播放列表写入失败");
                        }
                    }
                    else
                    {
                        //TODO：没有新的播放列表
                    }
                }
                catch (Exception ex)
                {
                    SeatManage.SeatManageComm.WriteLog.Write("网络未连接" + ex.Message);
                }
                finally
                {
                    timer1.Stop();
                }
            }
            //载入本地的播放列表
            if (LoadPlayList())
            {
                timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
                //播放列表初始化结束，触发事件
                PlayListHandleOver(null, null);
                timer.Start();
            }
        }
        int s = 0;
        void timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
            s += 1;
        }
        /// <summary>
        /// 获取播放列表
        /// </summary>
        private bool DownloadPlaylist(ref AMS_PlayListMd5 model)
        {
            while (true)
            {
                //获取今天的播放列表
                model = null;

                try
                {
                    List<SeatManage.ClassModel.AMS_PlayListMd5> playlist = SeatManage.Bll.AMS_PlayList.GetMd5PlayListByStatus(SeatManage.EnumType.LogStatus.Valid);
                    if (playlist.Count > 0)
                    {
                        model = playlist[0];
                    }
                    else
                    {
                        PlayListHandleEvent(this, "没有有效的播放列表");
                        return false;
                    }
                    timer1.Stop();
                    timer1.Dispose();
                    return true;
                }
                catch (Exception ex)
                {
                    PlayListHandleEvent(this, "服务器连接失败，正在重试……");
                    System.Threading.Thread.Sleep(2000);
                    if (s > 300)
                    {
                        timer1.Stop();
                        timer1.Dispose();
                        return false;
                    }

                }

                //if (model != null)
                //{
                //    timer1.Stop();
                //    timer1.Dispose();
                //    return true;
                //}
                //else
                //{
                //    SeatManage.SeatManageComm.WriteLog.Write(ex.Message);
                //    return false;
                //}
            }
        }

        /// <summary>
        /// 更新播放列表，并写入文件
        /// </summary>
        /// <param name="model">播放列表</param>
        /// <returns>成功写入返回true，否则返回false</returns>
        private bool WritePlayListToFile(AMS_PlayListMd5 model)
        {

            XmlDocument xmlDoc = new XmlDocument();
            //获取应用程序所在文件夹
            string FilePath = PlayerSetting.DefaultVideosPath + "PlayList.xml";

            try
            {

                if (model == null)
                {
                    return false;
                }
                xmlDoc.LoadXml(model.ToXml());
                DirectoryInfo d = new DirectoryInfo(PlayerSetting.DefaultVideosPath);
                if (!d.Exists)
                {
                    d.Create();
                }
                xmlDoc.Save(FilePath);
                return true;
            }
            catch (Exception ex)
            {
                SeatManage.SeatManageComm.WriteLog.Write(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 解析播放列表并下载视频文件
        /// </summary>
        /// <param name="model">下载的文件地址</param>
        private List<string> GetDownloadVideoFile(AMS_PlayListMd5 model)
        {
            //存放待下载的媒体文件
            List<string> downloadFiles = new List<string>();

            //播放列表存放的文件夹
            DirectoryInfo direct = new DirectoryInfo(PlayerSetting.DefaultVideosPath);
            if (!direct.Exists)
            {
                direct.Create();
            }
            //下载所有的媒体文件。
            foreach (AMS_VideoMd5Item vm in model.VideoFiles)
            {
                downloadFiles.Add(vm.RelativeUrl);
            }
            return downloadFiles;
        }

        /// <summary>
        /// 下载视频文件
        /// </summary>
        /// <param name="vm">视频文件相对路径</param>
        public void DownloadFile(List<string> videoFilePaths)
        {
            SeatManage.Bll.FileOperate fi = new SeatManage.Bll.FileOperate();
            fi.DownloadError += new SeatManage.Bll.EventHandleFileOperateError(fi_DownloadError);
            //执行下载操作
            for (int i = 0; i < videoFilePaths.Count; i++)
            {

                string path = videoFilePaths[i];
                string fullPath = string.Format("{0}{1}", PlayerSetting.DefaultVideosPath, path);
                try
                {
                    PlayListHandleEvent(this, string.Format("正在下载{0}", path));
                    fi.FileDownLoad(fullPath, path, SeatManage.EnumType.SeatManageSubsystem.MediaFiles);
                }
                catch (Exception ex)
                {
                    SeatManage.SeatManageComm.WriteLog.Write(ex.Message);
                }

            }


        }

        void fi_DownloadError(string message)
        {
            if (PlayListHandleEvent != null)
            {
                PlayListHandleEvent(this, message);
            }
        }

        /// <summary>
        /// 删除没用的视频文件
        /// </summary>
        private bool DeleteNullFile()
        {
            try
            {
                DirectoryInfo direct = new DirectoryInfo(PlayerSetting.DefaultVideosPath);
                string dv = PlayerSetting.DefaultVideo.Substring(PlayerSetting.DefaultVideo.LastIndexOf("\\") + 1);
                bool flag = false;
                //遍历文件夹中的文件
                foreach (FileInfo NextFile in direct.GetFiles())
                {
                    flag = false;
                    //名字不等于播放列表，或者名字不等于默认视频文件
                    if (NextFile.Name == "PlayList.xml" || NextFile.Name == dv)
                    {
                        //标记为不删除
                        flag = true;
                    }
                    if (!flag)
                    {
                        PlayListHandleEvent(this, string.Format("删除无用的视频文件{0}", NextFile.Name));
                        File.Delete(NextFile.FullName);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                SeatManage.SeatManageComm.WriteLog.Write(ex.Message);
                return false;
            }
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            getPlayFilePath();
            timer.Start();
        }

        /// <summary>
        /// 从本地载入播放列表
        /// </summary>
        public bool LoadPlayList()
        {
            try
            {
                string xmlDocPath = PlayerSetting.DefaultVideosPath + "PlayList.xml";
                if (System.IO.File.Exists(xmlDocPath))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(xmlDocPath);
                    AMS_PlayListMd5 plm = AMS_PlayListMd5.Parse(xmlDoc.OuterXml);
                    //播放列表赋值
                    plists = plm.PlayVideoItems;
                    //计算整个播放列表播放时间的间隔加上循环间隔时长
                    playListTimeLength = plm.PlayListTimeLength + plm.PlayElapsed;
                    //TODO:根据当前时间重新排列播放列表
                    RearrangePlayList();
                    return true;
                }
                else
                {
                    SeatManage.SeatManageComm.WriteLog.Write("播放列表不存在");
                    PlayListHandleEvent(this, "本地播放列表不存在！");
                    return false;
                }
            }
            catch
            {
                SeatManage.SeatManageComm.WriteLog.Write("载入播放列表失败");
                PlayListHandleEvent(this, "载入播放列表失败！");
                return false;
            }
        }
        /// <summary>
        /// 重新排列当前播放列表
        /// </summary>
        private void RearrangePlayList()
        {
            PlayListHandleEvent(this, "播放列表排列");

            int i = 1;
            do
            {
                //当前项播放时间
                DateTime playTime;
                if (plists.Count > 1)
                {
                    //当前项播放时间
                    playTime = DateTime.Parse(DateTime.Now.ToShortDateString() + " " + plists[i].PlayTime);
                }
                else
                {
                    playTime = DateTime.Parse(DateTime.Now.ToShortDateString() + " " + plists[0].PlayTime);
                }
                //当前时间
                DateTime dt = DateTime.Now;
                if (dt.CompareTo(playTime) >= 0)
                {
                    MoveVideo(plists[0]);
                }
                else
                {
                    return;
                }

            } while (true);

        }

        /// <summary>
        /// 获取当前时间播放的媒体文件路径
        /// </summary>
        /// <returns></returns>
        public void getPlayFilePath()
        {
            DateTime NowDate = DateTime.Now;//当前时间
            if (plists.Count > 0)
            {
                DateTime filePlayTime = DateTime.Parse(DateTime.Now.ToShortDateString() + " " + plists[0].PlayTime);
                //判断当前时间是不是大于第一个文件的播放时间。
                if (NowDate.CompareTo(filePlayTime) > 0)
                {
                    //获取路径
                    string relativurl = plists[0].RelativeUrl;
                    string path = PlayerSetting.DefaultVideosPath + relativurl;
                    string md5Value = plists[0].md5Value;
                    //更新播放时间
                    MoveVideo(plists[0]);
                    //md5校验
                    if (!string.IsNullOrEmpty(md5Value))
                    {
                        string MediaMd5 = SeatManage.SeatManageComm.SeatComm.GetMD5HashFromFile(path);
                        if (MediaMd5.Equals(md5Value))
                        {
                            //触发播放视频的事件
                            PlayVideo(this, path);
                        }
                        else
                        {
                            PlayListHandleEvent(this, string.Format("文件{0} MD5校验失败！", relativurl));
                        }
                    }
                    else
                    {
                        PlayVideo(this, path);
                    }

                }
            }
        }


        int playListTimeLength = 0;
        /// <summary>
        /// 把传递过来的项移动到最后一项
        /// </summary>
        /// <param name="videoItem"></param>
        private void MoveVideo(AMS_VideoMd5Item videoItem)
        {
            //计算当前项下次播放的时间
            DateTime dt = DateTime.Parse(DateTime.Now.ToShortDateString() + " " + videoItem.PlayTime).AddSeconds(playListTimeLength);
            //修改当前项播放的时间
            videoItem.PlayTime = dt.ToLongTimeString();
            //加到结尾
            plists.Add(videoItem);
            //移除第一项
            plists.RemoveAt(0);

        }
        /// <summary>
        /// 把图片文件读到内存中
        /// </summary>
        /// <param name="imgPath"></param>
        /// <returns></returns>
        public static BitmapImage InitImage(string imgPath)
        {
            if (File.Exists(imgPath))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(imgPath, FileMode.Open)))
                {
                    BitmapImage bitmapImage;
                    FileInfo fi = new FileInfo(imgPath);
                    byte[] bytes = reader.ReadBytes((int)fi.Length);
                    reader.Close();
                    bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = new MemoryStream(bytes);
                    bitmapImage.EndInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    return bitmapImage;
                }
            }
            else
            {
                return null;
            }
        }


    }
}

using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Dos.ORM;
using Dos.ORM.Common;

namespace WeiXinMsgService.Model
{

    /// <summary>
    /// 实体类M_tb_User (属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Table("tb_User")]
    [Serializable]
    public partial class tb_User : Entity
    {

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("OpenId:" + ID+"    ");
            sb.Append("ID:" + OpenId + "    ");
            sb.Append("SchoolNo:" + SchoolNo + "    ");
            sb.Append("ClientNo:" + ClientNo + "    ");
            sb.Append("CardNo:" + CardNo + "    ");
            sb.Append("StudentNo:" + StudentNo + "    ");
            sb.Append("State:" + State + "    ");
            sb.Append("CreateTime:" + CreateTime + "    ");
            return sb.ToString();
        }

        #region Model
        private int _ID;
        private string _OpenId;
        private string _SchoolNo;
        private string _ClientNo;
        private string _CardNo;
        private string _StudentNo;
        private string _NickName;
        private string _HeadImgUrl;
        private string _Name;
        private int _Sex;
        private string _Moblie;
        private string _Department;
        private string _ReaderType;
        private int _State;
        private int? _Integral;
        private bool _IsDealer;
        private DateTime _CreateTime;
        private DateTime? _ExpDate;

        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            get { return _ID; }
            set
            {
                this.OnPropertyValueChange(_.ID, _ID, value);
                this._ID = value;
            }
        }
        /// <summary>
        /// 微信openid
        /// </summary>
        public string OpenId
        {
            get { return _OpenId; }
            set
            {
                this.OnPropertyValueChange(_.OpenId, _OpenId, value);
                this._OpenId = value;
            }
        }
        /// <summary>
        /// 自有系统的用户ID
        /// </summary>
        public string SchoolNo
        {
            get { return _SchoolNo; }
            set
            {
                this.OnPropertyValueChange(_.SchoolNo, _SchoolNo, value);
                this._SchoolNo = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ClientNo
        {
            get { return _ClientNo; }
            set
            {
                this.OnPropertyValueChange(_.ClientNo, _ClientNo, value);
                this._ClientNo = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CardNo
        {
            get { return _CardNo; }
            set
            {
                this.OnPropertyValueChange(_.CardNo, _CardNo, value);
                this._CardNo = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string StudentNo
        {
            get { return _StudentNo; }
            set
            {
                this.OnPropertyValueChange(_.StudentNo, _StudentNo, value);
                this._StudentNo = value;
            }
        }
        /// <summary>
        /// 昵称-微信
        /// </summary>
        public string NickName
        {
            get { return _NickName; }
            set
            {
                this.OnPropertyValueChange(_.NickName, _NickName, value);
                this._NickName = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string HeadImgUrl
        {
            get { return _HeadImgUrl; }
            set
            {
                this.OnPropertyValueChange(_.HeadImgUrl, _HeadImgUrl, value);
                this._HeadImgUrl = value;
            }
        }
        /// <summary>
        /// 经度
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set
            {
                this.OnPropertyValueChange(_.Name, _Name, value);
                this._Name = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Sex
        {
            get { return _Sex; }
            set
            {
                this.OnPropertyValueChange(_.Sex, _Sex, value);
                this._Sex = value;
            }
        }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Moblie
        {
            get { return _Moblie; }
            set
            {
                this.OnPropertyValueChange(_.Moblie, _Moblie, value);
                this._Moblie = value;
            }
        }
        /// <summary>
        /// 纬度
        /// </summary>
        public string Department
        {
            get { return _Department; }
            set
            {
                this.OnPropertyValueChange(_.Department, _Department, value);
                this._Department = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ReaderType
        {
            get { return _ReaderType; }
            set
            {
                this.OnPropertyValueChange(_.ReaderType, _ReaderType, value);
                this._ReaderType = value;
            }
        }
        /// <summary>
        /// 状态-1为正常
        /// </summary>
        public int State
        {
            get { return _State; }
            set
            {
                this.OnPropertyValueChange(_.State, _State, value);
                this._State = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Integral
        {
            get { return _Integral; }
            set
            {
                this.OnPropertyValueChange(_.Integral, _Integral, value);
                this._Integral = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsDealer
        {
            get { return _IsDealer; }
            set
            {
                this.OnPropertyValueChange(_.IsDealer, _IsDealer, value);
                this._IsDealer = value;
            }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return _CreateTime; }
            set
            {
                this.OnPropertyValueChange(_.CreateTime, _CreateTime, value);
                this._CreateTime = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ExpDate
        {
            get { return _ExpDate; }
            set
            {
                this.OnPropertyValueChange(_.ExpDate, _ExpDate, value);
                this._ExpDate = value;
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// 获取实体中的标识列
        /// </summary>
        public override Field GetIdentityField()
        {
            return _.ID;
        }
        /// <summary>
        /// 获取实体中的主键列
        /// </summary>
        public override Field[] GetPrimaryKeyFields()
        {
            return new Field[] {
				_.ID};
        }

        /// <summary>
        /// 获取列信息
        /// </summary>
        public override Field[] GetFields()
        {
            return new Field[] {
				_.ID,
				_.OpenId,
				_.SchoolNo,
				_.ClientNo,
				_.CardNo,
				_.StudentNo,
				_.NickName,
				_.HeadImgUrl,
				_.Name,
				_.Sex,
				_.Moblie,
				_.Department,
				_.ReaderType,
				_.State,
				_.Integral,
				_.IsDealer,
				_.CreateTime,
				_.ExpDate};
        }

        /// <summary>
        /// 获取值信息
        /// </summary>
        public override object[] GetValues()
        {
            return new object[] {
				this._ID,
				this._OpenId,
				this._SchoolNo,
				this._ClientNo,
				this._CardNo,
				this._StudentNo,
				this._NickName,
				this._HeadImgUrl,
				this._Name,
				this._Sex,
				this._Moblie,
				this._Department,
				this._ReaderType,
				this._State,
				this._Integral,
				this._IsDealer,
				this._CreateTime,
				this._ExpDate};
        }
        #endregion

        #region _Field
        /// <summary>
        /// 字段信息
        /// </summary>
        public class _
        {
            /// <summary>
            /// * 
            /// </summary>
            public readonly static Field All = new Field("*", "tb_User");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field ID = new Field("ID", "tb_User", "");
            /// <summary>
            /// 微信openid
            /// </summary>
            public readonly static Field OpenId = new Field("OpenId", "tb_User", "微信openid");
            /// <summary>
            /// 自有系统的用户ID
            /// </summary>
            public readonly static Field SchoolNo = new Field("SchoolNo", "tb_User", "自有系统的用户ID");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field ClientNo = new Field("ClientNo", "tb_User", "");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field CardNo = new Field("CardNo", "tb_User", "");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field StudentNo = new Field("StudentNo", "tb_User", "");
            /// <summary>
            /// 昵称-微信
            /// </summary>
            public readonly static Field NickName = new Field("NickName", "tb_User", "昵称-微信");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field HeadImgUrl = new Field("HeadImgUrl", "tb_User", "");
            /// <summary>
            /// 经度
            /// </summary>
            public readonly static Field Name = new Field("Name", "tb_User", "经度");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Sex = new Field("Sex", "tb_User", "");
            /// <summary>
            /// 手机号码
            /// </summary>
            public readonly static Field Moblie = new Field("Moblie", "tb_User", "手机号码");
            /// <summary>
            /// 纬度
            /// </summary>
            public readonly static Field Department = new Field("Department", "tb_User", "纬度");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field ReaderType = new Field("ReaderType", "tb_User", "");
            /// <summary>
            /// 状态-1为正常
            /// </summary>
            public readonly static Field State = new Field("State", "tb_User", "状态-1为正常");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Integral = new Field("Integral", "tb_User", "");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field IsDealer = new Field("IsDealer", "tb_User", "");
            /// <summary>
            /// 创建时间
            /// </summary>
            public readonly static Field CreateTime = new Field("CreateTime", "tb_User", "创建时间");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field ExpDate = new Field("ExpDate", "tb_User", "");
        }
        #endregion
    }
}


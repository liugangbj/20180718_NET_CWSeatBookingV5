using System;
using Dos.ORM;

namespace DianziKejiDaXueYKTInterface.Model
{
    /// <summary>
    /// 实体类T_SM_Reader。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Table("T_SM_Reader")]
    [Serializable]
    public partial class T_SM_Reader : Entity
    {
        #region Model
        private string _CardNo;
        private string _CardID;
        private string _ReaderName;
        private string _Sex;
        private string _ReaderTypeName;
        private string _ReaderDeptName;
        private string _ReaderProName;
        private string _Flag;

        /// <summary>
        /// 
        /// </summary>
        [Field("CardNo")]
        public string CardNo
        {
            get { return _CardNo; }
            set
            {
                this.OnPropertyValueChange("CardNo");
                this._CardNo = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Field("CardID")]
        public string CardID
        {
            get { return _CardID; }
            set
            {
                this.OnPropertyValueChange("CardID");
                this._CardID = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Field("ReaderName")]
        public string ReaderName
        {
            get { return _ReaderName; }
            set
            {
                this.OnPropertyValueChange("ReaderName");
                this._ReaderName = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Field("Sex")]
        public string Sex
        {
            get { return _Sex; }
            set
            {
                this.OnPropertyValueChange("Sex");
                this._Sex = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Field("ReaderTypeName")]
        public string ReaderTypeName
        {
            get { return _ReaderTypeName; }
            set
            {
                this.OnPropertyValueChange("ReaderTypeName");
                this._ReaderTypeName = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Field("ReaderDeptName")]
        public string ReaderDeptName
        {
            get { return _ReaderDeptName; }
            set
            {
                this.OnPropertyValueChange("ReaderDeptName");
                this._ReaderDeptName = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Field("ReaderProName")]
        public string ReaderProName
        {
            get { return _ReaderProName; }
            set
            {
                this.OnPropertyValueChange("ReaderProName");
                this._ReaderProName = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Field("Flag")]
        public string Flag
        {
            get { return _Flag; }
            set
            {
                this.OnPropertyValueChange("Flag");
                this._Flag = value;
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// 获取实体中的主键列
        /// </summary>
        public override Field[] GetPrimaryKeyFields()
        {
            return new Field[] {
                _.CardNo,
            };
        }
        /// <summary>
        /// 获取列信息
        /// </summary>
        public override Field[] GetFields()
        {
            return new Field[] {
                _.CardNo,
                _.CardID,
                _.ReaderName,
                _.Sex,
                _.ReaderTypeName,
                _.ReaderDeptName,
                _.ReaderProName,
                _.Flag,
            };
        }
        /// <summary>
        /// 获取值信息
        /// </summary>
        public override object[] GetValues()
        {
            return new object[] {
                this._CardNo,
                this._CardID,
                this._ReaderName,
                this._Sex,
                this._ReaderTypeName,
                this._ReaderDeptName,
                this._ReaderProName,
                this._Flag,
            };
        }
        /// <summary>
        /// 是否是v1.10.5.6及以上版本实体。
        /// </summary>
        /// <returns></returns>
        public override bool V1_10_5_6_Plus()
        {
            return true;
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
            public readonly static Field All = new Field("*", "T_SM_Reader");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field CardNo = new Field("CardNo", "T_SM_Reader", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field CardID = new Field("CardID", "T_SM_Reader", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field ReaderName = new Field("ReaderName", "T_SM_Reader", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field Sex = new Field("Sex", "T_SM_Reader", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field ReaderTypeName = new Field("ReaderTypeName", "T_SM_Reader", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field ReaderDeptName = new Field("ReaderDeptName", "T_SM_Reader", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field ReaderProName = new Field("ReaderProName", "T_SM_Reader", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field Flag = new Field("Flag", "T_SM_Reader", "");
        }
        #endregion
    }
}
using Dos.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DianziKejiDaXueYKTInterface.Model
{

    /// </summary>
    [Table("T_SM_Reader")]
    [Serializable]
    public class T_SM_Reader : Entity
    {

        private string _cardNo;


        private string _cardID;


        private string _readerName;

        private string _sex;


        private string _readerTypeName;

        private string _readerDeptName;

        private string _readerProName;

        private string _flag;

        public string CardNo
        {
            get
            {
                return _cardNo;
            }

            set
            {
                _cardNo = value;
            }
        }

        public string CardID
        {
            get
            {
                return _cardID;
            }

            set
            {
                _cardID = value;
            }
        }

        public string ReaderName
        {
            get
            {
                return _readerName;
            }

            set
            {
                _readerName = value;
            }
        }

        public string Sex
        {
            get
            {
                return _sex;
            }

            set
            {
                _sex = value;
            }
        }

        public string ReaderTypeName
        {
            get
            {
                return _readerTypeName;
            }

            set
            {
                _readerTypeName = value;
            }
        }

        public string ReaderDeptName
        {
            get
            {
                return _readerDeptName;
            }

            set
            {
                _readerDeptName = value;
            }
        }

        public string ReaderProName
        {
            get
            {
                return _readerProName;
            }

            set
            {
                _readerProName = value;
            }
        }

        public string Flag
        {
            get
            {
                return _flag;
            }

            set
            {
                _flag = value;
            }
        }
    }
}
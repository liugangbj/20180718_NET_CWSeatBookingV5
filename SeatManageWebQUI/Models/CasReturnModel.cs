using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatManageWebQUI.Models
{
    public class CasReturnModel
    {
        /// <summary>
        /// 为学工号，不区分学生还是老师。
        /// </summary>
        public string user { get; set; }
        /// <summary>
        /// 如果有值则为教师。
        /// </summary>
        public string eduPersonStaffID { get; set; }
        /// <summary>
        /// 为所在单位。
        /// </summary>
        public string eduPersonOrgDN { get; set; }
        /// <summary>
        /// 为姓名
        /// </summary>
        public string cn { get; set; }
        /// <summary>
        /// 为LDAP的DN，用处不大。
        /// </summary>
        public string containerId { get; set; }
    }
}
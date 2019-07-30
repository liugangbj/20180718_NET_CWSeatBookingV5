using SeatManage.IWCFService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace ISpaceBookingWCFService
{
    /// <summary>
    /// 空间预约系统接口层
    /// </summary>
    [ServiceContract]
    public partial interface ISpaceBookingManager: IExceptionService
    {
        [OperationContract]
        string  Display();
    }
}

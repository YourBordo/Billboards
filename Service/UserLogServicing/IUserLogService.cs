using Billboards.Models;
using System;
using System.Collections.Generic;

namespace ModelServices.UserLogServicing
{
    public interface IUserLogService
    {
        ICollection<UserLog> GetUserLogs(long userId);

        string DownloadHisory(long userId, DateTime? startPeriod, DateTime? finishPeriod);
    }
}

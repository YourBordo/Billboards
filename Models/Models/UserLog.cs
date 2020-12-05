using System;

namespace Billboards.Models
{
    public class UserLog
    {
        public long UserId { get; }
        public DateTime LogTime { get; set; }
        public string ActionMessage { get; set; }

        public UserLog(long userId)
        {
            UserId = userId;
        }
    }
}

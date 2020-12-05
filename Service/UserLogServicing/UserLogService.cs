using Billboards.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ModelServices.UserLogServicing
{
    public class UserLogService : IUserLogService
    {
        public ICollection<UserLog> GetUserLogs(long userId)
        {
            string logsPath = Path.GetFullPath(Directory.GetCurrentDirectory() + @"\bin\Debug\netcoreapp2.2\logs");
            if (!Directory.Exists(logsPath))
            {
                return null;
            }


            List<UserLog> userLogs = new List<UserLog>();
            using (StreamReader str = new StreamReader(logsPath + @"\logsFile.log", System.Text.Encoding.Default))
            {
                string line;
                while ((line = str.ReadLine()) != null)
                {
                    var log = line.Split("|".ToCharArray());

                    if (log[1] == "INFO" && long.Parse(log[2]) == userId)
                    {
                        UserLog userLog = new UserLog(userId);
                        userLog.ActionMessage = line;
                        userLog.LogTime = Convert.ToDateTime(line.Split("|".ToCharArray())[0]);
                        userLogs.Add(userLog);
                    }
                }
            }
            return userLogs;
        }

        public string DownloadHisory(long userId, DateTime? startPeriod, DateTime? finishPeriod)
        {
            string dateTime = DateTime.Now.Ticks.ToString();

            string logsPath = Path.GetFullPath(Directory.GetCurrentDirectory() + @"\bin\Debug\netcoreapp2.2\logs");
            if (!Directory.Exists(logsPath))
            {
                return null;
            }

            string path = Path.GetFullPath(Directory.GetCurrentDirectory() + $@"\TempHistory\{dateTime}.txt");

            using (StreamReader str = new StreamReader(logsPath + @"\logsFile.log", System.Text.Encoding.Default))
            {
                using (StreamWriter fs = new StreamWriter(path))
                {
                    string line;
                    while ((line = str.ReadLine()) != null)
                    {
                        long userIdFromFile = 0;
                        var separaratedLine = line.Split("|".ToCharArray());
                        if (separaratedLine.Length < 2 && !long.TryParse(separaratedLine[2], out userIdFromFile))
                        {
                            continue;
                        }
                        else if (userIdFromFile == userId)
                        {
                            if (finishPeriod == null && startPeriod == null)
                            {
                                fs.WriteLine(line);

                            }
                            else if (finishPeriod == null && DateTime.Parse(line.Split("|".ToCharArray())[0]) >= startPeriod)
                            {
                                fs.WriteLine(line);
                            }
                            else if (startPeriod == null && DateTime.Parse(line.Split("|".ToCharArray())[0]) <= finishPeriod)
                            {
                                fs.WriteLine(line);
                            }
                            else if (DateTime.Parse(line.Split("|".ToCharArray())[0]) <= finishPeriod &&
                                DateTime.Parse(line.Split("|".ToCharArray())[0]) >= startPeriod)
                            {
                                fs.WriteLine(line);
                            }
                        }
                    }
                }
            }

            return path;
        }

    }
}

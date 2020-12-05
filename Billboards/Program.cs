using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Billboards
{
    public class Program
    {
        public static NLog.Logger Logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
          WebHost.CreateDefaultBuilder(args)  
            .UseStartup<Startup>()  
            .ConfigureLogging(logging =>  
            {  
                logging.ClearProviders();  
                logging.SetMinimumLevel(LogLevel.Information);  
            })  
            .UseNLog();  
    }
}

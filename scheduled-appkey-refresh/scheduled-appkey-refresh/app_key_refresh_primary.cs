using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using ScheduledAappkeyRefresh.Service;
using System.Threading.Tasks;

namespace ScheduledAappkeyRefresh
{
    public static class App_key_refresh_primary
    {
        [FunctionName("app_key_refresh_primary")]
        public static async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function #app_key_refresh_primary Executed at: {DateTime.Now}");

            if (await AppKeyRefresh.RefreshAsync(true))
            {
                log.LogInformation($"C# Timer trigger function #app_key_refresh_primary Completed at: {DateTime.Now}");
            }
            else
            {
                log.LogCritical($"C# Timer trigger function #app_key_refresh_primary Failed at: {DateTime.Now}");
            }
        }
    }
}

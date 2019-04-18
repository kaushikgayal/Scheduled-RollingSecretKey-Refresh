using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using ScheduledAappkeyRefresh.Service;

namespace ScheduledAappkeyRefresh
{
    public static class app_key_refresh_secondary
    {
        [FunctionName("app_key_refresh_secondary")]
        public static async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function #app_key_refresh_secondary Executed at: {DateTime.Now}");

            if (await AppKeyRefresh.RefreshAsync(false))
            {
                log.LogInformation($"C# Timer trigger function #app_key_refresh_secondary Completed at: {DateTime.Now}");
            }
            else
            {
                log.LogCritical($"C# Timer trigger function #app_key_refresh_secondary Failed at: {DateTime.Now}");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ScheduledAppkeyRefresh.Constants;

namespace ScheduledAppkeyRefresh.Service
{
    public static class WAF
    {
        private static HttpClient client = new HttpClient(new HttpClientHandler(), false);
        private static readonly int MaxRetryCount = 3;
        public static async Task<bool> UpdateAllRegisteredWAFRulesAsync(string newKey)
        {
            return await UpdateWAFRuleAsync(newKey, ImpervaRuleId.SiteName);
        }
        private static async Task<bool> UpdateWAFRuleAsync(string newKey, string ruleId)
        {
            var result = new HttpResponseMessage();
            try
            {
                var updateImpervaURL = $"<ImpervaWAF URL>";

                var httpRequest = new HttpRequestMessage(new HttpMethod("Post"), updateImpervaURL);
                for (int i = 0; i < MaxRetryCount; i++)
                {
                    result = await client.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
                    if (result.IsSuccessStatusCode) return true;
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return false;
        }
    }
}

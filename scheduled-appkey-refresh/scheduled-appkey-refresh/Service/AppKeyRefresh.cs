using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduledAppkeyRefresh.Constants;
using ScheduledAppkeyRefresh.Service;

namespace ScheduledAappkeyRefresh.Service
{
    /*
        ##Schedule 1:##

     1. Update Secondary Key in WAF.
     2. Update Primary Key in Vault.
     3. Take a break ( Next update after 3 days )

         ##Schedule 2:##

     1. Update Primary Key in WAF.
     2. Update Secondary Key in Vault.
     3. Take a break ( Next update after 3 days )

    */
    public static class AppKeyRefresh
    {
        public static async Task<bool> RefreshAsync(bool isPrimary)
        {
            /*
                 isPrimary=true is Schedule 1
                 isPrimary=false is Schedule 2
            */

            //create key
            var app_key = Guid.NewGuid().ToString();
            var objVault = new Vault();

            //if Primary Refresh, get secondary key
            //if Secondary Refresh, get primary key
            var existingKey = await objVault.GetVaultKey(app_key, isPrimary ? false : true);

            //update existing key from Vault in WAF which we are not updating currently
            //update rule
            return await WAF.UpdateAllRegisteredWAFRulesAsync(existingKey.ToString())
                ? await objVault.UpdateAppKey(app_key, isPrimary)
                : false;
        }
    }
}

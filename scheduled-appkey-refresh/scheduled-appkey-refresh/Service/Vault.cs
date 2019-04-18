using System;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using System.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace ScheduledAappkeyRefresh.Service
{
    public class Vault
    {
        private KeyVaultClient vaultClient;
        private readonly string vaultAddress;
        private readonly string primarySecretName="<Primary_Key_Name>";
        private readonly string secondarySecretName = "<Secondary_Key_Name>";

        public Vault()
        {
            vaultAddress= ConfigurationManager.AppSettings["VaultUri"];
            vaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetAccessToken));
        }

        private async Task<string> GetAccessToken(string authority, string resource, string scope)
        {
            var clientId = ConfigurationManager.AppSettings["ClientID"];
            var clientSecret = ConfigurationManager.AppSettings["ClientSecret"];
            ClientCredential clientCredential = new ClientCredential(clientId, clientSecret);
            var context = new AuthenticationContext(authority, TokenCache.DefaultShared);
            var result = await context.AcquireTokenAsync(resource, clientCredential);

            return result.AccessToken;
        }

        internal async Task<bool> UpdateAppKey(string value,bool isPrimary)
        {
            bool response;
            try
            {
                var secretName = isPrimary ? primarySecretName : secondarySecretName;
                var result= await vaultClient.SetSecretAsync(vaultAddress,secretName,value);
                response = true;
            }
            catch (Exception e)
            {
                throw;
            }
            return response;
        }

        internal async Task<string> GetVaultKey(string key, bool isPrimary)
        {
            string response=string.Empty;
            try
            {
                var secretName = isPrimary ? primarySecretName : secondarySecretName;
                var result = await vaultClient.GetSecretAsync(vaultAddress, secretName);
                response = result.Value;
            }
            catch (Exception e)
            {
                throw;
            }
            return response;
        }
    }
}

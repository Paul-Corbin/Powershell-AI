using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowershellAI
{
    internal class CredentialHelper
    {
        private const string CredentialTarget = "PowershellAI_API_Key";

        public static void StoreApiKey(string ApiKey)
        {
            var credential = new CredentialManagement.Credential
            {
                Target = CredentialTarget,
                Username = "APIKey",
                Password = ApiKey,
                PersistanceType = CredentialManagement.PersistanceType.LocalComputer
            };
            credential.Save();
        }
        public static string GetApiKey()
        {
            var credential = new CredentialManagement.Credential
            {
                Target = CredentialTarget
            };
            credential.Load();
            return credential.Password;
        }

        public static void DeleteApiKey()
        {
            var credential = new CredentialManagement.Credential
            {
                Target = CredentialTarget
            };
            credential.Delete();
        }
    }
}

using System.Runtime.CompilerServices;
using System.Windows;

namespace PowershellAI
{
    /// <summary>
    /// Interaction logic for CredentialHelper.xaml
    /// </summary>
    internal partial class CredentialHelper : Window
    {
        internal CredentialHelper()
        {
            InitializeComponent();
        }
        private const string CredentialTarget = "PowershellAI_API_Key";

        internal void StoreApiKey(string apiKey)
        {
            var credential = new CredentialManagement.Credential
            {
                Target = CredentialTarget,
                Username = "APIKey",
                Password = apiKey,
                PersistanceType = CredentialManagement.PersistanceType.LocalComputer
            };
            credential.Save();
        }
        internal string GetApiKey()
        {
            try
            {
                var credential = new CredentialManagement.Credential
                {
                    Target = CredentialTarget
                };
                credential.Load();
                return credential.Password;
            }
            catch
            {
                return "";
            }
        }

        internal void RequestApiKey()
        {
            this.Show();
        }
    }
}

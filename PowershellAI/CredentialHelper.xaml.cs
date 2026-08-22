using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;

namespace PowershellAI
{
    /// <summary>
    /// Interaction logic for CredentialHelper.xaml
    /// </summary>
    public partial class CredentialHelper : Window
    {   
        private const string DefaultInputString = "Enter command...";
        private bool _textCleared = false;
        internal CredentialHelper()
        {
            InitializeComponent();
            Hide();
        }
        private const string CredentialTarget = "PowershellAI_API_Key";

        private void TopBarDown(object sender, RoutedEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }
        private void CloseClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void MinimizeClick(object sender, RoutedEventArgs e) { this.WindowState = WindowState.Minimized; }

        private void FocusLost(object sender, RoutedEventArgs e)
        {
            if (!this.ApiKeyBox.Text.Equals("")) return;
            this.ApiKeyBox.Text = DefaultInputString;
            _textCleared = false;
        }

        private void KeyDown(object sender, RoutedEventArgs e)
        {
            if (_textCleared) return;
            this.ApiKeyBox.Text = "";
            _textCleared = true;
        }
        internal void StoreApiKey(object sender, RoutedEventArgs e)
        {
            var credential = new CredentialManagement.Credential
            {
                Target = CredentialTarget,
                Username = "APIKey",
                Password = this.ApiKeyBox.Text,
                PersistanceType = CredentialManagement.PersistanceType.LocalComputer
            };
            credential.Save();
            this.DialogResult = true;           
            this.Close();
        }
        internal string GetApiKey()
        {
            Debug.WriteLine("Attempting to retrieve API Key");
            try
            {
                var credential = new CredentialManagement.Credential
                {
                    Target = CredentialTarget
                };
                credential.Load();
                Debug.WriteLine("API Key retrieved successfully.");
                return credential.Password;
            }
            catch
            {
                Debug.WriteLine("Unable to retrieve API Key. Requesting API Key.");
                return RequestApiKey();
            }
        }

        internal string RequestApiKey()
        {
            Debug.WriteLine("Requesting API Key.");
            // Reset the text box state
            this.ApiKeyBox.Text = DefaultInputString;
            _textCleared = false;
            this.DialogResult = null;

            // Show the dialog modally
            this.ShowDialog();
            Debug.WriteLine("API Key dialog closed.");

            // Check if user saved the key
            if (this.DialogResult == true)
            {
                // After user saves, try to load the stored credential
                try
                {
                    var credential = new CredentialManagement.Credential
                    {
                        Target = CredentialTarget
                    };
                    credential.Load();
                    Debug.WriteLine("API Key successfully retrieved after dialog.");
                    return credential.Password;
                }
                catch
                {
                    Debug.WriteLine("API Key not stored. Requesting again.");
                    // If still not stored, ask again recursively
                    return RequestApiKey();
                }
            }
            else
            {
                // User cancelled the dialog
                Debug.WriteLine("User cancelled API Key dialog.");
                throw new OperationCanceledException("User cancelled API Key input.");
            }
        }
    }
}

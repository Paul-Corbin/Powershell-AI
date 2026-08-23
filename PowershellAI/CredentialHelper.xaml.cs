using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using Windows.Security.Credentials;

namespace PowershellAI
{
    /// <summary>
    /// Interaction logic for CredentialHelper.xaml
    /// </summary>
    public partial class CredentialHelper : Window
    {
        private const string DefaultInputString = "Enter API Key...";
        private bool _textCleared = false;
        private TaskCompletionSource<string?> _apiKeyTaskCompletionSource;
        public CredentialHelper()
        {
            InitializeComponent();
        }
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
            this.ResetInput();
        }
        private void ResetInput()
        {
            _textCleared = false;
            this.Hide();
            this.InputBox.Text = DefaultInputString;
        }
        private void MinimizeClick(object sender, RoutedEventArgs e) { this.WindowState = WindowState.Minimized; }
        private void ApiFocusLost(object sender, RoutedEventArgs e)
        {
            if (!this.InputBox.Text.Equals("")) return;
            this.InputBox.Text = DefaultInputString;
            _textCleared = false;
        }

        private void ApiKeyDown(object sender, RoutedEventArgs e)
        {
            if (_textCleared) return;
            this.InputBox.Text = "";
            _textCleared = true;
        }
        private async void SaveClick(object sender, RoutedEventArgs e)
        {
            if (this.InputBox.Text.Equals("") || this.InputBox.Text.Equals(DefaultInputString)) return;
            this.Hide();
            var apiKey = this.InputBox.Text;
            bool saveSuccess = await SaveAPIKey(apiKey);
            if (saveSuccess)
            {
                Debug.WriteLine("API Key saved successfully.");
            }
            else
            {
                Debug.WriteLine("Failed to save API Key.");
            }
            _apiKeyTaskCompletionSource?.SetResult(apiKey);
            this.Close();
        }
        internal Task<string?> LoadAPIKey()
        {
            return Task.Run(() =>
            {
                try
                {
                    var vault = new PasswordVault();
                    var credential = vault.Retrieve("PowershellAI", "APIKey");
                    if (credential?.Password != null)
                    {
                        return credential.Password;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error loading API key: {ex.Message}\nRequesting new key.");
                    this.Show();
                    _apiKeyTaskCompletionSource = new TaskCompletionSource<string?>();
                    return _apiKeyTaskCompletionSource.Task.Result;
                }
                return null;
            });
        }
        private Task<bool> SaveAPIKey(string apiKey)
        {
            return Task.Run(() =>
            {
                try
                {
                    var vault = new PasswordVault();
                    vault.Add(new PasswordCredential("PowershellAI", "APIKey", apiKey));
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error saving API key: {ex.Message}");
                    return false;
                }
            }
            );
        }
    }
}
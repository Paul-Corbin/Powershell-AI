using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using CredentialManagement;

namespace PowershellAI
{
    /// <summary>
    /// Interaction logic for CredentialHelper.xaml
    /// </summary>
    public partial class CredentialHelper : Window
    {
        private const string DefaultInputString = "Enter API Key...";
        private const string CredentialTarget = "PowershellAI_APIKey";
        private bool _textCleared = false;
        private TaskCompletionSource<string?> _apiKeyTaskCompletionSource;
        public CredentialHelper()
        {
            //For testing, delete any existing credentials for this target.
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

        // XAML handlers (correct casing/signatures)
        private void APIFocusLost(object sender, RoutedEventArgs e)
        {
            ApiFocusLost(sender, e);
        }

        private void APIKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // mimic original behavior
            if (_textCleared) return;
            this.InputBox.Text = "";
            _textCleared = true;
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
            bool saveSuccess = await Task.Run(() =>
            {
                try
                {
                    var cred = new Credential
                    {
                        Target = CredentialTarget,
                        Username = "apikey",
                        Password = apiKey,
                        PersistanceType = PersistanceType.LocalComputer
                    };
                    cred.Save();
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error saving API key: {ex.Message}");
                    return false;
                }
            });

            if (saveSuccess)
            {
                Debug.WriteLine("API Key saved successfully.");
            }
            else
            {
                Debug.WriteLine("Failed to save API Key.");
            }

            _apiKeyTaskCompletionSource?.SetResult(apiKey);
            this.Hide();
        }

        internal async Task<string?> LoadAPIKey()
        {
            return await Task.Run(() =>
            {
                try
                {
                    var cred = new Credential { Target = CredentialTarget };
                    if (cred.Load())
                    {
                        return string.IsNullOrEmpty(cred.Password) ? null : cred.Password;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error loading API key: {ex.Message}\nRequesting new key.");
                    // fall through to request new key
                }

                // Show UI to get new key
                this.Dispatcher.Invoke(() => this.Show());
                _apiKeyTaskCompletionSource = new TaskCompletionSource<string?>();
                return _apiKeyTaskCompletionSource.Task.Result;
            });
        }
    }
}
using CredentialManagement;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PowershellAI
{
    /// <summary>
    /// Interaction logic for CredentialHelper.xaml
    /// </summary>
    public partial class CredentialHelper : Window
    {
        private const string DefaultInputString = "Enter API Key...";
        private const string CredentialTarget = "PowershellAI_APIKey";
        private TaskCompletionSource<string?> _apiKeyTaskCompletionSource;

        public CredentialHelper()
        {
            InitializeComponent();
            InputBox.Text = DefaultInputString;
            this.Icon = new BitmapImage(new Uri("pack://application:,,,/Resources/icon.ico"));
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
            this.Hide();
            this.InputBox.Text = DefaultInputString;
        }

        private void MinimizeClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ApiFocusLost(object sender, RoutedEventArgs e)
        {
            if (!this.InputBox.Text.Equals("")) return;
            this.InputBox.Text = DefaultInputString;
        }

        private void ApiGainFocus(object sender, RoutedEventArgs e)
        {
            // mimic original behavior
            if (!InputBox.Text.Equals(DefaultInputString)) return;
            this.InputBox.Text = "";
        }

        private async void SaveClick(object sender, RoutedEventArgs e)
        {
            if (this.InputBox.Text.Equals("") || this.InputBox.Text.Equals(DefaultInputString)) return;
            var apiKey = this.InputBox.Text;
            ResetInput();
            var saveSuccess = await Task.Run(() =>
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
                return UpdateAPIKey().Result;
            });
        }

        public async Task<string?> UpdateAPIKey()
        {
            this.Dispatcher.Invoke(this.Show);
            _apiKeyTaskCompletionSource = new TaskCompletionSource<string?>();
            return await _apiKeyTaskCompletionSource.Task;
        }

        public async Task<bool> DeleteAPIKey()
        {
            return await Task.Run(() =>
            {
                try
                {
                    var cred = new Credential { Target = CredentialTarget };
                    if (cred.Exists())
                    {
                        cred.Delete();
                        Debug.WriteLine("API Key deleted successfully.");
                        return true;
                    }
                    else
                    {
                        Debug.WriteLine("No API Key found to delete.");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error deleting API key: {ex.Message}");
                    return false;
                }
            });
        }
    }
}
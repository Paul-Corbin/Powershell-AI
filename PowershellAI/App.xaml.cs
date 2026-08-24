using Hardcodet.Wpf.TaskbarNotification;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PowershellAI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static OutputWindow OutputWindow;
    public static InputWindow InputWindow;
    public static CredentialHelper CredentialHelper;
    private TaskbarIcon? _icon;
    internal static string ApiKey { get; private set; }
    internal static Hotkey? GlobalHotkey { get; private set; }

    private void ExitClick(object sender, RoutedEventArgs e)
    {
        Shutdown();
    }
    private async void Application_Startup(object sender, StartupEventArgs e)
    {
        try
        { 
            Debug.WriteLine("Application Startup");
            _icon = (TaskbarIcon?)FindResource("TrayIcon");
            _icon.IconSource = new BitmapImage(new Uri("pack://application:,,,/Resources/icon.ico"));
            _icon.ToolTipText = "Powershell AI";
            CredentialHelper = new CredentialHelper();
            ApiKey = await CredentialHelper.LoadAPIKey();
            if (string.IsNullOrEmpty(ApiKey))
            {
                Debug.WriteLine("API Key is empty. Exiting application.");
                Shutdown();
                return;
            }
            Debug.WriteLine("API Key loaded successfully.");
            GlobalHotkey = new Hotkey();
            InputWindow = new InputWindow();
            OutputWindow = new OutputWindow();
        } catch (Exception ex) {
            Debug.WriteLine($"Error during application startup: {ex.Message}");
            Shutdown();
        }
    }
    protected override void OnExit(ExitEventArgs e)
    {
        if (_icon == null) return;
        _icon.Dispose();
        base.OnExit(e);
    }
    private void ShowMenu(object sender, RoutedEventArgs e)
    {
        if (_icon?.ContextMenu == null) return;
        _icon.ContextMenu.IsOpen = true;
    }
    private static void RestartApplication()
    {
        Process.Start(Application.ResourceAssembly.Location);
        Application.Current.Shutdown();
    }
    private async void DeleteApiKey(object sender, RoutedEventArgs e)
    {
        try
        {
            var deleteSuccess = await CredentialHelper.DeleteAPIKey();
            Debug.WriteLine(deleteSuccess ? "API key deleted successfully." : "Failed to delete API key.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error deleting API key: {ex.Message}");
        }
        finally
        {
            RestartApplication();
        }
    }
    private async void UpdateApiKey(object sender, RoutedEventArgs e)
    {
        try
        {
            await CredentialHelper.UpdateAPIKey();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating API key: {ex.Message}");
        }
        finally
        {
            RestartApplication();
        }
    }
}
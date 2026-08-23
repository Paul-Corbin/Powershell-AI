using Hardcodet.Wpf.TaskbarNotification;
using System.Diagnostics;
using System.Windows;

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
        Debug.WriteLine("Application Startup");
        _icon = (TaskbarIcon?)FindResource("TrayIcon");
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
}
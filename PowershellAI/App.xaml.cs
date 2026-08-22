using Hardcodet.Wpf.TaskbarNotification;
using System.Windows;

namespace PowershellAI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static string ApiKey;
    public static OutputWindow OutputWindow;
    public static InputWindow InputWindow;
    private TaskbarIcon? _icon;
    internal static Hotkey? GlobalHotkey { get; private set; }

    private void ExitClick(object sender, RoutedEventArgs e)
    {
        Shutdown();
    }
    private void Application_Startup(object s, StartupEventArgs e)
    {
        CredentialHelper credentialHelper = new CredentialHelper();
        ApiKey = credentialHelper.GetApiKey();
        _icon = (TaskbarIcon?)FindResource("TrayIcon");
        GlobalHotkey = new Hotkey();
        InputWindow = new InputWindow();
        OutputWindow = new OutputWindow();
        base.OnStartup(e);
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
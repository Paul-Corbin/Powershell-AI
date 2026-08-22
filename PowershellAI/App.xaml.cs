using Hardcodet.Wpf.TaskbarNotification;
using System.Windows;

namespace PowershellAI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private TaskbarIcon? _icon;
    internal static Hotkey? GlobalHotkey { get; private set; }

    private void ExitClick(object sender, RoutedEventArgs e)
    {
        Shutdown();
    }
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        _icon = (TaskbarIcon?)FindResource("TrayIcon");
        GlobalHotkey = new Hotkey();
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
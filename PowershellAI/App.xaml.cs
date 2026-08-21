using Hardcodet.Wpf.TaskbarNotification;
using System.Configuration;
using System.Data;
using System.Windows;

namespace PowershellAI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private TaskbarIcon? Icon;
    private void ExitClick(object sender, RoutedEventArgs e)
    {
        Shutdown();
    }
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        Icon = (TaskbarIcon)FindResource("TrayIcon");
    }
    protected override void OnExit(ExitEventArgs e)
    {
        if (Icon == null) return;
        Icon.Dispose();
        base.OnExit(e);
    }
    private void ShowMenu(object sender, RoutedEventArgs e)
    {
        if (Icon == null) return;
        Icon.ContextMenu.IsOpen = true;
    }
}
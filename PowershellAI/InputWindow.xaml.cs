using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PowershellAI;

/// <summary>
/// Interaction logic for InputWindow.xaml
/// </summary>
public partial class InputWindow : Window
{
    private const string DefaultInputString = "Enter command...";
    private bool _submittingRequest = false;
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
    private void CloseClick(object sender, RoutedEventArgs e) { 
        this.ResetInput();
    }
    private void ResetInput()
    {
        this.Hide();
        this.InputBox.Text = DefaultInputString;
    }
    private void MinimizeClick(object sender, RoutedEventArgs e) { this.WindowState = WindowState.Minimized; }
    private void InputFocusLost(object sender, RoutedEventArgs e)
    {
        if (!this.InputBox.Text.Equals("")) return;
        this.InputBox.Text = DefaultInputString;
    }

    private void InputGainFocus(object sender, RoutedEventArgs e)
    {
        if (!InputBox.Text.Equals(DefaultInputString)) return;
        this.InputBox.Text = "";
    }
    private async void SubmitClick(object sender, RoutedEventArgs e) { 
        try
        {
            Debug.WriteLine("Submit");
            if (this.InputBox.Text.Equals("") || this.InputBox.Text.Equals(DefaultInputString)) return;
            var requestString = this.InputBox.Text;
            this.ResetInput();
            var request = new Request();
            _submittingRequest = true;
            var response = await request.Submit(requestString);
            Debug.WriteLine(response.ToString());
            App.OutputWindow.Load(response);
        } catch (Exception ex) {
            Debug.WriteLine(ex.Message);
        } finally {
            _submittingRequest = false;
        }
    }

    private void Open(object? sender, string? e)
    {
        if (this.IsVisible || _submittingRequest) return;
        this.ResetInput();
        this.Show();
        App.OutputWindow.Hide();
    }
    public InputWindow()
    {
        this.Icon = new BitmapImage(new Uri("pack://application:,,,/Resources/icon.ico"));
        InitializeComponent();
        if (App.GlobalHotkey == null) throw new Exception("Global hotkey is not initialized.");
        App.GlobalHotkey.HotkeyFired += Open;
    }
}

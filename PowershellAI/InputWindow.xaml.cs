using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PowershellAI;

/// <summary>
/// Interaction logic for InputWindow.xaml
/// </summary>
public partial class InputWindow : Window
{
    private OutputWindow _outputWindow;
    private readonly string _defaultInputString = "Enter command...";
    private bool _textCleared = false;
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
        _textCleared = false;
        _submittingRequest = false;
        this.Hide();
        this.InputBox.Text = _defaultInputString;
    }
    private void MinimizeClick(object sender, RoutedEventArgs e) { this.WindowState = WindowState.Minimized; }
    private void InputFocusLost(object sender, RoutedEventArgs e)
    {
        if (!this.InputBox.Text.Equals("")) return;
        this.InputBox.Text = _defaultInputString;
        _textCleared = false;
    }

    private void InputKeyDown(object sender, RoutedEventArgs e)
    {
        if (_textCleared) return;
        this.InputBox.Text = "";
        _textCleared = true;
    }
    private async void SubmitClick(object sender, RoutedEventArgs e) { 
        Debug.WriteLine("Submit");
        if (this.InputBox.Text.Equals("") || this.InputBox.Text.Equals(_defaultInputString)) return;
        var requestString = this.InputBox.Text;
        this.ResetInput();
        var request = new Request();
        _submittingRequest = true;
        var response = await request.Submit(requestString);
        Debug.WriteLine(response.ToString());
        _outputWindow.Load(response);
        _submittingRequest = false;
    }

    private void Open(object sender, string e)
    {
        if (this.IsVisible || _submittingRequest) return;
        this.ResetInput();
        this.Show();
        _outputWindow.Hide();
        this.InputBox.Focus();
    }
    public InputWindow()
    {
        _outputWindow = new OutputWindow();
        App.GlobalHotkey.HotkeyFired += Open;
        InitializeComponent();
    }
}

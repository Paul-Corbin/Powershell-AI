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
    private readonly string _defaultInputString = "Enter command...";
    private bool _textCleared = false;
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
    private void InputGotFocus(object sender, RoutedEventArgs e) {
        if (!this.InputBox.Text.Equals(_defaultInputString)) return;
        this.InputBox.Text = "";
    }
    private async void SubmitClick(object sender, RoutedEventArgs e) { 
        Debug.WriteLine("Submit");
        if (this.InputBox.Text.Equals("") || this.InputBox.Text.Equals(_defaultInputString)) return;
        var requestString = this.InputBox.Text;
        this.ResetInput();
        var request = new Request();
        var response = await request.Submit(requestString);
        Debug.WriteLine(response.ToString());
        var outputWindow = new OutputWindow();
        outputWindow.Load(response);
    }

    private void Open(object sender, string e)
    {
        this.ResetInput();
        this.Show();
        this.InputBox.Focus();
    }
    public InputWindow()
    {
        App.GlobalHotkey.HotkeyFired += Open;
        InitializeComponent();
        this.Hide();
    }
}

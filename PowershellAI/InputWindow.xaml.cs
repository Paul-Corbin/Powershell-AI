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
    private String defaultInputString;
    private void TopBarDown(object sender, RoutedEventArgs e)
    {
        DragMove();
    }
    private void CloseClick(object sender, RoutedEventArgs e) { this.Hide(); }

    private void MinimizeClick(object sender, RoutedEventArgs e) { this.WindowState = WindowState.Minimized; }
    private void InputFocusLost(object sender, RoutedEventArgs e)
    {
        if (defaultInputString == null) return;
        if (!this.InputBox.Text.Equals("")) return;
        this.InputBox.Text = defaultInputString;
    }
    private void InputGotFocus(object sender, RoutedEventArgs e) {
        if (defaultInputString == null) return;
        if (!this.InputBox.Text.Equals(defaultInputString)) return;
        this.InputBox.Text = "";
    }

    public void ResetInput()
    {
        if (defaultInputString == null) return;
        this.InputBox.Text = defaultInputString;
    }
    private async void SubmitClick(object sender, RoutedEventArgs e) { 
        Debug.WriteLine("Submit");
        if (this.InputBox.Text.Equals("") || this.InputBox.Text.Equals(defaultInputString)) return;
        this.Hide();
        Request request = new Request();
        Response response = await request.Submit(this.InputBox.Text);
        Debug.WriteLine(response.ToString());
        OutputWindow outputWindow = new OutputWindow();
        outputWindow.Load(response);
    }
    public InputWindow()
    {
        InitializeComponent();
        defaultInputString = this.InputBox.Text;
    }
}

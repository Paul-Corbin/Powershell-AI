using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace PowershellAI
{
    /// <summary>
    /// Interaction logic for ReferenceGrid.xaml
    /// </summary>
    public partial class ReferenceGrid : UserControl
    {
        private string _link = "";
        public void SetCommand(string command)
        {
            this.ReferenceCommand.Text = command;
        }
        public void SetLink(string link)
        {
            this._link = link;
            this.ReferenceLink.Text = link;
        }
        private void OpenLink(object sender, RoutedEventArgs e)
        {
            // Open the link in the default web browser
            Debug.WriteLine($"{this.ReferenceCommand.Text}: {this._link}");
            if (this._link == "") return;
            Process.Start(new ProcessStartInfo(this._link) { UseShellExecute = true });
        }
        public ReferenceGrid()
        {
            InitializeComponent();
        }
    }
}

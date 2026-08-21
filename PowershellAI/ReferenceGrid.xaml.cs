using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PowershellAI
{
    /// <summary>
    /// Interaction logic for ReferenceGrid.xaml
    /// </summary>
    public partial class ReferenceGrid : UserControl
    {
        private string link = "";
        public void SetCommand(string command)
        {
            this.ReferenceCommand.Text = command;
        }
        public void SetLink(string link)
        {
            this.link = link;
            this.ReferenceLink.Text = link;
        }

        public void OpenLink(object sender, RoutedEventArgs e)
        {
            // Open the link in the default web browser
            Debug.WriteLine($"{this.ReferenceCommand.Text}: {this.link}");
            if (this.link == "") return;
            Process.Start(this.link);
        }
        public ReferenceGrid()
        {
            InitializeComponent();
        }
    }
}

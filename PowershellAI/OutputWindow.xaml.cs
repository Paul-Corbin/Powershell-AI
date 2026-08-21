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
using System.Windows.Shapes;

namespace PowershellAI
{
    /// <summary>
    /// Interaction logic for OutputWindow.xaml
    /// </summary>
    public partial class OutputWindow : Window
    {
        private void TopBarDown(object sender, RoutedEventArgs e)
        {
            DragMove();
        }
        private void CloseClick(object sender, RoutedEventArgs e) { this.Hide(); }

        private void MinimizeClick(object sender, RoutedEventArgs e) { this.WindowState = WindowState.Minimized; }
        public OutputWindow()
        {
            InitializeComponent();
        }

        internal void Load(Response response)
        {
            int i = 0;
            foreach (var reference in response.getReferences())
            {
                ReferenceGrid newReferenceGrid = new ReferenceGrid();
                newReferenceGrid.ReferenceLink.Text = reference.Value;
                newReferenceGrid.ReferenceCommand.Text = reference.Key;
                RowDefinition newRow = new RowDefinition();
                newRow.Height = new GridLength(30);
                this.References.RowDefinitions.Add(newRow);
                Grid.SetRow(newReferenceGrid, i);
                this.References.Children.Add(newReferenceGrid);
            }
            this.Show();
        }
    }
}

using System;
using System.Collections;
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
            this.Hide();
            //Clear references so this can be reused.
            this.References.Children.Clear();
            this.References.RowDefinitions.Clear();
        }

        private void MinimizeClick(object sender, RoutedEventArgs e) { this.WindowState = WindowState.Minimized; }
        public OutputWindow()
        {
            InitializeComponent();
        }

        internal void Load(Response response)
        {
            int i = 0;
            int RowHeight = 35;
            this.CommandBlock1.Text = response.GetCommand();
            foreach (var reference in response.GetReferences())
            {
                Debug.WriteLine($"New row:\nCommand: {reference.Key}\nLink: {reference.Value}\nRow: {i}");
                ReferenceGrid newReferenceGrid = new ReferenceGrid();
                newReferenceGrid.SetCommand(reference.Key);
                newReferenceGrid.SetLink(reference.Value);
                RowDefinition newRow = new RowDefinition();
                newRow.Height = new GridLength(RowHeight);
                this.References.RowDefinitions.Add(newRow);
                Grid.SetRow(newReferenceGrid, i++);
                this.References.Children.Add(newReferenceGrid);
            }
            this.Height = this.TopBarGrid.Height+6;
            this.Height += i > 5 ? (5 * RowHeight) : (++i * RowHeight);
            this.Show();
        }
    }
}

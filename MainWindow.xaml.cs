using Comparator.Behaviour;
using Comparator.Logger;
using Comparator.Spec;
using Microsoft.Win32;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.IO;
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
using Path = System.IO.Path;

namespace Comparator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SimpleLogger simpleLogger;
        LogAutoScrollBehaviour logAutoScrollBehaviour;
        SpecFile specFile;
        SpecFile orderFile;
        SpecComparator specComparator;

        readonly SpecFileParameters specFileParameters = new SpecFileParameters() {
            Worksheet = "Специф",
            FirstRow = 16,
            RowsMaxCount = 999,
            VendorCodeColumn = 3,
            NameColumn = 4,
            UnitColumn = 5,
            QuantityColumn = 6,
            PositionColumn = 1,
        };
        readonly SpecFileParameters orderFileParameters = new SpecFileParameters()
        {
            Worksheet = "Заказная",
            FirstRow = 7,
            RowsMaxCount = 999,
            VendorCodeColumn = 2,
            NameColumn = 3,
            UnitColumn = 5,
            QuantityColumn = 4,
            PositionColumn = 1,
        };

        public MainWindow()
        {
            InitializeComponent();
            //DataContext = this;

            simpleLogger = new SimpleLogger();
            Binding logBinding = new Binding("LogData") { Source = simpleLogger };
            textBoxLog.SetBinding(TextBox.TextProperty, logBinding);

            logAutoScrollBehaviour = new LogAutoScrollBehaviour();
            Interaction.GetBehaviors(textBoxLog).Add(logAutoScrollBehaviour);

            specFile = new SpecFile(simpleLogger) { Parameters = specFileParameters };
            Binding specFileBinding = new Binding("File") { Source = specFile };
            textBoxSpecPath.SetBinding(TextBox.TextProperty, specFileBinding);

            orderFile = new SpecFile(simpleLogger) { Parameters = orderFileParameters };
            Binding orderFileBinding = new Binding("File") { Source = orderFile };
            textBoxOrderPath.SetBinding(TextBox.TextProperty, orderFileBinding);

            specComparator = new SpecComparator(specFile, orderFile, simpleLogger);
            dataGridResult.ItemsSource = specComparator.Differences;
        }

        private void buttonGetSpecFromPath_Click(object sender, RoutedEventArgs e)
        {
            specFile.LoadItems();
            specFile.MergeDuplicates();
        }

        private void buttonGetSpecPath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Excel files (*.xlx*)|*.xls*" };
            if (openFileDialog.ShowDialog() == true)
            {
                specFile.File = openFileDialog.FileName;
            }
        }

        private void buttonGetOrderPath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Excel files (*.xlx*)|*.xls*" };
            if (openFileDialog.ShowDialog() == true)
            {
                orderFile.File = openFileDialog.FileName;
            }
        }

        private void buttonGetOrderFromPath_Click(object sender, RoutedEventArgs e)
        {
            orderFile.LoadItems();
            orderFile.MergeDuplicates();
        }

        private void buttonCompare_Click(object sender, RoutedEventArgs e)
        {
            specComparator.Compare();
            dataGridResult.Items.Refresh();
        }
    }
}

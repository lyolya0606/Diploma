using System;
using System.Collections.Generic;
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

namespace Diploma {
    /// <summary>
    /// Interaction logic for ShowKineticsWindow.xaml
    /// </summary>
    public partial class ShowKineticsWindow : Window {
        public ShowKineticsWindow() {
            InitializeComponent();
            FillTable();
        }

        private void SetUpColumns() {
            //var column = new DataGridTextColumn {
            //    Header = "Логарифм предэкспоненциального множителя (A)",
            //    Binding = new Binding("A")
            //};
            //kinetics_DataGrid.Columns.Add(column);
            var column = new DataGridTextColumn {
                Header = "Номер реакции",
                Binding = new Binding("Number")
            };
            kinetics_DataGrid.Columns.Add(column);
            column = new DataGridTextColumn {
                Header = "lnA",
                Binding = new Binding("AValue")
            };
            kinetics_DataGrid.Columns.Add(column);
            //column = new DataGridTextColumn {
            //    Header = "Энергия активации (E)",
            //    Binding = new Binding("E")
            //};
            //kinetics_DataGrid.Columns.Add(column);
            column = new DataGridTextColumn {
                Header = "E, кДж/моль",
                Binding = new Binding("EValue")
            };
            kinetics_DataGrid.Columns.Add(column);
        }

        private record DataForTable {
           // public required string A { get; set; }
            public required int Number { get; set; }
            public required string AValue { get; set; }
           // public required string E { get; set; }
            public required string EValue { get; set; }
        }

        private void FillTable() {
            SetUpColumns();
            List<DataForTable> data = new();
            DatabaseWork databaseWork = new();
            List<List<string>> aData = databaseWork.GetAFromDatabase();
            List<List<string>> eData = databaseWork.GetEFromDatabase();
            for (int i = 0; i < aData.Count; i++) {
              //  data.Add(new DataForTable { A = aData[i][0], AValue = aData[i][1], E = eData[i][0], EValue = eData[i][1] });
                data.Add(new DataForTable { Number = i + 1, AValue = aData[i][1], EValue = eData[i][1] });
            }

            kinetics_DataGrid.ItemsSource = data;
        }

    }
}

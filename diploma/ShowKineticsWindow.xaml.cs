using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Diploma {
    /// <summary>
    /// Interaction logic for ShowKineticsWindow.xaml
    /// </summary>
    public partial class ShowKineticsWindow : Window {

        private List<List<int>> _kineticMatrix = new();
        private List<string> _kineticComponents = new();
        private List<double> _AValue = new();
        private List<double> _EValue = new();
        private List<string> _reactions = new();
        private int _mainProductIndex;
        public ShowKineticsWindow(List<string> reactions, List<double> AValue, List<double> EValue, int mainProductIndex) {
            _AValue = AValue;
            _EValue = EValue;
            _reactions = reactions;
            _mainProductIndex = mainProductIndex;

            Kinetic kinetic = new(_reactions);
            List<string> elements = kinetic.GetAllElements();
            InitializeComponent();

            FillTable();
            FillMatrixTable();
            component_ComboBox.Items.Clear();
            //Kinetic kinetic = new(_reactions);
            //List<string> elements = kinetic.GetAllElements();
            foreach (string el in elements) {
                component_ComboBox.Items.Add(el);
            }
            component_ComboBox.SelectedIndex = _mainProductIndex;
            //foreach (string el in elements) {
            //    component_ComboBox.Items.Add(el);
            //}
            //component_ComboBox.SelectedIndex = _mainProductIndex;

        }

        public List<List<int>> GetKineticMatrix() {
            return _kineticMatrix;
        }

        public List<string> GetKineticComponents() {
            return _kineticComponents;
        }

        public List<double> GetAValue() {
            return _AValue;
        }

        public List<double> GetEValue() {
            return _EValue;
        }

        public List<string> GetAllReactions() {
            return _reactions;
        }

        public int GetMainProductIndex() {
            return _mainProductIndex;
        }

        //private static List<string> _elements = new() { "CCl" + '\u2082' + "=CClH", "HF", "CrF" + '\u2083', "[CCl" + '\u2082' + "=CClH * HF * CrF" + '\u2083' + "]",
        //    "CFCl" + '\u2082' + "-CClH" + '\u2082', "CFCl=CClH", "HCl", "[CFCl" + '\u2082' + " - CClH" + '\u2082' + " * CrF" + '\u2083' + "]", "[CFCl=CClH * HF * CrF" + '\u2083' + "]",
        //    "CF" + '\u2082' + "Cl-CClH" + '\u2082', "CrF" + '\u2082' + "Cl", "CCl" + '\u2083' + "-CClH" + '\u2082', "[CCl" + '\u2083' + "-CClH" + '\u2082' + " * CrF" + '\u2083' + "]",
        //    "[CF" + '\u2082' + "Cl-CClH" + '\u2082' + " * CrF" + '\u2083' + "]", "CF" + '\u2082' + "=CClH", "[CF" + '\u2082' + "=CClH" + '\u2082' + " * HF * CrF" + "]", "CF" + '\u2083' + "-CClH" + '\u2082',
        //    "CF" + '\u2083' + "-CFH" + '\u2082', "[2CF" + '\u2083' + "-CClH" + '\u2082' + " * CrF" + '\u2083' + "]", "CF" + '\u2083' + "-CH" + '\u2083', "CF" + '\u2083' + "-CCl" + '\u2082' + "H",
        //    "CF" + '\u2083' + "-CFClH", "CF" + '\u2083' + "-CF" + '\u2082' + "H"};

        private void SetUpColumns() {
            var column = new DataGridTextColumn {
                Header = "Номер реакции",
                Binding = new Binding("Number")
            };
            column.IsReadOnly = true;
            kinetics_DataGrid.Columns.Add(column);
            column = new DataGridTextColumn {
                Header = "Реакция",
                Binding = new Binding("Reaction")
            };
            column.IsReadOnly = false;
            kinetics_DataGrid.Columns.Add(column);
            column = new DataGridTextColumn {
                Header = "lnA",
                Binding = new Binding("AValue")
            };
            kinetics_DataGrid.Columns.Add(column);
            column = new DataGridTextColumn {
                Header = "E, кДж/моль",
                Binding = new Binding("EValue")
            };
            kinetics_DataGrid.Columns.Add(column);
        }

        private record DataForTable {
            public required int Number { get; set; }
            public required string Reaction { get; set; }
            public required double AValue { get; set; }
            public required double EValue { get; set; }
        }

        private void FillTable() {
            SetUpColumns();
            List<DataForTable> data = new();
            for (int i = 0; i < _AValue.Count; i++) {
              //  data.Add(new DataForTable { A = aData[i][0], AValue = aData[i][1], E = eData[i][0], EValue = eData[i][1] });
                data.Add(new DataForTable { Number = i + 1, Reaction = _reactions[i], AValue = _AValue[i], EValue = _EValue[i] });
            }

            kinetics_DataGrid.ItemsSource = data;
        }


        private List<string> GetReactions() {
            List<string> reactions = new();
            //NumberFormatInfo format = new NumberFormatInfo();
            //format.NumberGroupSeparator = ".";
            List<double> aVal = new();
            List<double> eVal = new();
            NumberFormatInfo format = new NumberFormatInfo();
            format.NumberGroupSeparator = ".";

            for (int i = 0; i < kinetics_DataGrid.Items.Count; i++) {
                try {
                    var x = kinetics_DataGrid.Columns[1].GetCellContent(kinetics_DataGrid.Items[i]) as TextBlock;
                    var a = kinetics_DataGrid.Columns[2].GetCellContent(kinetics_DataGrid.Items[i]) as TextBlock;
                    var e = kinetics_DataGrid.Columns[3].GetCellContent(kinetics_DataGrid.Items[i]) as TextBlock;

                    if (x.Text.ToString() == "") {
                        break;
                    }
                    reactions.Add(x.Text.ToString().Replace(" ", ""));
                    aVal.Add(double.Parse(a.Text.ToString(), format));
                    eVal.Add(double.Parse(e.Text.ToString(), format));
                } catch (Exception) {

                }

            }
            _EValue = eVal;
            _AValue = aVal;
            _reactions = reactions;
            return reactions;
        }

        private void count_Button_Click(object sender, RoutedEventArgs e) {
            _reactions = GetReactions();
            matrix_DataGrid.ItemsSource = null;
            matrix_DataGrid.Columns.Clear();
            FillMatrixTable();
            count_Button.Background = Brushes.White;

            component_ComboBox.Items.Clear();
            Kinetic kinetic = new(_reactions);
            List<string> elements = kinetic.GetAllElements();
            foreach (string el in elements) {
                component_ComboBox.Items.Add(el);
            }
            component_ComboBox.SelectedIndex = 0;
        }

        private void kinetics_DataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e) {
            SolidColorBrush customGreenBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xC5, 0xFC, 0xDE));
            count_Button.Background = customGreenBrush;
        }

        private void FillMatrixTable() {
            Kinetic kinetic = new(_reactions);
            var matrix = kinetic.GetMatrix();
            var allElements = kinetic.GetAllElements();
            allElements.Insert(0, "№");

            List<List<int>> _data = new List<List<int>>();
            int num = 1;
            foreach (var dict in matrix) {
                List<int> row = new List<int>();
                row.Add(num);
                foreach (var key in dict.Keys) {
                    row.Add(dict[key]);
                }
                _data.Add(row);
                num++;
            }

            matrix_DataGrid.ItemsSource = _data;

            for (int i = 0; i < allElements.Count; i++) {
                var column = new DataGridTextColumn {
                    Header = allElements[i],
                    Binding = new Binding($"[{i}]")
                };
                if (i == 0) {
                    column.IsReadOnly = true;
                } else {
                    column.IsReadOnly = false;
                }
                matrix_DataGrid.Columns.Add(column);
            }


        }

        private bool _isAccepted = false;

        private void accept_Button_Click(object sender, RoutedEventArgs e) {
            if (matrix_DataGrid.Items.Count == 0) {
                MessageBox.Show("Нажмите кнопку Рассчитать матрицу", "Ошибка!");
                return;
            }
            _isAccepted = true;
            ReadMatrix();
            _mainProductIndex = component_ComboBox.SelectedIndex;
            SaveReactionsToFile();
            Close();
        }

        private void kinetics_DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e) {
            //int newRowIndex = kinetics_DataGrid.Items.Count;
            //e.Row.Item = new DataForTable { Number = newRowIndex, Reaction = "", AValue = "0", EValue = "0" };
        }

        private void kinetics_DataGrid_InitializingNewItem(object sender, InitializingNewItemEventArgs e) {
            int rowCount = kinetics_DataGrid.Items.Count - 1;

            if (e.NewItem is DataForTable newItem) {
                newItem.Number = rowCount;
            }
        }

        private List<List<int>> ReadMatrix() {
            List<List<int>> matrix = new();
            int rows = matrix_DataGrid.Items.Count;
            int cols = matrix_DataGrid.Columns.Count;

            for (int row = 0; row < rows; row++) {
                List<int> rowOfInts = new();
                for (int col = 1; col < cols; col++) {
                    // Получаем значение ячейки
                    var cellValue = (matrix_DataGrid.Columns[col].GetCellContent(matrix_DataGrid.Items[row]) as TextBlock)?.Text;
                    rowOfInts.Add(int.Parse(cellValue));
                   
                }
                matrix.Add(rowOfInts);
            }

            _kineticMatrix = matrix;
            return matrix;
        }

        private void save_Button_Click(object sender, RoutedEventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;

            bool? result = saveFileDialog.ShowDialog();

            if (result == true) {
                using (var sr = new StreamWriter(saveFileDialog.FileName)) {

                    for (int i = 0; i < kinetics_DataGrid.Items.Count; i++) {
                        var x = kinetics_DataGrid.Columns[1].GetCellContent(kinetics_DataGrid.Items[i]) as TextBlock;
                        var a = kinetics_DataGrid.Columns[2].GetCellContent(kinetics_DataGrid.Items[i]) as TextBlock;
                        var ev = kinetics_DataGrid.Columns[3].GetCellContent(kinetics_DataGrid.Items[i]) as TextBlock;
                        

                        if (x.Text.ToString() == "") {
                            break;
                        }
                    
                        sr.WriteLine(x.Text + "@" + a.Text + "@" + ev.Text);

                    }

                }
                MessageBox.Show("Файл сохранен", "Сохранено!");
            } else {
                MessageBox.Show("Файл не сохранен", "Ошибка!");
            }
        }

        private void get_Button_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt files (*.txt)|*.txt";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            bool? result = openFileDialog.ShowDialog();

            if (result != true) {
                MessageBox.Show("Файл не был прочтен", "Ошибка!");
                return;
            }

            using (StreamReader streamReader = new StreamReader(openFileDialog.FileName)) {
                ReadDataFromFile(streamReader);
            }
        }

        private void ReadDataFromFile(StreamReader streamReader) {
            List<string> data = new();

            string line = streamReader.ReadLine();
            while (line != null) {
                data.Add(line);
                line = streamReader.ReadLine();
            }

            kinetics_DataGrid.ItemsSource = null;
            kinetics_DataGrid.Columns.Clear();
            SetUpColumns();
            List<DataForTable> dataForTable = new();
            NumberFormatInfo format = new NumberFormatInfo();
            format.NumberGroupSeparator = ".";


            int i = 1;
            foreach (var el in data) {
                var list = el.Split("@");
                try {
                    dataForTable.Add(new DataForTable { Number = i, Reaction = list[0], AValue = double.Parse(list[1], format), EValue = double.Parse(list[2], format) });
                } catch (Exception) {
                    MessageBox.Show("В файле ошибка", "Ошибка!");
                    return;
                }
                i++;

            }
            kinetics_DataGrid.ItemsSource = dataForTable;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            if (_isAccepted) {
                e.Cancel = false;
            } else {
                MessageBox.Show("Нажмите кнопку Принять", "Предупреждение!");
                e.Cancel = true;
                return;
            }

        }

        private void SaveReactionsToFile() {
            //using (FileStream file = new FileStream("reactions.txt", FileMode.Open)) {
            //    StreamWriter fileWriter = new StreamWriter(file);

            //    foreach (var reaction in _reactions) {
            //        fileWriter.WriteLine(reaction);
            //    }
            //    fileWriter.Close();
            //}
        }

    }
}

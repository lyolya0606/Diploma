using System;
using System.Collections.Generic;
using System.Linq;
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
using LiveCharts.Defaults;
using LiveCharts;
using LiveCharts.Wpf;
using static Python.Runtime.TypeSpec;
using System.IO;
using System.Globalization;


namespace Diploma {
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window {
        DatabaseWork _databaseWork;
        bool isFirstEnter;
        private const int COUNT_OF_ELEMENTS = 23;
        private const int COUNT_OF_REACTIONS = 21;
        private List<List<double>> _concentrations = new();

        private int _mainProductIndex = 17;

        private List<int> _chosenElements = new();

        private List<List<int>> _kineticMatrix = new();
        private List<string> _kineticComponents = new();
        //private ChooseElementWindow _chooseElementWindow = new() { 17 };

        private static List<string> _elements = new() { "CCl" + '\u2082' + "=CClH", "HF", "CrF" + '\u2083', "[CCl" + '\u2082' + "=CClH * HF * CrF" + '\u2083' + "]",
            "CFCl" + '\u2082' + "-CClH" + '\u2082', "CFCl=CClH", "HCl", "[CFCl" + '\u2082' + " - CClH" + '\u2082' + " * CrF" + '\u2083' + "]", "[CFCl=CClH * HF * CrF" + '\u2083' + "]",
            "CF" + '\u2082' + "Cl-CClH" + '\u2082', "CrF" + '\u2082' + "Cl", "CCl" + '\u2083' + "-CClH" + '\u2082', "[CCl" + '\u2083' + "-CClH" + '\u2082' + " * CrF" + '\u2083' + "]",
            "[CF" + '\u2082' + "Cl-CClH" + '\u2082' + " * CrF" + '\u2083' + "]", "CF" + '\u2082' + "=CClH", "[CF" + '\u2082' + "=CClH" + '\u2082' + " * HF * CrF" + "]", "CF" + '\u2083' + "-CClH" + '\u2082',
            "CF" + '\u2083' + "-CFH" + '\u2082', "[2CF" + '\u2083' + "-CClH" + '\u2082' + " * CrF" + '\u2083' + "]", "CF" + '\u2083' + "-CH" + '\u2083', "CF" + '\u2083' + "-CCl" + '\u2082' + "H",
            "CF" + '\u2083' + "-CFClH", "CF" + '\u2083' + "-CF" + '\u2082' + "H"};

        private static List<string> _reactions = new() {
            _elements[0] + " + " + _elements[1] + " + " + _elements[2] + " -> " + _elements[3],
            _elements[3] + " -> " + _elements[4] + " + " + _elements[2],
            _elements[4] + " + " + _elements[2] + " -> " + _elements[7],
            _elements[7] + " -> " + _elements[5] + " + " + _elements[6] + " + " + _elements[2],
            _elements[5] + " + " + _elements[1] + " + " + _elements[2] + " -> " + _elements[8],
            _elements[8] + " -> " + _elements[9] + " + " + _elements[2],
            _elements[4] + " + " + _elements[2] + " -> " + _elements[9] + " + " + _elements[10],
            _elements[1] + " + " + _elements[10] + " -> " + _elements[6] + " + " + _elements[2],
            _elements[4] + " + " + _elements[10] + " -> " + _elements[11] + " + " + _elements[2],
            _elements[11] + " + " + _elements[2] + " -> " + _elements[12],
            _elements[12] + " -> " + _elements[0] + " + " + _elements[6] + " + " + _elements[2],
            _elements[9] + " + " + _elements[2] + " -> " + _elements[13],
            _elements[13] + " -> " + _elements[14] + " + " + _elements[6] + " + " + _elements[2],
            _elements[14] + " + " + _elements[1] + " + " + _elements[2] + " -> " + _elements[15],
            _elements[15] + " -> " + _elements[16] + " + " + _elements[2],
            _elements[9] + " + " + _elements[2] + " -> " + _elements[16] + " + " + _elements[10],
            _elements[16] + " + " + _elements[2] + " -> " + _elements[17] + " + " + _elements[10],
            "2" + _elements[16] + " + " + _elements[2] + " -> " + _elements[18],
            _elements[18] + " -> " + _elements[19] + " + " + _elements[20] + " + " + _elements[2],
            _elements[20] + " + " + _elements[2] + " -> " + _elements[21] + " + " + _elements[10],
            _elements[21] + " + " + _elements[2] + " -> " + _elements[22] + " + " + _elements[10],
        };

        private bool _isBackButton = false;
        private bool _ifFirstEnter = true;
        private List<double> _aValues = new();
        private List<double> _eValues = new();
        private string _method;

        private Dictionary<string, string> _dictMethods = new Dictionary<string, string>() {
            { "Метод Гира", "BDF"},
            { "Метод Адамса", "LSODA"},
            { "Неявный метод Рунге-Кутта третьего порядка точности", "Radau"},
        };

        public UserWindow() {
            InitializeComponent();
            //FillTable();
            //concChart.AxisX.Clear();
            //concChart.AxisY.Clear();
            //GetValues();
            FirstEntering();

            
            Kinetic kinetic = new(_reactions);

            _kineticMatrix = kinetic.GetBiMatrix();
            _kineticComponents = kinetic.GetAllElements();
            DatabaseWork databaseWork = new DatabaseWork();
            _aValues = databaseWork.GetAValues();
            _eValues = databaseWork.GetEValues();
            _chosenElements.Add(_mainProductIndex);

        }

        private void GetValues() {
            StreamReader sr = new StreamReader(@"..\..\..\ImportantFiles\method.txt");
            string line = sr.ReadLine();
            sr.Close();
            _method = _dictMethods[line];
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e) {
            var regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void SetUpColumns() {
            var column = new DataGridTextColumn {
                Header = "Вещество",
                Binding = new Binding("Element")
            };
            concDataGrid.Columns.Add(column);
            column = new DataGridTextColumn {
                Header = "Концентрация, моль/л",
                Binding = new Binding("Concentration")
            };
            concDataGrid.Columns.Add(column);
        }

        private record DataForTable {
            public required string Element { get; set; }
            public required double Concentration { get; set; }
        }


        private void FillTable() {
            SetUpColumns();
            List<DataForTable> data = new();

            data.Add(new DataForTable { Element = "CCl" + '\u2082' + "=CClH", Concentration = 0.01 });
            data.Add(new DataForTable { Element = "HF", Concentration = 0.01493 });
            data.Add(new DataForTable { Element = "CrF" + '\u2083', Concentration = 0.01 });
            data.Add(new DataForTable { Element = "[CCl" + '\u2082' + "=CClH * HF * CrF" + '\u2083' + "]", Concentration = 0.01 });
            data.Add(new DataForTable { Element = "CFCl" + '\u2082' + "-CClH" + '\u2082', Concentration = 0.01 });
            data.Add(new DataForTable { Element = "CFCl=CClH", Concentration = 0.01 });
            data.Add(new DataForTable { Element = "HCl", Concentration = 0.000007 });
            data.Add(new DataForTable { Element = "[CFCl" + '\u2082' + "-CClH" + '\u2082' + " * CrF" + '\u2083' + "]", Concentration = 0.01 });
            data.Add(new DataForTable { Element = "[CFCl=CClH * HF * CrF" + '\u2083' + "]", Concentration = 0.01 });
            data.Add(new DataForTable { Element = "CF" + '\u2082' + "Cl-CClH" + '\u2082', Concentration = 0.01 });
            data.Add(new DataForTable { Element = "CrF" + '\u2082' + "Cl", Concentration = 0.01 });
            data.Add(new DataForTable { Element = "CCl" + '\u2083' + "-CClH" + '\u2082', Concentration = 0.01 });
            data.Add(new DataForTable { Element = "[CCl" + '\u2083' + "-CClH" + '\u2082' +  " * CrF" + '\u2083' + "]", Concentration = 0.01 });
            data.Add(new DataForTable { Element = "[CF" + '\u2082' + "Cl-CClH" + '\u2082' + " * CrF" + '\u2083' + "]", Concentration = 0.01 });
            data.Add(new DataForTable { Element = "CF" + '\u2082' + "=CClH", Concentration = 0.00001 });
            data.Add(new DataForTable { Element = "[CF" + '\u2082' + "=CClH" + '\u2082' + " * HF * CrF" + '\u2083' + "]", Concentration = 0.01 });
            data.Add(new DataForTable { Element = "CF" + '\u2083' + "-CClH" + '\u2082', Concentration = 0.003733 });
            data.Add(new DataForTable { Element = "CF" + '\u2083' + "-CFH" + '\u2082', Concentration = 0 });
            data.Add(new DataForTable { Element = "[2CF" + '\u2083' + "-CClH" + '\u2082' + " * CrF" + '\u2083' + "]", Concentration = 0.01 });
            data.Add(new DataForTable { Element = "CF" + '\u2083' + "-CH" + '\u2083', Concentration = 0.00003 });
            data.Add(new DataForTable { Element = "CF" + '\u2083' + "-CCl" + '\u2082' + "H", Concentration = 0.01 });
            data.Add(new DataForTable { Element = "CF" + '\u2083' + "-CFClH", Concentration = 0.01 });
            data.Add(new DataForTable { Element = "CF" + '\u2083' + "-CF" + '\u2082' + "H", Concentration = 0.0000053 });
            concDataGrid.ItemsSource = data;
        }

        private List<double> GetStartConcentration() {
            List<double> startConcentrations = new();
            NumberFormatInfo format = new NumberFormatInfo();
            format.NumberGroupSeparator = ".";

            for (int i = 0; i < _kineticMatrix[0].Count; i++) {
                try {
                    var x = concDataGrid.Columns[1].GetCellContent(concDataGrid.Items[i]) as TextBlock;
         
                    startConcentrations.Add(double.Parse(x.Text, format));
                } catch (Exception)  {
                    MessageBox.Show("You entered bad data", "Warning!");
                } 

            }
            return startConcentrations;
        }

        private List<double> GetReactionSpeed() {
            List<double> reactionSpeed = new();

            for (int i = 0; i < COUNT_OF_REACTIONS; i++) {
                reactionSpeed.Add(2);
            }
            return reactionSpeed;
        }

        private double GetContactTime() {
            return double.Parse(timeTextBox.Text);
        }
        private double GetTemperature() {
            return double.Parse(temperatureTextBox.Text);
        }

        private async Task DoWorkAsync() {
            List<double> startConcentrations = new();
            try {
                startConcentrations = GetStartConcentration();
            } catch (Exception) {
                return;
            }
            //List<double> reactionSpeed = GetReactionSpeed();
            double contactTime = GetContactTime();
            double temperature = GetTemperature();


            await Task.Run(() => {
                //PythonMathModel pythonMathModel = new PythonMathModel(startConcentrations, _aValues, _eValues, temperature, contactTime, _method, _kineticMatrix);
                ////PythonMathModel pythonMathModel = new PythonMathModel(startConcentrations, _aValues, _eValues, temperature, contactTime, _method);
                //_concentrations = pythonMathModel.RunScript();
                PythonMathExe pythonMathExe = new(startConcentrations, _aValues, _eValues, temperature, contactTime, _kineticMatrix);
                pythonMathExe.PythonWork();
                _concentrations = pythonMathExe.GetPythonOutput();

            });
            
        }


        private async  void StartWorking() {
            finishConcLabel.Content = "-";
            calculateButton.Content = "Подсчет...";
            progressBar.IsIndeterminate = true;

            await DoWorkAsync();

            DrawCharts();
            calculateButton.Content = "Рассчитать";
            showTableButton.IsEnabled = true;

            finishConcLabel.Content = Math.Round(_concentrations[_mainProductIndex + 1][_concentrations[_mainProductIndex + 1].Count - 1], 4);
            progressBar.IsIndeterminate = false;
            progressBar.Value = 0;


        }

        // TODO: -0 wtf
        private void calculateButton_Click(object sender, RoutedEventArgs e) {
            StartWorking();
        }

        public Func<ChartPoint, string> PointLabel { get; set; }
        public SeriesCollection SeriesCollectionConc { get; set; }
        Func<double, string> FormatFunc = (x) => string.Format("{0:0.0000}", x);
        Func<double, string> FormatFuncX = (x) => string.Format("{0:0.00}", x);

        private void showTableButton_Click(object sender, RoutedEventArgs e) {
            TableWindow tableWindow = new TableWindow(_concentrations, _kineticComponents);
            tableWindow.ShowDialog();
        }

        private void chooseButtonClick(object sender, RoutedEventArgs e) {
            if (_ifFirstEnter) {
                List<int> firstElements = new() { _mainProductIndex };
                ChooseElementWindow chooseElementWindow = new(firstElements, _kineticComponents);
                chooseElementWindow.ShowDialog();
                _chosenElements = chooseElementWindow.GetChosenElements();
                _ifFirstEnter = false;
            } else {
                ChooseElementWindow chooseElementWindow = new(_chosenElements, _kineticComponents);
                chooseElementWindow.ShowDialog();
                _chosenElements = chooseElementWindow.GetChosenElements();
            }

            //_chooseElementWindow.ShowDialog();
            //_chosenElements = _chooseElementWindow.GetChosenElements();

        }


        private Brush[] _colors = { Brushes.Blue, Brushes.Red, Brushes.Purple, Brushes.Pink, Brushes.Orange, Brushes.Green, Brushes.Gold, Brushes.Gray,
        Brushes.GreenYellow, Brushes.SkyBlue, Brushes.DarkOliveGreen, Brushes.Black, Brushes.Brown, Brushes.DarkBlue, Brushes.HotPink, Brushes.Lime, Brushes.Plum, 
        Brushes.Tomato, Brushes.Cyan, Brushes.DarkViolet, Brushes.DarkMagenta, Brushes.Peru, Brushes.Maroon };

        private void back_ButtonClick(object sender, RoutedEventArgs e) {
            _isBackButton = true;
            new LoginWindow().Show();
            Close();
        }

        private void DrawCharts() {
            concChart.DataContext = null;
            concChart.AxisX.Clear();
            concChart.AxisY.Clear();
            
            concChart.AxisX.Add(new Axis { Title = "Время контакта, с", FontSize = 16, MinValue = 0, Foreground = Brushes.Black });
            concChart.AxisY.Add(new Axis { Title = "Концентрация, моль/л", LabelFormatter = FormatFunc, FontSize = 16, MinValue = 0, Foreground = Brushes.Black });
            PointLabel = chartPoint => $"{"Время контакта"}: {Math.Round(chartPoint.X, 4)}, {"Концентрация"}: {Math.Round(chartPoint.Y, 4)}";
            List<LineSeries> lines = new();
            SeriesCollectionConc = new SeriesCollection();
            int color = 0;

            foreach (var item in _chosenElements) {
                var points = new ChartValues<ObservablePoint>();
                for (var i = 0; i < _concentrations[0].Count; i++)
                    points.Add(new ObservablePoint {
                        X = Math.Round(_concentrations[0][i], 4),
                        Y = _concentrations[item + 1][i]
                        //Y = _concentrations[1][i]
                    });

                LineSeries line = new LineSeries {
                    Values = new ChartValues<ObservablePoint>(points),
                    PointGeometrySize = 8,
                    Fill = Brushes.Transparent,
                    LabelPoint = PointLabel,
                    Title = _kineticComponents[item],
                    Stroke = _colors[color],
               
                };
                lines.Add(line);
                color++;

                //SeriesCollectionConc.Add(line);


            }
            SeriesCollectionConc.AddRange(lines);
            concChart.DataContext = this;
            

            //var points = new ChartValues<ObservablePoint>();

            //for (var i = 0; i < _concentrations[0].Count; i++)
            //    points.Add(new ObservablePoint {
            //        X = _concentrations[0][i],
            //        Y = _concentrations[18][i]
            //    });
            //PointLabel = chartPoint => $"{"Время контакта"}: {Math.Round(chartPoint.X, 4)}, {"Концентрация"}: {Math.Round(chartPoint.Y, 4)}";


            //SeriesCollectionConc = new SeriesCollection {
            //    new LineSeries {
            //        Values = new ChartValues<ObservablePoint>(points),
            //        PointGeometrySize = 10,
            //        Fill = Brushes.Transparent,
            //        LabelPoint = PointLabel,
            //        Title = "sadfsgthyjujyhtgre"
            //    }
            //};


        }



       

        private void FirstEntering() {
            isFirstEnter = true;
            _databaseWork = new DatabaseWork();
            List<string> marks = _databaseWork.GetMarks();

            foreach (string mark in marks) {
                marks_ComboBox.Items.Add(mark);
            }

            marks_ComboBox.SelectedItem = marks[0];

            string name = _databaseWork.GetNameFreon(marks[0]);
            name_label.Content = name;

            string formula = _databaseWork.GetFormulaFreon(marks[0]);
            formula_label.Content = formula;

            string area = _databaseWork.GetArea(marks[0]);
           // area_label.Text = area;
            area_label.Content = area;

            string modeReactor = _databaseWork.GetModes(marks[0]);
            reactorModes_label.Content = modeReactor;

            string scheme = _databaseWork.GetSchemeFreon(marks[0]);
            scheme_image.Source = new BitmapImage(new Uri(scheme, UriKind.Relative));




            //List<Tuple<string, string>> equipment = _databaseWork.GetEquipment(marks[0]);
            //FillTableEquip(equipment);
            Dictionary<string, string> equipmentFull = new();

            List<Tuple<string, string>> equipmentStage = [];
            List<int> sequenceStage = _databaseWork.GetSequenceStages(marks[0]);
            foreach (int i in  sequenceStage) {
                equipmentStage.Add(new Tuple<string, string>(_databaseWork.GetStage(i), _databaseWork.GetStageEquipment(i)));
                Dictionary<string, string> equipmentNotFull = _databaseWork.GetEquipment(i);
                foreach (string key in equipmentNotFull.Keys) {
                    if (!equipmentFull.ContainsKey(key)) {
                        equipmentFull.Add(key, equipmentNotFull[key]);
                    }
                }
            }
            FillTableStage(equipmentStage);
            FillTableEquip(equipmentFull);

        }

        private void SetUpColumnsEquip() {
            var column = new DataGridTextColumn {
                Header = "Обозначение",
                Binding = new Binding("Designation")
            };
            designation_DataGrid.Columns.Add(column);
            column = new DataGridTextColumn {
                Header = "Оборудование",
                Binding = new Binding("Equipment")
            };
            designation_DataGrid.Columns.Add(column);
        }

        private record DataForTableEquip {
            public required string Designation { get; set; }
            public required string Equipment { get; set; }
        }

        private void SetUpColumnsStage() {
            var column = new DataGridTextColumn {
                Header = "Стадия",
                Binding = new Binding("Stage")
            };
            stage_DataGrid.Columns.Add(column);
            column = new DataGridTextColumn {
                Header = "Оборудование для стадии",
                Binding = new Binding("Equipment")
            };
            stage_DataGrid.Columns.Add(column);
        }

        private record DataForTableStage {
            public required string Stage { get; set; }
            public required string Equipment { get; set; }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            TabControl tabControl = (TabControl)sender;
            int selectedIndex = tabControl.SelectedIndex;

            if (selectedIndex == 0) {
                //FirstEntering();
            } else if (selectedIndex == 1 && _isFirstEnterMath) {
                _isFirstEnterMath = false;
                FillTable();
                concChart.AxisX.Add(new Axis { Title = "Время контакта, с", LabelFormatter = FormatFuncX, FontSize = 16, MinValue = 0, Foreground = Brushes.Black });
                concChart.AxisY.Add(new Axis { Title = "Концентрация, моль/л", LabelFormatter = FormatFunc, FontSize = 16, MinValue = 0, Foreground = Brushes.Black });
                //concChart.AxisX.Clear();
                //concChart.AxisY.Clear();
                concChart.DataContext = this;
                GetValues();
            }
        }
        bool _isFirstEnterMath = true;
        //bool isFirstEnter;
        private void marks_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (isFirstEnter) {
                isFirstEnter = false;
                return;
            }
            //designation_DataGrid.Items.Clear();
            designation_DataGrid.ItemsSource = null;
            designation_DataGrid.Columns.Clear();

            stage_DataGrid.ItemsSource = null;
            stage_DataGrid.Columns.Clear();
            //designation_DataGrid.Items.Refresh();

            string mark = (string)marks_ComboBox.SelectedItem;

            string name = _databaseWork.GetNameFreon(mark);
            name_label.Content = name;

            string formula = _databaseWork.GetFormulaFreon(mark);
            formula_label.Content = formula;

            string area = _databaseWork.GetArea(mark);
            // area_label.Text = area;
            area_label.Content = area;

            string modeReactor = _databaseWork.GetModes(mark);
            reactorModes_label.Content = modeReactor;

            string scheme = _databaseWork.GetSchemeFreon(mark);
            scheme_image.Source = new BitmapImage(new Uri(scheme, UriKind.Relative));

            //List<Tuple<string, string>> equipment = _databaseWork.GetEquipment(mark);
            //FillTableEquip(equipment);
            Dictionary<string, string> equipmentFull = new();

            List<Tuple<string, string>> equipmentStage = [];
            List<int> sequenceStage = _databaseWork.GetSequenceStages(mark);
            foreach (int i in sequenceStage) {
                equipmentStage.Add(new Tuple<string, string>(_databaseWork.GetStage(i), _databaseWork.GetStageEquipment(i)));
                Dictionary<string, string> equipmentNotFull = _databaseWork.GetEquipment(i);
                foreach (string key in equipmentNotFull.Keys) {
                    if (!equipmentFull.ContainsKey(key)) {
                        equipmentFull.Add(key, equipmentNotFull[key]);
                    }
                }
            }
            FillTableStage(equipmentStage);
            FillTableEquip(equipmentFull);
        }


        private void FillTableEquip(List<Tuple<string, string>> designations) {
            SetUpColumnsEquip();
            List<DataForTableEquip> data = new();
            for (int i = 0; i < designations.Count; i++) {
                data.Add(new DataForTableEquip {
                    Designation = designations[i].Item2,
                    Equipment = designations[i].Item1
                });
            }

            designation_DataGrid.ItemsSource = data;
        }


        private void FillTableEquip(Dictionary<string, string> designations) {
            SetUpColumnsEquip();
            List<DataForTableEquip> data = new();
            foreach (string key in designations.Keys) {
                data.Add(new DataForTableEquip {
                    Designation = key,
                    Equipment = designations[key],
                });
            }

            designation_DataGrid.ItemsSource = data;
        }

        private void kineticsButton_Click(object sender, RoutedEventArgs e) {
            ShowKineticsWindow showKineticsWindow = new(_reactions, _aValues, _eValues, _mainProductIndex);
            showKineticsWindow.ShowDialog();

            //Kinetic kinetic = new(_reactions);

            //_kineticMatrix = kinetic.GetBiMatrix();
            //_kineticComponents = kinetic.GetComponents();


            concDataGrid.ItemsSource = null;
            concDataGrid.Columns.Clear();


            _aValues = showKineticsWindow.GetAValue();
            _eValues = showKineticsWindow.GetEValue();
            _reactions = showKineticsWindow.GetAllReactions();
            _kineticMatrix = showKineticsWindow.GetKineticMatrix();
            _mainProductIndex = showKineticsWindow.GetMainProductIndex();
            _chosenElements = new() { _mainProductIndex };

            Kinetic kinetic = new(_reactions);
            _kineticComponents = kinetic.GetAllElements();
            //_kineticComponents.Remove("№");
            _kineticMatrix = kinetic.GetBiMatrix();

            concDataGrid.ItemsSource = null;
            concDataGrid.Columns.Clear();

            SetUpColumns();
            List<DataForTable> data = new();
            foreach (var component in _kineticComponents) {
                if (data.Count == _mainProductIndex) {
                    data.Add(new DataForTable { Element = component, Concentration = 0 });
                } else {
                    data.Add(new DataForTable { Element = component, Concentration = 0.01 });
                }
            }
            concDataGrid.ItemsSource = data;

        }

        private void reactorButton_Click(object sender, RoutedEventArgs e) {
            ReactorWindow reactorWindow = new();
            reactorWindow.ShowDialog();
        }

        private void FillTableStage(List<Tuple<string, string>> equipmentStage) {
            SetUpColumnsStage();
            List<DataForTableStage> data = new();
            for (int i = 0; i < equipmentStage.Count; i++) {
                data.Add(new DataForTableStage {
                    Stage = equipmentStage[i].Item1,
                    Equipment = equipmentStage[i].Item2.Substring(0, equipmentStage[i].Item2.Length - 2)
                });
            }

           stage_DataGrid.ItemsSource = data;
        }



        private void stage_DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            int selectedRow = stage_DataGrid.SelectedIndex;
            if (selectedRow == -1) { return; }
            string stageName = (selectedRow + 1).ToString();

            if (marks_ComboBox.SelectedIndex == 0) {
                string path = "..\\..\\..\\schemes\\" + stageName + "_scheme_freon134a.png";
                scheme_image.Source = new BitmapImage(new Uri(path, UriKind.Relative));
                // scheme_image.Source = new BitmapImage(new Uri(@"..\..\..\schemes\1_scheme_freon134a.png", UriKind.Relative));
            } else if (marks_ComboBox.SelectedIndex == 1) {
                string path = "..\\..\\..\\schemes\\" + stageName + "_scheme_freon125.png";
                scheme_image.Source = new BitmapImage(new Uri(path, UriKind.Relative));
            } else if (marks_ComboBox.SelectedIndex == 2) {
                string path = "..\\..\\..\\schemes\\" + stageName + "_scheme_freon227ea.png";
                scheme_image.Source = new BitmapImage(new Uri(path, UriKind.Relative));
            } else {
                string path = "..\\..\\..\\schemes\\" + stageName + "_scheme_freon23.png";
                scheme_image.Source = new BitmapImage(new Uri(path, UriKind.Relative));
            }
        }


        private void scheme_image_MouseDown(object sender, MouseButtonEventArgs e) {
            if (marks_ComboBox.SelectedIndex == 0) {
                scheme_image.Source = new BitmapImage(new Uri(@"..\..\..\schemes\scheme_freon134a.png", UriKind.Relative));
            } else if (marks_ComboBox.SelectedIndex == 1) {
                scheme_image.Source = new BitmapImage(new Uri(@"..\..\..\schemes\scheme_freon125.png", UriKind.Relative));
            } else if (marks_ComboBox.SelectedIndex == 2) {
                scheme_image.Source = new BitmapImage(new Uri(@"..\..\..\schemes\scheme_freon227ea.png", UriKind.Relative));
            } else {
                scheme_image.Source = new BitmapImage(new Uri(@"..\..\..\schemes\scheme_freon23.png", UriKind.Relative));
            }
        }


    }
}

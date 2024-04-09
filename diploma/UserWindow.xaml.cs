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
        private const int COUNT_OF_ELEMENTS = 23;
        private const int COUNT_OF_REACTIONS = 21;
        private List<List<double>> _concentrations = new();
        private List<int> _chosenElements = new() { 17 };
        //private ChooseElementWindow _chooseElementWindow = new() { 17 };
        private List<string> _elements = new() { "CCl" + '\u2082' + "=CClH", "HF", "CrF" + '\u2083', "[CCl" + '\u2082' + "=CClH * HF * CrF" + '\u2083' + "]",
            "CFCl" + '\u2082' + "-CClH" + '\u2082', "CFCl=CClH", "HCl", "[CFCl" + '\u2082' + " - CClH" + '\u2082' + " * CrF" + '\u2083' + "]", "[CFCl=CClH * HF * CrF" + '\u2083' + "]",
            "CF" + '\u2082' + "Cl-CClH" + '\u2082', "CrF" + '\u2082' + "Cl", "CCl" + '\u2083' + "-CClH" + '\u2082', "[CCl" + '\u2083' + "-CClH" + '\u2082' + " * CrF" + '\u2083' + "]",
            "[CF" + '\u2082' + "Cl-CClH" + '\u2082' + " * CrF" + '\u2083' + "]", "CF" + '\u2082' + "=CClH", "[CF" + '\u2082' + "=CClH" + '\u2082' + " * HF * CrF", "CF" + '\u2083' + "-CClH" + '\u2082',
            "CF" + '\u2083' + "-CFH" + '\u2082', "[2CF" + '\u2083' + "-CClH" + '\u2082' + " * CrF" + '\u2083' + "]", "CF" + '\u2083' + "-CH" + '\u2083', "CF" + '\u2083' + "-CCl" + '\u2082' + "H",
            "CF" + '\u2083' + "-CFClH", "CF" + '\u2083' + "-CF" + '\u2082' + "H"};
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
            FillTable();
            concChart.AxisX.Clear();
            concChart.AxisY.Clear();
            GetValues();
        }

        private void GetValues() {
            DatabaseWork databaseWork = new DatabaseWork();
            _aValues = databaseWork.GetAValues();
            _eValues = databaseWork.GetEValues();

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
            data.Add(new DataForTable { Element = "HF", Concentration = 0.01 });
            data.Add(new DataForTable { Element = "CrF" + '\u2083', Concentration = 0.01 });
            data.Add(new DataForTable { Element = "[CCl" + '\u2082' + "=CClH * HF * CrF" + '\u2083' + "]", Concentration = 0.01 });
            data.Add(new DataForTable { Element = "CFCl" + '\u2082' + "-CClH" + '\u2082', Concentration = 0.01 });
            data.Add(new DataForTable { Element = "CFCl=CClH", Concentration = 0.01 });
            data.Add(new DataForTable { Element = "HCl", Concentration = 0.01 });
            data.Add(new DataForTable { Element = "[CFCl" + '\u2082' + "-CClH" + '\u2082' + " * CrF" + '\u2083' + "]", Concentration = 0.01 });
            data.Add(new DataForTable { Element = "[CFCl=CClH * HF * CrF" + '\u2083' + "]", Concentration = 0.01 });
            data.Add(new DataForTable { Element = "CF" + '\u2082' + "Cl-CClH" + '\u2082', Concentration = 0.01 });
            data.Add(new DataForTable { Element = "CrF" + '\u2082' + "Cl", Concentration = 0.01 });
            data.Add(new DataForTable { Element = "CCl" + '\u2083' + "-CClH" + '\u2082', Concentration = 0.01 });
            data.Add(new DataForTable { Element = "[CCl" + '\u2083' + "-CClH" + '\u2082' +  " * CrF" + '\u2083' + "]", Concentration = 0.01 });
            data.Add(new DataForTable { Element = "[CF" + '\u2082' + "Cl-CClH" + '\u2082' + " * CrF" + '\u2083' + "]", Concentration = 0.01 });
            data.Add(new DataForTable { Element = "CF" + '\u2082' + "=CClH", Concentration = 0.01 });
            data.Add(new DataForTable { Element = "[CF" + '\u2082' + "=CClH" + '\u2082' + " * HF * CrF" + '\u2083' + "]", Concentration = 0.01 });
            data.Add(new DataForTable { Element = "CF" + '\u2083' + "-CClH" + '\u2082', Concentration = 0.04 });
            data.Add(new DataForTable { Element = "CF" + '\u2083' + "-CFH" + '\u2082', Concentration = 0 });
            data.Add(new DataForTable { Element = "[2CF" + '\u2083' + "-CClH" + '\u2082' + " * CrF" + '\u2083' + "]", Concentration = 0.01 });
            data.Add(new DataForTable { Element = "CF" + '\u2083' + "-CH" + '\u2083', Concentration = 0.01 });
            data.Add(new DataForTable { Element = "CF" + '\u2083' + "-CCl" + '\u2082' + "H", Concentration = 0.01 });
            data.Add(new DataForTable { Element = "CF" + '\u2083' + "-CFClH", Concentration = 0.01 });
            data.Add(new DataForTable { Element = "CF" + '\u2083' + "-CF" + '\u2082' + "H", Concentration = 0.01 });
            concDataGrid.ItemsSource = data;
        }

        private List<double> GetStartConcentration() {
            List<double> startConcentrations = new();
            NumberFormatInfo format = new NumberFormatInfo();
            format.NumberGroupSeparator = ".";

            for (int i = 0; i < COUNT_OF_ELEMENTS; i++) {
                try {
                    var x = concDataGrid.Columns[1].GetCellContent(concDataGrid.Items[i]) as TextBlock;
         
                    startConcentrations.Add(double.Parse(x.Text, format));
                } catch (Exception)  {
                    
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
                PythonMathModel pythonMathModel = new PythonMathModel(startConcentrations, _aValues, _eValues, temperature, contactTime, _method);
                _concentrations = pythonMathModel.RunScript();

            });
            
        }

        private async void StartWorking() {
            finishConcLabel.Content = "-";
            calculateButton.Content = "Подсчет...";
            progressBar.IsIndeterminate = true;

            await DoWorkAsync();


            DrawCharts();
            calculateButton.Content = "Рассчитать";
            showTableButton.IsEnabled = true;
            //finishConcLabel.Content = _concentrations[18][_concentrations[18].Count - 1];
            finishConcLabel.Content = Math.Round(_concentrations[18][_concentrations[18].Count - 1], 4);
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

        private void showTableButton_Click(object sender, RoutedEventArgs e) {
            TableWindow tableWindow = new TableWindow(_concentrations);
            tableWindow.ShowDialog();
        }

        private void chooseButtonClick(object sender, RoutedEventArgs e) {
            if (_ifFirstEnter) {
                List<int> firstElements = new() { 17 };
                ChooseElementWindow chooseElementWindow = new(firstElements);
                chooseElementWindow.ShowDialog();
                _chosenElements = chooseElementWindow.GetChosenElements();
                _ifFirstEnter = false;
            } else {
                ChooseElementWindow chooseElementWindow = new(_chosenElements);
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
            
            concChart.AxisX.Add(new Axis { Title = "Время контакта, с", FontSize = 15 });
            concChart.AxisY.Add(new Axis { Title = "Концентрация, моль/л", LabelFormatter = FormatFunc, FontSize = 15, MinValue = 0 });
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
                    });

                LineSeries line = new LineSeries {
                    Values = new ChartValues<ObservablePoint>(points),
                    PointGeometrySize = 8,
                    Fill = Brushes.Transparent,
                    LabelPoint = PointLabel,
                    Title = _elements[item],
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
    }
}

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
    /// Interaction logic for ChooseElementWindow.xaml
    /// </summary>
    public partial class ChooseElementWindow : Window {
        private const int COUNT_OF_ELEMENTS = 23;
        private List<CheckBox> _checkBoxes = new();
        private List<int> _chosenElements = new();
        private bool _firstEnter = true;
        private bool _fillAll = true;

        public ChooseElementWindow(List<int> chosenElements) {
            InitializeComponent();
            FillCheckBoxes();
            _chosenElements = chosenElements;

            foreach (int element in _chosenElements) {
                _checkBoxes[element].IsChecked = true;
            }
        }

        private void FillCheckBoxes() {
            List<string> elements = new List<string>() { "CCl" + '\u2082' + "=CClH", "HF", "CrF" + '\u2083', "[CCl" + '\u2082' + "=CClH * HF * CrF" + '\u2083' + "]",
            "CFCl" + '\u2082' + "-CClH" + '\u2082', "CFCl=CClH", "HCl", "[CFCl" + '\u2082' + " - CClH" + '\u2082' + " * CrF" + '\u2083' + "]", "[CFCl=CClH * HF * CrF" + '\u2083' + "]",
            "CF" + '\u2082' + "Cl-CClH" + '\u2082', "CrF" + '\u2082' + "Cl", "CCl" + '\u2083' + "-CClH" + '\u2082', "[CCl" + '\u2083' + "-CClH" + '\u2082' + " * CrF" + '\u2083' + "]",
            "[CF" + '\u2082' + "Cl-CClH" + '\u2082' + " * CrF" + '\u2083' + "]", "CF" + '\u2082' + "=CClH", "[CF" + '\u2082' + "=CClH" + '\u2082' + " * HF * CrF", "CF" + '\u2083' + "-CClH" + '\u2082',
            "CF" + '\u2083' + "-CFH" + '\u2082', "[2CF" + '\u2083' + "-CClH" + '\u2082' + " * CrF" + '\u2083' + "]", "CF" + '\u2083' + "-CH" + '\u2083', "CF" + '\u2083' + "-CCl" + '\u2082' + "H",
            "CF" + '\u2083' + "-CFClH", "CF" + '\u2083' + "-CF" + '\u2082' + "H"};

   
            for (int i = 0; i < COUNT_OF_ELEMENTS; i++) {
                CheckBox box = new CheckBox();
                box.Content = elements[i];
                box.Name = "c" + i.ToString();
                _checkBoxes.Add(box);
            }

            //if (_firstEnter) {
            //    _checkBoxes[17].IsChecked = true;
            //    _firstEnter = false;
            //}

            elementsGroupBox.Content = new StackPanel() {
                Children = { _checkBoxes[0], _checkBoxes[1], _checkBoxes[2], _checkBoxes[3], _checkBoxes[4], _checkBoxes[5], _checkBoxes[6], _checkBoxes[7], _checkBoxes[8], _checkBoxes[9],
                _checkBoxes[10], _checkBoxes[11], _checkBoxes[12], _checkBoxes[13], _checkBoxes[14], _checkBoxes[15], _checkBoxes[16], _checkBoxes[17], _checkBoxes[18], _checkBoxes[19],
                _checkBoxes[20], _checkBoxes[21], _checkBoxes[22] }
            };
        }

        public List<int> GetChosenElements() {
            return _chosenElements;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e) {
            _chosenElements.Clear();

            foreach (CheckBox box in _checkBoxes) {
                if (box.IsChecked == true) {
                    _chosenElements.Add(int.Parse(box.Name.ToString().Substring(1)));
                }
            }

            if (_chosenElements.Count == 0) {
                MessageBox.Show("Вещества не выбраны", "Ошибка!", MessageBoxButton.OK);
                return;
            } else {
                this.Close();
            }

        }

        private void allButton_Click(object sender, RoutedEventArgs e) {

            for (int i = 0; i < _checkBoxes.Count; i++) {
                _checkBoxes[i].IsChecked = _fillAll;
                allButton.Content = !_fillAll ? "Выбрать все" : "Отменить все";
            }
            _fillAll = !_fillAll;
     
        }
    }
}

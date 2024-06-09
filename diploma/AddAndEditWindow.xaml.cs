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
    /// Interaction logic for AddAndEditWindow.xaml
    /// </summary>
    public partial class AddAndEditWindow : Window {
        private readonly bool _isEdit;
        private readonly string _id;
        private readonly string _table;
        private List<string> _firstLabel = new();
        private List<string> _secondLabel = new();
        private List<string> _thirdLabel = new();
        private List<string> _fourthLabel = new();

        public AddAndEditWindow(string table, bool isEdit, string id, List<string> firstLabel, List<string> secondLabel, List<string> thirdLabel, List<string> fourthLabel) {
            InitializeComponent();
            _isEdit = isEdit;
            _id = id;
            _firstLabel = firstLabel;
            _secondLabel = secondLabel;
            _thirdLabel = thirdLabel;
            _fourthLabel = fourthLabel;
            _table = table;
            ShowLabels();

        }

        private void ShowLabels() {
            first_Label.Content = _firstLabel[0];
            first_TextBox.Text = _firstLabel[1];

            second_Label.Content = _secondLabel[0];
            second_TextBox.Text = _secondLabel[1];

            third_Label.Content = _thirdLabel[0];
            third_TextBox.Text = _thirdLabel[1];

            fourth_Label.Content = _fourthLabel[0];
            fourth_TextBox.Text = _fourthLabel[1];

            if (second_Label.Content == "") {
                second_TextBox.IsEnabled = false;
            }
            if (third_Label.Content == "") {
                third_TextBox.IsEnabled = false;
            }
            if (fourth_Label.Content == "") {
                fourth_TextBox.IsEnabled = false;
            }
        }

        private void OK_Button_Click(object sender, RoutedEventArgs e) {
            DatabaseWork databaseWork = new DatabaseWork();
            if (_isEdit) {     
                if (_table == "Готовая продукция") {

                    try {
                        databaseWork.UpdateFinalProduct(_id, first_TextBox.Text, second_TextBox.Text, third_TextBox.Text, fourth_TextBox.Text);
                    } catch (Exception) {
                        MessageBox.Show("Ошибка запроса", "Ошибка!");
                        return;
                    }

                } else if (_table == "Оборудование") {

                    try {
                        databaseWork.UpdateEquipment(_id, first_TextBox.Text, second_TextBox.Text);
                    } catch (Exception) {
                        MessageBox.Show("Ошибка запроса", "Ошибка!");
                        return;
                    }

                } else if (_table == "Стадия") {

                    try {
                        databaseWork.UpdateStage(_id, first_TextBox.Text);
                    } catch (Exception) {
                        MessageBox.Show("Ошибка запроса", "Ошибка!");
                        return;
                    }
                } else if (_table == "Химическая формула") {

                    try {
                        databaseWork.UpdateChemic(_id, first_TextBox.Text);
                    } catch (Exception) {
                        MessageBox.Show("Ошибка запроса", "Ошибка!");
                        return;
                    }
                } else if (_table == "Рецепт") {

                    try {
                        databaseWork.UpdateRecipe(first_TextBox.Text, second_TextBox.Text, third_TextBox.Text, fourth_TextBox.Text);
                    } catch (Exception) {
                        MessageBox.Show("Ошибка запроса", "Ошибка!");
                        return;
                    }
                } else if (_table == "Кинетический параметр") {

                    try {
                        databaseWork.UpdateKinetic(_id, first_TextBox.Text, second_TextBox.Text, third_TextBox.Text);
                    } catch (Exception) {
                        MessageBox.Show("Ошибка запроса", "Ошибка!");
                        return;
                    }
                } else if (_table == "Значение кинетического параметра") {

                    try {
                        databaseWork.UpdateKineticValue(first_TextBox.Text, second_TextBox.Text, third_TextBox.Text);
                    } catch (Exception) {
                        MessageBox.Show("Ошибка запроса", "Ошибка!");
                        return;
                    }
                } else if (_table == "Единица измерения") {

                    try {
                        databaseWork.UpdateUnit(_id, first_TextBox.Text);
                    } catch (Exception) {
                        MessageBox.Show("Ошибка запроса", "Ошибка!");
                        return;
                    }
                } else if (_table == "Параметр оборудования") {

                    try {
                        databaseWork.UpdateEquipmentParameter(_id, first_TextBox.Text, second_TextBox.Text, third_TextBox.Text);
                    } catch (Exception) {
                        MessageBox.Show("Ошибка запроса", "Ошибка!");
                        return;
                    }
                } else if (_table == "Значение параметра оборудования") {

                    try {
                        databaseWork.UpdateEquipmentParameterValue(first_TextBox.Text, second_TextBox.Text, third_TextBox.Text);
                    } catch (Exception) {
                        MessageBox.Show("Ошибка запроса", "Ошибка!");
                        return;
                    }
                } else if (_table == "Пользователь") {

                    try {
                        databaseWork.UpdateUser(_id, first_TextBox.Text, second_TextBox.Text);
                    } catch (Exception) {
                        MessageBox.Show("Ошибка запроса", "Ошибка!");
                        return;
                    }

                }

            } else {
                if (_table == "Готовая продукция") {
                    try {
                        databaseWork.InsertFinalProduct(first_TextBox.Text, second_TextBox.Text, third_TextBox.Text, fourth_TextBox.Text);
                    } catch (Exception) {
                        MessageBox.Show("Ошибка запроса", "Ошибка!");
                        return;
                    }

                } else if (_table == "Оборудование") {
                    try {
                        databaseWork.InsertEquipment(first_TextBox.Text, second_TextBox.Text);
                    } catch (Exception) {
                        MessageBox.Show("Ошибка запроса", "Ошибка!");
                        return;
                    }

                } else if (_table == "Стадия") {
                    try {
                        databaseWork.InsertStage(first_TextBox.Text);
                    } catch (Exception) {
                        MessageBox.Show("Ошибка запроса", "Ошибка!");
                        return;
                    }
                } else if (_table == "Химическая формула") {
                    try {
                        databaseWork.InsertChemic(first_TextBox.Text);
                    } catch (Exception) {
                        MessageBox.Show("Ошибка запроса", "Ошибка!");
                        return;
                    }
                } else if (_table == "Рецепт") {

                    try {
                        databaseWork.InsertRecipe(first_TextBox.Text, second_TextBox.Text, third_TextBox.Text, fourth_TextBox.Text);
                    } catch (Exception) {
                        MessageBox.Show("Ошибка запроса", "Ошибка!");
                        return;
                    }
                } else if (_table == "Кинетический параметр") {

                    try {
                        databaseWork.InsertKinetic(first_TextBox.Text, second_TextBox.Text, third_TextBox.Text);
                    } catch (Exception) {
                        MessageBox.Show("Ошибка запроса", "Ошибка!");
                        return;
                    }
                } else if (_table == "Значение кинетического параметра") {

                    try {
                        databaseWork.InsertKineticValue(first_TextBox.Text, second_TextBox.Text, third_TextBox.Text);
                    } catch (Exception) {
                        MessageBox.Show("Ошибка запроса", "Ошибка!");
                        return;
                    }
                } else if (_table == "Единица измерения") {

                    try {
                        databaseWork.InsertUnit(first_TextBox.Text);
                    } catch (Exception) {
                        MessageBox.Show("Ошибка запроса", "Ошибка!");
                        return;
                    }
                } else if (_table == "Параметр оборудования") {

                    try {
                        databaseWork.InsertEquipmentParameter(first_TextBox.Text, second_TextBox.Text, third_TextBox.Text);
                    } catch (Exception) {
                        MessageBox.Show("Ошибка запроса", "Ошибка!");
                        return;
                    }
                } else if (_table == "Значение параметра оборудования") {

                    try {
                        databaseWork.InsertEquipmentParameterValue(first_TextBox.Text, second_TextBox.Text, third_TextBox.Text);
                    } catch (Exception) {
                        MessageBox.Show("Ошибка запроса", "Ошибка!");
                        return;
                    }
                } else if (_table == "Пользователь") {

                    try {
                        databaseWork.InsertUser(first_TextBox.Text, second_TextBox.Text);
                    } catch (Exception) {
                        MessageBox.Show("Ошибка запроса", "Ошибка!");
                        return;
                    }
                }
            }
            this.Close();
        }
    }
}

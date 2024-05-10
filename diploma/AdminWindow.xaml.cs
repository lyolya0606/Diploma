using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window {
        DatabaseWork _databaseWork;
        bool isFirstEnter;
        public AdminWindow() {
            InitializeComponent();
            FirstEnter();
        }

        private void back_Button_Click(object sender, RoutedEventArgs e) {
            Hide();
            new LoginWindow().Show();
            Close();
        }

        private void SetUpColumnsFinalProduct() {
            var column = new DataGridTextColumn {
                Header = "Номер",
                Binding = new Binding("ID")
            };
            base_DataGrid.Columns.Add(column);
            column = new DataGridTextColumn {
                Header = "Номер химической формулы",
                Binding = new Binding("IDChemic")
            };
            base_DataGrid.Columns.Add(column);
            column = new DataGridTextColumn {
                Header = "Название",
                Binding = new Binding("Name")
            };
            base_DataGrid.Columns.Add(column);

            column = new DataGridTextColumn {
                Header = "Марка",
                Binding = new Binding("Designation")
            };
            base_DataGrid.Columns.Add(column);

            column = new DataGridTextColumn {
                Header = "Область применения",
                Binding = new Binding("Area")
            };
            base_DataGrid.Columns.Add(column);
        }

        private record DataForFinalProduct {
            public required string ID { get; set; }
            public required string IDChemic { get; set; }
            public required string Name { get; set; }
            public required string Designation { get; set; }
            public required string Area { get; set; }
        }

        private void SetUpColumnsEquipment() {
            var column = new DataGridTextColumn {
                Header = "Номер",
                Binding = new Binding("ID")
            };
            base_DataGrid.Columns.Add(column);
            column = new DataGridTextColumn {
                Header = "Оборудование",
                Binding = new Binding("Name")
            };
            base_DataGrid.Columns.Add(column);

            column = new DataGridTextColumn {
                Header = "Обозначение",
                Binding = new Binding("Designation")
            };
            base_DataGrid.Columns.Add(column);
        }

        private record DataForEquipment {
            public required string ID { get; set; }
            public required string Name { get; set; }
            public required string Designation { get; set; }
        }

        private void SetUpColumnsRecipe() {
            var column = new DataGridTextColumn {
                Header = "Номер готового продукта",
                Binding = new Binding("IDProduct")
            };
            base_DataGrid.Columns.Add(column);
            column = new DataGridTextColumn {
                Header = "Номер стадии",
                Binding = new Binding("IDStage")
            };
            base_DataGrid.Columns.Add(column);

            column = new DataGridTextColumn {
                Header = "Последовательность",
                Binding = new Binding("Sequence")
            };
            base_DataGrid.Columns.Add(column);

            column = new DataGridTextColumn {
                Header = "Схема",
                Binding = new Binding("Scheme")
            };
            base_DataGrid.Columns.Add(column);
        }

        private record DataForRecipe {
            public required string IDProduct { get; set; }
            public required string IDStage { get; set; }
            public required string Sequence { get; set; }
            public required string Scheme { get; set; }
        }

        private void SetUpColumnsStage() {
            var column = new DataGridTextColumn {
                Header = "Номер",
                Binding = new Binding("ID")
            };
            base_DataGrid.Columns.Add(column);
            column = new DataGridTextColumn {
                Header = "Название",
                Binding = new Binding("Name")
            };
            base_DataGrid.Columns.Add(column);
        }

        private record DataForStage {
            public required string ID { get; set; }
            public required string Name { get; set; }
        }

        private void SetUpColumnsChemic() {
            var column = new DataGridTextColumn {
                Header = "Номер",
                Binding = new Binding("ID")
            };
            base_DataGrid.Columns.Add(column);
            column = new DataGridTextColumn {
                Header = "Формула",
                Binding = new Binding("Formula")
            };
            base_DataGrid.Columns.Add(column);
        }

        private record DataForChemic {
            public required string ID { get; set; }
            public required string Formula { get; set; }
        }

        private void SetUpColumnsKineticValue() {
            var column = new DataGridTextColumn {
                Header = "Номер кинетического параметра",
                Binding = new Binding("ID")
            };
            base_DataGrid.Columns.Add(column);
            column = new DataGridTextColumn {
                Header = "Номер готового продукта",
                Binding = new Binding("IDProd")
            };
            base_DataGrid.Columns.Add(column);
            column = new DataGridTextColumn {
                Header = "Значение",
                Binding = new Binding("Value")
            };
            base_DataGrid.Columns.Add(column);
        }

        private record DataForKineticValue {
            public required string ID { get; set; }
            public required string IDProd { get; set; }
            public required string Value { get; set; }
        }


        private void SetUpColumnsKinetic() {
            var column = new DataGridTextColumn {
                Header = "Номер",
                Binding = new Binding("ID")
            };
            base_DataGrid.Columns.Add(column);
            column = new DataGridTextColumn {
                Header = "Номер единицы измерения",
                Binding = new Binding("IDUnit")
            };
            base_DataGrid.Columns.Add(column);
            column = new DataGridTextColumn {
                Header = "Название",
                Binding = new Binding("Name")
            };
            base_DataGrid.Columns.Add(column);

            column = new DataGridTextColumn {
                Header = "Обозначение",
                Binding = new Binding("Designation")
            };
            base_DataGrid.Columns.Add(column);
        }

        private record DataForKinetic {
            public required string ID { get; set; }
            public required string IDUnit { get; set; }
            public required string Name { get; set; }
            public required string Designation { get; set; }
        }

        private void SetUpColumnsUnit() {
            var column = new DataGridTextColumn {
                Header = "Номер",
                Binding = new Binding("ID")
            };
            base_DataGrid.Columns.Add(column);
            column = new DataGridTextColumn {
                Header = "Обозначение",
                Binding = new Binding("Designation")
            };
            base_DataGrid.Columns.Add(column);
        }

        private record DataForUnit {
            public required string ID { get; set; }
            public required string Designation { get; set; }
        }

        private void FirstEnter() {
            isFirstEnter = true;
            _databaseWork = new DatabaseWork();

            tables_ComboBox.Items.Add("Готовая продукция");
            tables_ComboBox.Items.Add("Оборудование");
            tables_ComboBox.Items.Add("Стадия");
            tables_ComboBox.Items.Add("Химическая формула");
            tables_ComboBox.Items.Add("Рецепт");
            tables_ComboBox.Items.Add("Кинетический параметр");
            tables_ComboBox.Items.Add("Значение кинетического параметра");
            tables_ComboBox.Items.Add("Единица измерения");

            tables_ComboBox.SelectedIndex = 0;

            //SetUpColumnsFinalProduct();
            List<List<string>> dt = _databaseWork.GetTableFinalProduct();

            FillFinalProduct(dt);
        }

        private void FillFinalProduct(List<List<string>> dt) {
            SetUpColumnsFinalProduct();
            List<DataForFinalProduct> data = new();
            for (int i = 0; i < dt.Count; i++) {
                data.Add(new DataForFinalProduct {
                    ID = dt[i][0],
                    IDChemic = dt[i][1],
                    Name = dt[i][2],
                    Designation = dt[i][3],
                    Area = dt[i][4]
                }); 
            }

            base_DataGrid.ItemsSource = data;
        }

        private void base_DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e) {

            //var selectedCell = e.AddedCells[0];
            //var selectedRow = base_DataGrid.SelectedItem as DataRowView;

            //var firstCellValue = selectedRow.Row.ItemArray[0];
            //string value = firstCellValue.ToString();
            //int selectedRow = base_DataGrid.SelectedIndex;
            //var firstCellValue = selectedRow.Row.ItemArray[0].ToString();
            //string x = base_DataGrid.ItemsSource[(IEnumerable<selectedRow>)];
            //string id = (base_DataGrid.ItemsSource[selectedRow].Row[0].ToString();
            //add_Button.Content = value;
        }
        string? currentID;
        string? currentName;
        string? currentDesignation;
        string? currentArea;
        string currentTable;
        string currentIDChemic;
        string currentIDStage;
        string currentSequence;
        string currentScheme;
        string currentValue;

        private void base_DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) {

            edit_Button.IsEnabled = true;
            delete_Button.IsEnabled = true;
            int selectedRow = base_DataGrid.SelectedIndex;

            if (selectedRow == -1) { return; }

            string table = (string)tables_ComboBox.SelectedItem;

            if (table == "Готовая продукция") {
                TextBlock? id = base_DataGrid.Columns[0].GetCellContent(base_DataGrid.Items[selectedRow]) as TextBlock;
                currentID = id?.Text;

                TextBlock? IDChemic = base_DataGrid.Columns[1].GetCellContent(base_DataGrid.Items[selectedRow]) as TextBlock;
                currentIDChemic = IDChemic?.Text;

                TextBlock? name = base_DataGrid.Columns[2].GetCellContent(base_DataGrid.Items[selectedRow]) as TextBlock;
                currentName = name?.Text;

                TextBlock? designation = base_DataGrid.Columns[3].GetCellContent(base_DataGrid.Items[selectedRow]) as TextBlock;
                currentDesignation = designation?.Text;
                
                TextBlock? area = base_DataGrid.Columns[4].GetCellContent(base_DataGrid.Items[selectedRow]) as TextBlock;
                currentArea = area?.Text;

                currentTable = "Готовая продукция";

            } else if (table == "Оборудование") {
                TextBlock? id = base_DataGrid.Columns[0].GetCellContent(base_DataGrid.Items[selectedRow]) as TextBlock;
                currentID = id?.Text;

                TextBlock? name = base_DataGrid.Columns[1].GetCellContent(base_DataGrid.Items[selectedRow]) as TextBlock;
                currentName = name?.Text;

                TextBlock? designation = base_DataGrid.Columns[2].GetCellContent(base_DataGrid.Items[selectedRow]) as TextBlock;
                currentDesignation = designation?.Text;

                currentTable = "Оборудование";
            } else if (table == "Стадия") {
                TextBlock? id = base_DataGrid.Columns[0].GetCellContent(base_DataGrid.Items[selectedRow]) as TextBlock;
                currentID = id?.Text;

                TextBlock? name = base_DataGrid.Columns[1].GetCellContent(base_DataGrid.Items[selectedRow]) as TextBlock;
                currentName = name?.Text;

                currentTable = "Стадия";
            } else if (table == "Химическая формула") {
                TextBlock? id = base_DataGrid.Columns[0].GetCellContent(base_DataGrid.Items[selectedRow]) as TextBlock;
                currentID = id?.Text;

                TextBlock? name = base_DataGrid.Columns[1].GetCellContent(base_DataGrid.Items[selectedRow]) as TextBlock;
                currentName = name?.Text;

                currentTable = "Химическая формула";
            } else if (table == "Рецепт") {
                TextBlock? id = base_DataGrid.Columns[0].GetCellContent(base_DataGrid.Items[selectedRow]) as TextBlock;
                currentID = id?.Text;

                TextBlock? idStage = base_DataGrid.Columns[1].GetCellContent(base_DataGrid.Items[selectedRow]) as TextBlock;
                currentIDStage = idStage?.Text;

                TextBlock? seq = base_DataGrid.Columns[2].GetCellContent(base_DataGrid.Items[selectedRow]) as TextBlock;
                currentSequence = seq?.Text;

                TextBlock? scheme = base_DataGrid.Columns[3].GetCellContent(base_DataGrid.Items[selectedRow]) as TextBlock;
                currentScheme = scheme?.Text;

                currentTable = "Рецепт";
            } else if (table == "Кинетический параметр") {
                TextBlock? id = base_DataGrid.Columns[0].GetCellContent(base_DataGrid.Items[selectedRow]) as TextBlock;
                currentID = id?.Text;

                TextBlock? idStage = base_DataGrid.Columns[1].GetCellContent(base_DataGrid.Items[selectedRow]) as TextBlock;
                currentIDStage = idStage?.Text;

                TextBlock? name = base_DataGrid.Columns[2].GetCellContent(base_DataGrid.Items[selectedRow]) as TextBlock;
                currentName = name?.Text;

                TextBlock? des = base_DataGrid.Columns[3].GetCellContent(base_DataGrid.Items[selectedRow]) as TextBlock;
                currentDesignation = des?.Text;

                currentTable = "Кинетический параметр";
            } else if (table == "Значение кинетического параметра") {
                TextBlock? id = base_DataGrid.Columns[0].GetCellContent(base_DataGrid.Items[selectedRow]) as TextBlock;
                currentID = id?.Text;

                TextBlock? idStage = base_DataGrid.Columns[1].GetCellContent(base_DataGrid.Items[selectedRow]) as TextBlock;
                currentIDStage = idStage?.Text;

                TextBlock? val = base_DataGrid.Columns[2].GetCellContent(base_DataGrid.Items[selectedRow]) as TextBlock;
                currentValue = val?.Text;

                currentTable = "Значение кинетического параметра";
            } else if (table == "Единица измерения") {
                TextBlock? id = base_DataGrid.Columns[0].GetCellContent(base_DataGrid.Items[selectedRow]) as TextBlock;
                currentID = id?.Text;

                TextBlock? name = base_DataGrid.Columns[1].GetCellContent(base_DataGrid.Items[selectedRow]) as TextBlock;
                currentName = name?.Text;

                currentTable = "Единица измерения";
            }


        }

        private void edit_Button_Click(object sender, RoutedEventArgs e) {
            List<string> first = new();
            List<string> second = new();
            List<string> third = new();
            List<string> fourth = new();
            if (currentTable == "Готовая продукция") {
                first.Add("Номер химической формулы");
                first.Add(currentIDChemic);
                second.Add("Название");
                second.Add(currentName);
                third.Add("Марка");
                third.Add(currentDesignation);
                fourth.Add("Область применения");
                fourth.Add(currentArea);
            } else if (currentTable == "Оборудование") {
                first.Add("Название");
                first.Add(currentName);
                second.Add("Обозначение");
                second.Add(currentDesignation);
                third.Add("");
                third.Add("");
                fourth.Add("");
                fourth.Add("");
            } else if (currentTable == "Стадия") {
                first.Add("Название");
                first.Add(currentName);
                second.Add("");
                second.Add("");
                third.Add("");
                third.Add("");
                fourth.Add("");
                fourth.Add("");
            } else if (currentTable == "Химическая формула") {
                first.Add("Формула");
                first.Add(currentName);
                second.Add("");
                second.Add("");
                third.Add("");
                third.Add("");
                fourth.Add("");
                fourth.Add("");
            } else if (currentTable == "Рецепт") {
                first.Add("Номер готового продукта");
                first.Add(currentID);
                second.Add("Номер стадии");
                second.Add(currentIDStage);
                third.Add("Последовательность");
                third.Add(currentSequence);
                fourth.Add("Схема");
                fourth.Add(currentScheme);
            } else if (currentTable == "Кинетический параметр") {
                first.Add("Номер единицы измерения");
                first.Add(currentID);
                second.Add("Название");
                second.Add(currentName);
                third.Add("Обозначение");
                third.Add(currentDesignation);
                fourth.Add("");
                fourth.Add("");
            } else if (currentTable == "Значение кинетического параметра") {
                first.Add("Номер кинетического параметра");
                first.Add(currentID);
                second.Add("Номер готового продукта");
                second.Add(currentIDStage);
                third.Add("Значение");
                third.Add(currentValue);
                fourth.Add("");
                fourth.Add("");
            } else if (currentTable == "Единица измерения") {
                first.Add("Обозначение");
                first.Add(currentName);
                second.Add("");
                second.Add("");
                third.Add("");
                third.Add("");
                fourth.Add("");
                fourth.Add("");
            }

            AddAndEditWindow addAndEditWindow = new AddAndEditWindow(currentTable, true, currentID, first, second, third, fourth);
            addAndEditWindow.ShowDialog();


            FillAfterUpdate(currentTable);

        }

        private void add_Button_Click(object sender, RoutedEventArgs e) {
            string table = (string)tables_ComboBox.SelectedItem;
            List<string> first = new();
            List<string> second = new();
            List<string> third = new();
            List<string> fourth = new();
            if (table == "Готовая продукция") {
                first.Add("Номер химической формулы");
                first.Add("");
                second.Add("Название");
                second.Add("");
                third.Add("Марка");
                third.Add("");
                fourth.Add("Область применения");
                fourth.Add("");
            } else if (table == "Оборудование") {
                first.Add("Название");
                first.Add("");
                second.Add("Обозначение");
                second.Add("");
                third.Add("");
                third.Add("");
                fourth.Add("");
                fourth.Add("");
            } else if (table == "Стадия") {
                first.Add("Название");
                first.Add("");
                second.Add("");
                second.Add("");
                third.Add("");
                third.Add("");
                fourth.Add("");
                fourth.Add("");
            } else if (table == "Химическая формула") {
                first.Add("Формула");
                first.Add("");
                second.Add("");
                second.Add("");
                third.Add("");
                third.Add("");
                fourth.Add("");
                fourth.Add("");
            } else if (table == "Рецепт") {
                first.Add("Номер готового продукта");
                first.Add("");
                second.Add("Номер стадии");
                second.Add("");
                third.Add("Последовательность");
                third.Add("");
                fourth.Add("Схема");
                fourth.Add("");
            } else if (table == "Кинетический параметр") {
                first.Add("Номер единицы измерения");
                first.Add("");
                second.Add("Название");
                second.Add("");
                third.Add("Обозначение");
                third.Add("");
                fourth.Add("");
                fourth.Add("");
            } else if (table == "Значение кинетического параметра") {
                first.Add("Номер кинетического параметра");
                first.Add("");
                second.Add("Номер готового продукта");
                second.Add("");
                third.Add("Значение");
                third.Add("");
                fourth.Add("");
                fourth.Add("");
            } else if (table == "Единица измерения") {
                first.Add("Обозначение");
                first.Add("");
                second.Add("");
                second.Add("");
                third.Add("");
                third.Add("");
                fourth.Add("");
                fourth.Add("");
            }

            AddAndEditWindow addAndEditWindow = new AddAndEditWindow(table, false, currentID, first, second, third, fourth);
            addAndEditWindow.ShowDialog();


            FillAfterUpdate(table);
        }

        private void delete_Button_Click(object sender, RoutedEventArgs e) {
            if (currentTable == "Готовая продукция") {
                MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить запись из таблицы Готовая продукция:" +
                    $"{currentName} {currentDesignation}?", "Удаление записи", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes) {
                    _databaseWork.DeleteFinalProduct(currentID);
                } else {
                    return;
                }

            } else if (currentTable == "Оборудование") {
                MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить запись из таблицы Оборудование: " +
                     $"{currentName} {currentDesignation}?", "Удаление записи", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes) {
                    _databaseWork.DeleteEquipment(currentID);
                } else {
                    return;
                }

            } else if (currentTable == "Стадия") {
                MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить запись из таблицы Стадия: " +
                     $"{currentName}?", "Удаление записи", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes) {
                    _databaseWork.DeleteStage(currentID);
                } else {
                    return;
                }
            } else if (currentTable == "Химическая формула") {
                MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить запись из таблицы Химическая формула: " +
                     $"{currentName}?", "Удаление записи", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes) {
                    _databaseWork.DeleteChemic(currentID);
                } else {
                    return;
                }
            } else if (currentTable == "Рецепт") {
                MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить запись из таблицы Рецепт: " +
                     $"{currentID} - {currentIDStage}?", "Удаление записи", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes) {
                    _databaseWork.DeleteRecipe(currentID, currentIDStage);
                } else {
                    return;
                }
            } else if (currentTable == "Кинетический параметр") {
                MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить запись из таблицы Кинетический параметр: " +
                     $"{currentName}?", "Удаление записи", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes) {
                    _databaseWork.DeleteKinetic(currentID);
                } else {
                    return;
                }
            } else if (currentTable == "Значение кинетического параметра") {
                MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить запись из таблицы Значение кинетического параметра: " +
                     $"{currentID} - {currentIDStage}?", "Удаление записи", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes) {
                    _databaseWork.DeleteKineticValue(currentID, currentIDStage);
                } else {
                    return;
                }
            } else if (currentTable == "Единица измерения") {
                MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить запись из таблицы Единица измерения: " +
                     $"{currentName}?", "Удаление записи", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes) {
                    _databaseWork.DeleteUnit(currentID);
                } else {
                    return;
                }
            }

            FillAfterUpdate(currentTable);
        }

        
        private void FillAfterUpdate(string table) {
            base_DataGrid.ItemsSource = null;
            base_DataGrid.Columns.Clear();

            if (table == "Готовая продукция") {
                List<List<string>> dt = _databaseWork.GetTableFinalProduct();
                base_DataGrid.SelectedIndex = -1;
                FillFinalProduct(dt);
            } else if (table == "Оборудование") {
                List<List<string>> dt = _databaseWork.GetTableEquipment();
                base_DataGrid.SelectedIndex = -1;
                FillEquipment(dt);
            } else if (table == "Стадия") {
                List<List<string>> dt = _databaseWork.GetTableStage();
                base_DataGrid.SelectedIndex = -1;
                FillStage(dt);
            } else if (table == "Химическая формула") {
                List<List<string>> dt = _databaseWork.GetTableChemic();
                base_DataGrid.SelectedIndex = -1;
                FillChemic(dt);
            } else if (table == "Рецепт") {
                List<List<string>> dt = _databaseWork.GetTableRecipe();
                base_DataGrid.SelectedIndex = -1;
                FillRecipe(dt);
            } else if (table == "Кинетический параметр") {
                List<List<string>> dt = _databaseWork.GetTableKinetic();
                base_DataGrid.SelectedIndex = -1;
                FillKinetic(dt);
            } else if (table == "Значение кинетического параметра") {
                List<List<string>> dt = _databaseWork.GetTableKineticValue();
                base_DataGrid.SelectedIndex = -1;
                FillKineticValue(dt);
            } else if (table == "Единица измерения") {
                List<List<string>> dt = _databaseWork.GetTableUnit();
                base_DataGrid.SelectedIndex = -1;
                FillUnit(dt);
            }
        }

        private void FillKinetic(List<List<string>> dt) {
            SetUpColumnsKinetic();
            List<DataForKinetic> data = new();
            for (int i = 0; i < dt.Count; i++) {
                data.Add(new DataForKinetic {
                    ID = dt[i][0],
                    IDUnit = dt[i][1],
                    Name = dt[i][2],
                    Designation = dt[i][3]
                });
            }

            base_DataGrid.ItemsSource = data;
        }

        private void FillKineticValue(List<List<string>> dt) {
            SetUpColumnsKineticValue();
            List<DataForKineticValue> data = new();
            for (int i = 0; i < dt.Count; i++) {
                data.Add(new DataForKineticValue {
                    ID = dt[i][0],
                    IDProd = dt[i][1],
                    Value = dt[i][2]
                });
            }

            base_DataGrid.ItemsSource = data;
        }


        private void FillEquipment(List<List<string>> dt) {
            SetUpColumnsEquipment();
            List<DataForEquipment> data = new();
            for (int i = 0; i < dt.Count; i++) {
                data.Add(new DataForEquipment {
                    ID = dt[i][0],
                    Name = dt[i][1],
                    Designation = dt[i][2]
                });
            }

            base_DataGrid.ItemsSource = data;
        }


        private void FillStage(List<List<string>> dt) {
            SetUpColumnsStage();
            List<DataForStage> data = new();
            for (int i = 0; i < dt.Count; i++) {
                data.Add(new DataForStage {
                    ID = dt[i][0],
                    Name = dt[i][1]
                });
            }

            base_DataGrid.ItemsSource = data;
        }

        private void FillChemic(List<List<string>> dt) {
            SetUpColumnsChemic();
            List<DataForChemic> data = new();
            for (int i = 0; i < dt.Count; i++) {
                data.Add(new DataForChemic {
                    ID = dt[i][0],
                    Formula = dt[i][1]
                });
            }

            base_DataGrid.ItemsSource = data;
        }

        private void FillRecipe(List<List<string>> dt) {
            SetUpColumnsRecipe();
            List<DataForRecipe> data = new();
            for (int i = 0; i < dt.Count; i++) {
                data.Add(new DataForRecipe {
                    IDProduct = dt[i][0],
                    IDStage = dt[i][1],
                    Sequence = dt[i][2],
                    Scheme = dt[i][3]
                });
            }

            base_DataGrid.ItemsSource = data;
        }

        private void FillUnit(List<List<string>> dt) {
            SetUpColumnsUnit();
            List<DataForUnit> data = new();
            for (int i = 0; i < dt.Count; i++) {
                data.Add(new DataForUnit {
                    ID = dt[i][0],
                    Designation = dt[i][1]
                });
            }

            base_DataGrid.ItemsSource = data;
        }

        private void tables_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (isFirstEnter) {
                isFirstEnter = false;
                return;
            }

            base_DataGrid.ItemsSource = null;
            base_DataGrid.Columns.Clear();

            string table = (string)tables_ComboBox.SelectedItem;

            FillAfterUpdate(table);
        }
    }
}

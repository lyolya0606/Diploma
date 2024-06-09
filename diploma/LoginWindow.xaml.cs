using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Diploma {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window {
        DatabaseWork _databaseWork = new();
        int _count = 4;
        public LoginWindow() {
            InitializeComponent();
            FillComboBox();
        }

        private void FillComboBox() {
            List<string> logins = _databaseWork.GetLogins();
            foreach (string log in logins) {
                loginComboBox.Items.Add(log);
            }

        }

        private void loginButton_Click(object sender, RoutedEventArgs e) {
            string login = loginComboBox.Text;
            string password = passwordTextBox.Password;

            if (_count < 0) {
                loginButton.IsEnabled = false;
                return;
            }

            if (!CheckPassword(password, login)) {
                error_Label.Content = "Неверный пароль! Осталось попыток: " + _count;
                _count--;
                return;
            }

            if (login == "user") {
                new UserWindow().Show();
                Close();
            } else if (login == "admin") {
                new AdminWindow().Show();
                Close();
            }
        }

        private bool CheckPassword(string password, string login) {
            return password == _databaseWork.GetPassword(login);
        }
    }
}
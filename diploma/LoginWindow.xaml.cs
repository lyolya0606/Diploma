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
        public LoginWindow() {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e) {
            if (loginComboBox.Text == "user" && passwordTextBox.Password == "user") {
                //Hide();
                new UserWindow().Show();
                Close();
            } else if (loginComboBox.Text == "admin" && passwordTextBox.Password == "admin") {
                new AdminWindow().Show();
                Close();
            }
        }
    }
}
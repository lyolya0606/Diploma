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
    /// Interaction logic for ReactorWindow.xaml
    /// </summary>
    public partial class ReactorWindow : Window {
        public ReactorWindow() {
            InitializeComponent();
            FillLabels();
        }

        private void FillLabels() {
            DatabaseWork databaseWork = new();
            string firstReactor = "Первый реактор: ";
            string secondReactor = "Второй реактор: ";

            string[] seq = { "d", "h", "V" };
           

            foreach (string seqName in seq) {
                string reactorName = "Р5";
                firstReactor += databaseWork.GetReacorsParameters(seqName, reactorName) + "; ";

                reactorName = "Р8";
                secondReactor += databaseWork.GetReacorsParameters(seqName, reactorName) + "; ";

            }

            secondReactor = secondReactor.Remove(secondReactor.Length - 2);
            secondReactor += ".";

            first_Label.Content = firstReactor;
            second_Label.Content= secondReactor;


        }
    }
}

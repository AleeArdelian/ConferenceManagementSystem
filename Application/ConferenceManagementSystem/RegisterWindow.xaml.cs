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

namespace ConferenceManagementSystem
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LoginAsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selection = ((sender as ComboBox).SelectedItem as ComboBoxItem).Content.ToString();

            if (selection == "Listener")
            {
                affiliationLabel.Visibility = Visibility.Hidden;
                affiliationTextBox.Visibility = Visibility.Hidden;
                websiteLabel.Visibility = Visibility.Hidden;
                websiteTextBox.Visibility = Visibility.Hidden;
            }
            if (selection == "Author")
            {
                affiliationLabel.Visibility = Visibility.Visible;
                affiliationTextBox.Visibility = Visibility.Visible;
                websiteLabel.Visibility = Visibility.Hidden;
                websiteTextBox.Visibility = Visibility.Hidden;

            }
            if (selection == "PC Member")
            {
                affiliationLabel.Visibility = Visibility.Visible;
                affiliationTextBox.Visibility = Visibility.Visible;
                websiteLabel.Visibility = Visibility.Visible;
                websiteTextBox.Visibility = Visibility.Visible;
            }
        }
    }
}

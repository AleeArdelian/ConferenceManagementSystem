using ConferenceManagementSystem.Controller;
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
using Section = ConferenceManagementSystem.Domain.Section;

namespace ConferenceManagementSystem
{
    /// <summary>
    /// Interaction logic for DeadlineWindow.xaml
    /// </summary>
    public partial class DeadlineWindow : Window
    {
        CMSController controller;
        Section section;

        public DeadlineWindow(CMSController cont, Section sec)
        {
            this.controller = cont;
            this.section = sec;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Int32 day = Int32.Parse(dayCombo.SelectedItem.ToString());
            Int32 month =Int32.Parse( monthCombo.SelectedItem.ToString());
            Int32 year = Int32.Parse(yearCombo.SelectedItem.ToString());

            DateTime date = new DateTime(year, month, day);

        }

    }
}

using ConferenceManagementSystem.Controller;
using ConferenceManagementSystem.Entities;
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
    /// Interaction logic for SuperUserWindow.xaml
    /// </summary>
    public partial class SuperUserWindow : Window
    {
        private CMSController controller;
        private User user;

        public SuperUserWindow(CMSController controller, User user)
        {
            this.user = user;
            this.controller = controller;
            InitializeComponent();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ProfileItem_Selected(object sender, RoutedEventArgs e)
        {
            contentFrame.Content = new ProfilePage(controller, user);
        }

        private void ConferencesItem_Selected(object sender, RoutedEventArgs e)
        {
            contentFrame.Content = new ConfManagementPage(controller, user);
        }
    }
}

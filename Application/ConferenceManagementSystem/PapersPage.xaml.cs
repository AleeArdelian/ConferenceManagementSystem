using ConferenceManagementSystem.Controller;
using ConferenceManagementSystem.Domain;
using ConferenceManagementSystem.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConferenceManagementSystem
{
    /// <summary>
    /// Interaction logic for PapersPage.xaml
    /// </summary>
    public partial class PapersPage : Page
    {
        CMSController controller;
        User user;
        public List<Conference> conferences { get; set; }
        public List<Domain.Section> sections { get; set; }


        public PapersPage(CMSController controller, User user)
        {
            this.controller = controller;
            this.user = user;
            InitializeComponent();
            conferences = this.controller.getConferences();
            DataContext = this;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Conference conference = (Conference)conferencesListView.SelectedItems[0];
            sections = controller.getSectionsOfConference(conference);
            sectionsListView.ItemsSource = sections;
        }
    }
}

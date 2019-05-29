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
using Section = ConferenceManagementSystem.Domain.Section;

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
        public List<Section> sections { get; set; }
        public List<Paper> papers { get; set; }


        public PapersPage(CMSController controller, User user)
        {
            this.controller = controller;
            this.user = user;
            InitializeComponent();
            conferences = this.controller.getConferences();
            DataContext = this;
            if(user.RoleID==1 || user.RoleID == 5)
            {
                papersLabel.Visibility = Visibility.Hidden;
                papersListView.Visibility = Visibility.Hidden;
                changeDeadButton.Visibility = Visibility.Hidden;
                
            }
            if(user.RoleID == 4)
            {
                changeDeadButton.Visibility = Visibility.Hidden;
            }
            else
            {
                loadPapers();
            }
        }

        private void loadPapers()
        {
            papers = controller.getPapers();
            papersListView.ItemsSource = papers;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Conference conference = (Conference)conferencesListView.SelectedItems[0];
            sections = controller.getSectionsOfConference(conference);
            sectionsListView.ItemsSource = sections;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (user.RoleID == 1 || user.RoleID == 4)
            {   
                if(sectionsListView.SelectedItems.Count > 1 || sectionsListView.SelectedItems.Count == 0)
                {
                    MessageBox.Show("You can select only one session");
                }

                Section section = (Section)sectionsListView.SelectedItems[0];
                try
                {
                    if (paperNameTextBox.Text != "" && topicTextBox.Text != "" && paperLocationTextBox.Text != "" && abstractLocationTextBox.Text != "")
                    {
                        controller.addPaper(paperNameTextBox.Text, topicTextBox.Text, paperLocationTextBox.Text, abstractLocationTextBox.Text, section.ID, user.ID);
                        MessageBox.Show("Success!");
                    }
                    else
                    {
                        MessageBox.Show("invalid paper name, topic, or locations!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("You can't upload papers");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {


            if (sectionsListView.SelectedItems.Count > 1 || sectionsListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("You can select only one session");
                return;
            }
            Section section = (Section)sectionsListView.SelectedItems[0];

            DeadlineWindow deadlineWindow = new DeadlineWindow(controller, section);
            deadlineWindow.ShowDialog();


        }
    }
}

using ConferenceManagementSystem.Controller;
using ConferenceManagementSystem.Domain;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConferenceManagementSystem
{
    /// <summary>
    /// Interaction logic for ReviewPage.xaml
    /// </summary>
    public partial class ReviewPage : Page
    {

        CMSController controller;
        User user;
        public List<Paper> papers { get; set; }

        public ReviewPage(CMSController con, User u)
        {
            this.controller = con;
            this.user = u;
            InitializeComponent();
            loadPapers();
        }

        private void loadPapers()
        {
            papers = controller.getPapers();
            papersListView.ItemsSource = papers;
        }
    }
}

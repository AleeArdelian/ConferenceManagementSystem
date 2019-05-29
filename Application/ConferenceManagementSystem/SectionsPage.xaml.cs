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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConferenceManagementSystem
{
    /// <summary>
    /// Interaction logic for SectionsPage.xaml
    /// </summary>
    public partial class SectionsPage : Page
    {
        CMSController controller;
        User user;
        List<Section> sections { get; set; }
        Conference conf;

        public SectionsPage(Conference conf, CMSController controller, User user)
        {
            this.conf = conf;
            this.user = user;
            this.controller = controller;
            InitializeComponent();
            if (conf == null)
            {
                throw new Exception("No conference selected!");
            } 
            sections = this.controller.getSections(conf.ID);
            sectionsListView.ItemsSource = sections;
            DataContext = this;
        }
    }
}

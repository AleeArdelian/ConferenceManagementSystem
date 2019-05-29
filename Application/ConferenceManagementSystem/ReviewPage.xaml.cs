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

        public ReviewPage(CMSController con, User u)
        {
            this.controller = con;
            this.user = u;
            InitializeComponent();
        }
    }
}

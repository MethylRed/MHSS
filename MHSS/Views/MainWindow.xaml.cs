using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Collections.Generic;
using MHSS.Models.Data;

namespace MHSS.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public List<Skill> Skill { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            //Skill = Master.Skills;
            //this.DataContext = this;
        }
    }
}

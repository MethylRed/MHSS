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

namespace MHSS.Views.SubViews
{
    /// <summary>
    /// DecoRegistView.xaml の相互作用ロジック
    /// </summary>
    public partial class DecoRegistView : UserControl
    {
        public DecoRegistView()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PlaceHolderText.Visibility = string.IsNullOrEmpty(NameTextBox.Text)
                ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SkillNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PlaceHolderSkillText.Visibility = string.IsNullOrEmpty(SkillNameTextBox.Text)
                ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}

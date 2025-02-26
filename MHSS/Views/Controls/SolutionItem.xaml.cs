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

namespace MHSS.Views.Controls
{
    /// <summary>
    /// SolutionItem.xaml の相互作用ロジック
    /// </summary>
    public partial class SolutionItem : UserControl
    {
        public SolutionItem()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Expander.IsExpanded = true;
            double width = Target.ActualWidth;
            double height = Target.ActualHeight;

            RenderTargetBitmap bitmap = new((int)width, (int)height, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(Target);

            Clipboard.SetImage(bitmap);
        }
    }
}

using System.Windows.Controls;

namespace MHSS.Views.Controls
{
    /// <summary>
    /// Interaction logic for SkillLevelSelector
    /// </summary>
    public partial class SkillLevelSelector : UserControl
    {
        public SkillLevelSelector()
        {
            InitializeComponent();
        }

        private void ComboBox_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            e.Handled = true;
        }
    }
}

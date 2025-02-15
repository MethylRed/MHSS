using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHSS.ViewModels.Controls
{
    internal class SkillLevelSelectorItems : SubViewModelBase
    {
        /// <summary>
        /// ComboBoxのItemに表示させる文字列
        /// </summary>
        public string DisplayStr { get; set; }

        /// <summary>
        /// スキルレベル
        /// </summary>
        public int SkillLevel { get; set; }


        public SkillLevelSelectorItems(string displayStr, int skillLevel)
        {
            DisplayStr = displayStr;
            SkillLevel = skillLevel;
        }
    }
}

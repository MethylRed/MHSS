using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using MHSS.Models;
using System.Collections.ObjectModel;
using MHSS.Models.Data;
using MHSS.Views.Controls;

namespace MHSS.ViewModels.Controls
{
    internal class SkillLevelSelectorsByCategoryViewModel : SubViewModelBase
    {
        /// <summary>
        /// カテゴリ名
        /// </summary>
        public string CategoryName { get; init; }

        /// <summary>
        /// スキル選択アイテムのViewModel
        /// </summary>
        public ObservableCollection<SkillLevelSelectorViewModel> SkillLevelSelectorVM { get; init; }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="skills"></param>
        public SkillLevelSelectorsByCategoryViewModel(string categoryName, IEnumerable<Skill> skills)
        {
            CategoryName = categoryName;
            SkillLevelSelectorVM = new(skills.Select(x => new SkillLevelSelectorViewModel(x.Name)));
        }

        /// <summary>
        /// 選択されたスキルのレベル
        /// </summary>
        /// <returns>選択されているスキルの配列</returns>
        public IReadOnlyList<Skill> SelectedSkills()
        {
            IReadOnlyList<Skill> selectedSkills = new List<Skill>();
            selectedSkills = SkillLevelSelectorVM/*.Where(s => s.IsSelected.Value)*/
                .Select(s => s.SelectedSkill).ToList();
            return selectedSkills;
        }
    }
}

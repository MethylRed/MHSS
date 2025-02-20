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
        public ObservableCollection<SkillLevelSelectorViewModel> SkillLevelSelectorVMs { get; init; }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="categoryName">カテゴリ名</param>
        /// <param name="skills">スキルのコレクション</param>
        public SkillLevelSelectorsByCategoryViewModel(string categoryName, IEnumerable<Skill> skills)
        {
            CategoryName = categoryName;
            SkillLevelSelectorVMs = new(skills.Select(x => new SkillLevelSelectorViewModel(x.Name)));
        }

        /// <summary>
        /// スキルの条件コレクションを返す
        /// </summary>
        /// <returns>選択されているスキルの配列</returns>
        public IReadOnlyList<Skill> SkillsCondition()
        {
            IReadOnlyList<Skill> selectedSkills = new List<Skill>();
            selectedSkills = SkillLevelSelectorVMs.Select(s => s.SelectedSkill).ToList();
            return selectedSkills;
        }
    }
}

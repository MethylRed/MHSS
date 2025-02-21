using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using MHSS.Models.Data;
using MHSS.Models.Utility;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Reactive.Bindings;
using MHSS.ViewModels.Controls;
using Google.OrTools.LinearSolver;

namespace MHSS.ViewModels.SubView
{
    internal class SkillSelectViewModel : SubViewModelBase
    {
        /// <summary>
        /// カテゴリ別スキル選択アイテムのViewModel
        /// </summary>
        public ReactivePropertySlim<ObservableCollection<SkillLevelSelectorsByCategoryViewModel>> SkillLevelSelectorsByCategoryVMs { get; } = new();


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SkillSelectViewModel()
        {
            // カテゴリ別にスキル条件選択のComboBoxを配置
            ObservableCollection<SkillLevelSelectorsByCategoryViewModel> oldColl = SkillLevelSelectorsByCategoryVMs.Value;
            if (oldColl != null)
            {
                foreach (var item in oldColl)
                {
                    ((IDisposable)item).Dispose();
                }
            }
            SkillLevelSelectorsByCategoryVMs.Value = new ObservableCollection<SkillLevelSelectorsByCategoryViewModel>(
                Master.Skills.GroupBy(s => s.Category).OrderBy(g => Kind.SkillCategory.IndexOf(g.Key))
                .Select(g => new SkillLevelSelectorsByCategoryViewModel(g.Key, g))
            );
        }

        /// <summary>
        /// スキルの検索条件を取得
        /// </summary>
        /// <returns></returns>
        public Condition MakeSkillCondition()
        {
            Condition condition = new();
            condition.Skills = SkillLevelSelectorsByCategoryVMs.Value.SelectMany(v => v.SkillsCondition()).ToList();
            bool b = SkillLevelSelectorsByCategoryVMs.Value.Select(s => s.JudgeSecret()).Aggregate(true, (acc, x) => acc && x);
            condition.Secret = b;
            return condition;
        }
    }
}
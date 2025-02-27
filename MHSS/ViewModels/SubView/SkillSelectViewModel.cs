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
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Text.RegularExpressions;

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

            // 全スキル選択VM
            var skillLevelSelectorVMs = SkillLevelSelectorsByCategoryVMs.Value.SelectMany(v => v.SkillLevelSelectorVMs);

            // 極意を発動させられるシリーズスキルのSelectorVM
            var seriesSkillsWithSecret = skillLevelSelectorVMs
                .Where(s => (s.SelectedSkill.Category == "シリーズスキル") &&
                    ((s.SelectedSkill.ActivateSkillName1.Contains("極意")) ||
                     (s.SelectedSkill.ActivateSkillName2.Contains("極意"))));

            // 各極意を発動させられるシリーズスキルのDictionary
            var seriesSkillWithSecretDict = seriesSkillsWithSecret.GroupBy(g => g.SelectedSkill.ActivateSkillName1).ToDictionary(g => g.Key, g => g.ToList())
                .Concat(seriesSkillsWithSecret.GroupBy(g => g.SelectedSkill.ActivateSkillName2).ToDictionary(g => g.Key, g => g.ToList()))
                .GroupBy(kvp => kvp.Key)
                .ToDictionary(g => g.Key, g => g.SelectMany(kvp => kvp.Value)).Where(kvp => kvp.Key.Contains("極意"))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            // 極意で上限が解放可能なスキルのSelectorVM
            var limitBreakSkills = skillLevelSelectorVMs.Where(s => (s.SelectedSkill.Category != "シリーズスキル") && (s.SelectedSkill.ActivateSkillName1.Contains("極意")));

            // 極意に関係しないスキルのSelectorVM
            var otherSkills = skillLevelSelectorVMs
                .Where(v => !((v.SelectedSkill.ActivateSkillName1.Contains("極意")) || v.SelectedSkill.ActivateSkillName2.Contains("・極意")));

            // 極意を発動させられるシリーズスキルが選ばれたとき
            foreach (var item in seriesSkillsWithSecret)
            {
                item.SelectedItem.Subscribe(isSelected =>
                {
                    // 発動スキル一覧
                    var items = Regex.Replace(item.Items.Value.Last().DisplayStr, @"\([^)]*\)", "").Trim().Split("&");

                    // レベル1以上か固定されたとき
                    if ((item.SelectedItem.Value.SkillLevel != 0) || item.IsFixed.Value)
                    {
                        // 全ての発動スキルについて
                        foreach (var str in items)
                        {
                            // 発動スキルが極意だったら
                            if (str.Contains("極意"))
                            {
                                // その極意に対応するスキルのIsFixedを2回bit反転して値を変更せずに通知を送る
                                limitBreakSkills.Single(x => x.SelectedSkill.ActivateSkillName1 == str).IsFixed.Value = !limitBreakSkills.Single(x => x.SelectedSkill.ActivateSkillName1 == str).IsFixed.Value;
                                limitBreakSkills.Single(x => x.SelectedSkill.ActivateSkillName1 == str).IsFixed.Value = !limitBreakSkills.Single(x => x.SelectedSkill.ActivateSkillName1 == str).IsFixed.Value;
                            }
                        }
                        item.IsSelected.Value = true;
                    }
                    else
                    {
                        foreach (var str in items)
                        {
                            if (str.Contains("極意"))
                            {
                                limitBreakSkills.Single(x => x.SelectedSkill.ActivateSkillName1 == str).IsFixed.Value = !limitBreakSkills.Single(x => x.SelectedSkill.ActivateSkillName1 == str).IsFixed.Value;
                                limitBreakSkills.Single(x => x.SelectedSkill.ActivateSkillName1 == str).IsFixed.Value = !limitBreakSkills.Single(x => x.SelectedSkill.ActivateSkillName1 == str).IsFixed.Value;
                            }
                        }
                        item.IsSelected.Value = false;
                    }
                });
                item.IsFixed.Subscribe(isSelected =>
                {
                    // 発動スキル一覧
                    var items = Regex.Replace(item.Items.Value.Last().DisplayStr, @"\([^)]*\)", "").Trim().Split("&");

                    // レベル1以上か固定されたとき
                    if ((item.SelectedItem.Value.SkillLevel != 0) || item.IsFixed.Value)
                    {
                        foreach (var str in items)
                        {
                            if (str.Contains("極意"))
                            {
                                limitBreakSkills.Single(x => x.SelectedSkill.ActivateSkillName1 == str).IsFixed.Value = !limitBreakSkills.Single(x => x.SelectedSkill.ActivateSkillName1 == str).IsFixed.Value;
                                limitBreakSkills.Single(x => x.SelectedSkill.ActivateSkillName1 == str).IsFixed.Value = !limitBreakSkills.Single(x => x.SelectedSkill.ActivateSkillName1 == str).IsFixed.Value;
                            }
                        }
                        item.IsSelected.Value = true;
                    }
                    else
                    {
                        foreach (var str in items)
                        {
                            if (str.Contains("極意"))
                            {
                                limitBreakSkills.Single(x => x.SelectedSkill.ActivateSkillName1 == str).IsFixed.Value = !limitBreakSkills.Single(x => x.SelectedSkill.ActivateSkillName1 == str).IsFixed.Value;
                                limitBreakSkills.Single(x => x.SelectedSkill.ActivateSkillName1 == str).IsFixed.Value = !limitBreakSkills.Single(x => x.SelectedSkill.ActivateSkillName1 == str).IsFixed.Value;
                            }
                        }
                        item.IsSelected.Value = false;
                    }
                });
                item.IsSelected.Subscribe(i =>
                {
                    if (i)　item.BackgroundColor.Value = item.BackgroundColor.Value = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C4E1FF"));
                    else item.BackgroundColor.Value = Brushes.White;
                });
            }


            // 極意で上限が解放可能なスキルが選ばれたとき
            foreach (var item in limitBreakSkills)
            {
                item.SelectedItem.Subscribe(isselected =>
                {
                    // 極意が必要なレベルかどうか
                    var needSecret = item.SelectedItem.Value.DisplayStr.Contains("極意");

                    // 極意がひとつでも発動しているかどうか
                    var ActiveSecret = seriesSkillWithSecretDict[item.SelectedSkill.ActivateSkillName1]
                    .Any(s => s.SelectedItem.Value.DisplayStr.Contains(item.SelectedSkill.ActivateSkillName1));

                    // レベル1以上か固定されたとき
                    if ((item.SelectedItem.Value.SkillLevel != 0) || item.IsFixed.Value)
                    {
                        // 極意が必要なレベルが選ばれたとき & その極意が一つも発動していなかったら
                        if (needSecret && !ActiveSecret)
                        {
                            item.SatisfySecret.Value = false;
                        }
                        else
                        {
                            item.SatisfySecret.Value = true;
                        }
                        item.IsSelected.Value = true;
                    }
                    else
                    {
                        item.SatisfySecret.Value = true;
                        item.IsSelected.Value = false;
                    }
                });
                item.IsFixed.Subscribe(isselected =>
                {
                    var needSecret = item.SelectedItem.Value.DisplayStr.Contains("極意");
                    var ActiveSecret = seriesSkillWithSecretDict[item.SelectedSkill.ActivateSkillName1]
                    .Any(s => s.SelectedItem.Value.DisplayStr.Contains(item.SelectedSkill.ActivateSkillName1));
                    
                    // レベル1以上か固定されたとき
                    if ((item.SelectedItem.Value.SkillLevel != 0) || item.IsFixed.Value)
                    {
                        // 極意が必要なレベルが選ばれたとき & その極意が一つも発動していなかったら
                        if (needSecret && !ActiveSecret)
                        {
                            item.SatisfySecret.Value = false;
                        }
                        else
                        {
                            item.SatisfySecret.Value = true;
                        }
                        item.IsSelected.Value = true;
                    }
                    else
                    {
                        item.SatisfySecret.Value = true;
                        item.IsSelected.Value = false;
                    }
                });
                item.SatisfySecret.Subscribe(i =>
                {
                    if (item.IsSelected.Value)
                    {
                        if (i) item.BackgroundColor.Value = item.BackgroundColor.Value = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C4E1FF"));
                        else item.BackgroundColor.Value = item.BackgroundColor.Value = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCCCC"));
                    }
                    else item.BackgroundColor.Value = Brushes.White;
                });
                item.IsSelected.Subscribe(i =>
                {
                    if (i)
                    {
                        if (item.SatisfySecret.Value) item.BackgroundColor.Value = item.BackgroundColor.Value = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C4E1FF"));
                        else item.BackgroundColor.Value = item.BackgroundColor.Value = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCCCC"));
                    }
                    else item.BackgroundColor.Value = Brushes.White;
                });
            }


            // 極意とは関係ないスキル
            foreach (var item in otherSkills)
            {
                item.SelectedItem.Subscribe(isSelected =>
                {
                    //レベルが0以外 or 固定されたら
                    if ((item.SelectedItem.Value.SkillLevel != 0) || item.IsFixed.Value)
                    {
                        item.IsSelected.Value = true;
                    }
                    else
                    {
                        item.IsSelected.Value = false;
                    }
                });
                item.IsFixed.Subscribe(isSelected =>
                {
                    //レベルが0以外 or 固定されたら
                    if ((item.SelectedItem.Value.SkillLevel != 0) || item.IsFixed.Value)
                    {
                        item.IsSelected.Value = true;
                    }
                    else
                    {
                        item.IsSelected.Value = false;
                    }
                });
                item.IsSelected.Subscribe(i =>
                {
                    if (i) item.BackgroundColor.Value = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C4E1FF"));
                    else item.BackgroundColor.Value = Brushes.White;
                });
            }
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
            condition.SatisfySecret = b;
            return condition;
        }
    }
}
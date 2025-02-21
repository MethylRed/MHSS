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



            var x = SkillLevelSelectorsByCategoryVMs.Value.SelectMany(v => v.SkillLevelSelectorVMs);

            // 極意が発動するシリーズスキル
            var seriesSkillsActiveSecret1 = x.Where(v => ((v.SelectedSkill.Category == "シリーズスキル") && (v.SelectedSkill.ActivateSkillName1.Contains("極意")) && !(v.SelectedSkill.ActivateSkillName2.Contains("極意"))));
            var seriesSkillsActiveSecret2 = x.Where(v => ((v.SelectedSkill.Category == "シリーズスキル") && !(v.SelectedSkill.ActivateSkillName1.Contains("極意")) && (v.SelectedSkill.ActivateSkillName2.Contains("極意"))));
            var seriesSkillsActiveSecret12 = x.Where(v => ((v.SelectedSkill.Category == "シリーズスキル") && (v.SelectedSkill.ActivateSkillName1.Contains("極意")) && (v.SelectedSkill.ActivateSkillName2.Contains("極意"))));

            // 極意で上限解放ができるスキル
            var skillsHaveSecret = x.Where(v => ((v.SelectedSkill.Category != "シリーズスキル") && (v.SelectedSkill.ActivateSkillName2.Contains("極意"))));

            // 上記2つ以外
            var otherSkills = x.Where(v => !((v.SelectedSkill.Category == "シリーズスキル") && ((v.SelectedSkill.ActivateSkillName1.Contains("極意")) || (v.SelectedSkill.ActivateSkillName2.Contains("極意")))) && !((v.SelectedSkill.Category != "シリーズスキル") && (v.SelectedSkill.ActivateSkillName2.Contains("極意"))));


            
            foreach (var item in seriesSkillsActiveSecret1)
            {
                item.SelectedItem.Subscribe(isSelected =>
                {
                    var activeSecretSum = seriesSkillsActiveSecret1.Where(v => (item.SelectedSkill.Name != v.SelectedSkill.Name)).Where(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName1)).Select(v => v.ActiveSecret1)
                    .Union(seriesSkillsActiveSecret2.Where(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName2)).Select(v => v.ActiveSecret2))
                    .Union(seriesSkillsActiveSecret12.Where(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName1)).Select(v => v.ActiveSecret1))
                    .Union(seriesSkillsActiveSecret12.Where(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName2)).Select(v => v.ActiveSecret2))
                    .DefaultIfEmpty(false).Aggregate((acc, current) => acc || current);

                    var s = skillsHaveSecret.Single(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName2));
                    
                    //レベルが0以外 or 固定されたら
                    if ((item.SelectedItem.Value.SkillLevel != 0) || item.IsFixed.Value)
                    {
                        // 極意が発動時
                        if (item.SelectedSkill.Level >= item.SelectedSkill.MaxLevel1)
                        {
                            item.ActiveSecret1 = true;
                            s.SatisfySecret.Value |= true;
                        }
                        else
                        {
                            item.ActiveSecret1 = false;
                            s.SatisfySecret.Value = activeSecretSum;
                        }
                        item.IsSelected.Value = true;
                    }
                    else
                    {
                        item.IsSelected.Value = false;
                        item.ActiveSecret1 = false;

                        // これは間違い。対応する極意のActiveSecretの論理和を代入する。
                        s.SatisfySecret.Value = activeSecretSum;
                    }
                });
                item.IsFixed.Subscribe(isSelected =>
                {
                    var activeSecretSum = seriesSkillsActiveSecret1.Where(v => (item.SelectedSkill.Name != v.SelectedSkill.Name)).Where(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName1)).Select(v => v.ActiveSecret1)
                    .Union(seriesSkillsActiveSecret2.Where(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName2)).Select(v => v.ActiveSecret2))
                    .Union(seriesSkillsActiveSecret12.Where(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName1)).Select(v => v.ActiveSecret1))
                    .Union(seriesSkillsActiveSecret12.Where(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName2)).Select(v => v.ActiveSecret2))
                    .DefaultIfEmpty(false).Aggregate((acc, current) => acc || current);

                    var s = skillsHaveSecret.Single(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName2));
                    
                    //レベルが0以外 or 固定されたら
                    if ((item.SelectedItem.Value.SkillLevel != 0) || item.IsFixed.Value)
                    {
                        // 極意が発動時
                        if (item.SelectedSkill.Level >= item.SelectedSkill.MaxLevel1)
                        {
                            item.ActiveSecret1 = true;
                            s.SatisfySecret.Value |= true;
                        }
                        else
                        {
                            item.ActiveSecret1 = false;
                            s.SatisfySecret.Value = activeSecretSum;
                        }
                        item.IsSelected.Value = true;
                    }
                    else
                    {
                        item.IsSelected.Value = false;
                        item.ActiveSecret1 = false;
                        s.SatisfySecret.Value = activeSecretSum;
                    }
                });

                item.IsSelected.Subscribe(i =>
                {
                    if (i) item.BackgroundColor.Value = item.BackgroundColor.Value = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0EFFF"));
                    else item.BackgroundColor.Value = Brushes.White;
                });
            }

            foreach (var item in seriesSkillsActiveSecret2)
            {
                item.SelectedItem.Subscribe(isSelected =>
                {
                    var activeSecretSum = seriesSkillsActiveSecret1.Where(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName1)).Select(v => v.ActiveSecret1)
                    .Union(seriesSkillsActiveSecret2.Where(v => (item.SelectedSkill.Name != v.SelectedSkill.Name)).Where(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName2)).Select(v => v.ActiveSecret2))
                    .Union(seriesSkillsActiveSecret12.Where(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName1)).Select(v => v.ActiveSecret1))
                    .Union(seriesSkillsActiveSecret12.Where(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName2)).Select(v => v.ActiveSecret2))
                    .DefaultIfEmpty(false).Aggregate((acc, current) => acc || current);

                    var s = skillsHaveSecret.Single(v => (item.SelectedSkill.ActivateSkillName2 == v.SelectedSkill.ActivateSkillName2));
                    
                    //レベルが0以外 or 固定されたら
                    if ((item.SelectedItem.Value.SkillLevel != 0) || item.IsFixed.Value)
                    {
                        // 極意が発動時
                        if (item.SelectedSkill.Level >= item.SelectedSkill.MaxLevel2)
                        {
                            item.ActiveSecret2 = true;
                            s.SatisfySecret.Value |= true;
                        }
                        else
                        {
                            item.ActiveSecret2 = false;
                            s.SatisfySecret.Value = activeSecretSum;
                        }
                        item.IsSelected.Value = true;
                    }
                    else
                    {
                        item.IsSelected.Value = false;
                        item.ActiveSecret2 = false;
                        s.SatisfySecret.Value = activeSecretSum;
                    }
                });
                item.IsFixed.Subscribe(isSelected =>
                {
                    var activeSecretSum = seriesSkillsActiveSecret1.Where(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName1)).Select(v => v.ActiveSecret1)
                    .Union(seriesSkillsActiveSecret2.Where(v => (item.SelectedSkill.Name != v.SelectedSkill.Name)).Where(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName2)).Select(v => v.ActiveSecret2))
                    .Union(seriesSkillsActiveSecret12.Where(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName1)).Select(v => v.ActiveSecret1))
                    .Union(seriesSkillsActiveSecret12.Where(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName2)).Select(v => v.ActiveSecret2))
                    .DefaultIfEmpty(false).Aggregate((acc, current) => acc || current);

                    var s = skillsHaveSecret.Single(v => (item.SelectedSkill.ActivateSkillName2 == v.SelectedSkill.ActivateSkillName2));
                    
                    //レベルが0以外 or 固定されたら
                    if ((item.SelectedItem.Value.SkillLevel != 0) || item.IsFixed.Value)
                    {
                        // 極意が発動時
                        if (item.SelectedSkill.Level >= item.SelectedSkill.MaxLevel2)
                        {
                            item.ActiveSecret2 = true;
                            s.SatisfySecret.Value |= true;
                        }
                        else
                        {
                            item.ActiveSecret2 = false;
                            s.SatisfySecret.Value = activeSecretSum;
                        }
                        item.IsSelected.Value = true;
                    }
                    else
                    {
                        item.IsSelected.Value = false;
                        item.ActiveSecret2 = false;
                        s.SatisfySecret.Value = activeSecretSum;
                    }
                });

                item.IsSelected.Subscribe(i =>
                {
                    if (i) item.BackgroundColor.Value = item.BackgroundColor.Value = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0EFFF"));
                    else item.BackgroundColor.Value = Brushes.White;
                });
            }

            // どっちも極意
            foreach (var item in seriesSkillsActiveSecret12)
            {
                item.SelectedItem.Subscribe(isSelected =>
                {
                    var activeSecretSum = seriesSkillsActiveSecret1.Where(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName1)).Select(v => v.ActiveSecret1)
                    .Union(seriesSkillsActiveSecret2.Where(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName2)).Select(v => v.ActiveSecret2))
                    .Union(seriesSkillsActiveSecret12.Where(v => (item.SelectedSkill.Name != v.SelectedSkill.Name)).Where(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName1)).Select(v => v.ActiveSecret1))
                    .Union(seriesSkillsActiveSecret12.Where(v => (item.SelectedSkill.Name != v.SelectedSkill.Name)).Where(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName2)).Select(v => v.ActiveSecret2))
                    .DefaultIfEmpty(false).Aggregate((acc, current) => acc || current);

                    var s1 = skillsHaveSecret.Single(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName2));
                    var s2 = skillsHaveSecret.Single(v => (item.SelectedSkill.ActivateSkillName2 == v.SelectedSkill.ActivateSkillName2));
                    
                    //レベルが0以外 or 固定されたら
                    if ((item.SelectedItem.Value.SkillLevel != 0) || item.IsFixed.Value)
                    {
                        // 極意が発動時
                        if (item.SelectedSkill.Level >= item.SelectedSkill.MaxLevel1)
                        {
                            // どっちも発動
                            if (item.SelectedSkill.Level >= item.SelectedSkill.MaxLevel2)
                            {
                                item.ActiveSecret1 = true;
                                item.ActiveSecret2 = true;
                                s1.SatisfySecret.Value |= true;
                                s2.SatisfySecret.Value |= true;
                            }
                            // レベルが少ない方だけ発動
                            else
                            {
                                item.ActiveSecret1 = true;
                                item.ActiveSecret2 = false;
                                s1.SatisfySecret.Value |= true;
                                s2.SatisfySecret.Value = activeSecretSum;
                            }
                        }
                        // 発動なし
                        else
                        {
                            item.ActiveSecret1 = false;
                            item.ActiveSecret2 = false;
                            s1.SatisfySecret.Value = activeSecretSum;
                            s2.SatisfySecret.Value = activeSecretSum;
                        }
                        item.IsSelected.Value = true;
                    }
                    else
                    {
                        item.IsSelected.Value = false;
                        item.ActiveSecret1 = false;
                        item.ActiveSecret2 = false;
                        s1.SatisfySecret.Value = activeSecretSum;
                        s2.SatisfySecret.Value = activeSecretSum;
                    }
                });
                item.IsFixed.Subscribe(isSelected =>
                {
                    var activeSecretSum = seriesSkillsActiveSecret1.Where(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName1)).Select(v => v.ActiveSecret1)
                    .Union(seriesSkillsActiveSecret2.Where(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName2)).Select(v => v.ActiveSecret2))
                    .Union(seriesSkillsActiveSecret12.Where(v => (item.SelectedSkill.Name != v.SelectedSkill.Name)).Where(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName1)).Select(v => v.ActiveSecret1))
                    .Union(seriesSkillsActiveSecret12.Where(v => (item.SelectedSkill.Name != v.SelectedSkill.Name)).Where(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName2)).Select(v => v.ActiveSecret2))
                    .DefaultIfEmpty(false).Aggregate((acc, current) => acc || current);

                    var s1 = skillsHaveSecret.Single(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName2));
                    var s2 = skillsHaveSecret.Single(v => (item.SelectedSkill.ActivateSkillName2 == v.SelectedSkill.ActivateSkillName2));

                    //レベルが0以外 or 固定されたら
                    if ((item.SelectedItem.Value.SkillLevel != 0) || item.IsFixed.Value)
                    {
                        // 極意が発動時
                        if (item.SelectedSkill.Level >= item.SelectedSkill.MaxLevel1)
                        {
                            // どっちも発動
                            if (item.SelectedSkill.Level >= item.SelectedSkill.MaxLevel2)
                            {
                                item.ActiveSecret1 = true;
                                item.ActiveSecret2 = true;
                                s1.SatisfySecret.Value |= true;
                                s2.SatisfySecret.Value |= true;
                            }
                            // レベルが少ない方だけ発動
                            else
                            {
                                item.ActiveSecret1 = true;
                                item.ActiveSecret2 = false;
                                s1.SatisfySecret.Value |= true;
                                s2.SatisfySecret.Value = activeSecretSum;
                            }
                        }
                        // 発動なし
                        else
                        {
                            item.ActiveSecret1 = false;
                            item.ActiveSecret2 = false;
                            s1.SatisfySecret.Value |= false;
                            s2.SatisfySecret.Value = activeSecretSum;
                        }
                        item.IsSelected.Value = true;
                    }
                    else
                    {
                        item.IsSelected.Value = false;
                        item.ActiveSecret1 = false;
                        item.ActiveSecret2 = false;
                        s1.SatisfySecret.Value |= false;
                        s2.SatisfySecret.Value = activeSecretSum;
                    }
                });

                item.IsSelected.Subscribe(i =>
                {
                    if (i) item.BackgroundColor.Value = item.BackgroundColor.Value = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0EFFF"));
                    else item.BackgroundColor.Value = Brushes.White;
                });
            }

            // 極意があるスキル
            foreach (var item in skillsHaveSecret)
            {
                item.SelectedItem.Subscribe(isSelected =>
                {
                    //レベルが0以外 or 固定されたら
                    if ((item.SelectedItem.Value.SkillLevel != 0) || item.IsFixed.Value)
                    {
                        // 対応するシリーズスキル
                        var s1 = seriesSkillsActiveSecret1.Where(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName2)).Select(v => v.ActiveSecret1);
                        var s2 = seriesSkillsActiveSecret2.Where(v => (item.SelectedSkill.ActivateSkillName2 == v.SelectedSkill.ActivateSkillName2)).Select(v => v.ActiveSecret2);
                        var s121 = seriesSkillsActiveSecret12.Where(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName2)).Select(v => v.ActiveSecret1);
                        var s122 = seriesSkillsActiveSecret12.Where(v => (item.SelectedSkill.ActivateSkillName2 == v.SelectedSkill.ActivateSkillName2)).Select(v => v.ActiveSecret2);


                        // 極意が必要な場合
                        if (item.SelectedSkill.Level > item.SelectedSkill.MaxLevel1)
                        {
                            // 一つでも極意が発動している場合
                            if (s1.Union(s2).Union(s121).Union(s122).DefaultIfEmpty(false).Aggregate((acc, current) => acc || current))
                            {
                                item.SatisfySecret.Value = true;
                            }
                            else
                            {
                                item.SatisfySecret.Value = false;
                            }
                        }
                        else
                        {
                            item.SatisfySecret.Value = true;
                        }
                        item.IsSelected.Value = true;
                    }
                    else
                    {
                        item.IsSelected.Value = false;
                        item.SatisfySecret.Value = true;
                    }
                });
                item.IsFixed.Subscribe(isSelected =>
                {
                    //レベルが0以外 or 固定されたら
                    if ((item.SelectedItem.Value.SkillLevel != 0) || item.IsFixed.Value)
                    {
                        // 対応するシリーズスキル
                        var s1 = seriesSkillsActiveSecret1.Where(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName2)).Select(v => v.ActiveSecret1);
                        var s2 = seriesSkillsActiveSecret2.Where(v => (item.SelectedSkill.ActivateSkillName2 == v.SelectedSkill.ActivateSkillName2)).Select(v => v.ActiveSecret2);
                        var s121 = seriesSkillsActiveSecret12.Where(v => (item.SelectedSkill.ActivateSkillName1 == v.SelectedSkill.ActivateSkillName2)).Select(v => v.ActiveSecret1);
                        var s122 = seriesSkillsActiveSecret12.Where(v => (item.SelectedSkill.ActivateSkillName2 == v.SelectedSkill.ActivateSkillName2)).Select(v => v.ActiveSecret2);


                        // 極意が必要な場合
                        if (item.SelectedSkill.Level > item.SelectedSkill.MaxLevel1)
                        {
                            // 一つでも極意が発動している場合
                            if (s1.Union(s2).Union(s121).Union(s122).DefaultIfEmpty(false).Aggregate((acc, current) => acc || current))
                            {
                                item.SatisfySecret.Value = true;
                            }
                            else
                            {
                                item.SatisfySecret.Value = false;
                            }
                        }
                        else
                        {
                            item.SatisfySecret.Value = true;
                        }
                        item.IsSelected.Value = true;
                    }
                    else
                    {
                        item.IsSelected.Value = false;
                        item.SatisfySecret.Value = true;
                    }
                });

                item.IsSelected.Subscribe(i =>
                {
                    if (i)
                    {
                        if (item.SatisfySecret.Value)
                        {
                            item.BackgroundColor.Value = item.BackgroundColor.Value = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0EFFF"));
                        }
                        else
                        {
                            item.BackgroundColor.Value = item.BackgroundColor.Value = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FADBD8"));
                        }
                    }
                    else item.BackgroundColor.Value = Brushes.White;
                });
                item.SatisfySecret.Subscribe(i =>
                {
                    if (i)
                    {
                        if (item.IsSelected.Value)
                        {
                            item.BackgroundColor.Value = item.BackgroundColor.Value = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0EFFF"));
                        }
                        else item.BackgroundColor.Value = Brushes.White;
                    }
                    else
                    {
                        if (item.IsSelected.Value)
                        {
                            item.BackgroundColor.Value = item.BackgroundColor.Value = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FADBD8"));
                        }
                        else item.BackgroundColor.Value = Brushes.White;
                    }
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
                    if (i) item.BackgroundColor.Value = item.BackgroundColor.Value = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0EFFF"));
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
            condition.Secret = b;
            return condition;
        }
    }
}
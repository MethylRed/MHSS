using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using MHSS.Models.Data;
using Reactive.Bindings;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Reactive.Disposables;

namespace MHSS.ViewModels.Controls
{
    internal class SkillLevelSelectorViewModel : SubViewModelBase
    {
        /// <summary>
        /// ComboBoxに表示するアイテムリスト
        /// </summary>
        public ReactivePropertySlim<ObservableCollection<SkillLevelSelectorItems>> Items { get; } = new();

        /// <summary>
        /// 被選択アイテム
        /// </summary>
        public ReactivePropertySlim<SkillLevelSelectorItems> SelectedItem { get; } = new();

        /// <summary>
        /// スキルレベル固定有無
        /// </summary>
        public ReactivePropertySlim<bool> IsFixed { get; } = new(false);

        /// <summary>
        /// 初期値以外の選択有無
        /// </summary>
        public ReactivePropertySlim<bool> IsSelected { get; } = new(false);

        /// <summary>
        /// 選択状態  0:非選択 1:選択(OK) 2:選択(NG)
        /// </summary>
        //public ReactivePropertySlim<int> SelectedState { get; } = new(0);

        /// <summary>
        /// 極意が発動しているか
        /// </summary>
        public bool ActiveSecret1 { get; set; } = true;

        /// <summary>
        /// 極意が発動しているか
        /// </summary>
        public bool ActiveSecret2 { get; set; } = true;

        /// <summary>
        /// 極意による上限解放が必要なレベルを指定していて、極意が発動しているかどうか
        /// </summary>
        public ReactivePropertySlim<bool> SatisfySecret { get; set; } = new(true);

        /// <summary>
        /// 背景色
        /// </summary>
        public ReactivePropertySlim<SolidColorBrush> BackgroundColor { get; } = new(Brushes.White);

        /// <summary>
        /// スキル名
        /// </summary>
        public string SkillName { get; set; }

        /// <summary>
        /// 選択されたスキル
        /// </summary>
        internal Skill SelectedSkill
        {
            get
            {
                var s = Master.Skills.Single(s => s.Name == SkillName);
                return new Skill() { Name = SkillName,
                                     Category = s.Category,
                                     Level = SelectedItem.Value.SkillLevel,
                                     ActivateSkillName1 = s.ActivateSkillName1,
                                     ActivateSkillName2 = s.ActivateSkillName2,
                                     MaxLevel1 = s.MaxLevel1,
                                     MaxLevel2 = s.MaxLevel2,
                                     IsFixed = IsFixed.Value };
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="skillName">スキル名</param>
        public SkillLevelSelectorViewModel(string skillName)
        {
            SkillName = skillName;

            // ComboBoxのItemを作成
            //Skill s = Master.Skills.Where(x => x.Name == SkillName).First();
            Skill s = Master.Skills.Single(x => x.Name == SkillName);
            ObservableCollection<SkillLevelSelectorItems> items = new()
            {
                // Lv0はわざわざLv0と書かない
                new SkillLevelSelectorItems(" " + s.Name, 0)
            };
            // シリーズスキルのとき
            // 発動スキルが書かれてないと消えちゃうので発動スキルが書かれてることも条件
            if ((s.Category == "シリーズスキル") && (s.ActivateSkillName1 != string.Empty))
            {
                items.Add(new SkillLevelSelectorItems($"{s.ActivateSkillName1}({s.Name}+{s.MaxLevel1})", s.MaxLevel1));
                if (s.ActivateSkillName2 != string.Empty)
                {
                    items.Add(new SkillLevelSelectorItems($"{s.ActivateSkillName2}({s.Name}+{s.MaxLevel2})", s.MaxLevel2));
                }
            }
            else
            {
                for (int i = 1; i <= s.MaxLevel1; i++)
                {
                    // スキル名+Lvを表示
                    items.Add(new SkillLevelSelectorItems($"{s.Name} Lv{i}", i));
                }
                for (int i = s.MaxLevel1 + 1; i<= s.MaxLevel2; i++)
                {
                    // 極意があるとき
                    items.Add(new SkillLevelSelectorItems($"{s.Name} Lv{i}({s.ActivateSkillName2})".Replace("()", ""), i));

                }
            }
            Items.Value = items;


            // 選択されたスキルの情報の初期値
            SelectedItem.Value = Items.Value.First();

            //// スキルレベルか固定有無が変更されたら
            //// IsSelected = True ：スキルレベルが0以外 or レベル固定
            //// IsSelected = False：スキルレベルが0 and レベル非固定
            //SelectedItem.Subscribe(isSelected =>
            //{
            //    if ((SelectedItem.Value.SkillLevel != 0) || IsFixed.Value)
            //    {
            //        //IsSelected.Value = true;
            //        if ((SelectedSkill.Level > SelectedSkill.MaxLevel1) &&
            //            (SelectedSkill.Category != "シリーズスキル") &&
            //            (SelectedSkill.ActivateSkillName2 != ""))
            //        {
            //            SelectedState.Value = 2;
            //            JudgeSecret = false;
            //        }
            //        else
            //        {
            //            SelectedState.Value = 1;
            //            JudgeSecret = true;
            //        }
            //    }
            //    else
            //    {
            //        //IsSelected.Value = false;
            //        SelectedState.Value = 0;
            //        JudgeSecret = true;
            //    }
            //});
            //IsFixed.Subscribe(isFixed =>
            //{
            //    if ((SelectedItem.Value.SkillLevel != 0) || IsFixed.Value)
            //    {
            //        //IsSelected.Value = true;
            //        if ((SelectedSkill.Level > SelectedSkill.MaxLevel1) &&
            //            (SelectedSkill.Category != "シリーズスキル") &&
            //            (SelectedSkill.ActivateSkillName2 != ""))
            //        {
            //            SelectedState.Value = 2;
            //            JudgeSecret = false;
            //        }
            //        else
            //        {
            //            SelectedState.Value = 1;
            //            JudgeSecret = true;
            //        }
            //    }
            //    else
            //    {
            //        //IsSelected.Value = false;
            //        SelectedState.Value = 0;
            //        JudgeSecret = true;
            //    }
            //});

            //SelectedState.Subscribe(i =>
            //{
            //    switch (i)
            //    {
            //        case 0: BackgroundColor.Value = Brushes.White; break;
            //        case 1: BackgroundColor.Value = BackgroundColor.Value = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0EFFF")); break;
            //        case 2: BackgroundColor.Value = BackgroundColor.Value = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FADBD8")); break;
            //    }
            //});

        }
    }
}

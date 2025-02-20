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
            string displayName = "";

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
                displayName = $"{s.ActivateSkillName1}({s.Name}+{s.MaxLevel1})";
                items.Add(new SkillLevelSelectorItems(displayName, s.MaxLevel1));
                if (s.ActivateSkillName2 != string.Empty)
                {
                    displayName = $"{s.ActivateSkillName2}({s.Name}+{s.MaxLevel2})";
                    items.Add(new SkillLevelSelectorItems(displayName, s.MaxLevel2));
                }
            }
            else
            {
                for (int i = 1; i <= s.MaxLevel2; i++)
                {
                    // スキル名+Lvを表示
                    displayName = s.Name + " Lv" + i.ToString();
                    items.Add(new SkillLevelSelectorItems(displayName, i));
                }
            }
            Items.Value = items;


            // 選択されたスキルの情報の初期値
            SelectedItem.Value = Items.Value.First();

            // スキルレベルか固定有無が変更されたら
            // IsSelected = True ：スキルレベルが0以外 or レベル固定
            // IsSelected = False：スキルレベルが0 and レベル非固定
            SelectedItem.Subscribe(isSelected =>
            {
                IsSelected.Value = (SelectedItem.Value.SkillLevel != 0) || IsFixed.Value;
            });
            IsFixed.Subscribe(isFixed =>
            {
                IsSelected.Value = (SelectedItem.Value.SkillLevel != 0) || IsFixed.Value;
            });

            // スキルレベルが0以外 or レベル固定のとき、ComboBoxの色を変更する
            IsSelected.Subscribe(b =>
            {
                if (b)
                {
                    BackgroundColor.Value = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0EFFF"));
                }
                else
                {
                    BackgroundColor.Value = Brushes.White;
                }
            });
        }
    }
}

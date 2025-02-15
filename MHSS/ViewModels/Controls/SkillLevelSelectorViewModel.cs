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
        /// 初期値以外が選択されているか
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


        internal Skill SelectedSkill
        {
            get
            {
                return new Skill() { Name = SkillName, Level = SelectedItem.Value.SkillLevel };
            }
        }

        public SkillLevelSelectorViewModel(string skillName)
        {
            SkillName = skillName;

            // ComboBoxのItemを作成
            Skill s = Master.Skills.Where(x => x.Name == SkillName).First();
            ObservableCollection<SkillLevelSelectorItems> items = new()
            {
                new SkillLevelSelectorItems(" " + s.Name, 0)
            };
            for (int i = 1; i <= s.MaxLevel2; i++)
            {
                string displayName = s.Name + " Lv" + i.ToString();
                items.Add(new SkillLevelSelectorItems(displayName, i));
            }
            Items.Value = items;

            // 選択されたスキルの情報の初期値
            SelectedItem.Value = Items.Value.First();

            SelectedItem.Subscribe(isSelected => { IsSelected.Value = SelectedItem.Value.SkillLevel != 0; });
            IsSelected.Subscribe(b =>
            {
                if (b) // 初期値以外が選択されたとき
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

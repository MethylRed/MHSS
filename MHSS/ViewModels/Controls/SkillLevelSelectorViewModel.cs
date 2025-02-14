using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using MHSS.Models.Data;
using Reactive.Bindings;
using System.Collections.ObjectModel;

namespace MHSS.ViewModels.Controls
{
    internal class SkillLevelSelectorViewModel : BindableBase
    {
        /// <summary>
        /// スキル
        /// </summary>
        public Skill Skill { get; set; }

        public ReactivePropertySlim<ObservableCollection<SkillLevelSelectorItems>> Items { get; } = new();

        /// <summary>
        /// 被選択スキル
        /// </summary>
        public ReactivePropertySlim<SkillLevelSelectorItems> SelectedSkill { get; set; }



        public SkillLevelSelectorViewModel(Skill skill)
        {
            Skill = skill;

            // ComboBoxのItemを作成
            ObservableCollection<SkillLevelSelectorItems> items = new()
            {
                new SkillLevelSelectorItems(skill.Name, 0)
            };
            for (int i = 1; i <= skill.MaxLevel2; i++)
            {
                string displayName = skill.Name + " Lv" + i.ToString();
                items.Add(new SkillLevelSelectorItems(displayName, i));
            }
            Items.Value = items;

            // 選択されたスキルの情報の初期値
            SelectedSkill.Value = Items.Value.First();
        }


        ///// <summary>
        ///// 選択されているスキルを返却
        ///// </summary>
        //internal Skill SelectedSkill
        //{
        //    get
        //    {
        //        return new Skill(SkillName, SelectedLevel.Value.Level, isFixed: IsFixDisp.Value == FixStr);
        //    }
        //}
    }
}

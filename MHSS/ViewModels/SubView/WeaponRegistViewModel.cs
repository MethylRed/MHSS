using MHSS.Models.Config;
using MHSS.Models.Data;
using MHSS.Models.Utility;
using MHSS.ViewModels.Controls;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MHSS.ViewModels.SubView
{
    internal class WeaponRegistViewModel : SubViewModelBase
    {
        /// <summary>
        /// 登録武器アイテムのViewModels
        /// </summary>
        public ReactivePropertySlim<ObservableCollection<WeaponSelectItemViewModel>> WeaponRegistItemVMs { get; set; } = new(new());

        /// <summary>
        /// 登録コマンド
        /// </summary>
        public ReactiveCommand RegistCommand { get; } = new();

        /// <summary>
        /// 名前
        /// </summary>
        public ReactivePropertySlim<string> Name { get; } = new("");

        /// <summary>
        /// 武器種選択ComboBoxに表示するアイテムリスト
        /// </summary>
        public ReactivePropertySlim<ObservableCollection<string>> WeaponKindSelectItems { get; } = new();

        /// <summary>
        /// 選択された武器種
        /// </summary>
        public ReactivePropertySlim<string> SelectedWeaponKind { get; } = new();

        /// <summary>
        /// 攻撃力
        /// </summary>
        public ReactivePropertySlim<string> Attack { get; } = new();

        /// <summary>
        /// 会心率
        /// </summary>
        public ReactivePropertySlim<string> Affinity { get; } = new();

        /// <summary>
        /// 防御力
        /// </summary>
        public ReactivePropertySlim<string> DefBonus { get; } = new();

        /// <summary>
        /// 属性1選択ComboBoxに表示するアイテムリスト
        /// </summary>
        public ReactivePropertySlim<ObservableCollection<string>> ElementSelectItems1 { get; } = new();

        /// <summary>
        /// 選択された属性1
        /// </summary>
        public ReactivePropertySlim<string> SelectedElement1 { get; } = new();

        /// <summary>
        /// 属性値1
        /// </summary>
        public ReactivePropertySlim<string> ElementValue1 { get; } = new();

        /// <summary>
        /// 属性2選択ComboBoxに表示するアイテムリスト
        /// </summary>
        public ReactivePropertySlim<ObservableCollection<string>> ElementSelectItems2 { get; } = new();

        /// <summary>
        /// 選択された属性2
        /// </summary>
        public ReactivePropertySlim<string> SelectedElement2 { get; } = new();

        /// <summary>
        /// 属性値2
        /// </summary>
        public ReactivePropertySlim<string> ElementValue2 { get; } = new();

        /// <summary>
        /// スロット1
        /// </summary>
        public ReactivePropertySlim<string> Slot1 { get; } = new();

        /// <summary>
        /// スロット2
        /// </summary>
        public ReactivePropertySlim<string> Slot2 { get; } = new();

        /// <summary>
        /// スロット3
        /// </summary>
        public ReactivePropertySlim<string> Slot3 { get; } = new();

        /// <summary>
        /// スキル1選択ComboBoxに表示するアイテムリスト
        /// </summary>
        public ReactivePropertySlim<ObservableCollection<string>> SkillSelectItems1 { get; } = new();

        /// <summary>
        /// 選択されたスキル1
        /// </summary>
        public ReactivePropertySlim<string> SelectedSkill1 { get; } = new();

        /// <summary>
        /// スキル値1
        /// </summary>
        public ReactivePropertySlim<string> SkillLevel1 { get; } = new();

        /// <summary>
        /// スキル2選択ComboBoxに表示するアイテムリスト
        /// </summary>
        public ReactivePropertySlim<ObservableCollection<string>> SkillSelectItems2 { get; } = new();

        /// <summary>
        /// 選択されたスキル2
        /// </summary>
        public ReactivePropertySlim<string> SelectedSkill2 { get; } = new();

        /// <summary>
        /// スキル値2
        /// </summary>
        public ReactivePropertySlim<string> SkillLevel2 { get; } = new();

        /// <summary>
        /// スキル3選択ComboBoxに表示するアイテムリスト
        /// </summary>
        public ReactivePropertySlim<ObservableCollection<string>> SkillSelectItems3 { get; } = new();

        /// <summary>
        /// 選択されたスキル3
        /// </summary>
        public ReactivePropertySlim<string> SelectedSkill3 { get; } = new();

        /// <summary>
        /// スキル値3
        /// </summary>
        public ReactivePropertySlim<string> SkillLevel3 { get; } = new();

        /// <summary>
        /// スキル4選択ComboBoxに表示するアイテムリスト
        /// </summary>
        public ReactivePropertySlim<ObservableCollection<string>> SkillSelectItems4 { get; } = new();

        /// <summary>
        /// 選択されたスキル4
        /// </summary>
        public ReactivePropertySlim<string> SelectedSkill4 { get; } = new();

        /// <summary>
        /// スキル値4
        /// </summary>
        public ReactivePropertySlim<string> SkillLevel4 { get; } = new();



        /// <summary>
        /// 背景色
        /// </summary>
        public ReactivePropertySlim<SolidColorBrush> BackgroundColor { get; } = new(Brushes.White);

        public WeaponRegistViewModel()
        {
            // 武器種選択ComboBox表示用アイテムを作成
            ObservableCollection<string> items = new();
            foreach (WeaponKind i in Enum.GetValues(typeof(WeaponKind)))
            {
                items.Add(Kind.WeaponKindsToString(i));
            }
            WeaponKindSelectItems.Value = items;
            SelectedWeaponKind.Value = WeaponKindSelectItems.Value.First();


            // 属性1選択ComboBox表示用アイテムを作成
            items = new();
            foreach (var i in Kind.ElementType.Keys)
            {
                items.Add(i);
            }
            ElementSelectItems1.Value = items;
            SelectedElement1.Value = ElementSelectItems1.Value.First();

            // 属性2選択ComboBox表示用アイテムを作成
            items = new();
            foreach (var i in Kind.ElementType.Keys)
            {
                items.Add(i);
            }
            ElementSelectItems2.Value = items;
            SelectedElement2.Value = ElementSelectItems2.Value.First();

            // スキル1選択ComboBox表示用アイテムを作成
            items = new();
            items.Add("");
            foreach (var i in Master.Skills)
            {
                items.Add(i.Name);
            }
            SkillSelectItems1.Value = items;
            SkillSelectItems2.Value = items;
            SkillSelectItems3.Value = items;
            SkillSelectItems4.Value = items;
            SelectedSkill1.Value = SkillSelectItems1.Value.First();
            SelectedSkill2.Value = SkillSelectItems2.Value.First();
            SelectedSkill3.Value = SkillSelectItems3.Value.First();
            SelectedSkill4.Value = SkillSelectItems4.Value.First();

            RegistCommand.Subscribe(() =>
            {
                // 同じ名前の武器があったら
                if (Master.Weapons.SelectMany(w => w).Union(Master.Weapons.SelectMany(w => w)).Any(w => w.Name == Name.Value))
                {
                    MessageBox.Show("この名前の武器は既に存在します。", "エラー", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                }
                else if (string.IsNullOrWhiteSpace(Name.Value))
                {
                    MessageBox.Show("名前を入力してください。", "エラー", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                }
                else
                {
                    WeaponRegistItemVMs.Value.Insert(0, new(GetWeapon(), false));
                    Master.AddWeapons[(int)Kind.WeaponNameToKind(SelectedWeaponKind.Value)].Add(GetWeapon());
                }
            });

            foreach (var weapon in Master.AddWeapons.SelectMany(w => w))
            {
                WeaponRegistItemVMs.Value.Add(new(weapon, false));
            }
        }

        private Weapon GetWeapon()
        {
            Weapon weapon = new()
            {
                EquipKind = EquipKind.Weapon,
                WeaponKind = Kind.WeaponNameToKind(SelectedWeaponKind.Value),
                Name = Name.Value,
                SeriesName = "",
                Attack = Utility.ParseOrDefault(Attack.Value),
                Affinity = Utility.ParseOrDefault(Affinity.Value),
                ElementType1 = (Element)Kind.ElementType[SelectedElement1.Value],
                ElementValue1 = Utility.ParseOrDefault(ElementValue1.Value),
                ElementType2 = (Element)Kind.ElementType[SelectedElement2.Value],
                ElementValue2 = Utility.ParseOrDefault(ElementValue2.Value),
                SlotType = 0,
                Slot1 = Utility.ParseOrDefault(Slot1.Value),
                Slot2 = Utility.ParseOrDefault(Slot2.Value),
                Slot3 = Utility.ParseOrDefault(Slot3.Value),
                DefBonus = Utility.ParseOrDefault(DefBonus.Value),
                UniqueStatus = ""
            };
            List<Skill> skill = new();
            if (!string.IsNullOrWhiteSpace(SelectedSkill1.Value))
            {
                skill.Add(new Skill
                {
                    Name = SelectedSkill1.Value,
                    Level = Utility.ParseOrDefault(SkillLevel1.Value)
                });
            }

            if (!string.IsNullOrWhiteSpace(SelectedSkill2.Value))
            {
                skill.Add(new Skill
                {
                    Name = SelectedSkill2.Value,
                    Level = Utility.ParseOrDefault(SkillLevel2.Value)
                });
            }
            if (!string.IsNullOrWhiteSpace(SelectedSkill3.Value))
            {
                skill.Add(new Skill
                {
                    Name = SelectedSkill3.Value,
                    Level = Utility.ParseOrDefault(SkillLevel3.Value)
                });
            }
            if (!string.IsNullOrWhiteSpace(SelectedSkill4.Value))
            {
                skill.Add(new Skill
                {
                    Name = SelectedSkill4.Value,
                    Level = Utility.ParseOrDefault(SkillLevel4.Value)
                });
            }
            weapon.Skills = skill;

            return weapon;
        }
    }
}

using MHSS.Models.Data;
using MHSS.ViewModels.Controls;
using MHSS.Views.Controls;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MHSS.ViewModels.SubView
{
    internal class WeaponSelectViewModel : SubViewModelBase
    {
        /// <summary>
        /// 武器選択アイテムのViewModels
        /// </summary>
        public ReactivePropertySlim<ObservableCollection<WeaponSelectItemViewModel>> WeaponSelectItemVMs { get; } = new(new());

        /// <summary>
        /// 武器選択アイテムのViewModel
        /// </summary>
        public ReactivePropertySlim<WeaponSelectItemViewModel> WeaponSelectItemVM { get; } = new();

        /// <summary>
        /// 武器種選択ComboBoxに表示するアイテムリスト
        /// </summary>
        public ReactivePropertySlim<ObservableCollection<string>> WeaponKindSelectItems { get; } = new();

        /// <summary>
        /// スキル選択ComboBoxに表示するアイテムリスト
        /// </summary>
        public ReactivePropertySlim<ObservableCollection<string>> SkillSelectItems { get; } = new();

        /// <summary>
        /// 属性選択ComboBoxに表示するアイテムリスト
        /// </summary>
        public ReactivePropertySlim<ObservableCollection<string>> ElementSelectItems { get; } = new();

        /// <summary>
        /// 選択された武器種
        /// </summary>
        public ReactivePropertySlim<string> SelectedWeaponKind { get; } = new();

        /// <summary>
        /// 選択されたスキル
        /// </summary>
        public ReactivePropertySlim<string> SelectedSkillName { get; } = new();

        /// <summary>
        /// 選択された属性
        /// </summary>
        public ReactivePropertySlim<string> SelectedElement { get; } = new();

        /// <summary>
        /// スキル選択の有効/無効
        /// </summary>
        public ReactivePropertySlim<bool> IsEnabledSkillSelect { get; set; } = new(false);

        /// <summary>
        /// 属性選択の有効/無効
        /// </summary>
        public ReactivePropertySlim<bool> IsEnabledElementSelect { get; set; } = new(false);


        public ReactiveCommand ClearCommand { get; } = new();

        public Weapon Weapon { get; set; } = new();

        /// <summary>
        /// 背景色
        /// </summary>
        public ReactivePropertySlim<SolidColorBrush> BackgroundColor { get; } = new(Brushes.White);

        /// <summary>
        /// 武器が持つスキルのみのリスト
        /// </summary>
        //public static List<string> SkillNamesWithWeapon => Master.Weapons.SelectMany(w => w).SelectMany(w => w.Skills).Select(x => x.Name).Distinct().ToList();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WeaponSelectViewModel()
        {
            ClearCommand.Subscribe(() =>
            {
                WeaponSelectItemVM.Value = new(new());
                Weapon = new Weapon();
            });

            // 武器種選択ComboBox表示用アイテムを作成
            ObservableCollection<string> items = new();
            items.Add("---");
            foreach (WeaponKind i in Enum.GetValues(typeof(WeaponKind)))
            {
                items.Add(Kind.WeaponKindsToString(i));
            }
            WeaponKindSelectItems.Value = items;
            SelectedWeaponKind.Value = WeaponKindSelectItems.Value.First();


            // スキル選択ComboBox表示用アイテムを作成
            items = new();
            items.Add("---");
            SkillSelectItems.Value = items;
            SelectedSkillName.Value = SkillSelectItems.Value.First();
            SelectedSkillName.Subscribe(selected =>
            {
                WeaponSelectItemVMs.Value.Clear();
                if (selected == "---")
                {
                    if (SelectedWeaponKind.Value == "---") return;
                    var x = SelectedElement.Value == "---"
                    ? Master.Weapons[(int)Kind.WeaponNameToKind(SelectedWeaponKind.Value)]
                    : Master.Weapons[(int)Kind.WeaponNameToKind(SelectedWeaponKind.Value)]
                          .Where(w => w.Skills.Any(s => s.Name == selected));
                    foreach (var weapon in x)
                    {
                        WeaponSelectItemVMs.Value.Add(new(weapon));
                    }
                }
                else
                {
                    if (SelectedWeaponKind.Value == "---") return;
                    var x = SelectedElement.Value == "---"
                    ? Master.Weapons[(int)Kind.WeaponNameToKind(SelectedWeaponKind.Value)]
                          .Where(w => w.Skills.Any(s => s.Name == selected))
                    : Master.Weapons[(int)Kind.WeaponNameToKind(SelectedWeaponKind.Value)]
                          .Where(w => w.Skills.Any(s => s.Name == selected))
                          .Where(w => w.ElementType1 == (Element)Kind.ElementType[SelectedElement.Value] ||
                                      w.ElementType2 == (Element)Kind.ElementType[SelectedElement.Value]);
                    foreach (var weapon in x)
                    {
                        WeaponSelectItemVMs.Value.Add(new(weapon));
                    }
                }
            });


            // 属性選択ComboBox表示用アイテムを作成
            items = new();
            items.Add("---");
            foreach (var i in Kind.ElementType.Keys)
            {
                items.Add(i);
            }
            ElementSelectItems.Value = items;
            SelectedElement.Value = ElementSelectItems.Value.First();
            SelectedElement.Subscribe(selected =>
            {
                WeaponSelectItemVMs.Value.Clear();
                if (selected == "---")
                {
                    if (SelectedWeaponKind.Value == "---") return;
                    var x = SelectedSkillName.Value == "---"
                    ? Master.Weapons[(int)Kind.WeaponNameToKind(SelectedWeaponKind.Value)]
                    : Master.Weapons[(int)Kind.WeaponNameToKind(SelectedWeaponKind.Value)]
                          .Where(w => w.Skills.Any(s => s.Name == selected));
                    foreach (var weapon in x)
                    {
                        WeaponSelectItemVMs.Value.Add(new(weapon));
                    }
                }
                else
                {
                    if (SelectedWeaponKind.Value == "---") return;
                    var x = SelectedSkillName.Value == "---"
                    ? Master.Weapons[(int)Kind.WeaponNameToKind(SelectedWeaponKind.Value)]
                          .Where(w => w.ElementType1 == (Element)Kind.ElementType[SelectedElement.Value] ||
                                      w.ElementType2 == (Element)Kind.ElementType[SelectedElement.Value])
                    : Master.Weapons[(int)Kind.WeaponNameToKind(SelectedWeaponKind.Value)]
                          .Where(w => w.Skills.Any(s => s.Name == selected))
                          .Where(w => w.ElementType1 == (Element)Kind.ElementType[SelectedElement.Value] ||
                                      w.ElementType2 == (Element)Kind.ElementType[SelectedElement.Value]);
                    foreach (var weapon in x)
                    {
                        WeaponSelectItemVMs.Value.Add(new(weapon));
                    }
                }
            });

            SelectedWeaponKind.Subscribe(selected =>
            {
                WeaponSelectItemVMs.Value.Clear();

                // 武器指定なし
                if (selected == "---")
                {
                    items = new();
                    items.Add("---");
                    SelectedElement.Value = ElementSelectItems.Value.First();
                    IsEnabledSkillSelect.Value = false;
                    IsEnabledElementSelect.Value = false;
                }
                else
                {
                    items = new();
                    items.Add("---");
                    foreach (var i in SkillNamesWithWeapon(selected))
                    {
                        items.Add(i);
                    }
                    foreach (var weapon in Master.Weapons[(int)Kind.WeaponNameToKind(selected)])
                    {
                        WeaponSelectItemVMs.Value.Add(new(weapon));
                    }
                    IsEnabledSkillSelect.Value = true;
                    IsEnabledElementSelect.Value = true;
                }
                SkillSelectItems.Value = items;
                SelectedSkillName.Value = SkillSelectItems.Value.First();
            });
        }

        private List<string> SkillNamesWithWeapon(string weaponKind)
        {
            return Master.Weapons[(int)Kind.WeaponNameToKind(weaponKind)].SelectMany(w => w.Skills).Select(x => x.Name).Distinct().ToList();
        }
    }
}

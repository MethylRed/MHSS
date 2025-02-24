using MHSS.Models.Data;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MHSS.ViewModels.SubView
{
    internal class WeaponSelectViewModel : SubViewModelBase
    {
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
            // 武器種選択ComboBox表示用アイテムを作成
            ObservableCollection<string> items = new();
            items.Add("---");
            foreach (WeaponKind i in Enum.GetValues(typeof(WeaponKind)))
            {
                items.Add(Kind.WeaponKindsToString(i));
            }
            WeaponKindSelectItems.Value = items;
            // 初期値
            SelectedWeaponKind.Value = WeaponKindSelectItems.Value.First();
            SelectedWeaponKind.Subscribe(selected =>
            {
                // 武器指定なし
                if (selected == "---")
                {
                    items = new();
                    items.Add("---");
                }
                else
                {
                    items = new();
                    items.Add("---");
                    foreach (var i in SkillNamesWithWeapon(selected))
                    {
                        items.Add(i);
                    }
                    SkillSelectItems.Value = items;
                    // 初期値
                    SelectedSkillName.Value = SkillSelectItems.Value.First();
                }
            });




            // スキル選択ComboBox表示用アイテムを作成
            items = new();
            items.Add("---");
            //foreach (var i in SkillNamesWithWeapon)
            //{
            //    items.Add(i);
            //}
            SkillSelectItems.Value = items;
            // 初期値
            SelectedSkillName.Value = SkillSelectItems.Value.First();


            // 属性選択ComboBox表示用アイテムを作成
            items = new();
            items.Add("---");
            foreach (var i in Kind.ElementType.Keys)
            {
                items.Add(i);
            }
            ElementSelectItems.Value = items;
            // 初期値
            SelectedElement.Value = ElementSelectItems.Value.First();
        }



        private List<string> SkillNamesWithWeapon(string weaponKind)
        {
            return Master.Weapons[(int)Kind.WeaponNameToKind(weaponKind)].SelectMany(w => w.Skills).Select(x => x.Name).Distinct().ToList();
        }
    }
}

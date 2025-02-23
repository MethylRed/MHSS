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
        /// 選択された装飾品名
        /// </summary>
        public ReactivePropertySlim<string> SelectedName { get; } = new();

        /// <summary>
        /// 選択された武器種
        /// </summary>
        public ReactivePropertySlim<string> SelectedWeaponKind { get; } = new();

        /// <summary>
        /// 選択されたスキル
        /// </summary>
        public ReactivePropertySlim<string> SelectedSkill { get; } = new();

        /// <summary>
        /// 選択された属性
        /// </summary>
        public ReactivePropertySlim<string> SelectedElement { get; } = new();

        /// <summary>
        /// 背景色
        /// </summary>
        public ReactivePropertySlim<SolidColorBrush> BackgroundColor { get; } = new(Brushes.White);


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
    }
}

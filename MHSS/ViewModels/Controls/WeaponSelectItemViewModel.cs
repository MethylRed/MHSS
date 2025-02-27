using MHSS.Models.Data;
using MHSS.ViewModels.SubView;
using MHSS.Views.Controls;
using MHSS.Views.SubViews;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHSS.ViewModels.Controls
{
    internal class WeaponSelectItemViewModel : SubViewModelBase
    {
        /// <summary>
        /// UserControl全体をクリックすると発火するコマンド
        /// </summary>
        public ReactiveCommand ClickCommand { get; } = new();

        // 武器のステータスを表示するプロパティ
        public ReactivePropertySlim<string> Name { get; set; } = new("");
        public ReactivePropertySlim<string> Attack { get; set; } = new("");
        public ReactivePropertySlim<string> Affinity { get; set; } = new("");
        public ReactivePropertySlim<string> Slots { get; set; } = new("");
        public ReactivePropertySlim<string> ElementType1 { get; set; } = new("");
        public ReactivePropertySlim<string> ElementValue1 { get; set; } = new("");
        public ReactivePropertySlim<string> ElementType2 { get; set; } = new("");
        public ReactivePropertySlim<string> ElementValue2 { get; set; } = new("");
        public ReactivePropertySlim<string> DefBonus { get; set; } = new("");
        public ReactivePropertySlim<string> UniqueStatus { get; set; } = new("");
        public ReactivePropertySlim<string> Skills { get; set; } = new("");

        /// <summary>
        /// UserControlのクリック可不可のためのブール変数
        /// </summary>
        public ReactivePropertySlim<bool> IsEnabled { get; set; } = new(true);


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="weapon">武器</param>
        /// <param name="isEnabled">UserControlのクリック可不可のためのブール変数</param>
        public WeaponSelectItemViewModel(Weapon weapon, bool isEnabled)
        {
            // クリックコマンドの定義
            ClickCommand.Subscribe(_ =>
            {
                // 現在固定されている武器の固定を解除
                WeaponSelectVM.Weapon.IsLock = false;
                
                // 固定されたUserControlはクリックできないようして表示
                WeaponSelectVM.WeaponSelectItemVM.Value = new(weapon, false);

                // 固定された武器を格納
                weapon.IsLock = true;
                WeaponSelectVM.Weapon = weapon;
            });

            // UserControlに表示するテキスト群
            Name.Value = weapon.Name;
            Attack.Value = $"{weapon.Attack}({weapon.Attack*Kind.WeaponCoefficient(weapon.WeaponKind)})";
            Affinity.Value = $"{weapon.Affinity}%";
            Slots.Value = $"{weapon.Slot1}-{weapon.Slot2}-{weapon.Slot3}";
            ElementType1.Value = "/Views/Controls/image/" + ElementIcon(weapon.ElementType1);
            var ev1 = weapon.ElementValue1 == 0 ? "" : weapon.ElementValue1.ToString();
            ElementValue1.Value = ev1;
            ElementType2.Value = "/Views/Controls/image/" + ElementIcon(weapon.ElementType2);
            var ev2 = weapon.ElementValue2 == 0 ? "" : weapon.ElementValue2.ToString();
            ElementValue2.Value = ev2;
            DefBonus.Value = $"{weapon.DefBonus}";
            UniqueStatus.Value = weapon.UniqueStatus;
            var s = string.Join("", weapon.Skills.Select(skill => $"{skill.Name}Lv{skill.Level}, "));
            if (s.Count() > 2) s = s.Remove(s.Length - 2, 2);
            Skills.Value = s;
            IsEnabled.Value = isEnabled;
        }

        /// <summary>
        /// 属性に対応する画像のファイルパスを返す
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private string ElementIcon(Element element)
        {
            return element switch
            {
                Element.Fire => "Fire.png",
                Element.Water => "Water.png",
                Element.Thunder => "Thunder.png",
                Element.Ice => "Ice.png",
                Element.Dragon => "Dragon.png",
                Element.Poison => "Poison.png",
                Element.Blast => "Blast.png",
                Element.Sleep => "Sleep.png",
                Element.Paralysis => "Paralysis.png",
                _ => "None.png"
            };
        }
    }
}

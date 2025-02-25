using MHSS.Models.Data;
using MHSS.ViewModels.SubView;
using MHSS.Views.Controls;
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
        public ReactiveCommand ClickCommand { get; } = new();

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



        public WeaponSelectItemViewModel(Weapon weapon)
        {
            ClickCommand.Subscribe(_ =>
            {
                WeaponSelectVM.WeaponSelectItemVM.Value = new(weapon);
                WeaponSelectVM.Weapon = weapon;
            });

            Name.Value = weapon.Name;
            Attack.Value = $"{weapon.Attack}({weapon.Attack*Kind.WeaponCoefficient(weapon.WeaponKind)})";
            Affinity.Value = $"{weapon.Affinity}%";
            Slots.Value = $"{weapon.Slot1}-{weapon.Slot2}-{weapon.Slot3}";
            ElementType1.Value = "/Views/Controls/" + ElementIcon(weapon.ElementType1);
            var ev1 = weapon.ElementValue1 == 0 ? "" : weapon.ElementValue1.ToString();
            ElementValue1.Value = ev1;
            ElementType2.Value = "/Views/Controls/" + ElementIcon(weapon.ElementType2);
            var ev2 = weapon.ElementValue2 == 0 ? "" : weapon.ElementValue2.ToString();
            ElementValue2.Value = ev2;
            DefBonus.Value = $"{weapon.DefBonus}";
            UniqueStatus.Value = weapon.UniqueStatus;
            var s = string.Join("", weapon.Skills.Select(skill => $"{skill.Name}Lv{skill.Level}, "));
            if (s.Count() > 2) s = s.Remove(s.Length - 2, 2);
            Skills.Value = s;
        }

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

using MHSS.Models.Data;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHSS.ViewModels.Controls
{
    internal class WeaponSelectItemViewModel : SubViewModelBase
    {
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
            Name.Value = weapon.Name;
            Attack.Value = $"{weapon.Attack}({weapon.Attack*Kind.WeaponCoefficient(weapon.WeaponKind)})";
            Affinity.Value = $"{weapon.Affinity}%";
            Slots.Value = $"{weapon.Slot1}-{weapon.Slot2}-{weapon.Slot3}";
            ElementType1.Value = "./Models/Data/image/" + ElementIcon(weapon.ElementType1);
            ElementValue1.Value = $"{weapon.ElementValue1}";
            ElementType2.Value = "./Models/Data/image/" + ElementIcon(weapon.ElementType2);
            ElementValue2.Value = $"{weapon.ElementValue2}";
            DefBonus.Value = $"{weapon.DefBonus}";
            UniqueStatus.Value = weapon.UniqueStatus;
            Skills.Value = string.Join("", weapon.Skills.Select(skill => $"{skill.Name}Lv{skill.Level}"));
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

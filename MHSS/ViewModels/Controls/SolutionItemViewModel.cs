using MHSS.Models.Data;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHSS.ViewModels.Controls
{
    internal class SolutionItemViewModel : SubViewModelBase
    {
        public ReactivePropertySlim<string> Skill { get; set; } = new(string.Empty);


        public ReactivePropertySlim<string> Weapon { get; set; } = new(string.Empty);


        public ReactivePropertySlim<string> Head { get; set; } = new(string.Empty);


        public ReactivePropertySlim<string> Body { get; set; } = new(string.Empty);


        public ReactivePropertySlim<string> Arm { get; set; } = new(string.Empty);


        public ReactivePropertySlim<string> Waist { get; set; } = new(string.Empty);


        public ReactivePropertySlim<string> Leg { get; set; } = new(string.Empty);


        public ReactivePropertySlim<string> Charm { get; set; } = new(string.Empty);


        public ReactivePropertySlim<ObservableCollection<string>> Deco { get; set; } = new();

        public ReactivePropertySlim<string> Def { get; set; } = new();
        public ReactivePropertySlim<string> Fire { get; set; } = new();
        public ReactivePropertySlim<string> Water { get; set; } = new();
        public ReactivePropertySlim<string> Thunder { get; set; } = new();
        public ReactivePropertySlim<string> Ice { get; set; } = new();
        public ReactivePropertySlim<string> Dragon { get; set; } = new();


        public SolutionItemViewModel(SearchedEquips searchedEquips)
        {
            Weapon.Value = searchedEquips.Weapon.Name;
            Head.Value = searchedEquips.Head.Name;
            Body.Value = searchedEquips.Body.Name;
            Arm.Value = searchedEquips.Arm.Name;
            Waist.Value = searchedEquips.Waist.Name;
            Leg.Value = searchedEquips.Leg.Name;
            Charm.Value = searchedEquips.Charm.Name;

            var decos = searchedEquips.Decos.OrderByDescending(d => d.Slot1)
                                             //.GroupBy(x => x)
                                             //.Select(g => new { g.Key.Name, Count = g.Count() })
                                             .ToList();

            Deco.Value = new();
            for (int i = 0; i < 21; i++)
            {
                Deco.Value.Add("");
            }
            foreach (var d in decos)
            {
                for (int i = 0; i < 21; i++)
                {
                    if (searchedEquips.Slots[i] >= d.Slot1)
                    {
                        if (Deco.Value[i] == "")
                        {
                            Deco.Value[i] = d.Name;
                            break;
                        }
                    }
                }
            }
            for (int i = 0; i < 21; i++)
            {
                Deco.Value[i] = $"【{searchedEquips.Slots[i]}】   {Deco.Value[i]}";
            }

            Def.Value = searchedEquips.Def.ToString();
            Fire.Value = searchedEquips.ResFire.ToString();
            Water.Value = searchedEquips.ResWater.ToString();
            Thunder.Value = searchedEquips.ResThunder.ToString();
            Ice.Value = searchedEquips.ResIce.ToString();
            Dragon.Value = searchedEquips.ResDragon.ToString();


            string seriesDisp = "", groupDisp = "";
            foreach (var skill in searchedEquips.Skills)
            {
                var s = Master.Skills.Single(a => a.Name == skill.Name);
                if (s.Category == "シリーズスキル")
                {
                    if (skill.Level >= s.MaxLevel1)
                    {
                        seriesDisp += ((skill.Level >= s.MaxLevel2) ? s.ActivateSkillName2 : s.ActivateSkillName1) + "\n";
                    }
                }
                else if (s.Category == "グループスキル")
                {
                    if (skill.Level >= s.MaxLevel1)
                    {
                        groupDisp += ((skill.Level >= s.MaxLevel2) ? s.ActivateSkillName2 : s.ActivateSkillName1) + "\n";
                    }
                }
                else
                {
                    Skill.Value += $"{skill.Name}Lv{skill.Level}\n";
                }

            }
            Skill.Value += seriesDisp + groupDisp;
        }
    }
}

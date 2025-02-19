using MHSS.Models.Data;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHSS.ViewModels.SubView
{
    internal class SolutionViewModel /*: SubViewModelBase*/
    {
        public ReactivePropertySlim<string> Skill { get; set; } = new(string.Empty);

        
        public ReactivePropertySlim<string> Weapon { get; set; } = new(string.Empty);

        
        public ReactivePropertySlim<string> Head { get; set; } = new(string.Empty);
        
        
        public ReactivePropertySlim<string> Body { get; set; } = new(string.Empty);
        
        
        public ReactivePropertySlim<string> Arm { get; set; } = new(string.Empty);
        
        
        public ReactivePropertySlim<string> Waist { get; set; } = new(string.Empty);
        
        
        public ReactivePropertySlim<string> Leg { get; set; } = new(string.Empty);
        
        
        public ReactivePropertySlim<string> Charm { get; set; } = new(string.Empty);


        public ReactivePropertySlim<string> Deco { get; set; } = new(string.Empty);


        public SolutionViewModel(SearchedEquips searchedEquips)
        {
            Weapon.Value = searchedEquips.Weapon.Name;
            Head.Value = searchedEquips.Head.Name;
            Body.Value = searchedEquips.Body.Name;
            Arm.Value = searchedEquips.Arm.Name;
            Waist.Value = searchedEquips.Waist.Name;
            Leg.Value = searchedEquips.Leg.Name;
            Charm.Value = searchedEquips.Charm.Name;

            var decos = searchedEquips.Decos.OrderByDescending(d => d.Slot1)
                                             .GroupBy(x => x)
                                             .Select(g => new { g.Key.Name, Count = g.Count() })
                                             .ToList();




            foreach (var item in decos)
            {
                Deco.Value += item.Name + "*" + item.Count.ToString() + "\n";
            }



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

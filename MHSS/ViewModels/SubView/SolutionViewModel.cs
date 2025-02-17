using MHSS.Models.Data;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHSS.ViewModels.SubView
{
    internal class SolutionViewModel : SubViewModelBase
    {
        public ReactivePropertySlim<string> disp { get; set; } = new(string.Empty);

        
        public ReactivePropertySlim<string> Weapon { get; set; } = new(string.Empty);

        
        public ReactivePropertySlim<string> Head { get; set; } = new(string.Empty);
        
        
        public ReactivePropertySlim<string> Body { get; set; } = new(string.Empty);
        
        
        public ReactivePropertySlim<string> Arm { get; set; } = new(string.Empty);
        
        
        public ReactivePropertySlim<string> Waist { get; set; } = new(string.Empty);
        
        
        public ReactivePropertySlim<string> Leg { get; set; } = new(string.Empty);
        
        
        public ReactivePropertySlim<string> Charm { get; set; } = new(string.Empty);

        public SolutionViewModel(SearchedEquips searchedEquips)
        {
            //disp.Value = "";
            Weapon.Value = "ああああああああああああああ";
            //Weapon.Value = searchedEquip.Weapon.Name;
            Head.Value = searchedEquips.Head.Name;
            Body.Value = searchedEquips.Body.Name;
            Arm.Value = searchedEquips.Arm.Name;
            Waist.Value = searchedEquips.Waist.Name;
            Leg.Value = searchedEquips.Leg.Name;
            Charm.Value = searchedEquips.Charm.Name;
            //var counts = searchedEquips.Decos.GroupBy(x => x)
            //                                    .Select(g => new { g.Key.Name, Count = g.Count() })
            //                                    .ToList();
            //foreach (var item in counts)
            //{
            //    disp.Value += item.Name + "*" + item.Count.ToString() + "\n";
            //}
            //foreach (var skill in searchedEquips.Skills)
            //{
            //    disp.Value += "\n" + skill.Name + "Lv" + skill.Level.ToString();
            //}
        }

        /// <summary>
        /// 検索結果の装備構成を受け取り表示用文字列を作成
        /// </summary>
        /// <param name="searchedEquips"></param>
        public void ShowResult(SearchedEquips searchedEquips)
        {
            disp.Value = "";
                Weapon.Value = "ああああああああああああああ";
                //Weapon.Value = searchedEquip.Weapon.Name;
                Head.Value = searchedEquips.Head.Name;
                Body.Value = searchedEquips.Body.Name;
                Arm.Value = searchedEquips.Arm.Name;
                Waist.Value = searchedEquips.Waist.Name;
                Leg.Value = searchedEquips.Leg.Name;
                Charm.Value = searchedEquips.Charm.Name;

                disp.Value += searchedEquips.Weapon.Name + "\n";
                disp.Value += searchedEquips.Head.Name + "\n";
                disp.Value += searchedEquips.Body.Name + "\n";
                disp.Value += searchedEquips.Arm.Name + "\n";
                disp.Value += searchedEquips.Waist.Name + "\n";
                disp.Value += searchedEquips.Leg.Name + "\n";
                disp.Value += searchedEquips.Charm.Name + "\n";
                var counts = searchedEquips.Decos.GroupBy(x => x)
                                                .Select(g => new { g.Key.Name, Count = g.Count() })
                                                .ToList();
                foreach (var item in counts)
                {
                    disp.Value += item.Name + "*" + item.Count.ToString() + "\n";
                }
                foreach (var skill in searchedEquips.Skills)
                {
                    disp.Value += "\n" + skill.Name + "Lv" + skill.Level.ToString();
                }
        }
    }
}

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

        public SolutionViewModel()
        {
            disp.Value = "test";
        }

        /// <summary>
        /// 検索結果の装備構成を受け取り表示用文字列を作成
        /// </summary>
        /// <param name="searchedEquips"></param>
        public void ShowResult(List<SearchedEquips> searchedEquips)
        {
            disp.Value = "";
            foreach (var searchedEquip in searchedEquips)
            {
                disp.Value += searchedEquip.Weapon.Name + "\n";
                disp.Value += searchedEquip.Head.Name + "\n";
                disp.Value += searchedEquip.Body.Name + "\n";
                disp.Value += searchedEquip.Arm.Name + "\n";
                disp.Value += searchedEquip.Waist.Name + "\n";
                disp.Value += searchedEquip.Leg.Name + "\n";
                disp.Value += searchedEquip.Charm.Name + "\n";
                foreach (var deco in searchedEquip.Decos)
                {
                    disp.Value += deco.Name + "\n";
                }
            }
        }
    }
}

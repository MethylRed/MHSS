using MHSS.Models.Data;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MHSS.ViewModels.Controls;

namespace MHSS.ViewModels.SubView
{
    internal class SolutionViewModel : SubViewModelBase
    {
        public ReactivePropertySlim<ObservableCollection<SolutionItemViewModel>> SolutionItemVMs { get; } = new();


        public SolutionViewModel(List<SearchedEquips> equips)
        {
            ObservableCollection<SolutionItemViewModel> oldColl = SolutionItemVMs.Value;
            if (oldColl != null)
            {
                foreach (var item in oldColl)
                {
                    ((IDisposable)item).Dispose();
                }
            }
            SolutionItemVMs.Value = new();
            foreach (var item in equips)
            {
                SolutionItemVMs.Value.Add(new(item));
            }
        }
    }
}

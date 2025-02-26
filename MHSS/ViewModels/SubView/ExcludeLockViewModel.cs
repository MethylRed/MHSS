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
    internal class ExcludeLockViewModel : SubViewModelBase
    {
        public ReactivePropertySlim<ObservableCollection<ExcludeLockItem>> ExludeLockItemVMs { get; } = new();

        public ExcludeLockViewModel() { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHSS.ViewModels.SubView
{
    internal class SolutionViewModel : SubViewModelBase
    {
        public int Count { get; init; }

        public SolutionViewModel(int count)
        {
            Count = count;
        }
    }
}

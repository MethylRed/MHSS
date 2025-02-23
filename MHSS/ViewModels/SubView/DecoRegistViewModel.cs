using MHSS.Models.Data;
using MHSS.ViewModels.Controls;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHSS.ViewModels.SubView
{
    internal class DecoRegistViewModel : SubViewModelBase
    {
        /// <summary>
        /// カテゴリ別スキル選択アイテムのViewModel
        /// </summary>
        public ReactivePropertySlim<ObservableCollection<DecoRegistByCategoryViewModel>> DecoRegistByCategoryVMs { get; } = new();


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DecoRegistViewModel()
        {
            // カテゴリ別にスキル条件選択のComboBoxを配置
            ObservableCollection<DecoRegistByCategoryViewModel> oldColl = DecoRegistByCategoryVMs.Value;
            if (oldColl != null)
            {
                foreach (var item in oldColl)
                {
                    ((IDisposable)item).Dispose();
                }
            }
            DecoRegistByCategoryVMs.Value = new ObservableCollection<DecoRegistByCategoryViewModel>(
                Master.Decos.GroupBy(s => s.Slot1).OrderBy(g => g.Key)
                .Select(g => new DecoRegistByCategoryViewModel(g.Key, g))
            );
        }
    }
}

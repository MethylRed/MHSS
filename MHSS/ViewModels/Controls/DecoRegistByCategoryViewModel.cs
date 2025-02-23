using MHSS.Models.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHSS.ViewModels.Controls
{
    internal class DecoRegistByCategoryViewModel : SubViewModelBase
    {
        /// <summary>
        /// カテゴリ名(スロット毎)
        /// </summary>
        public string CategoryName { get; init; }

        /// <summary>
        /// 装飾品登録アイテムのViewModel
        /// </summary>
        public ObservableCollection<DecoRegistItemViewModel> DecoRegistVMs { get; init; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="slotSize">スロットサイズ</param>
        /// <param name="decos">装飾品のコレクション</param>
        public DecoRegistByCategoryViewModel(int slotSize, IEnumerable<Deco> decos)
        {
            CategoryName = $"Lv{slotSize}装飾品";
            DecoRegistVMs = new(decos.Select(x => new DecoRegistItemViewModel(x)));
        }
    }
}

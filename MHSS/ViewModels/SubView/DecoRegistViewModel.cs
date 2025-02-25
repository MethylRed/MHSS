using MHSS.Models.Config;
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
        /// 検索コマンド
        /// </summary>
        public ReactiveCommand SearchCommand { get; } = new();

        /// <summary>
        /// クリアコマンド
        /// </summary>
        public ReactiveCommand ClearCommand { get; } = new();

        /// <summary>
        /// 全装飾品所持数を0にする
        /// </summary>
        public ReactiveCommand ResetDecoCountCommand { get; } = new();

        /// <summary>
        /// 全装飾品所持数をmaxにする
        /// </summary>
        public ReactiveCommand MaxDecoCountCommand { get; } = new();

        /// <summary>
        /// 検索文字列
        /// </summary>
        public ReactivePropertySlim<string> FilterName { get; set; } = new("");

        /// <summary>
        /// 検索スキル文字列
        /// </summary>
        public ReactivePropertySlim<string> FilterSkillName { get; set; } = new("");


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DecoRegistViewModel()
        {
            LoadControls();

            SearchCommand.Subscribe(() => LoadControls(FilterName.Value, FilterSkillName.Value));
            ClearCommand.Subscribe(() => LoadControls());
            ResetDecoCountCommand.Subscribe(() => SetDecoCount(0));
            MaxDecoCountCommand.Subscribe(() => SetDecoCount(Config.MaxDecoCount));
        }


        private void SetDecoCount(int count)
        {
            LoadControls();
            foreach (var c in DecoRegistByCategoryVMs.Value)
            {
                c.SetDecoCount(count);
            }
        }



        /// <summary>
        /// 名前またはスキル名に合致するものを表示
        /// </summary>
        /// <param name="filterName"></param>
        private void LoadControls(string filterName = "", string filterSkillName = "")
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
                Master.Decos.Where(d => d.Name.Contains(filterName) &&
                    d.Skills.Any(s => s.Name.Contains(filterSkillName)))
                .GroupBy(s => s.Slot1).OrderBy(g => g.Key)
                .Select(g => new DecoRegistByCategoryViewModel(g.Key, g))
            );
        }
    }
}

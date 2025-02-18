using System.Reflection;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Documents;
using Google.OrTools.LinearSolver;
using System.Linq;
using Reactive.Bindings;
using System.Collections.ObjectModel;
using MHSS.Models.Config;
using MHSS.Models.Data;
using MHSS.Models.Utility;
using MHSS.ViewModels.SubView;
using MHSS.Views.Controls;
using MHSS.ViewModels.Controls;
using Reactive.Bindings.Extensions;

namespace MHSS.ViewModels
{
    internal class MainWindowViewModel : BindableBase
    {
        /// <summary>
        /// 検索コマンド
        /// </summary>
        public DelegateCommand SearchCommand { get; private set; }

        /// <summary>
        /// 検索条件・実行のインスタンス
        /// </summary>
        private Solve Solve { get; set; }

        /// <summary>
        /// 検索回数
        /// </summary>
        public ReactivePropertySlim<string> SearchCount { get; set; } = new();

        /// <summary>
        /// 各VMで参照を共有するためのインスタンス
        /// </summary>
        internal static MainWindowViewModel Instance { get; set; }


        /// <summary>
        /// スキル選択のViewModel
        /// </summary>
        public ReactivePropertySlim<SkillSelectViewModel> SkillSelectVM { get; } = new();

        /// <summary>
        /// 結果表示のViewModel
        /// </summary>
        public ReactiveCollection<SolutionViewModel> SolutionVMs { get; } = new();


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowViewModel()
        {
            Instance = this;

            // データの読み込み
            CSVLoader.LoadCsvSkill();
            CSVLoader.LoadCsvEquip();
            //CSVLoader.LoadCsvHead();
            //CSVLoader.LoadCsvBody();
            //CSVLoader.LoadCsvArm();
            //CSVLoader.LoadCsvWaist();
            //CSVLoader.LoadCsvLeg();
            //CSVLoader.LoadCsvCharm();
            //CSVLoader.LoadCsvDeco();

            // ボタンクリックイベントの定義
            SearchCommand = new DelegateCommand(Search);

            // ViewModelのインスタンスを生成
            SkillSelectVM.Value = new();

            SearchCount.Value = Config.Instance.MaxSearchCount.ToString();
        }

        /// <summary>
        /// 検索を実行
        /// </summary>
        private void Search()
        {
            // スキルの検索条件を取得
            Condition condition = SkillSelectVM.Value.MakeSkillCondition();

            // ソルバーを宣言
            Solve = new(condition);

            // 求解し表示
            var s = Solve.Search(Utility.ParseOrDefault(SearchCount.Value, Config.Instance.MaxSearchCount));
            SolutionVMs.Clear();
            foreach (var item in s)
            {
                SolutionVMs.Add(new SolutionViewModel(item));
            }

            Debug.WriteLine("\n Check is finished.");
        }
    }
}

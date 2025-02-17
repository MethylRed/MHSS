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
        public DelegateCommand SearchCommand { get; private set; }
        private Solve Solve { get; set; }

        /// <summary>
        /// VMで参照を共有するためのインスタンス
        /// </summary>
        internal static MainWindowViewModel Instance { get; set; }


        /// <summary>
        /// スキル選択のViewModel
        /// </summary>
        public ReactivePropertySlim<SkillSelectViewModel> SkillSelectVM { get; } = new();

        /// <summary>
        /// カテゴリ別スキル選択アイテムのViewModel
        /// </summary>
        public ReactivePropertySlim<ObservableCollection<SolutionViewModel>> ObservableCollectionSolutionVM { get; set; } = new();

        public ReactivePropertySlim<SolutionViewModel> SolutionVM { get; } = new();



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
            ObservableCollectionSolutionVM.Value = new();
            SkillSelectVM.Value = new();
            SolutionVM.Value = new(new());

        }

        /// <summary>
        /// 検索を実行
        /// </summary>
        private void Search()
        {
            // スキルの検索条件を取得
            Condition condition = SkillSelectVM.Value.MakeCondition();

            // ソルバーを宣言
            Solve = new(condition);

            // 求解し表示
            //SolutionVM.Value.ShowResult(Solve.Search(1)[0]);

            ObservableCollectionSolutionVM.Value.Add(new(Solve.Search(1)[0]));

            
            //var s = Solve.Search(2);
            //foreach (var item in s)
            //{
            //    ObservableCollectionSolutionVM.Value.Add(new SolutionViewModel(item));
            //}




            //int count = 2;
            //for (int i = 0; i < count ; i++)
            //{
            //    SolutionVM.Value.ShowResult(Solve.Search(count)[i]);
            //    ObservableCollectionSolutionVM.Value.Add()

            //}


            Debug.WriteLine("\n Check is finished.");
        }
    }
}

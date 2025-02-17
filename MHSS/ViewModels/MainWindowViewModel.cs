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

namespace MHSS.ViewModels
{
    internal class MainWindowViewModel : BindableBase
    {
        public DelegateCommand ClickCommand { get; set; }
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
            ClickCommand = new DelegateCommand(OnClick);
            SearchCommand = new DelegateCommand(Search);

            // ViewModelのインスタンスを生成
            SkillSelectVM.Value = new();
            SolutionVM.Value = new();

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
            SolutionVM.Value.ShowResult(Solve.Search(1));

            
            Debug.WriteLine("\n Check is finished.");
        }

        private void OnClick()
        {
#if DEBUG
            // logic部分の確認

            if (true)
            {
                #region LoadCSV
                // CSVファイルの読み込み
                // 全スキルの情報を表示
                // 装備は代表として101番目の情報を表示
                CSVLoader.LoadCsvSkill();
                foreach (var item in Master.Skills) { Debug.WriteLine(item); }
                CSVLoader.LoadCsvHead();
                CSVLoader.LoadCsvBody();
                CSVLoader.LoadCsvArm();
                CSVLoader.LoadCsvWaist();
                CSVLoader.LoadCsvLeg();
                CSVLoader.LoadCsvCharm();
                CSVLoader.LoadCsvDeco();

                var x = Master.Heads[100];
                string s = "";
                for (int i = 0; i < 7; i++)
                {
                    x = i switch
                    {
                        0 => Master.Heads[100],
                        1 => Master.Bodies[100],
                        2 => Master.Arms[100],
                        3 => Master.Waists[100],
                        4 => Master.Legs[100],
                        5 => Master.Charms[100],
                        6 => Master.Decos[100],
                        _ => Master.Heads[100],
                    };
                    s = "";
                    foreach (PropertyInfo prop in x.GetType().GetProperties())
                    {
                        if (prop.Name != "Skill") s += prop.GetValue(x) + ",";
                        else
                        {
                            foreach (var item in x.Skills) s += "\n" + item;
                        }
                    }
                    Debug.WriteLine(s);
                    Debug.WriteLine("\n");
                }
                #endregion

                //#region DefineVariables
                // 個数変数の定義を確認
                // 各装備種類について、個数変数の数 = 装備の数と総和を表示
                //Solve = new();
                //Debug.WriteLine(Solve.Variables.Count);
                //Debug.WriteLine(string.Join("\n", Solve.Variables.Keys));
                //#endregion

                //#region DefineConstraint
                //// 制約式の定義を確認
                //Debug.WriteLine(Solve.Constraints.Count);
                //Debug.WriteLine(string.Join("\n", Solve.Constraints.Keys));
                //#endregion
            }
#endif
        }
    }
}

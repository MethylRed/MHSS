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
        public List<Skill> Skill { get; set; }
        public DelegateCommand SolveCommand { get; private set; }
        private Solve _Solve { get; set; }

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
        public ReactivePropertySlim<ObservableCollection<SolutionViewModel>> SolutionVM { get; } = new();



        public MainWindowViewModel()
        {
            Instance = this;
            SolutionVM.Value = new();

            CSVLoader.LoadCsvSkill();
            CSVLoader.LoadCsvEquip();
            //CSVLoader.LoadCsvHead();
            //CSVLoader.LoadCsvBody();
            //CSVLoader.LoadCsvArm();
            //CSVLoader.LoadCsvWaist();
            //CSVLoader.LoadCsvLeg();
            //CSVLoader.LoadCsvCharm();
            //CSVLoader.LoadCsvDeco();
            ClickCommand = new DelegateCommand(OnClick);
            SolveCommand = new DelegateCommand(Solve);

            SkillSelectVM.Value = new();
            SolutionVM.Value = new ObservableCollection<SolutionViewModel>();

        }

        /// <summary>
        /// 検索を実行
        /// </summary>
        private void Solve()
        {
            // スキルの検索条件を取得
            Condition condition = SkillSelectVM.Value.MakeCondition();

            _Solve = new(condition);

            Solver.ResultStatus resultStatus = _Solve.Solver.Solve();
            if (resultStatus != Solver.ResultStatus.OPTIMAL)
            {
                Debug.WriteLine("NOT OPTIMAL SOLUTION!");
            }
            else
            {
                Debug.WriteLine(_Solve.Solver.Objective().Value());

                Dictionary<Equip, int> SolutionEquips = _Solve.Variables
                    .Where(v => v.Value.SolutionValue() > 0)
                    .ToDictionary(
                        v => Master.AllEquips.FirstOrDefault(x => x.Name == v.Key),
                        v => (int)(v.Value.SolutionValue()));

                foreach (var equip in SolutionEquips)
                {
                    if (equip.Key.EquipKind != EquipKind.Deco)
                    {
                        Debug.WriteLine(equip.Key.Name);
                    }
                    else
                    {
                        Debug.WriteLine(equip.Key.Name + "*" +  equip.Value.ToString());
                    }
                }



            }

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

                var x = Master.Head[100];
                string s = "";
                for (int i = 0; i < 7; i++)
                {
                    x = i switch
                    {
                        0 => Master.Head[100],
                        1 => Master.Body[100],
                        2 => Master.Arm[100],
                        3 => Master.Waist[100],
                        4 => Master.Leg[100],
                        5 => Master.Charm[100],
                        6 => Master.Deco[100],
                        _ => Master.Head[100],
                    };
                    s = "";
                    foreach (PropertyInfo prop in x.GetType().GetProperties())
                    {
                        if (prop.Name != "Skill") s += prop.GetValue(x) + ",";
                        else
                        {
                            foreach (var item in x.Skill) s += "\n" + item;
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

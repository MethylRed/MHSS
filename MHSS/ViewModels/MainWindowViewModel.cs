﻿using System.Reflection;
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
using Reactive.Bindings.Disposables;
using Reactive.Bindings.Extensions;
using System.Reactive.Linq;
using System.Threading.Tasks;
//using System.Reactive.Disposables;

namespace MHSS.ViewModels
{
    internal class MainWindowViewModel : BindableBase, IDisposable
    {
        /// <summary>
        /// 検索コマンド
        /// </summary>
        public AsyncReactiveCommand SearchCommand { get; }

        /// <summary>
        /// 追加スキル検索コマンド
        /// </summary>
        public AsyncReactiveCommand SearchExtraSkillsCommand { get; }

        /// <summary>
        /// 表示用検索結果数
        /// </summary>
        public ReactivePropertySlim<string> ShowCount { get; set; } = new("0");

        /// <summary>
        /// ビジー状態
        /// </summary>
        public ReactivePropertySlim<bool> IsBusy { get; } = new(false);

        /// <summary>
        /// 検索条件・実行のインスタンス
        /// </summary>
        private Solve Solve { get; set; }

        /// <summary>
        /// 検索回数
        /// </summary>
        public ReactivePropertySlim<string> SearchCount { get; set; } = new();
        
        /// <summary>
        /// 検索回数
        /// </summary>
        public ReactivePropertySlim<string> Def { get; set; } = new();

        /// <summary>
        /// 検索回数
        /// </summary>
        public ReactivePropertySlim<string> ResFire { get; set; } = new();

        /// <summary>
        /// 検索回数
        /// </summary>
        public ReactivePropertySlim<string> ResWater { get; set; } = new();

        /// <summary>
        /// 検索回数
        /// </summary>
        public ReactivePropertySlim<string> ResThunder { get; set; } = new();

        /// <summary>
        /// 検索回数
        /// </summary>
        public ReactivePropertySlim<string> ResIce { get; set; } = new();

        /// <summary>
        /// 検索回数
        /// </summary>
        public ReactivePropertySlim<string> ResDragon { get; set; } = new();

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

            // 検索ボタンクリックイベントの定義
            SearchCommand = IsBusy.Select(x => !x).ToAsyncReactiveCommand();
            SearchCommand.Subscribe(async () =>
            {
                IsBusy.Value = true;
                try
                {
                    await Task.Run(() => Search());
                }
                finally
                {
                    IsBusy.Value = false;
                }
            }).AddTo(Disposable);

            // 追加スキル検索ボタンクリックイベントの定義
            SearchExtraSkillsCommand = IsBusy.Select(x => !x).ToAsyncReactiveCommand();
            SearchExtraSkillsCommand.Subscribe(async () =>
            {
                IsBusy.Value = true;
                await SearchExtraSkills();
                IsBusy.Value = false;
            }).AddTo(Disposable);


            // ViewModelのインスタンスを生成
            SkillSelectVM.Value = new();

            SearchCount.Value = Config.Instance.MaxSearchCount.ToString();
            SearchCount.Value = "1";
            Def.Value = "0";
            ResFire.Value = "";
            ResWater.Value = "";
            ResThunder.Value = "";
            ResIce.Value = "";
            ResDragon.Value = "";
        }

        /// <summary>
        /// 検索を実行
        /// </summary>
        /// <returns></returns>
        private void Search()
        {
            // スキルの検索条件を取得
            Condition condition = GetCondition();

            // ソルバーを宣言
            Solve = new(condition);

            // 求解し表示
            System.Windows.Application.Current.Dispatcher.Invoke(() => SolutionVMs.Clear());
            int count = 0;
            ShowCount.Value = "0";

            while (count < int.Min(Utility.ParseOrDefault(SearchCount.Value, Config.Instance.MaxSearchCount), Config.Instance.MaxSearchCount))
            {
                SearchedEquips searchedEquips = Solve.SearchSingle(count);

                if (searchedEquips == null)
                {
                    break;
                }
                else
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        SolutionVMs.Add(new SolutionViewModel(searchedEquips));
                        ShowCount.Value = (++count).ToString();
                    });
                }
            }
            Debug.WriteLine("Check is finished.");
        }

        /// <summary>
        /// 追加スキル検索
        /// </summary>
        /// <returns></returns>
        private async Task SearchExtraSkills()
        {
            // 元の検索条件を保持
            Condition masterCondition = GetCondition();

            // 進捗管理用
            int count = 0;

            // 検索結果
            List<Skill> skills = new();

            // すべてのスキルについて
            foreach (var skill in masterCondition.Skills)
            {
                // 元の検索条件のスキルレベル
                int l = skill.Level;

                // 元の検索条件のスキルレベル+1から上限まで検索
                for (int level = skill.Level+1; level <= skill.MaxLevel2; level++)
                {
                    // スキルの検索条件を取得
                    Condition condition = new(masterCondition);
                    condition.Skills.Single(s => s.Name == skill.Name).Level = level;

                    // ソルバーを宣言
                    Solve = new(condition);

                    Solver.ResultStatus status = await Task.Run(() => Solve.Solver.Solve());
                    if (status != Solver.ResultStatus.OPTIMAL) break;
                    skills.Add(new Skill() { Name = skill.Name, Level = condition.Skills.Single(s => s.Name == skill.Name).Level });
                    //Debug.WriteLine($"{skill.Name}Lv{condition.Skills.Single(s => s.Name == skill.Name).Level}");
                }
            }

            //foreach (var skill in Master.Skills)
            //{
            //    // 元の検索条件のスキルレベル
            //    int l = masterCondition.Skills.Single(s => s.Name == skill.Name).Level;
            //    for (int level = 1; level <= skill.MaxLevel2 - l; level++)
            //    {
            //        // スキルの検索条件を取得
            //        Condition condition = SkillSelectVM.Value.MakeSkillCondition();
            //        condition.Def = Utility.ParseOrDefault(Def.Value);
            //        condition.ResFire = Utility.ParseOrDefaultDouble(ResFire.Value, double.NegativeInfinity);
            //        condition.ResWater = Utility.ParseOrDefaultDouble(ResWater.Value, double.NegativeInfinity);
            //        condition.ResThunder = Utility.ParseOrDefaultDouble(ResThunder.Value, double.NegativeInfinity);
            //        condition.ResIce = Utility.ParseOrDefaultDouble(ResIce.Value, double.NegativeInfinity);
            //        condition.ResDragon = Utility.ParseOrDefaultDouble(ResDragon.Value, double.NegativeInfinity);
            //        condition.Skills.Single(s => s.Name == skill.Name).Level = level;

            //        // ソルバーを宣言
            //        Solve = new(condition);

            //        Solver.ResultStatus status = await Task.Run(() => Solve.Solver.Solve());
            //        if (status != Solver.ResultStatus.OPTIMAL) break;
            //        skills.Add(new Skill() { Name = skill.Name, Level = condition.Skills.Single(s => s.Name == skill.Name).Level });
            //        Debug.WriteLine($"{skill.Name}Lv{condition.Skills.Single(s => s.Name == skill.Name).Level}");
            //    }
            //}
        }

        /// <summary>
        /// 検索条件を取得
        /// </summary>
        /// <returns></returns>
        private Condition GetCondition()
        {
            Condition condition = SkillSelectVM.Value.MakeSkillCondition();
            condition.Def = Utility.ParseOrDefault(Def.Value);
            condition.ResFire = Utility.ParseOrDefaultDouble(ResFire.Value, double.NegativeInfinity);
            condition.ResWater = Utility.ParseOrDefaultDouble(ResWater.Value, double.NegativeInfinity);
            condition.ResThunder = Utility.ParseOrDefaultDouble(ResThunder.Value, double.NegativeInfinity);
            condition.ResIce = Utility.ParseOrDefaultDouble(ResIce.Value, double.NegativeInfinity);
            condition.ResDragon = Utility.ParseOrDefaultDouble(ResDragon.Value, double.NegativeInfinity);

            return condition;
        }


        protected CompositeDisposable Disposable { get; } = new CompositeDisposable();

        /// <summary>
        /// disposeフラグ
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing">disposeフラグ</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Disposable.Dispose();
                }
                _disposed = true;
            }
        }

        /// <summary>
        /// ファイナライザ
        /// </summary>
        ~MainWindowViewModel()
        {
            Dispose(false);
        }
    }
}

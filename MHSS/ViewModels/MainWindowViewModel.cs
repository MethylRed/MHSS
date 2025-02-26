using System.Reflection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Prism.Commands;
using Prism.Mvvm;
using Google.OrTools.LinearSolver;
using Reactive.Bindings;
using Reactive.Bindings.Disposables;
using Reactive.Bindings.Extensions;
using MHSS.Models.Config;
using MHSS.Models.Data;
using MHSS.Models.Utility;
using MHSS.ViewModels.SubView;
using MHSS.Views.Controls;
using MHSS.ViewModels.Controls;
using MHSS.Views.SubViews;
//using System.Reactive.Disposables;

namespace MHSS.ViewModels
{
    internal class MainWindowViewModel : BindableBase, IDisposable
    {
        /// <summary>
        /// 検索結果表示タブの選択インデックス
        /// </summary>
        public ReactivePropertySlim<int> SelectedResultTabIndex { get; set; } = new(0);

        /// <summary>
        /// 検索コマンド
        /// </summary>
        public AsyncReactiveCommand SearchCommand { get; }

        /// <summary>
        /// 追加スキル検索コマンド
        /// </summary>
        public AsyncReactiveCommand SearchExtraSkillsCommand { get; }

        /// <summary>
        /// スキル条件リセットコマンド
        /// </summary>
        public ReactiveCommand ResetCommand { get; } = new();

        /// <summary>
        /// 表示用検索結果数
        /// </summary>
        public ReactivePropertySlim<string> ShowCount { get; set; } = new("0");


        public ReactivePropertySlim<ObservableCollection<Skill>> ExtraSkills { get; set; } = new(new());
        public ReactivePropertySlim<string> ForDisplayExtraSkills { get; set; } = new("");

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
        /// 装飾品登録のViewModel
        /// </summary>
        public ReactivePropertySlim<DecoRegistViewModel> DecoRegistVM { get; } = new();

        /// <summary>
        /// 武器選択のViewModel
        /// </summary>
        public ReactivePropertySlim<WeaponSelectViewModel> WeaponSelectVM { get; } = new();

        /// <summary>
        /// 結果表示のViewModel
        /// </summary>
        public ReactivePropertySlim<SolutionViewModel> SolutionVM { get; } = new();

        /// <summary>
        /// 装備の除外固定のViewModel
        /// </summary>
        public ReactivePropertySlim<ExcludeLockViewModel> ExcludeLockVM { get; } = new();


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowViewModel()
        {
            Instance = this;

            AppDomain.CurrentDomain.ProcessExit += (s, e) => FileManager.SaveDecoCount();

            // データの読み込み
            FileManager.LoadCsvSkill();
            FileManager.LoadCsvEquip();
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
            DecoRegistVM.Value = new();
            WeaponSelectVM.Value = new();
            SolutionVM.Value = new(new());
            ExcludeLockVM.Value = new();

            // スキル条件をリセット
            ResetCommand.Subscribe(() =>
            {
                foreach (var item in SkillSelectVM.Value.SkillLevelSelectorsByCategoryVMs.Value)
                {
                    item.Reset();
                }
            });

            SearchCount.Value = Config.Instance.MaxSearchCount.ToString();
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

            // 解を表示するVMを初期化
            System.Windows.Application.Current.Dispatcher.Invoke(() => SolutionVM.Value = new(new()));

            if (!condition.SatisfySecret) return;

            // ソルバーを宣言
            Solve = new(condition);

            // 求解し表示
            int count = 0;
            ShowCount.Value = "0";

            List<SearchedEquips> equips = new();
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
                        equips.Add(searchedEquips);
                        ShowCount.Value = (++count).ToString();
                    });
                }
            }
            SolutionVM.Value = new(equips);
            SelectedResultTabIndex.Value = 0;
            Debug.WriteLine("Check is finished.");
        }

        /// <summary>
        /// 追加スキル検索
        /// </summary>
        /// <returns></returns>
        private async Task SearchExtraSkills()
        {
            Stopwatch sw = Stopwatch.StartNew();

            // 元の検索条件を保持
            Condition masterCondition = GetCondition();
            if (!masterCondition.SatisfySecret) return;

            ConcurrentBag<Skill> skills = new();

            // 進捗管理用
            int count = 0;
            int maxDegreeOfParallelism = Math.Min((Environment.ProcessorCount)/2, 6);
            var options = new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism };

            // 検索結果
            //List<Skill> skills = new();

            await Task.Run(() =>
            {
                Parallel.ForEach(masterCondition.Skills, options, skill =>
                {
                    count++;
                    for (int level = skill.Level + 1; level <= int.Max(skill.MaxLevel1, skill.MaxLevel2); level++)
                    {
                        if ((skill.Category == "シリーズスキル" || skill.Category == "グループスキル") &&
                             (level != skill.MaxLevel1 && level != skill.MaxLevel2))
                        {
                            continue;
                        }
                        Condition condition = GetCondition();
                        condition.Skills.Single(s => s.Name == skill.Name).Level = level;
                        Solve solve = new Solve(condition);

                        Solver.ResultStatus status = solve.Solver.Solve();

                        if (status != Solver.ResultStatus.OPTIMAL) break;

                        skills.Add(new Skill { Name = skill.Name, Level = level, 
                        ActivateSkillName1 = skill.ActivateSkillName1,
                        ActivateSkillName2 = skill.ActivateSkillName2,
                        Category = skill.Category});
                    }
                });
            });

            ExtraSkills.Value = new(skills);

            //var x = ExtraSkills.Value.GroupBy(s => s.Name).Select(g => new { g.Key, Count = g.Count() });
            var y = ExtraSkills.Value.GroupBy(s => s.Name).Reverse();
            StringBuilder sb = new();
            //foreach (var item in x)
            //{
            //    sb.Append($"{item.Key}  ");
            //    for (int i = 1; i <= item.Count; i++)
            //    {
            //        sb.Append($"Lv{i}, ");
            //    }
            //    sb.Remove(sb.Length - 2, 2);
            //    sb.Append('\n');
            //}
            foreach (var item in y)
            {
                sb.Append($"{item.Key}  ");
                foreach (var it in item.Reverse())
                {
                    sb.Append($"Lv{it.Level}, ");
                }
                sb.Remove(sb.Length - 2, 2);
                sb.Append("\n");
            }
            ForDisplayExtraSkills.Value = sb.ToString();
            SelectedResultTabIndex.Value = 1;
            sw.Stop();
            Debug.WriteLine(sw.ElapsedMilliseconds);
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

            if (WeaponSelectVM.Value.Weapon.Name == "")
            {
                if (WeaponSelectVM.Value.SelectedWeaponKind.Value == "---")
                {
                    condition.Equips.AddRange(Master.Weapons.SelectMany(w => w));
                }
                else
                {
                    condition.Equips.AddRange(Master.Weapons[(int)Kind.WeaponNameToKind(WeaponSelectVM.Value.SelectedWeaponKind.Value)]);
                }
            }
            else
            {
                condition.Equips.Add(WeaponSelectVM.Value.Weapon);
            }
            condition.Equips.AddRange(Master.Heads.Union(Master.Bodies).Union(Master.Arms).Union(Master.Waists)
                                                    .Union(Master.Legs).Union(Master.Charms).Union(Master.Decos));

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

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
using System.Windows.Media;
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
        public AsyncReactiveCommand ResetCommand { get; } = new();

        /// <summary>
        /// 表示用検索結果数
        /// </summary>
        public ReactivePropertySlim<string> ShowCount { get; set; } = new("0");

        /// <summary>
        /// 注意
        /// </summary>
        public ReactivePropertySlim<string> NoticeStr { get; set; } = new("");

        /// <summary>
        /// 背景色
        /// </summary>
        public ReactivePropertySlim<SolidColorBrush> NoticeBackgroundColor { get; } = new(Brushes.White);

        /// <summary>
        /// 追加スキル検索結果
        /// そのうち、レベルをクリックしたらスキル条件に追加されるようにしたい
        /// </summary>
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
        /// 武器登録のViewModel
        /// </summary>
        public ReactivePropertySlim<WeaponRegistViewModel> WeaponRegistVM { get; } = new();


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowViewModel()
        {
            Instance = this;

            // アプリケーション終了時に実行するメソッドを登録
            AppDomain.CurrentDomain.ProcessExit += (s, e) => FileManager.SaveDecoCount();
            AppDomain.CurrentDomain.ProcessExit += (s, e) => FileManager.SaveAddWeapon();

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
            WeaponRegistVM.Value = new();

            // スキル条件のリセットコマンドを定義
            ResetCommand = IsBusy.Select(x => !x).ToAsyncReactiveCommand();
            ResetCommand.Subscribe(async () =>
            {
                IsBusy.Value = true;
                await Task.Run(() =>
                {
                    foreach (var item in SkillSelectVM.Value.SkillLevelSelectorsByCategoryVMs.Value)
                    {
                        item.Reset();
                    }
                });
                IsBusy.Value = false;
            });

            // 画面表示テキストをセット
            SearchCount.Value = Config.Instance.MaxSearchCount.ToString();
            SearchCount.Value = "10";
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
            List<SearchedEquips> equips = new();
            
            // スキルの検索条件を取得
            Condition condition = GetCondition();

            // 解を表示するVMを初期化
            System.Windows.Application.Current.Dispatcher.Invoke(() => SolutionVM.Value = new(new()));

            // 極意条件を満たしていない場合終了
            if (!condition.SatisfySecret) return;

            // ソルバーを宣言
            Solve = new(condition);

            // 求解し表示
            int count = 0;
            ShowCount.Value = "0";

            // 検索回数が指定値以下の場合実行
            while (count < int.Min(Utility.ParseOrDefault(SearchCount.Value, Config.Instance.MaxSearchCount), Config.Instance.MaxSearchCount))
            {
                // 検索
                SearchedEquips searchedEquips = Solve.SearchSingle(count);

                // 検索結果が無い場合終了
                if (searchedEquips == null)
                {
                    break;
                }
                else
                {
                    // UI操作
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        ShowCount.Value = (++count).ToString();
                    });
                    equips.Add(searchedEquips);
                }
            }
            // 結果を表示
            SolutionVM.Value = new(equips);
            SelectedResultTabIndex.Value = 0;
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                NoticeStr.Value = "";
                NoticeBackgroundColor.Value = Brushes.White;
                if (count == 0)
                {
                    if (condition.Equips.Any(e => e.IsLock))
                    {
                        NoticeStr.Value = "装備の除外/固定が設定されています。\n設定を外すことで一致する結果が見つかる可能性があります。";
                        NoticeBackgroundColor.Value = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FCF8E3"));
                    }
                }
            });
        }

        /// <summary>
        /// 追加スキル検索
        /// </summary>
        /// <returns></returns>
        private async Task SearchExtraSkills()
        {
            Stopwatch sw = Stopwatch.StartNew();

            ConcurrentBag<Skill> resultSkills = new();
            
            // 元の検索条件を保持
            Condition masterCondition = GetCondition();

            // 極意条件を満たしていない場合は終了
            if (!masterCondition.SatisfySecret) return;

            // 進捗管理用
            int count = 0;

            // マルチスレッド設定
            // 環境次第だが、6スレッド以下で実行するようにハードコーディング
            // たぶん4～6スレッドくらいが一番良い
            int maxDegreeOfParallelism = Math.Min((Environment.ProcessorCount)/2, 6);
            var options = new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism };

            await Task.Run(() =>
            {
                // 全スキルを走査
                Parallel.ForEach(masterCondition.Skills, options, skill =>
                {
                    count++;
                    // 全レベルを走査
                    for (int level = skill.Level + 1; level <= int.Max(skill.MaxLevel1, skill.MaxLevel2); level++)
                    {
                        // シリーズ/グループスキルの場合は発動するレベルのときだけ検索する
                        if ((skill.Category == "シリーズスキル" || skill.Category == "グループスキル") &&
                             (level != skill.MaxLevel1 && level != skill.MaxLevel2))
                        {
                            continue;
                        }
                        Condition condition = GetCondition();
                        condition.Skills.Single(s => s.Name == skill.Name).Level = level;
                        Solve solve = new Solve(condition);

                        // 検索
                        Solver.ResultStatus status = solve.Solver.Solve();

                        if (status != Solver.ResultStatus.OPTIMAL) break;

                        // 結果を格納
                        resultSkills.Add(new Skill { Name = skill.Name, Level = level, 
                        ActivateSkillName1 = skill.ActivateSkillName1,
                        ActivateSkillName2 = skill.ActivateSkillName2,
                        MaxLevel1 = skill.MaxLevel1,
                        MaxLevel2 = skill.MaxLevel2,
                        Category = skill.Category});
                    }
                });
            });

            ExtraSkills.Value = new(resultSkills);

            // 検索結果をスキル名でグループ化してスキル条件の順番に並び変える(~10ms)
            var normalSkills = ExtraSkills.Value.Where(s => s.Category != "シリーズスキル" && s.Category != "グループスキル")
                .GroupBy(s => s.Name)
                .OrderBy(g => masterCondition.Skills.Select(s => s.Name).ToList().IndexOf(g.Key));
            var seriesSkills = ExtraSkills.Value.Where(s => s.Category == "シリーズスキル" || s.Category == "グループスキル")
                .GroupBy(s => s.Name)
                .OrderBy(g => masterCondition.Skills.Select(s => s.Name).ToList().IndexOf(g.Key));

            // 表示用文字列を作成
            StringBuilder sb = new();
            foreach (var group in normalSkills)
            {
                sb.Append($"{group.Key}  ");
                foreach (var it in group.OrderBy(s => s.Level))
                {
                    sb.Append($"Lv{it.Level}, ");
                }
                sb.Remove(sb.Length - 2, 2);
                sb.Append("\n");
            }
            foreach (var group in seriesSkills)
            {
                foreach (var it in group.OrderBy(s => s.Level))
                {
                    if (it.Level == it.MaxLevel1)
                    {
                        sb.Append($"{it.ActivateSkillName1}({it.Name}Lv{it.Level})\n");
                    }
                    if (it.Level == it.MaxLevel2)
                    {
                        sb.Append($"{it.ActivateSkillName2}({it.Name}Lv{it.Level})\n");
                    }
                }
            }
            // 画面に表示
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

            // 武器固定がない場合
            if (WeaponSelectVM.Value.Weapon.Name == "")
            {
                // 武器種指定がない場合
                if (WeaponSelectVM.Value.SelectedWeaponKind.Value == "---")
                {
                    // 全武器を検索対象にする
                    condition.Equips.AddRange(Master.Weapons.SelectMany(w => w));
                    condition.Equips.AddRange(Master.AddWeapons.SelectMany(w => w));
                }
                else
                {
                    // 属性指定がない場合
                    if (WeaponSelectVM.Value.SelectedElement.Value == "---")
                    {
                        // 指定武器種のみすべて追加
                        condition.Equips.AddRange(Master.Weapons[(int)Kind.WeaponNameToKind(WeaponSelectVM.Value.SelectedWeaponKind.Value)]);
                        condition.Equips.AddRange(Master.AddWeapons[(int)Kind.WeaponNameToKind(WeaponSelectVM.Value.SelectedWeaponKind.Value)]);
                    }
                    else
                    {
                        // 指定武器種の内、属性が一致するもののみを追加
                        condition.Equips.AddRange(Master.Weapons[(int)Kind.WeaponNameToKind(WeaponSelectVM.Value.SelectedWeaponKind.Value)]
                            .Where(w => (w.ElementType1 == (Element)Kind.ElementType[WeaponSelectVM.Value.SelectedElement.Value]) ||
                                        (w.ElementType2 == (Element)Kind.ElementType[WeaponSelectVM.Value.SelectedElement.Value])));
                    }
                }
            }
            else
            {
                // 固定された武器だけ追加
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

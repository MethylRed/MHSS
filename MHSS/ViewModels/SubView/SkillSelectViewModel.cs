using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using MHSS.Models.Data;
using MHSS.Models.Utility;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Reactive.Bindings;
using MHSS.ViewModels.Controls;
using Google.OrTools.LinearSolver;

namespace MHSS.ViewModels.SubView
{
    internal class SkillSelectViewModel : SubViewModelBase
    {
        /// <summary>
        /// カテゴリ別スキル選択アイテムのViewModel
        /// </summary>
        public ReactivePropertySlim<ObservableCollection<SkillLevelSelectorsByCategoryViewModel>> SkillLevelSelectorsByCategoryVM { get; } = new();


        //public AsyncReactiveCommand SolveCommand { get; private set; }
        public DelegateCommand SolveCommand { get; private set; }

        private Solve _Solve { get; set; }

        public SkillSelectViewModel()
        {
            // カテゴリ別にスキル条件選択のComboBoxを配置
            ObservableCollection<SkillLevelSelectorsByCategoryViewModel> oldColl = SkillLevelSelectorsByCategoryVM.Value;
            if (oldColl != null)
            {
                foreach (var item in oldColl)
                {
                    ((IDisposable)item).Dispose();
                }
            }
            SkillLevelSelectorsByCategoryVM.Value = new ObservableCollection<SkillLevelSelectorsByCategoryViewModel>(
                Master.Skills.GroupBy(s => s.Category).OrderBy(g => Kind.SkillCategory.IndexOf(g.Key))
                .Select(g => new SkillLevelSelectorsByCategoryViewModel(g.Key, g))
            );

            SolveCommand = new DelegateCommand(Solve);
        }

        /// <summary>
        /// 検索を実行
        /// </summary>
        private void Solve()
        {
            // スキルの検索条件を取得
            Condition condition = MakeCondition();

            _Solve = new(condition);

            Solver.ResultStatus resultStatus = _Solve.Solver.Solve();
            if (resultStatus != Solver.ResultStatus.OPTIMAL)
            {
                Debug.WriteLine("NOT OPTIMAL SOLUTION!");
            }
            else
            {
                Debug.WriteLine(_Solve.Solver.Objective().Value());

                var a = _Solve.Variables.Where(v => v.Value.SolutionValue() > 0);

                foreach (var item in a)
                {
                    Debug.WriteLine(item.Key + " *" + item.Value.SolutionValue().ToString());
                }
                //Debug.WriteLine(string.Join("\n",_Solve.Variables.Where(v => v.Value.SolutionValue() > 0).Select(k => k.Key)));
            }

            Debug.WriteLine("\n Check is finished.");
        }

        private Condition MakeCondition()
        {
            Condition condition = new();

            condition.Skills = SkillLevelSelectorsByCategoryVM.Value.SelectMany(v => v.SelectedSkills()).ToList();

            return condition;
        }
    }
}
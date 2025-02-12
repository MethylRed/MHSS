using System.Reflection;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Documents;
using MHSS.Models.Repository;
using MHSS.Models.Utility;
using Google.OrTools.LinearSolver;

namespace MHSS.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "MHSS";
        public DelegateCommand ClickCommand { get; set; }
        private Solve solve {  get; set; }
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {
            ClickCommand = new DelegateCommand(OnClick);
        }

        private void OnClick()
        {
#if DEBUG
            // logic部分の確認

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

            #region DefineVariables
            // 個数変数の定義を確認
            // 各装備種類について、個数変数の数 = 装備の数と総和を表示
            solve = new();
            int a = 0;
            for (int i = 0; i < solve.EquipVariablesList.Count; i++)
            {
                Debug.WriteLine(solve.EquipVariablesList[i].Count);
                a += solve.EquipVariablesList[i].Count;
            }
            Debug.WriteLine(solve.DecoVariables.Count);
            a += solve.DecoVariables.Count;
            Debug.WriteLine(a);
            #endregion

            #region DefineConstraint
            // 制約式の定義を確認
            
            double b = 0;
            string[] EquipNameList = { "Head", "Body", "Arm", "Waist", "Leg", "Charm" };

            for (int i = 0; i < solve.EquipVariablesList.Count; i++)
            {
                foreach (var equip in solve.EquipVariablesList[i])
                {
                    b += solve.EquipConstraints[EquipNameList[i]].GetCoefficient(equip.Value);
                }
            }
            Debug.WriteLine(b);
            #endregion
#endif
        }
    }
}

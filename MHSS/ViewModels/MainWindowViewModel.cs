using System.Reflection;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Documents;
using MHSS.Models.Repository;
using MHSS.Models.Utility;

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
            CSVLoader.LoadCsvSkill();
            foreach (var item in Master.Skills)
            {
                Debug.WriteLine(item);
            }

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

            solve = new();
            Debug.WriteLine(solve.EquipVariables.Count);
            Debug.WriteLine(solve.DecoVariables.Count);
#endif
        }
    }
}

using System.Reflection;
using Models.Repository;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Diagnostics;
using System.Windows.Documents;
using static Google.Protobuf.WellKnownTypes.Field.Types;

namespace MHSS.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "MHSS";
        public DelegateCommand ClickCommand { get; set; }
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
            //Models.Utility.CSVLoader.LoadCsvSkill();
            //foreach (var item in Master.Skills)
            //{
            //    Debug.WriteLine(item);
            //}

            Models.Utility.CSVLoader.LoadCsvHead();
            var x = Master.Head[100];
            Debug.WriteLine(
                //EquipKinds.EquipKindsToString(x.EquipKind), 
                x.Name, x.SeriesName,
                            x.Slot1, x.Slot2, x.Slot3, x.Def, x.ResFire, x.ResWater,
                            x.ResThunder, x.ResIce, x.ResDragon);

            foreach (var item in x.Skill)
            {
                Debug.WriteLine(item);
            }
#endif
        }
    }
}

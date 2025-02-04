using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Diagnostics;

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
            Debug.WriteLine("TEST");
            Models.Utility.CSVLoader.LoadCsvSkill();
        }
    }
}

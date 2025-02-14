using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using MHSS.Models.Data;
using MHSS.Models.Utility;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace MHSS.ViewModels.SubView
{
    public class SkillSelectViewModel : BindableBase
    {
        public ObservableCollection<Skill> CategorizedSkill { get; set; } = new();

        public SkillSelectViewModel()
        {
            var s = Master.Skills;//.Where(s => s.Category.Contains("クエスト")).ToList();

            //var s = new List<Skill>()
            //{
            //    new Skill(){ Name = "a", Category = "クエスト"},
            //    new Skill(){ Name = "b", Category = "アイテム"}
            //};
            CategorizedSkill.Clear();
            foreach (var skill in s)
            {
                CategorizedSkill.Add(skill);
            }
        }
    }
}

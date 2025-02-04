using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using Csv;
using Models.Repository;
using System.Diagnostics;
using System.Xml.Linq;

namespace Models.Utility
{
    public class CSVLoader
    {
        private const string CsvSkill = "./Data/Skill.csv";

        public static void LoadCsvSkill()
        {
            Master.Skills = new();

            string str = File.ReadAllText(CsvSkill);
            foreach(ICsvLine line in CsvReader.ReadFromText(str))
            {
                Skill skill = new();
                skill.Name = line[@"名前"];
                skill.Level = 0;
                skill.MaxLevel1 = int.Parse(line[@"上限1"]);
                skill.MaxLevel2 = int.Parse(line[@"上限2"]);
                
                Master.Skills.Add(skill);
            }

            //Master.Skills = CsvReader.ReadFromText(str)
            //    .Select(line => new
            //    {
            //        Name = line[@"名前"],
            //        Level = 0,
            //        MaxLevel1 = int.Parse(line[@"上限1"]),
            //        MaxLevel2 = int.Parse(line[@"上限2"])
            //    })
            //    .GroupBy(x => new { x.Name, x.Level, x.MaxLevel1, x.MaxLevel2 })
            //    .Select(group => new Skill(group.Key.Name, group.Key.Level, group.Key.MaxLevel1, group.Key.MaxLevel2))
            //    .ToList();

#if DEBUG
            foreach (var item in Master.Skills)
            {
                Debug.WriteLine(item);
            }
#endif
        }

        public static void LoadCsvArmor(string fileName, List<Armor> armors, EquipKind equipKind)
        {
            string str = File.ReadAllText(fileName);
            foreach(ICsvLine line in CsvReader.ReadFromText(str))
            {
                Armor armor = new();
                armor.Name = line[@"名前"];
                armor.SeriesName = "";
                armor.Slot1 = int.Parse(line[@"スロット1"]);
                armor.Slot2 = int.Parse(line[@"スロット2"]);
                armor.Slot3 = int.Parse(line[@"スロット3"]);
                armor.Def = int.Parse(line[@"最終防御力"]);
                armor.ResFire = int.Parse(line[@"火耐性"]);
                armor.ResWater = int.Parse(line[@"水耐性"]);
                armor.ResThunder = int.Parse(line[@"雷耐性"]);
                armor.ResIce = int.Parse(line[@"氷耐性"]);
                armor.ResDragon = int.Parse(line[@"龍耐性"]);

                List<Skill> skill = new();
                for(int i = 0; i < 5; i++)
                {
                    string skillName = line[@"スキル系統" + i];
                }
            }
        }
    }
}

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
        private const string CsvSkill = "./Models/Data/Skill.csv";
        private const string CsvHead = "./Models/Data/MHW_EQUIP_HEAD.csv";

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
        }

        public static void LoadCsvHead()
        {
            Master.Head = new();
            LoadCsvArmor(CsvHead, Master.Head, EquipKind.head);
        }

        private static void LoadCsvArmor(string fileName, List<Armor> armors, EquipKind equipKind)
        {
            string str = File.ReadAllText(fileName);
            int j = 0;
            foreach(ICsvLine line in CsvReader.ReadFromText(str))
            {
                j++;
                if (j == 100)
                {
                    Debug.Write("");
                }
                Armor armor = new()
                {
                    EquipKind = equipKind,
                    Name = line[@"名前"],
                    SeriesName = "",
                    Slot1 = int.Parse(line[@"スロット1"]),
                    Slot2 = int.Parse(line[@"スロット2"]),
                    Slot3 = int.Parse(line[@"スロット3"]),
                    Def = int.Parse(line[@"最終防御力"]),
                    ResFire = int.Parse(line[@"火耐性"]),
                    ResWater = int.Parse(line[@"水耐性"]),
                    ResThunder = int.Parse(line[@"雷耐性"]),
                    ResIce = int.Parse(line[@"氷耐性"]),
                    ResDragon = int.Parse(line[@"龍耐性"])
                };

                List<Skill> skill = new();
                for(int i = 1; i <= Config.Config.Instance.MaxArmorSkillCount; i++)
                {
                    string skillName = line[@"スキル系統" + i];
                    if (string.IsNullOrWhiteSpace(skillName)) break;
                    int skillLevel = int.Parse(line[@"スキル値" + i]);
                    skill.Add(new Skill
                    {
                        Name = skillName,
                        Level = skillLevel
                    });
                }
                armor.Skill = skill;

                armors.Add( armor );
            }
        }
    }
}

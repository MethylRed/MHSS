using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using Csv;
using System.Diagnostics;
using System.Xml.Linq;
using MHSS.Models.Repository;
using MHSS.Models.Config;

namespace MHSS.Models.Utility
{
    public class CSVLoader
    {
        // ファイルパス定数
        private const string CsvSkill = "./Models/Data/MHWs_SKILL.csv";
        private const string CsvHead = "./Models/Data/MHWs_HEAD.csv";
        private const string CsvBody = "./Models/Data/MHWs_BODY.csv";
        private const string CsvArm = "./Models/Data/MHWs_ARM.csv";
        private const string CsvWaist = "./Models/Data/MHWs_WST.csv";
        private const string CsvLeg = "./Models/Data/MHWs_LEG.csv";
        private const string CsvDeco = "./Models/Data/MHWs_DECO.csv";
        private const string CsvCharm = "./Models/Data/MHWs_CHARM.csv";

        private static readonly string[] CsvWeapons =
        {
            "./Models/Data/Weapon/MHWs_GREATSWORD.csv",
            "./Models/Data/Weapon/MHWs_LONGSWORD.csv"
        };

        private const string CsvGS = "./Models/Data/Weapon/MHWs_GREATSWORD.csv";
        private const string CsvLS = "./Models/Data/Weapon/MHWs_LONGSWORD.csv";
        private const string CsvSnS = "./Models/Data/Weapon/MHWs_SWORDANDSHIELD.csv";
        private const string CsvDB = "./Models/Data/Weapon/MHWs_DUALBLADES.csv";
        private const string CsvHM = "./Models/Data/Weapon/MHWs_HAMMER.csv";
        private const string CsvHH = "./Models/Data/Weapon/MHWs_HUNTINGHORN.csv";
        private const string CsvLC = "./Models/Data/Weapon/MHWs_LANCE.csv";
        private const string CsvGL = "./Models/Data/Weapon/MHWs_GUNLANCE.csv";
        private const string CsvSA = "./Models/Data/Weapon/MHWs_SWITCHAXE.csv";
        private const string CsvCB = "./Models/Data/Weapon/MHWs_CHARGEBLADE.csv";
        private const string CsvIG = "./Models/Data/Weapon/MHWs_INSECTGLAIVE.csv";
        private const string CsvLBG = "./Models/Data/Weapon/MHWs_LIGHTBOWGUN.csv";
        private const string CsvHBG = "./Models/Data/Weapon/MHWs_HEAVYBOWGUN.csv";
        private const string CsvBOW = "./Models/Data/Weapon/MHWs_BOW.csv";

        /// <summary>
        /// スキルのリストを読み込む
        /// </summary>
        public static void LoadCsvSkill()
        {
            Master.Skills = new();

            string str = File.ReadAllText(CsvSkill);
            foreach (ICsvLine line in CsvReader.ReadFromText(str))
            {
                Skill skill = new()
                {
                    Name = line[@"名前"],
                    Type = int.Parse(line[@"タイプ"]),
                    Level = 0,
                    MaxLevel1 = int.Parse(line[@"上限1"]),
                    MaxLevel2 = int.Parse(line[@"上限2"])
                };

                Master.Skills.Add(skill);
            }
        }

        /// <summary>
        /// 頭防具の読み込み
        /// </summary>
        public static void LoadCsvHead()
        {
            Master.Head = new();
            LoadCsvArmor(CsvHead, Master.Head, EquipKind.Head);
        }

        /// <summary>
        /// 胴防具の読み込み
        /// </summary>
        public static void LoadCsvBody()
        {
            Master.Body = new();
            LoadCsvArmor(CsvBody, Master.Body, EquipKind.Body);
        }

        /// <summary>
        /// 腕防具の読み込み
        /// </summary>
        public static void LoadCsvArm()
        {
            Master.Arm = new();
            LoadCsvArmor(CsvArm, Master.Arm, EquipKind.Arm);
        }

        /// <summary>
        /// 腰防具の読み込み
        /// </summary>
        public static void LoadCsvWaist()
        {
            Master.Waist = new();
            LoadCsvArmor(CsvWaist, Master.Waist, EquipKind.Waist);
        }

        /// <summary>
        /// 脚防具の読み込み
        /// </summary>
        public static void LoadCsvLeg()
        {
            Master.Leg = new();
            LoadCsvArmor(CsvLeg, Master.Leg, EquipKind.Leg);
        }

        /// <summary>
        /// 護石の読み込み
        /// </summary>
        public static void LoadCsvCharm()
        {
            Master.Charm = new();
            string str = File.ReadAllText(CsvCharm);
            foreach (ICsvLine line in CsvReader.ReadFromText(str))
            {
                Equip charm = new()
                {
                    EquipKind = EquipKind.Charm,
                    Name = line[@"名前"],
                    //SeriesName = "",
                    Slot1 = int.Parse(line[@"スロット1"]),
                    Slot2 = int.Parse(line[@"スロット2"]),
                    Slot3 = int.Parse(line[@"スロット3"]),
                    Def = 0,
                    ResFire = 0,
                    ResWater = 0,
                    ResThunder = 0,
                    ResIce = 0,
                    ResDragon = 0
                };

                List<Skill> skill = new();
                for (int i = 1; i <= Config.Config.Instance.MaxCharmSkillCount; i++)
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
                charm.Skill = skill;

                Master.Charm.Add(charm);
            }
        }

        /// <summary>
        /// 装飾品の読み込み
        /// </summary>
        public static void LoadCsvDeco()
        {
            Master.Deco = new();
            string str = File.ReadAllText(CsvDeco);
            foreach (ICsvLine line in CsvReader.ReadFromText(str))
            {
                Equip deco = new()
                {
                    EquipKind = EquipKind.Deco,
                    Name = line[@"名前"],
                    //SeriesName = "",
                    Slot1 = int.Parse(line[@"スロットサイズ"]),
                    Slot2 = 0,
                    Slot3 = 0,
                    Def = 0,
                    ResFire = 0,
                    ResWater = 0,
                    ResThunder = 0,
                    ResIce = 0,
                    ResDragon = 0
                };

                List<Skill> skill = new();
                for (int i = 1; i <= Config.Config.Instance.MaxDecoSkillCount; i++)
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
                deco.Skill = skill;

                Master.Deco.Add(deco);
            }
        }

        /// <summary>
        /// 武器読み込み
        /// </summary>
        public static void LoadCsvWeapon()
        {
            Master.Weapons = new();
            foreach(var csvWeapon in CsvWeapons)
            {
                List<Weapon> weapon = new();
                string str = File.ReadAllText(csvWeapon);
                foreach (ICsvLine line in CsvReader.ReadFromText(str))
                {
                    Weapon w = new Weapon()
                    {
                        EquipKind = EquipKind.Weapon,
                        WeaponKind = 
                    };
                }
            }

        }

        /// <summary>
        /// 防具読み込み
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="armors">格納先</param>
        /// <param name="equipKind">部位</param>
        private static void LoadCsvArmor(string fileName, List<Equip> armors, EquipKind equipKind)
        {
            string str = File.ReadAllText(fileName);
            foreach (ICsvLine line in CsvReader.ReadFromText(str))
            {
                Equip armor = new()
                {
                    EquipKind = equipKind,
                    Name = line[@"名前"],
                    //SeriesName = "",
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
                for (int i = 1; i <= Config.Config.Instance.MaxArmorSkillCount; i++)
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

                armors.Add(armor);
            }
        }
    }
}

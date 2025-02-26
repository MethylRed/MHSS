using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using Csv;
using System.Diagnostics;
using System.Xml.Linq;
using MHSS.Models.Data;
using MHSS.Models.Config;
using System.Text.Unicode;

namespace MHSS.Models.Utility
{
    public class FileManager
    {
        // ファイルパス定数
        private const string CsvSkill = "./Models/Data/CSV/MHWs_SKILL.csv";
        private const string CsvHead = "./Models/Data/CSV/MHWs_HEAD.csv";
        private const string CsvBody = "./Models/Data/CSV/MHWs_BODY.csv";
        private const string CsvArm = "./Models/Data/CSV/MHWs_ARM.csv";
        private const string CsvWaist = "./Models/Data/CSV/MHWs_WST.csv";
        private const string CsvLeg = "./Models/Data/CSV/MHWs_LEG.csv";
        private const string CsvDeco = "./Models/Data/CSV/MHWs_DECO.csv";
        private const string CsvCharm = "./Models/Data/CSV/MHWs_CHARM.csv";

        private const string JsonDecoCount = "./Models/Data/save/DecoCount.json";
        private const string CsvAddWeapon = "./Models/Data/save/CsvAddWeapon.csv";

        // 武器のファイルパス定数
        private static readonly string[] CsvWeapons =
        {
            "./Models/Data/CSV/Weapon/MHWs_GREATSWORD.csv",
            "./Models/Data/CSV/Weapon/MHWs_LONGSWORD.csv",
            "./Models/Data/CSV/Weapon/MHWs_SWORDANDSHIELD.csv",
            "./Models/Data/CSV/Weapon/MHWs_DUALBLADES.csv",
            "./Models/Data/CSV/Weapon/MHWs_HAMMER.csv",
            "./Models/Data/CSV/Weapon/MHWs_HUNTINGHORN.csv",
            "./Models/Data/CSV/Weapon/MHWs_LANCE.csv",
            "./Models/Data/CSV/Weapon/MHWs_GUNLANCE.csv",
            "./Models/Data/CSV/Weapon/MHWs_SWITCHAXE.csv",
            "./Models/Data/CSV/Weapon/MHWs_CHARGEBLADE.csv",
            "./Models/Data/CSV/Weapon/MHWs_INSECTGLAIVE.csv",
            "./Models/Data/CSV/Weapon/MHWs_LIGHTBOWGUN.csv",
            "./Models/Data/CSV/Weapon/MHWs_HEAVYBOWGUN.csv",
            "./Models/Data/CSV/Weapon/MHWs_BOW.csv"
        };

        //private const string CsvGS = "./Models/Data/CSV/Weapon/MHWs_GREATSWORD.csv";
        //private const string CsvLS = "./Models/Data/CSV/Weapon/MHWs_LONGSWORD.csv";
        //private const string CsvSnS = "./Models/Data/CSV/Weapon/MHWs_SWORDANDSHIELD.csv";
        //private const string CsvDB = "./Models/Data/CSV/Weapon/MHWs_DUALBLADES.csv";
        //private const string CsvHM = "./Models/Data/CSV/Weapon/MHWs_HAMMER.csv";
        //private const string CsvHH = "./Models/Data/CSV/Weapon/MHWs_HUNTINGHORN.csv";
        //private const string CsvLC = "./Models/Data/CSV/Weapon/MHWs_LANCE.csv";
        //private const string CsvGL = "./Models/Data/CSV/Weapon/MHWs_GUNLANCE.csv";
        //private const string CsvSA = "./Models/Data/CSV/Weapon/MHWs_SWITCHAXE.csv";
        //private const string CsvCB = "./Models/Data/CSV/Weapon/MHWs_CHARGEBLADE.csv";
        //private const string CsvIG = "./Models/Data/CSV/Weapon/MHWs_INSECTGLAIVE.csv";
        //private const string CsvLBG = "./Models/Data/CSV/Weapon/MHWs_LIGHTBOWGUN.csv";
        //private const string CsvHBG = "./Models/Data/CSV/Weapon/MHWs_HEAVYBOWGUN.csv";
        //private const string CsvBOW = "./Models/Data/CSV/Weapon/MHWs_BOW.csv";


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
                    Category = line["カテゴリ"],
                    Level = 0,
                    ActivateSkillName1 = Utility.NormalizeEmpty(line[@"発動スキル1"]),
                    ActivateSkillName2 = Utility.NormalizeEmpty(line[@"発動スキル2"]),
                    MaxLevel1 = Utility.ParseOrDefault(line[@"上限1"]),
                    MaxLevel2 = Utility.ParseOrDefault(line[@"上限2"])
                };
                Master.Skills.Add(skill);
            }
        }

        /// <summary>
        /// 装備の読み込み
        /// </summary>
        public static void LoadCsvEquip()
        {
            LoadCsvWeapon();
            LoadCsvHead();
            LoadCsvBody();
            LoadCsvArm();
            LoadCsvWaist();
            LoadCsvLeg();
            LoadCsvCharm();
            LoadCsvDeco();

            //Master.AllEquips = Master.Heads
            //                .Union(Master.Bodies).Union(Master.Arms).Union(Master.Waists)
            //                .Union(Master.Legs).Union(Master.Charms).Union(Master.Decos)
            //                .Union(Master.Weapons.SelectMany(w => w)).ToList();
        }

        /// <summary>
        /// 頭防具の読み込み
        /// </summary>
        public static void LoadCsvHead()
        {
            Master.Heads = new();
            LoadCsvArmor(CsvHead, Master.Heads, EquipKind.Head);
        }

        /// <summary>
        /// 胴防具の読み込み
        /// </summary>
        public static void LoadCsvBody()
        {
            Master.Bodies = new();
            LoadCsvArmor(CsvBody, Master.Bodies, EquipKind.Body);
        }

        /// <summary>
        /// 腕防具の読み込み
        /// </summary>
        public static void LoadCsvArm()
        {
            Master.Arms = new();
            LoadCsvArmor(CsvArm, Master.Arms, EquipKind.Arm);
        }

        /// <summary>
        /// 腰防具の読み込み
        /// </summary>
        public static void LoadCsvWaist()
        {
            Master.Waists = new();
            LoadCsvArmor(CsvWaist, Master.Waists, EquipKind.Waist);
        }

        /// <summary>
        /// 脚防具の読み込み
        /// </summary>
        public static void LoadCsvLeg()
        {
            Master.Legs = new();
            LoadCsvArmor(CsvLeg, Master.Legs, EquipKind.Leg);
        }

        /// <summary>
        /// 護石の読み込み
        /// </summary>
        public static void LoadCsvCharm()
        {
            Master.Charms = new();
            string str = File.ReadAllText(CsvCharm);
            foreach (ICsvLine line in CsvReader.ReadFromText(str))
            {
                Equip charm = new()
                {
                    EquipKind = EquipKind.Charm,
                    Name = line[@"名前"],
                    SeriesName = "",
                    SlotType = 0,
                    Slot1 = Utility.ParseOrDefault(line[@"スロット1"]),
                    Slot2 = Utility.ParseOrDefault(line[@"スロット2"]),
                    Slot3 = Utility.ParseOrDefault(line[@"スロット3"]),
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
                    string skillName = line[$"スキル系統{i}"];
                    if (string.IsNullOrWhiteSpace(skillName)) break;
                    int skillLevel = Utility.ParseOrDefault(line[$"スキル値{i}"]);
                    skill.Add(new Skill
                    {
                        Name = skillName,
                        Level = skillLevel
                    });
                }
                charm.Skills = skill;

                Master.Charms.Add(charm);
            }
        }

        /// <summary>
        /// 装飾品の読み込み
        /// </summary>
        public static void LoadCsvDeco()
        {
            Master.Decos = new();
            string str = File.ReadAllText(CsvDeco);
            foreach (ICsvLine line in CsvReader.ReadFromText(str))
            {
                Deco deco = new()
                {
                    HaveCount = 0,
                    EquipKind = EquipKind.Deco,
                    Name = line[@"名前"],
                    SeriesName = "",
                    SlotType = Utility.ParseOrDefault(line[@"スロットタイプ"], 2),
                    //SlotType = 2,
                    Slot1 = Utility.ParseOrDefault(line[@"スロットサイズ"]),
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
                    string skillName = line[$"スキル系統{i}"];
                    if (string.IsNullOrWhiteSpace(skillName)) break;
                    int skillLevel = Utility.ParseOrDefault(line[$"スキル値{i}"]);
                    skill.Add(new Skill
                    {
                        Name = skillName,
                        Level = skillLevel
                    });
                }
                deco.Skills = skill;

                Master.Decos.Add(deco);
            }
            LoadDecoCount();
        }

        /// <summary>
        /// 武器読み込み
        /// </summary>
        public static void LoadCsvWeapon()
        {
            Master.Weapons = new();
            for (int i = 0; i < CsvWeapons.Length; i++)
            {
                List<Weapon> weapon = new();
                string str = File.ReadAllText(CsvWeapons[i]);
                foreach (ICsvLine line in CsvReader.ReadFromText(str))
                {
                    Weapon w = new()
                    {
                        EquipKind = EquipKind.Weapon,
                        WeaponKind = (WeaponKind)i,
                        Name = line[@"名前"],
                        SeriesName = "",
                        Attack = Utility.ParseOrDefault(line[@"攻撃力"]),
                        Affinity = Utility.ParseOrDefault(line[@"会心率"]),
                        ElementType1 = (Element)Kind.ElementType.GetValueOrDefault(line[@"属性1"],0),
                        ElementValue1 = Utility.ParseOrDefault(line[@"属性値1"]),
                        ElementType2 = (Element)Kind.ElementType.GetValueOrDefault(line[@"属性2"],0),
                        ElementValue2 = Utility.ParseOrDefault(line[@"属性値2"]),
                        SlotType = 0,
                        Slot1 = Utility.ParseOrDefault(line[@"スロット1"]),
                        Slot2 = Utility.ParseOrDefault(line[@"スロット2"]),
                        Slot3 = Utility.ParseOrDefault(line[@"スロット3"]),
                        Def = 0,
                        DefBonus = Utility.ParseOrDefault(line[@"防御力ボーナス"]),
                        ResFire = 0,
                        ResWater = 0,
                        ResThunder = 0,
                        ResIce = 0,
                        ResDragon = 0
                    };
                    List<Skill> skill = new();
                    for (int j = 1; j <= Config.Config.Instance.MaxWeaponSkillCount; j++)
                    {
                        string skillName = line[$"スキル系統{j}"];
                        if (string.IsNullOrWhiteSpace(skillName)) break;
                        int skillLevel = Utility.ParseOrDefault(line[$"スキル値{j}"]);
                        skill.Add(new Skill
                        {
                            Name = skillName,
                            Level = skillLevel
                        });
                    }
                    w.Skills = skill;

                    weapon.Add(w);
                }
                Master.Weapons.Add(weapon);
            }
            LoadAddWeapon();
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
                    SeriesName = line[@"シリーズ名"],
                    SlotType = 1,
                    Slot1 = Utility.ParseOrDefault(line[@"スロット1"]),
                    Slot2 = Utility.ParseOrDefault(line[@"スロット2"]),
                    Slot3 = Utility.ParseOrDefault(line[@"スロット3"]),
                    Def = Utility.ParseOrDefault(line[@"最終防御力"]),
                    ResFire = Utility.ParseOrDefault(line[@"火耐性"]),
                    ResWater = Utility.ParseOrDefault(line[@"水耐性"]),
                    ResThunder = Utility.ParseOrDefault(line[@"雷耐性"]),
                    ResIce = Utility.ParseOrDefault(line[@"氷耐性"]),
                    ResDragon = Utility.ParseOrDefault(line[@"龍耐性"])
                };

                List<Skill> skill = new();
                for (int i = 1; i <= Config.Config.Instance.MaxArmorSkillCount; i++)
                {
                    string skillName = line[$"スキル系統{i}"];
                    if (string.IsNullOrWhiteSpace(skillName)) break;
                    int skillLevel = Utility.ParseOrDefault(line[$"スキル値{i}"]);
                    skill.Add(new Skill
                    {
                        Name = skillName,
                        Level = skillLevel
                    });
                }
                armor.Skills = skill;

                armors.Add(armor);
            }
        }

        /// <summary>
        /// 装飾品の所持数をロードする 泣シミュがJSON形式なので合わせる
        /// </summary>
        private static void LoadDecoCount()
        {
            // ファイルがない場合は作成
            if (!File.Exists(JsonDecoCount))
            {
                // ディレクトリも作成
                string defualtStr = JsonSerializer.Serialize(new Dictionary<string, int>());
                Directory.CreateDirectory(Path.GetDirectoryName(JsonDecoCount));
                File.WriteAllText(JsonDecoCount, defualtStr);
            }
            else
            {
                string json = File.ReadAllText(JsonDecoCount);
                // 何も書かれていない場合は終了
                if (string.IsNullOrWhiteSpace(json))
                {
                    return;
                }
                else
                {
                    JsonSerializerOptions options = new();
                    options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All);
                    Dictionary<string, int> decoCounts = JsonSerializer.Deserialize<Dictionary<string, int>>(json, options);

                    foreach (var deco in Master.Decos)
                    {
                        deco.HaveCount = decoCounts.TryGetValue(deco.Name, out int count) ? int.Min(count, Config.Config.MaxDecoCount) : 0;
                    }
                }
            }
        }

        /// <summary>
        /// 装飾品の所持数をセーブする 泣シミュがJSON形式なので合わせる
        /// </summary>
        public static void SaveDecoCount()
        {
            Dictionary<string, int> data = new();

            foreach (var deco in Master.Decos)
            {
                data.Add(deco.Name, deco.HaveCount);
            }
            JsonSerializerOptions options = new();
            options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All);
            string json = JsonSerializer.Serialize(data, options);

            File.WriteAllText(JsonDecoCount, json);
        }

        /// <summary>
        /// 登録武器のロード
        /// </summary>
        private static void LoadAddWeapon()
        {
            // ファイルがない場合は作成
            if (!File.Exists(CsvAddWeapon))
            {
                // ディレクトリも作成
                string defualtStr = "種類,名前,レア度,攻撃力,会心率,属性1,属性値1,属性2,属性値2,スロットタイプ,スロット1,スロット2,スロット3,防御力ボーナス,入手時期,スキル系統1,スキル値1,スキル系統2,スキル値2,スキル系統3,スキル値3,スキル系統4,スキル値4,スキル系統5,スキル値5\n";
                Directory.CreateDirectory(Path.GetDirectoryName(CsvAddWeapon));
                File.WriteAllText(CsvAddWeapon, defualtStr);
            }
            else
            {
                for (int i = 0; i < 14; i++) Master.AddWeapons.Add(new());
                string str = File.ReadAllText(CsvAddWeapon);
                foreach (ICsvLine line in CsvReader.ReadFromText(str))
                {
                    Weapon w = new()
                    {
                        EquipKind = EquipKind.Weapon,
                        WeaponKind = (WeaponKind)Utility.ParseFromCsvLineOrDefault(line, "種類"),
                        Name = line[@"名前"],
                        SeriesName = "",
                        Attack = Utility.ParseOrDefault(line[@"攻撃力"]),
                        Affinity = Utility.ParseOrDefault(line[@"会心率"]),
                        ElementType1 = (Element)Kind.ElementType.GetValueOrDefault(line[@"属性1"], 0),
                        ElementValue1 = Utility.ParseOrDefault(line[@"属性値1"]),
                        ElementType2 = (Element)Kind.ElementType.GetValueOrDefault(line[@"属性2"], 0),
                        ElementValue2 = Utility.ParseOrDefault(line[@"属性値2"]),
                        SlotType = 0,
                        Slot1 = Utility.ParseOrDefault(line[@"スロット1"]),
                        Slot2 = Utility.ParseOrDefault(line[@"スロット2"]),
                        Slot3 = Utility.ParseOrDefault(line[@"スロット3"]),
                        Def = 0,
                        DefBonus = Utility.ParseOrDefault(line[@"防御力ボーナス"]),
                        ResFire = 0,
                        ResWater = 0,
                        ResThunder = 0,
                        ResIce = 0,
                        ResDragon = 0
                    };
                    List<Skill> skill = new();
                    for (int j = 1; j <= Config.Config.Instance.MaxWeaponSkillCount; j++)
                    {
                        string skillName = line[$"スキル系統{j}"];
                        if (string.IsNullOrWhiteSpace(skillName)) break;
                        int skillLevel = Utility.ParseOrDefault(line[$"スキル値{j}"]);
                        skill.Add(new Skill
                        {
                            Name = skillName,
                            Level = skillLevel
                        });
                    }
                    w.Skills = skill;
                    Master.Weapons[Utility.ParseFromCsvLineOrDefault(line, "種類")].Add(w);
                }
            }
        }

        /// <summary>
        /// 登録武器のセーブ
        /// </summary>
        public static void SaveAddWeapon()
        {
            foreach (var weapon in Master.AddWeapons.SelectMany(w => w))
            {
                StringBuilder sb = new();
                sb.Append($"{(int)weapon.WeaponKind},");
                sb.Append($"{weapon.Name},0,");
                sb.Append($"{weapon.Attack},");
                sb.Append($"{weapon.Affinity},");
                sb.Append($"{Kind.ElementTypeStr[(int)weapon.ElementType1]},");
                sb.Append($"{weapon.ElementValue1},");
                sb.Append($"{Kind.ElementTypeStr[(int)weapon.ElementType2]},");
                sb.Append($"{weapon.ElementValue2},0,");
                sb.Append($"{weapon.Slot1},");
                sb.Append($"{weapon.Slot2},");
                sb.Append($"{weapon.Slot3},");
                sb.Append($"{weapon.DefBonus},,");

                int i = 0;
                foreach (var s in weapon.Skills)
                {
                    sb.Append($"{s.Name},{s.Level},");
                    i++;
                }
                for (int j = i; j < Config.Config.Instance.MaxWeaponSkillCount; j++)
                {
                    sb.Append(",,");
                }
                sb = sb.Remove(sb.Length - 1, 1);

                using (StreamWriter writer = new StreamWriter(CsvAddWeapon, append: true))
                {
                    writer.WriteLine(sb.ToString());
                }
            }
        }
    }
}

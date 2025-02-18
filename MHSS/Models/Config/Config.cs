using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Csv;

namespace MHSS.Models.Config
{
    public class Config
    {
        private static Config instance;
        public static Config Instance
        {
            get
            {
                instance ??= new();
                return instance;
            }
        }

        /// <summary>
        /// ConfigのCSVファイルパス
        /// </summary>
        private const string CsvConfig = "./Models/Config/Config.csv";

        /// <summary>
        /// 防具に付くスキルの最大個数
        /// </summary>
        public int MaxArmorSkillCount { get; set; }

        /// <summary>
        /// 武器に付くスキルの最大個数
        /// </summary>
        public int MaxWeaponSkillCount { get; set; }

        /// <summary>
        /// 護石に付くスキルの最大個数
        /// </summary>
        public int MaxCharmSkillCount { get; set; }

        /// <summary>
        /// 装飾品に付くスキルの最大個数
        /// </summary>
        public int MaxDecoSkillCount { get; set; }

        /// <summary>
        /// 検索の最大回数
        /// </summary>
        public int MaxSearchCount { get; set; }

        private Config()
        {
            string str = File.ReadAllText(CsvConfig, Encoding.UTF8);
            foreach (ICsvLine line in CsvReader.ReadFromText(str))
            {
                MaxArmorSkillCount = Utility.Utility.ParseFromCsvLineOrDefault(line, "防具に付くスキルの最大個数", 5);
                MaxWeaponSkillCount = Utility.Utility.ParseFromCsvLineOrDefault(line, "武器に付くスキルの最大個数", 5);
                MaxCharmSkillCount = Utility.Utility.ParseFromCsvLineOrDefault(line, "護石に付くスキルの最大個数", 2);
                MaxDecoSkillCount = Utility.Utility.ParseFromCsvLineOrDefault(line, "装飾品に付くスキルの最大個数", 2);
                MaxSearchCount = Utility.Utility.ParseFromCsvLineOrDefault(line, "検索の最大回数", 50);
            }
        }
    }
}

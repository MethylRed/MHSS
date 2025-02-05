using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Csv;

namespace Models.Config
{
    public class Config
    {
        private static Config instance;
        public static Config Instance
        {
            get
            {
                if (instance == null) instance = new();
                return instance;
            }
        }
        private const string CsvConfig = "./Models/Config/Config.csv";

        /// <summary>
        /// 防具に付くスキルの最大個数
        /// </summary>
        public int MaxArmorSkillCount { get; set; }

        /// <summary>
        /// 護石に付くスキルの最大個数
        /// </summary>
        public int MaxCharmSkillCount { get; set; }

        /// <summary>
        /// 装飾品に付くスキルの最大個数
        /// </summary>
        public int MaxDecoSkillCount { get; set; }

        private Config()
        {
            string str = File.ReadAllText(CsvConfig,Encoding.UTF8);


            foreach (ICsvLine line in CsvReader.ReadFromText(str))
            {
                MaxArmorSkillCount = int.Parse(line[@"防具に付くスキルの最大個数"]);
                MaxCharmSkillCount = int.Parse(line[@"護石に付くスキルの最大個数"]);
                MaxDecoSkillCount = int.Parse(line[@"装飾品に付くスキルの最大個数"]);
            }
        }
    }
}

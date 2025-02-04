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
        public int MaxArmorSkillCount { get; set; }

        private Config()
        {
            string str = File.ReadAllText(CsvConfig,Encoding.UTF8);


            foreach (ICsvLine line in CsvReader.ReadFromText(str))
            {
                MaxArmorSkillCount = int.Parse(line[@"防具に付くスキルの最大個数"]);
            }
        }
    }
}

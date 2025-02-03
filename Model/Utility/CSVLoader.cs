using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using Csv;
using Models.Repository;

namespace Models.Utility
{
    internal class CSVLoader
    {
        private const string CsvSkill = "Skill.csv";

        static internal void LoadCsvSkill()
        {
            string str = File.ReadAllText(CsvSkill);

            Master.Skills = CsvReader.ReadFromText(str).Select(line => new
            {
                Name = line[@"名前"],
                Level = line[]
            })
        }

    }
}

using Csv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHSS.Models.Utility
{
    public class Utility
    {
        /// <summary>
        /// 文字列を数値に変換する 変換できない場合指定値を返す
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ParseOrDefault(string input, int defaultValue = 0)
        {
            return int.TryParse(input, out int result) ? result : defaultValue;
        }

        /// <summary>
        /// CSVから読み込んだ文字列を数値に変換する
        /// エラーの場合指定値を返す
        /// </summary>
        /// <param name="line"></param>
        /// <param name="columnName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ParseFromCsvLineOrDefault(ICsvLine line, string columnName, int defaultValue = 0)
        {
            try
            {
                return ParseOrDefault(line[columnName], defaultValue);
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}

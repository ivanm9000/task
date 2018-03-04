using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivan_Mihalj_Task.MODEL
{
    class Parser
    {
        public string parseToBigBinary(string first, string second, int padLeftFirst, int padLeftSecond)
        {
            string str = "";

            str = String.Format("{0}{1}", Convert.ToString(Convert.ToInt64(first, 16), 2).PadLeft(padLeftFirst, '0'), Convert.ToString(Convert.ToInt64(second, 16), 2).PadLeft(padLeftSecond, '0'));
            return str;
        }

        public string normalizeString(string fullName)
        {
            string str = "";

            str = fullName.Replace(" ", "").Replace("-", "").Replace("_", "").Replace("&", "").Replace(",", "").Replace(".", "");
            str = str.ToLower();

            return str;
        }

        public int getCompanyPrefixLength(string part)
        {
            switch (part)
            {
                case "000":
                    return 40;
                case "001":
                    return 37;
                case "010":
                    return 34;
                case "011":
                    return 30;
                case "100":
                    return 27;
                case "101":
                    return 24;
                case "110":
                    return 20;
                default:
                    return 0;
            }
        }

        public string parseToBinary(string item,int length)
        {
            string str = "";

            str = String.Format(Convert.ToString(Convert.ToInt64(item, 10), 2).PadLeft(length, '0'));
            return str;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivan_Mihalj_Task.MODEL
{
    class HexNumber
    {
        public bool isHex(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (!((str[i] >= '0' && str[i] <= '9') || (str[i] >= 'a' && str[i] <= 'f') || (str[i] >= 'A' && str[i] <= 'F')))
                {
                    return false;
                }
            }
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justech
{
    class FCSCHECK
    {



        public static string FCS(string data)
        {

            string hostlink = data + computefcs(data) + "*" + (char)13;

            return hostlink;

        }
        public static string FCS_R(string data)
        {

            string hostlink = data + computefcs(data) + "*" + (char)13;

            return hostlink;

        }
        private static string computefcs(string linkstring)
        {

            char infcs = (char)linkstring[0];
            int fcsresult = (int)infcs;
            for (int i = 1; i < linkstring.Length; i++)
            {

                infcs = (char)linkstring[i];
                fcsresult ^= (int)infcs;
            }
            return fcsresult.ToString("X2");
        }
        public static bool CHECKFCS(string reeives)
        {

            int i = reeives.IndexOf('*');
            string data = reeives.Substring(0, i - 2);
            if (reeives.Substring(i - 2, 2).Equals(computefcs(data)))
                return true;
            else return false;

        }
    }
}

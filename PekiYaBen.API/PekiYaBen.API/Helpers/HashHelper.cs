using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PekiYaBen.API.Helpers
{
    public static class HashHelper
    {
        public static string ConvertToMd5x(string pass)
        {

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string passMd5 = MD5Hash(md5, pass);
            char[] dd = passMd5.ToCharArray();
            String bir = dd[0].ToString() + dd[1].ToString() + dd[2].ToString() + dd[3].ToString() + dd[4].ToString() + dd[5].ToString() + dd[6].ToString() + dd[7].ToString();
            String iki = dd[8].ToString() + dd[9].ToString() + dd[10].ToString() + dd[11].ToString() + dd[12].ToString() + dd[13].ToString() + dd[14].ToString() + dd[15].ToString();
            String uc = dd[16].ToString() + dd[17].ToString() + dd[18].ToString() + dd[19].ToString() + dd[20].ToString() + dd[21].ToString() + dd[22].ToString() + dd[23].ToString();
            String dort = dd[24].ToString() + dd[25].ToString() + dd[26].ToString() + dd[27].ToString() + dd[28].ToString() + dd[29].ToString() + dd[30].ToString() + dd[31].ToString();

            string[] karma = { iki, dort, bir, uc };
            string password = MD5Hash(md5, string.Concat(karma));

            return password;
        }

        private static string MD5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}

using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Emoda.PekiYaBen.Business.Helpers
{
    public static class ImageHelper
    {
        public static void SaveImage(string base64Image, string path)
        {
            base64Image = Regex.Replace(base64Image, @"^data:image\/[a-zA-Z]+;base64,", string.Empty);

            byte[] bytes = Convert.FromBase64String(base64Image);

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                SaveMemoryStream(ms, path);
            }

        }

        public static void SaveMemoryStream(MemoryStream ms, string FileName)
        {
            FileStream outStream = File.OpenWrite(FileName);
            ms.WriteTo(outStream);
            outStream.Flush();
            outStream.Close();
        }
    }
}

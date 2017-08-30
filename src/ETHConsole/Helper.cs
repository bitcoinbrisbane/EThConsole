using System;
using System.IO;
using System.Text;

namespace ETHConsole
{
    public class Helper
    {
        internal static string GetABIFromFile(String path)
        {
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                String text = streamReader.ReadToEnd();
                return text;
            }
        }

        internal static string GetBytesFromFile(String path)
        {
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                String text = streamReader.ReadToEnd();
                return "0x" + text;
            }
        }
        
        internal static string GetBytesFromFile(String path, String contractname)
        {
            var fileStream = new FileStream(String.Format("{0}/{1}.bin",path, contractname) , FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                String text = streamReader.ReadToEnd();
                return "0x" + text;
            }
        }
    }
}
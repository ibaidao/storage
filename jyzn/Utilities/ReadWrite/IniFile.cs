using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace Utilities
{
    /// <summary>
    /// ini 配置文件的读写
    /// </summary>
    public class IniFile
    {
        //配置文件路径
        private static readonly string iniFileName = Environment.CurrentDirectory + '\\' + "Config.ini";

        #region API函数声明

        [DllImport("kernel32")]//返回0表示失败，非0为成功 
        private static extern long WritePrivateProfileString(string section, string key,
            string val, string filePath);

        [DllImport("kernel32")]//返回取得字符串缓冲区的长度 
        private static extern long GetPrivateProfileString(string section, string key,
            string def, StringBuilder retVal, int size, string filePath);

        #endregion



        #region 读写Ini文件

        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="NoText"></param>
        /// <returns></returns>
        public static string ReadIniData(string Section, string Key, string NoText="")
        {
            if (!File.Exists(iniFileName)) return NoText;

            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(Section, Key, NoText, temp, 1024, iniFileName);
            return temp.ToString();
        }

        /// <summary>
        /// 写入配置文件
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool WriteIniData(string Section, string Key, string Value)
        {
            if (!File.Exists(iniFileName)) return false;

            long OpStation = WritePrivateProfileString(Section, Key, Value, iniFileName);

            return OpStation == 0 ? false : true;
        }
        #endregion

    }
}
using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TIEVision.COMMON
{
    public class IniInfoHelper
    {
        public string iniFilePath = "";
        public static FileIniDataParser fileIniData = new FileIniDataParser();
        IniData parsedData = null;
        public static IniInfoHelper mContext = null;

        public static IniInfoHelper GetInstance()
        {
            if (mContext == null)
            {
                mContext = new IniInfoHelper();
            }
            return mContext;
        }

        public IniInfoHelper()
        {
            iniFilePath = Application.StartupPath + "\\TIEVision.ini";
            //parsedData = fileIniData.ReadFile(iniFilePath);
        }


        public string GetValueInfo(string parentNode, string childNode)
        {
            parsedData = fileIniData.ReadFile(iniFilePath);
            return parsedData[parentNode][childNode];
        }


        public bool SetValueInfo(string parentNode, string childNode, string value)
        {
            bool bRetVal = true;
            try
            {
                parsedData[parentNode][childNode] = value;
                fileIniData.WriteFile(iniFilePath, parsedData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                bRetVal = false;
                 
            }
            return bRetVal;
        }




    }
}

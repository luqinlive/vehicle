using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TIEVision.Model
{

    public class Province
    {
        public string code { get; set; }
        public string name { get; set; }
    }

    public class Citys
    {
        public string code { get; set; }
        public string name { get; set; }
        public string provinceCode { get; set; }
    }

    public class Areas
    {
        public string code { get; set; }
        public string name { get; set; }
        public string cityCode { get; set; }
        public string provinceCode { get; set; }
    }

    public class NationalCode
    {
        private static NationalCode mContext = null;

        public static List<Province> mProvinceList = new List<Province>();
        public static List<Citys> mCitysList = new List<Citys>();
        public static List<Areas> mAreasList = new List<Areas>();


        NationalCode()
        {
            try
            {
                string province = File.ReadAllText(Application.StartupPath + "\\config\\provinces.txt");
                Province[] mProvince = JsonConvert.DeserializeObject<Province[]>(province);
                if(null !=mProvince)
                {
                    mProvinceList = new List<Province>(mProvince);
                }
                string citys = File.ReadAllText(Application.StartupPath + "\\config\\citys.txt");
                Citys[] mCitys = JsonConvert.DeserializeObject<Citys[]>(citys);
                if(null!= mCitys)
                {
                    mCitysList = new List<Citys>(mCitys);
                }
                string areas = File.ReadAllText(Application.StartupPath + "\\config\\areas.txt");
                Areas[] mAreas  = JsonConvert.DeserializeObject<Areas[]>(areas);
                if(null != mAreas)
                {
                    mAreasList = new List<Areas>(mAreas);
                }
            }
            catch { }
            
        }

        public static NationalCode GetInstance()
        {
            if (mContext == null)
            {
                mContext = new NationalCode();

            }
            return mContext;
        }

        public  string GetAreaCodeName(string AreaCode)
        {
            string retVal ="";
            Areas m_Area = mAreasList.FindLast(x => AreaCode == x.code);
            if(null != m_Area)
            {
                Citys m_City = mCitysList.FindLast(c => m_Area.cityCode == c.code);
                if(null != m_City)
                {
                    Province m_Province = mProvinceList.FindLast(p=>m_City.provinceCode == p.code);
                    if(null != m_Province)
                    {
                        retVal +=m_Province.name+"-";
                    }
                    retVal += m_City.name+"-";
                }
                retVal += m_Area.name;
            }
           return retVal;
        }

        public static string CalculateSex(string idcard)
        {
            if(!string.IsNullOrEmpty(idcard))
            {
                if(idcard.Length == 18)
                {
                    string  strSex = idcard.Substring(14, 3);
                    if (int.Parse(strSex) % 2 == 0)
                    {
                        return "女";
                    }else{
                        return "男";
                    }
                }
            }
            return  "";
        }

         public static int CalculateAge(string birthDay)
        {
            DateTime birthDate=DateTime.Parse(birthDay);
            DateTime nowDateTime=DateTime.Now;
            int age = nowDateTime.Year - birthDate.Year;
            //再考虑月、天的因素
            if (nowDateTime.Month < birthDate.Month || (nowDateTime.Month == birthDate.Month && nowDateTime.Day < birthDate.Day))
            { 
                age--; 
            }
            return age;
        }

    }
}

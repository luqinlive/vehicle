using IRVision.COMMON;
using IRVision.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRVision.DAL
{
    public class CrossingInfoDAL
    {

        public int UpdateCrossingInfo(string crossing_id , string json,string base64Image)
        {
            MySqlDbHelper dbHelper = new MySqlDbHelper();
            dbHelper.Connect();
            string sqlStr = "update tb_crossing set CROSSING_CONFIG='" + json + "' where CROSSING_ID='" + crossing_id + "'";
            if(!string.IsNullOrEmpty(base64Image))
            {
                sqlStr = "update tb_crossing set CROSSING_CONFIG='" + json + "' ,IMAGE_DATA='" + base64Image + "'  where CROSSING_ID='" + crossing_id + "'";
            }
            int retVal = dbHelper.ExecuteNonQuery(sqlStr);
            dbHelper.Close();
            return retVal;
        }

        public List<CrossingInfo> GetCrossingInfos()
        {
            List<CrossingInfo> mList = new List<CrossingInfo>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * from  tb_crossing");
            CrossingInfo model = new CrossingInfo();
           
            DataSet ds = null;


            MySqlDbHelper dbHelper = new MySqlDbHelper();
            dbHelper.Connect();
            ds = dbHelper.GetDataSet(strSql.ToString());
            dbHelper.Close();

            
            
            //DataSet ds = objODPHelper.Query(strSql.ToString(), null);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        model = DataRowToModel(ds.Tables[0].Rows[i]);
                        mList.Add(model);
                    }
                }
                else
                {
                    return null;
                }
            }
            return mList;
        }

        private CrossingInfo DataRowToModel(DataRow row)
        {
            CrossingInfo model = new CrossingInfo();
            if (row != null)
            {
                if (row["ID"] != null)
                {
                    model.ID = row["ID"].ToString();
                }
                if (row["CROSSING_ID"] != null)
                {
                    model.CROSSING_ID = row["CROSSING_ID"].ToString();
                }
                if (row["CROSSING_NAME"] != null)
                {
                    model.CROSSING_NAME = row["CROSSING_NAME"].ToString();
                }
                if (row["CREATE_TIME"] != null)
                {
                    model.CREATE_TIME = row["CREATE_TIME"].ToString();
                }
                if (row["IMAGE_DATA"] != null)
                {
                    model.IMAGE_DATA = row["IMAGE_DATA"].ToString();
                }
                if (row["CROSSING_CONFIG"] != null)
                {
                    model.CROSSING_CONFIG = row["CROSSING_CONFIG"].ToString();
                }
                if (row["DESC"] != null)
                {
                    model.DESC = row["DESC"].ToString();
                }
            }
            return model;
        }
    }
}

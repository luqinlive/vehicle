using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IRVision.COMMON
{
    public class MySqlDbHelper
    {
        MySqlConnection m_connection = null;

        MySqlTransaction m_trans = null;

        public MySqlDbHelper()
        {

        }

        ~MySqlDbHelper()
        {
            Close();
        }

        public void BeginTrans()
        {
            m_trans = m_connection.BeginTransaction(IsolationLevel.ReadCommitted);
        }


        public void TransactionCommit()
        {
            m_trans.Commit();
            m_trans = null;
        }

        public void TransactionRollBack()
        {
            m_trans.Rollback();
            m_trans = null;
        }


        public void Close()
        {
            if (m_connection != null)
            {
                m_connection.Close();
            }
        }

        public bool Connect()
        {
            bool result = false;
            //string connectString = ConfigurationManager.ConnectionStrings["MyContext"].ConnectionString;
            string connectString = System.Configuration.ConfigurationManager.AppSettings["mysqlConn"].ToString();
            //var builder = new DbConnectionStringBuilder();
           // builder.ConnectionString = connectString;
           // builder["password"] = EncodeHelper.DesDecrypt(Regex.Match(connectString, @"password=([^;]+)").Groups[1].Value);
            try
            {
                m_connection = new MySqlConnection();
                m_connection.ConnectionString = connectString;
                m_connection.Open();
                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result = false;
            }
            return result;
        }

        public DataSet GetDataSet(string sqlText)
        {
            return GetDataSet(sqlText, CommandType.Text, null);
        }
        public DataSet GetDataSet(string sqlText, params MySqlParameter[] parameters)
        {
            return GetDataSet(sqlText, CommandType.Text, parameters);
        }
        public DataSet GetDataSet(string sqlText, CommandType cmdType, params MySqlParameter[] parameters)
        {
            DataSet set = new DataSet();
            try
            {
                MySqlCommand command = new MySqlCommand(sqlText, m_connection);
                command.CommandType = cmdType;
                command.CommandTimeout = 60 * 4;
                if (parameters != null && parameters.Length > 0)
                    command.Parameters.AddRange(parameters);
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = command;
                adapter.Fill(set);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Mysql GetDataSet :" + ex.Message);
            }
            return set;
        }

        public object ExecuteScalar(string sqlText)
        {
            return ExecuteScalar(sqlText, CommandType.Text, null);
        }
        public object ExecuteScalar(string sqlText, params MySqlParameter[] parameters)
        {
            return ExecuteScalar(sqlText, CommandType.Text, parameters);
        }
        public object ExecuteScalar(string sqlText, CommandType cmdType, params MySqlParameter[] parameters)
        {
            object result = null;
            MySqlCommand command = new MySqlCommand(sqlText, m_connection);
            command.CommandType = cmdType;
            if (parameters != null && parameters.Length > 0)
                command.Parameters.AddRange(parameters);
            try
            {
                result = command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result = null;
            }
            return result;
        }

        public int ExecuteNonQuery(string sqlText)
        {
            return ExecuteNonQuery(sqlText, CommandType.Text, null);
        }
        public int ExecuteNonQuery(string sqlText, params MySqlParameter[] parameters)
        {
            return ExecuteNonQuery(sqlText, CommandType.Text, parameters);
        }
        public int ExecuteNonQuery(string sqlText, CommandType cmdType, params MySqlParameter[] parameters)
        {
            int result = 0;

            MySqlCommand command = new MySqlCommand(sqlText, m_connection);
            if (m_trans != null)
                command.Transaction = m_trans;
            command.CommandType = cmdType;
            if (parameters != null && parameters.Length > 0)
                command.Parameters.AddRange(parameters);
            try
            {
                result = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " sql: " + sqlText);

                Console.WriteLine(ex.Message);
                result = -1;
            }
            return result;
        }



        public MySqlDataReader ExecuteDataReader(string sqlText)
        {
            return ExecuteDataReader(sqlText, CommandType.Text, null);
        }
        public MySqlDataReader ExecuteDataReader(string sqlText, params MySqlParameter[] parameters)
        {
            return ExecuteDataReader(sqlText, CommandType.Text, parameters);
        }
        public MySqlDataReader ExecuteDataReader(string sqlText, CommandType cmdType, params MySqlParameter[] parameters)
        {
            MySqlDataReader result = null;
            MySqlCommand command = new MySqlCommand(sqlText, m_connection);
            command.CommandType = cmdType;
            if (parameters != null && parameters.Length > 0)
                command.Parameters.AddRange(parameters);
            try
            {
                result = command.ExecuteReader();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result = null;
            }
            return result;
        }
    }
}

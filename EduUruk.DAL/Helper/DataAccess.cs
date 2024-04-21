using Microsoft.Data.SqlClient;
using System.Data;

namespace EduUruk.DAL.Helper
{
    public class DataAccess
    {
        public string getData(string sql, ref DataSet ds)
        {
            SqlConnection con = new(Constants.GetConnectionString());
            SqlDataAdapter dt = new(sql, con);

            try
            {
                dt.SelectCommand.CommandTimeout = 99999999;
                con.Open();
                dt.Fill(ds);
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                con.Close();
            }
        }
        public string exeQuery(string sql)
        {
            SqlConnection con = new(Constants.GetConnectionString());
            SqlCommand cmd = new(sql, con);

            try
            {
                cmd.CommandTimeout = 999999;
                con.Open();
                cmd.ExecuteNonQuery();
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                con.Close();
            }
        }
        public string FillDataSet(string spName, DataTable Parameters, ref DataSet ds)
        {
            SqlConnection con = new(Constants.GetConnectionString());
            SqlDataAdapter dt = new(spName, con);

            dt.SelectCommand.CommandType = CommandType.StoredProcedure;


            for (int i = 0; i < Parameters.Rows.Count; i++)
            {
                dt.SelectCommand.Parameters.Add(new SqlParameter(Parameters.Rows[i]["ParameterName"].ToString(), Parameters.Rows[i]["ParameterValue"].ToString()));
            }

            try
            {
                con.Open();
                dt.SelectCommand.CommandTimeout = 999999;
                dt.Fill(ds);
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                con.Close();
            }


        }
        public string getPublicData(string sql, ref DataSet ds)
        {
            SqlConnection con = new(Constants.GetConnectionString());
            SqlDataAdapter dt = new(sql, con);

            try
            {
                con.Close();
                dt.SelectCommand.CommandTimeout = 999999;
                dt.Fill(ds);
                return "1";
            }
            catch (Exception ex) { return ex.Message; }
            finally
            {
                con.Close();
            }
        }
        public string Update(string spName, DataTable Parameters, string ParameterOutName, ref int ParameterOutValue)
        {
            SqlConnection con = new(Constants.GetConnectionString());

            SqlCommand cmd = new(spName, con);
            cmd.CommandTimeout = 999999;

            cmd.CommandType = CommandType.StoredProcedure;

            for (int i = 0; i < Parameters.Rows.Count; i++)
                cmd.Parameters.Add(new SqlParameter(Parameters.Rows[i]["ParameterName"].ToString(), Parameters.Rows[i]["ParameterValue"].ToString()));

            if (ParameterOutName != "")
            {
                SqlParameter PKParameter = new();
                PKParameter.Direction = ParameterDirection.Output;
                PKParameter.DbType = DbType.Int32;
                PKParameter.ParameterName = ParameterOutName;
                cmd.Parameters.Add(PKParameter);
            }
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();

                if (ParameterOutName != "")
                    ParameterOutValue = Convert.ToInt32(cmd.Parameters[ParameterOutName].Value);
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                con.Close();
            }


        }
    }
}

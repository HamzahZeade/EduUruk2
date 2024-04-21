using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace EduUruk.DAL.EnitityDAL
{
    public class DBProvider : IDisposable
    {
        //internal ApplicationDbContext DbContext
        //{
        //    get; set;
        //}

        private SqlConnection DBConnection
        {
            get; set;
        }

        private SqlCommand myCommand
        {
            get; set;
        }

        public SqlParameterCollection Parameters
        {
            get; set;
        }

        public bool IsModelStateValid
        {
            get; set;
        }

        public DBProvider()
        {
            //  DbContext = new ApplicationDbContext();
            SetSqlClient();
        }
        public bool IsModelValid(dynamic model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            return (Validator.TryValidateObject(model, context, results));
        }
        private void SetSqlClient()
        {
            //  DBConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            // myCommand = new SqlCommand() { Connection = DBConnection };
            // Parameters = myCommand.Parameters;
        }

        //protected bool FetchNonQuery(string CmdQuery)
        //{
        //    bool result = true;
        //    using (OracleConnection myConnection = DBConnection)
        //    {
        //        myCommand.CommandText = CmdQuery;
        //        myCommand.CommandType = CommandType.StoredProcedure;
        //        //Execute the command
        //        myConnection.Open();
        //        if (myCommand.ExecuteNonQuery() > 0)
        //        {
        //            result = true;
        //        }
        //        myConnection.Close();
        //    }
        //    Parameters.Clear();
        //    return result;
        //}

        //protected async Task<bool> FetchNonQueryAsync(string CmdQuery)
        //{
        //    bool result = true;
        //    using (OracleConnection myConnection = DBConnection)
        //    {
        //        myCommand.CommandText = CmdQuery;
        //        myCommand.CommandType = CommandType.StoredProcedure;
        //        //Execute the command
        //        await myConnection.OpenAsync();
        //        if (await myCommand.ExecuteNonQueryAsync() > 0)
        //        {
        //            result = true;
        //        }
        //        myConnection.Close();
        //    }
        //    Parameters.Clear();
        //    return result;
        //}

        //public DataTable FetchQuery(string CmdQuery)
        //{
        //    DataTable dt = new DataTable();
        //    using (OracleConnection myConnection = DBConnection)
        //    {
        //        myCommand.CommandText = CmdQuery;
        //        myCommand.CommandType = CommandType.StoredProcedure;
        //        //Execute the command
        //        OracleDataAdapter da = new OracleDataAdapter(myCommand);
        //        // Execute the command
        //        myConnection.Open();
        //        da.Fill(dt);
        //        myConnection.Close();
        //    }
        //    Parameters.Clear();
        //    return dt;
        //}

        //protected async Task<DataTable> FetchQueryAsync(string CmdQuery)
        //{
        //    DataTable dt = new DataTable();
        //    using (SqlConnection myConnection = DBConnection)
        //    {
        //        myCommand.CommandText = CmdQuery;
        //        myCommand.CommandType = CommandType.StoredProcedure;
        //        //Execute the command
        //        DataAdapter da = new DataAdapter(myCommand);
        //        da.co
        //        // Execute the command
        //        await myConnection.OpenAsync();
        //        await Task.Run(() => da.Fill(dt));
        //        myConnection.Close();
        //    }
        //    Parameters.Clear();
        //    return dt;
        //}


        public virtual void Dispose()
        {
            SetSqlClient();
        }

    }
}

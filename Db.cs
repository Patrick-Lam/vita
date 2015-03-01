using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace vita
{
    public class Db
    {
        public static DataSet GetData(string _connString, string _commandText, SqlParameter[] _sqlParameters)
        {
            DataSet ds = new DataSet();

            using (SqlConnection sqlConnection = new SqlConnection(_connString))
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = _commandText;

                foreach (SqlParameter sqlParameter in _sqlParameters)
                {
                    sqlCommand.Parameters.Add(sqlParameter);
                }

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(ds);
                sqlDataAdapter.Dispose();

                sqlCommand.Dispose();
            }

            return ds;
        }

        public static bool ExecuteNonQueryByCommandText(string _connString, string _commandText, SqlParameter[] _sqlParameters)
        {
            bool executeNonQuerySuccessed = false;

            using (SqlConnection sqlConnection = new SqlConnection(_connString))
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                SqlTransaction sqlTrans;

                sqlTrans = sqlConnection.BeginTransaction();

                sqlCommand.Connection = sqlConnection;
                sqlCommand.Transaction = sqlTrans;

                try
                {
                    sqlCommand.CommandText = _commandText;

                    foreach (SqlParameter sqlParameter in _sqlParameters)
                    {
                        sqlCommand.Parameters.Add(sqlParameter);
                    }

                    sqlCommand.ExecuteNonQuery();

                    sqlTrans.Commit();

                    executeNonQuerySuccessed = true;
                }
                catch (Exception _e)
                {
                    // 事务提交失败

                    executeNonQuerySuccessed = false;

                    try
                    {
                        sqlTrans.Rollback();
                    }
                    catch (Exception __e)
                    {
                        // 事务回滚失败
                    }
                }
                finally
                {
                    sqlCommand.Dispose();
                }
            }

            return executeNonQuerySuccessed;
        }
    }
}

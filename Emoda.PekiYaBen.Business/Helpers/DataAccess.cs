using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Emoda.PekiYaBen.Business.Helpers
{
    public class DataAccess
    {
        protected string ConnectionString { get; set; }

        public DataAccess()
        {
        }

        public DataAccess(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();
            return connection;
        }

        protected DbCommand GetCommand(DbConnection connection, string commandText, CommandType commandType)
        {
            SqlCommand command = new SqlCommand(commandText, connection as SqlConnection);
            command.CommandType = commandType;
            return command;
        }

        protected SqlParameter GetParameter(string parameter, object value)
        {
            SqlParameter parameterObject = new SqlParameter(parameter, value != null ? value : DBNull.Value);
            parameterObject.Direction = ParameterDirection.Input;
            return parameterObject;
        }

        protected SqlParameter GetParameterOut(string parameter, SqlDbType type, object value = null, ParameterDirection parameterDirection = ParameterDirection.InputOutput)
        {
            SqlParameter parameterObject = new SqlParameter(parameter, type); ;

            if (type == SqlDbType.NVarChar || type == SqlDbType.VarChar || type == SqlDbType.NText || type == SqlDbType.Text)
            {
                parameterObject.Size = -1;
            }

            parameterObject.Direction = parameterDirection;

            if (value != null)
            {
                parameterObject.Value = value;
            }
            else
            {
                parameterObject.Value = DBNull.Value;
            }

            return parameterObject;
        }

        public void ExecuteNonQuery(string procedureName, List<SqlParameter> parameters, CommandType commandType = CommandType.Text)
        {
            try
            {

                using (IDbConnection con = new SqlConnection(this.ConnectionString))
                {
                    con.Open();
                    using (var trans = con.BeginTransaction())
                    {
                        IDbCommand command = con.CreateCommand();
                        command.CommandType = commandType;
                        command.CommandText = procedureName;
                        command.Transaction = trans;
                        if (parameters != null && parameters.Count > 0)
                        {
                            foreach (SqlParameter param in parameters)
                            {
                                param.Value = (object)param.Value ?? DBNull.Value;
                                command.Parameters.Add(param);
                            }
                        }
                        command.ExecuteNonQuery();
                        trans.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                //LogException("Failed to ExecuteScalar for " + procedureName, ex, parameters);
                throw;
            }
        }

        public object ExecuteScalar(string procedureName, List<SqlParameter> parameters, CommandType commandType = CommandType.Text)
        {
            object returnValue = null;
            try
            {

                using (IDbConnection con = new SqlConnection(this.ConnectionString))
                {
                    con.Open();
                    using (var trans = con.BeginTransaction())
                    {
                        IDbCommand command = con.CreateCommand();
                        command.CommandType = commandType;
                        command.CommandText = procedureName;
                        command.Transaction = trans;
                        if (parameters != null && parameters.Count > 0)
                        {
                            foreach (SqlParameter param in parameters)
                            {
                                command.Parameters.Add(param);
                            }
                        }
                        returnValue = command.ExecuteScalar();
                        trans.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                //LogException("Failed to ExecuteScalar for " + procedureName, ex, parameters);
                throw;
            }

            return returnValue;
        }

        public DataTable GetDataReader(string procedureName, List<SqlParameter> parameters, CommandType commandType = CommandType.Text)
        {

            DataTable dataTable = new DataTable();
            try
            {

                using (IDbConnection con = new SqlConnection(this.ConnectionString))
                {
                    con.Open();
                    IDbCommand command = con.CreateCommand();
                    command.CommandType = commandType;
                    command.CommandText = procedureName;
                    if (parameters != null && parameters.Count > 0)
                    {
                        foreach (SqlParameter param in parameters)
                        {
                            command.Parameters.Add(param);
                        }
                    }
                    IDataReader ds = command.ExecuteReader(CommandBehavior.CloseConnection);
                    dataTable.Load(ds);
                }
            }
            catch (Exception ex)
            {
                //LogException("Failed to ExecuteScalar for " + procedureName, ex, parameters);
                throw;
            }

            return dataTable;
        }
    }
}
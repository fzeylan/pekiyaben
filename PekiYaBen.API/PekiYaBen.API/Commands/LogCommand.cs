using Emoda.PekiYaBen.Business.Helpers;
using PekiYaBen.API.Helpers;
using PekiYaBen.API.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PekiYaBen.API.Commands
{
    public class LogCommand
    {
        private readonly AppSettings _settings;
        public LogCommand(AppSettings configuration)
        {
            _settings = configuration;
        }

        public CommandResponse Log(ApiLogItem logItem)
        {
            CommandResponse response = new CommandResponse();
            try
            {
                DataAccess da = new DataAccess(_settings.ConnectionString);


                string insertQuery = "Insert INTO [Log] (RequestTime, ResponseMillis, StatusCode, Method, Path, QueryString, RequestBody, ResponseBody) " +
                    "VALUES(@RequestTime, @ResponseMillis, @StatusCode, @Method, @Path, @QueryString, @RequestBody, @ResponseBody)";

                var updateParameters = new List<SqlParameter> {
                    new SqlParameter("RequestTime", logItem.RequestTime),
                    new SqlParameter("ResponseMillis", logItem.ResponseMillis),
                    new SqlParameter("StatusCode", logItem.StatusCode),
                    new SqlParameter("Method", logItem.Method),
                    new SqlParameter("Path", logItem.Path),
                    new SqlParameter("QueryString", logItem.QueryString),
                    new SqlParameter("RequestBody", logItem.RequestBody),
                    new SqlParameter("ResponseBody", logItem.ResponseBody)
                };

                da.ExecuteNonQuery(insertQuery, updateParameters);
                response = new CommandResponse { Data = null, Succeed = true };
            }
            catch (Exception)
            {
                response = new CommandResponse { Data = null, Succeed = false };
            }

            return response;
        }

    }
}
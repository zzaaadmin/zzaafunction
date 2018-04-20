﻿using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteCase.OpenInformation.Demo
{
    public static class WhitecaseInformation
    {
        public static List<string> FindIndustry(TraceWriter log,string industry)
        {
            List<string> output = new List<string>();

            try
            {
                log.Info($"Function=FindIndustry");
                //SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                //builder.DataSource = "zzaasqlserver.database.windows.net";
                //builder.UserID = "zzaauid";
                //builder.Password = "ZZaapwd12345!";
                //builder.InitialCatalog = "zzaasqldemo";

                //log.Info($"ConnectionString={builder.ConnectionString}");

                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString;
                log.Info($"ConnectionString={connectionString}");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    //SELECT TOP 1 Name FROM DBO.Industries
                    //INNER JOIN FREETEXTTABLE(Industries, Name, 'Sovereign') as ftt
                    //ON ftt.[KEY] = Industries.id
                    //ORDER BY ftt.RANK DESC
                    sb.Append("SELECT TOP 1 Name FROM DBO.Industries ");
                    sb.Append("INNER JOIN FREETEXTTABLE(Industries, Name, ' ");
                    sb.Append("Sovereign ");
                    sb.Append("') as ftt ");
                    sb.Append("ON ftt.[KEY] = Industries.id ");
                    sb.Append("ORDER BY ftt.RANK DESC ");

                    String sql = sb.ToString();
                    log.Info($"Query={sql}");

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                output.Add(reader.GetString(0));
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                log.Error("Error Message", ex);
                throw;
            }

            return output;
        }

        public static List<string> ListIndustry(TraceWriter log, string character)
        {
            List<string> output = new List<string>();
            //output.Add("Alpha");
            //output.Add("Beta");

            try
            {
                log.Info($"Function=ListIndustry");
                //SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                //builder.DataSource = "zzaasqlserver.database.windows.net";
                //builder.UserID = "zzaauid";
                //builder.Password = "ZZaapwd12345!";
                //builder.InitialCatalog = "zzaasqldemo";

                //log.Info($"ConnectionString={builder.ConnectionString}");

                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString;
                log.Info($"ConnectionString={connectionString}");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT T.Ind_Name ");
                    sb.Append("FROM [dbo].[Industries] T ");
                    sb.Append("Where T.Ind_Name Like '");
                    sb.Append(character);
                    sb.Append("%';");
                    String sql = sb.ToString();
                    log.Info($"Query={sql}");

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                output.Add(reader.GetString(0));
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                log.Error("Error Message", ex);
                throw;
            }

            return output;
        }
    }
}

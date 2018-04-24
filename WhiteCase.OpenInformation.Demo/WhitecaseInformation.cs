using Microsoft.Azure.WebJobs.Host;
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
                    sb.Append("INNER JOIN FREETEXTTABLE(Industries, Name, '");
                    sb.Append(industry);
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
                    sb.Append("SELECT T.Name ");
                    sb.Append("FROM [dbo].[Industries] T ");
                    sb.Append("Where T.Name Like '");
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

        public static List<string> FindPractice(TraceWriter log, string practice)
        {
            List<string> output = new List<string>();

            try
            {
                log.Info($"Function=FindPractice");
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
                    sb.Append("SELECT TOP 1 Name FROM DBO.Practices ");
                    sb.Append("INNER JOIN FREETEXTTABLE(Practices, Name, '");
                    sb.Append(practice);
                    sb.Append("') as ftt ");
                    sb.Append("ON ftt.[KEY] = Practices.id ");
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

        public static List<string> ListPractice(TraceWriter log, string character)
        {
            List<string> output = new List<string>();
            //output.Add("Alpha");
            //output.Add("Beta");

            try
            {
                log.Info($"Function=ListPractice");
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
                    sb.Append("SELECT T.Name ");
                    sb.Append("FROM DBO.Practices T ");
                    sb.Append("Where T.Name Like '");
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

        public static List<string> FindCountry(TraceWriter log, string country)
        {
            List<string> output = new List<string>();

            try
            {
                log.Info($"Function=FindCountry");
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

                    //SELECT TOP 1 Country FROM DBO.Regions
                    //INNER JOIN FREETEXTTABLE(Regions, Country, '"India"') as ftt
                    //ON ftt.[KEY] = Regions.id
                    //ORDER BY ftt.RANK DESC
                    sb.Append("SELECT TOP 1 Country FROM DBO.Regions ");
                    sb.Append("INNER JOIN FREETEXTTABLE(Regions, Country, '");
                    sb.Append(country);
                    sb.Append("') as ftt ");
                    sb.Append("ON ftt.[KEY] = Regions.id ");
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

        public static List<string> ListCountry(TraceWriter log, string region)
        {
            List<string> output = new List<string>();
            //output.Add("Alpha");
            //output.Add("Beta");

            try
            {
                log.Info($"Function=ListCountry");
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

                    //; With CTE_Region AS
                    // (SELECT TOP 1 Region FROM DBO.Regions
                    // INNER JOIN FREETEXTTABLE(Regions, Region, '"Asia Pacific"') as ftt
                    // ON ftt.[KEY] = Regions.id
                    // ORDER BY ftt.RANK DESC)
                    //SELECT R.Country FROM dBO.Regions R
                    //JOIN CTE_Region C ON R.Region = C.Region

                    sb.Append("; With CTE_Region AS ");
                    sb.Append("(SELECT TOP 1 Region FROM DBO.Regions ");
                    sb.Append("INNER JOIN FREETEXTTABLE(Regions, Region, '");
                    sb.Append(region);
                    sb.Append("') as ftt ");
                    sb.Append("ON ftt.[KEY] = Regions.id ");
                    sb.Append("ORDER BY ftt.RANK DESC) ");
                    sb.Append("SELECT R.Country FROM dBO.Regions R ");
                    sb.Append("JOIN CTE_Region C ON R.Region = C.Region ");

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

        public static List<string> ListRegion(TraceWriter log)
        {
            List<string> output = new List<string>();
            //output.Add("Alpha");
            //output.Add("Beta");

            try
            {
                log.Info($"Function=ListCountry");
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

                    //; With CTE_Region AS
                    // (SELECT TOP 1 Region FROM DBO.Regions
                    // INNER JOIN FREETEXTTABLE(Regions, Region, '"Asia Pacific"') as ftt
                    // ON ftt.[KEY] = Regions.id
                    // ORDER BY ftt.RANK DESC)
                    //SELECT R.Country FROM dBO.Regions R
                    //JOIN CTE_Region C ON R.Region = C.Region

                    sb.Append("SELECT DISTINCT REGION FROM DBO.Regions");

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

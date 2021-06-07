using FacebookStats.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FacebookStats.Services
{
    public class SQLHealper
    {
        public static string DataConnection = @"Data Source=104.219.233.61;Initial Catalog=fbmarketing;User ID=qhunain;Password=G8z?0ds3;";

        public static List<PostWallInstance> GetPostWallInstances()
        {
            List<PostWallInstance> postWallInstances = new List<PostWallInstance>();
            SqlConnection con = new SqlConnection(DataConnection);
            string query = "SELECT [Id] ,[HeaderMessage],[WebSiteDisplayURL],[WebSiteURL],[MaxBudget],[ImageFiles],[AdName],[ImageHash],[Locations],[PageId],[DateStart],[DateEnd],[AdId] FROM [qhunain].[PostWallInstances]";
            SqlCommand cmd = new SqlCommand(query, con);
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    postWallInstances.Add(new PostWallInstance
                    {
                        Id = int.Parse(reader[0].ToString()),
                        HeaderMessage = reader[1].ToString(),
                        WebSiteDisplayURL = reader[2].ToString(),
                        WebSiteURL = reader[3].ToString(),
                        MaxBudget = reader[4].ToString(),
                        ImageFiles = reader[5].ToString(),
                        AdName = reader[6].ToString(),
                        ImageHash = reader[7].ToString(),
                        Locations = reader[8].ToString(),
                        PageId = reader[9].ToString(),
                        DateStart = reader[10].ToString(),
                        DateEnd = reader[11].ToString(),
                        AdId = reader[12].ToString()
                    });
                }
                return postWallInstances;
            }
            catch (SqlException e)
            {
                return null;
            }
            finally
            {
                con.Close();
            }
        }


        public static int Insert(PostWallInstance postWallInstance)
        {
            int id = 0;
            SqlConnection con = new SqlConnection(DataConnection);
            string query = @"INSERT INTO [qhunain].[PostWallInstances] ([HeaderMessage],[WebSiteDisplayURL],[WebSiteURL],[MaxBudget],[ImageFiles],[AdName],[ImageHash],[Locations],[PageId],[DateStart],[DateEnd],[AdId]) VALUES ('" + postWallInstance.HeaderMessage + "' ,'" + postWallInstance.WebSiteDisplayURL + "' ,'" + postWallInstance.WebSiteURL + "' ,'" + postWallInstance.MaxBudget + "' ,'" + postWallInstance.ImageFiles + "','" + postWallInstance.AdName + "' ,'" + postWallInstance.ImageHash + "' ,'" + postWallInstance.Locations + "' ,'" + postWallInstance.PageId + "' ,'" + postWallInstance.DateStart + "' ,'" + postWallInstance.DateEnd + "','" + postWallInstance.AdId + "'); SELECT SCOPE_IDENTITY()";
            SqlCommand cmd = new SqlCommand(query, con);
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int.TryParse(reader[0].ToString(),out id);
                    return id;
                }
            }
            catch (SqlException e)
            {
                return id;
            }
            finally
            {
                con.Close();
            }
            return id;
        }


        public static bool Update(PostWallInstance postWallInstance)
        {
            SqlConnection con = new SqlConnection(DataConnection);
            string query = "UPDATE [qhunain].[PostWallInstances] SET [HeaderMessage] = '" + postWallInstance.HeaderMessage + "' ,[WebSiteDisplayURL] = '" + postWallInstance.WebSiteDisplayURL + "' ,[WebSiteURL] = '" + postWallInstance.WebSiteURL + "' ,[MaxBudget] = '" + postWallInstance.MaxBudget + " ' ,[ImageFiles] = '" + postWallInstance.ImageFiles + "',[AdName] = '" + postWallInstance.AdName + "',[ImageHash] = '" + postWallInstance.ImageHash + "',[Locations] = '" + postWallInstance.Locations + "',[PageId] = '" + postWallInstance.PageId + "',[DateStart] = '" + postWallInstance.DateStart + "',[DateEnd] = '" + postWallInstance.DateEnd + "',[AdId] = '" + postWallInstance.AdId + "'  WHERE Id = " + postWallInstance.Id + " ";
            SqlCommand cmd = new SqlCommand(query, con);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (SqlException e)
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        public static bool Delete(int id)
        {
            SqlConnection con = new SqlConnection(DataConnection);
            string query = "DELETE FROM [qhunain].[PostWallInstances]  WHERE Id = " + id + " ";
            SqlCommand cmd = new SqlCommand(query, con);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (SqlException e)
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Data.Common;
using System.Data.Entity;
using System.Data;

namespace Project.Models.DB
{
    public partial class db_corelogin : Microsoft.EntityFrameworkCore.DbContext
    {

        #region Properties

        public virtual System.Data.Entity.DbSet<Login> Login { get; set; }
        //public virtual System.Data.Entity.DbSet<ToDoTask> ToDoTask { get; set; }
        public System.Data.Entity.DbSet<ToDoTask> ToDoTask { get; set; }
        public string ConnectionString { get; set; }

        #endregion

        //public db_coreloginContext()
        //{
        //    Console.WriteLine("aaaaaaa");
        //}

        public db_corelogin(DbContextOptions<db_corelogin> options)
            : base(options)
        {
            ConnectionString = @"Server=DESKTOP-FO7B6CB\SQLEXPRESS;Database=db_corelogin;Trusted_Connection=True;user id=appuser;password=34g65c;";
        }

    
        protected void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<LoginByUsernamePassword>().MapToStoredProcedures();
            modelBuilder.Entity<ToDoTask>().ToTable("t_ToDoList");
        }

       
        #region  Methods  

        /// <summary>  
        /// Login by username and password store procedure method.  
        /// </summary>  
        /// <param name="usernameVal">Username value parameter</param>  
        /// <param name="passwordVal">Password value parameter</param>  
        /// <returns>Returns - List of logins by username and password</returns>  
        public async Task<List<LoginByUsernamePassword>> LoginByUsernamePasswordMethodAsync(string usernameVal, string passwordVal)
        {
            // Initialization db connection. 
            //ConnectionString = @"Server=DESKTOP-FO7B6CB\SQLEXPRESS;Database=db_corelogin;Trusted_Connection=True;user id=appuser;password=34g65c;";

            List<LoginByUsernamePassword> lst = new List<LoginByUsernamePassword>();

            try
            {   DataTable dt = new DataTable();

                // Settings.  
                SqlParameter usernameParam = new SqlParameter("@username", usernameVal ?? (object)DBNull.Value);
                SqlParameter passwordParam = new SqlParameter("@password", passwordVal ?? (object)DBNull.Value);

                // Processing.  
                string sqlQuery = "EXEC [dbo].[LoginByUsernamePassword]" +
                                    "@username, @password";
                using (var conn = new SqlConnection(ConnectionString))

                

                using (var command = new SqlCommand(sqlQuery, conn) { CommandType = CommandType.StoredProcedure })
                {
                    conn.Open();
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 5000;
                    command.Parameters.AddWithValue("@username", usernameParam.SqlValue);
                    command.Parameters.AddWithValue("@password", passwordParam.SqlValue);
                    command.ExecuteNonQuery();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            LoginByUsernamePassword obj = new LoginByUsernamePassword();

                            obj.Username = dr["username"].ToString();
                            obj.Password = dr["password"].ToString();

                            lst.Add(obj);
                        }
                    }

                    else
                    {
                        throw new Exception("Wrong name or password"); 
                        //Console.WriteLine("Not good connection with DB");
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
                Console.WriteLine(ex.Message);
            }

            // Info.  
            return lst;
        }



        public async Task<List<ToDoTask>> saveInDb(ToDoTask toDoTask)
        {
            // Initialization db connection. 
            //ConnectionString = @"Server=DESKTOP-FO7B6CB\SQLEXPRESS;Database=db_corelogin;Trusted_Connection=True;user id=appuser;password=34g65c;";

            List<ToDoTask> lst = new List<ToDoTask>();

            try
            {
                DataTable dt = new DataTable();

                // Settings.
                //SqlParameter IdTask = new SqlParameter("@IdTask", toDoTask.IdTask);
                SqlParameter toDo = new SqlParameter("@TaskDescription", toDoTask.TaskDescription ?? " without value");
                SqlParameter date = new SqlParameter("@InsertDate", toDoTask.InsertDate);
                SqlParameter Finished = new SqlParameter("@Finished", toDoTask.Finished);
                SqlParameter InProgress = new SqlParameter("@InProgress", toDoTask.InProgress);

                // Processing.  
                string sqlQuery = @"INSERT INTO t_ToDoList(TaskDescription,Finished, InProgress, InsertDate) VALUES" + (toDo, Finished, InProgress, date); 
                using (var conn = new SqlConnection(ConnectionString))



                using (var command = new SqlCommand(sqlQuery, conn) { CommandType = CommandType.StoredProcedure })
                {
                    conn.Open();
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 5000;
                    //command.Parameters.AddWithValue("@IdTask", IdTask.SqlValue);
                    command.Parameters.AddWithValue("@TaskDescription", toDo.SqlValue);
                    command.Parameters.AddWithValue("@Finished", Finished.Value);
                    command.Parameters.AddWithValue("@InProgress", InProgress.Value);
                    command.Parameters.AddWithValue("@InsertDate", date.SqlValue);
                    command.ExecuteNonQuery();               

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
                Console.WriteLine(ex.Message);
            }

            // Info.  
            return lst;
        }


        public void DeleteRow(int id)
        {
            //ConnectionString = @"Server=DESKTOP-FO7B6CB\SQLEXPRESS;Database=db_corelogin;Trusted_Connection=True;user id=appuser;password=34g65c;";
            
            string query = @"delete  from t_ToDoList WHERE IdTask = " + id;
        }


        public async Task<List<ToDoTask>> ListFromDb()
        {
            // Initialization db connection. 
            //ConnectionString = @"Server=DESKTOP-FO7B6CB\SQLEXPRESS;Database=db_corelogin;Trusted_Connection=True;user id=appuser;password=34g65c;";

            List<ToDoTask> lst = new List<ToDoTask>();

            try
            {
                DataTable dt = new DataTable();

                // Processing.  
                string sqlQuery = @"select * from t_ToDoList";
                using (var conn = new SqlConnection(ConnectionString))



                using (var command = new SqlCommand(sqlQuery, conn) { CommandType = CommandType.Text })
                {
                    conn.Open();
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 5000;                  
                    command.ExecuteNonQuery();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            ToDoTask obj = new ToDoTask();

                            obj.IdTask = Convert.ToInt32(dr["IdTask"]);
                            obj.TaskDescription = dr["TaskDescription"].ToString();
                            obj.Finished = Convert.ToBoolean(dr["Finished"]);
                            obj.InProgress = Convert.ToBoolean(dr["InProgress"]);
                            obj.InsertDate = Convert.ToDateTime(dr["InsertDate"]);

                            lst.Add(obj);
                        }
                    }

                    //else
                    //{
                    //    throw new Exception("Wrong name or password");
                    //    //Console.WriteLine("Not good connection with DB");
                    //}

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
                Console.WriteLine(ex.Message);
            }

            // Info.  
            return lst;
        }


        #endregion

    }
}

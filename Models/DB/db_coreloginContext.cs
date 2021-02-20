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
    public partial class db_coreloginContext : Microsoft.EntityFrameworkCore.DbContext
    {

        public db_coreloginContext()
        {
        }

        public db_coreloginContext(DbContextOptions<db_coreloginContext> options)
            : base(options)
        {
        }

        public virtual System.Data.Entity.DbSet<Login> Login { get; set; }


        protected  void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            //modelBuilder.Entity<Login>(entity =>
            //{
            //    entity.ToTable("Login");

            //    entity.Property(e => e.Id).HasColumnName("id");

            //    entity.Property(e => e.Password)
            //        .IsRequired()
            //        .HasMaxLength(50)
            //        .IsUnicode(false)
            //        .HasColumnName("password");

            //    entity.Property(e => e.Username)
            //        .IsRequired()
            //        .HasMaxLength(50)
            //        .IsUnicode(false)
            //        .HasColumnName("username");
            //});

            //OnModelCreatingPartial(modelBuilder);

            //modelBuilder.Types().Configure(t => t.MapToStoredProcedures());
            //context.Database.ExecuteSqlCommand("EXECUTE doStuff");
            modelBuilder.Entity<LoginByUsernamePassword>().MapToStoredProcedures();
        }

       
        #region Login by username and password store procedure method.  

        /// <summary>  
        /// Login by username and password store procedure method.  
        /// </summary>  
        /// <param name="usernameVal">Username value parameter</param>  
        /// <param name="passwordVal">Password value parameter</param>  
        /// <returns>Returns - List of logins by username and password</returns>  
        public async Task<List<LoginByUsernamePassword>> LoginByUsernamePasswordMethodAsync(string usernameVal, string passwordVal)
        {
            // Initialization db connection. 
            var yourConnectionString = @"Server=DESKTOP-FO7B6CB\SQLEXPRESS;Database=db_corelogin;Trusted_Connection=True;user id=;password=;";

            List<LoginByUsernamePassword> lst = new List<LoginByUsernamePassword>();

            try
            {   DataTable dt = new DataTable();

                // Settings.  
                SqlParameter usernameParam = new SqlParameter("@username", usernameVal ?? (object)DBNull.Value);
                SqlParameter passwordParam = new SqlParameter("@password", passwordVal ?? (object)DBNull.Value);

                // Processing.  
                string sqlQuery = "EXEC [dbo].[LoginByUsernamePassword]" +
                                    "@username, @password";
                using (var conn = new SqlConnection(yourConnectionString))

                

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
                        Console.WriteLine("Not good connection with DB");
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

        #endregion

    }
}

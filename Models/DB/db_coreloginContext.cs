using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Data.Common;
using System.Data.Entity;
using System.Data;

#nullable disable

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

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=db_corelogin;Trusted_Connection=True;user id=appuser;password=34g65c;Application Name=ClientCard;");
//            }
//        }

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

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        #region Login by username and password store procedure method.  

        /// <summary>  
        /// Login by username and password store procedure method.  
        /// </summary>  
        /// <param name="usernameVal">Username value parameter</param>  
        /// <param name="passwordVal">Password value parameter</param>  
        /// <returns>Returns - List of logins by username and password</returns>  
        public async Task<List<LoginByUsernamePassword>> LoginByUsernamePasswordMethodAsync(string usernameVal, string passwordVal)
        {
            // Initialization. 

            var yourConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=db_corelogin;Trusted_Connection=True;user id=appuser;password=34g65c;";

            List<LoginByUsernamePassword> lst = new List<LoginByUsernamePassword>();

            try
            {   DataTable dt = new DataTable();
                //List<LoginByUsernamePassword> lst = new List<LoginByUsernamePassword>();

                // Settings.  
                SqlParameter usernameParam = new SqlParameter("@username", usernameVal ?? (object)DBNull.Value);
                SqlParameter passwordParam = new SqlParameter("@password", passwordVal ?? (object)DBNull.Value);

                // Processing.  
                string sqlQuery = "EXEC [dbo].[LoginByUsernamePassword]" +
                                    "@username, @password";

                //List<LoginByUsernamePassword> subjectAreaList = DbModelBuilder.Database.SqlQuery<LoginByUsernamePassword> ("EXECUTE sp_GetSubjectAreasBySubstituteID @id", new SqlParameter("id", 1)).ToList();
                using (var conn = new SqlConnection(yourConnectionString))

                

                using (var command = new SqlCommand(sqlQuery, conn) { CommandType = CommandType.StoredProcedure })
                {
                    conn.Open();
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@username", usernameParam.ToString());
                    command.Parameters.AddWithValue("@password", passwordParam.ToString());
                    command.ExecuteNonQuery();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        LoginByUsernamePassword obj = new LoginByUsernamePassword();

                        obj.Username = dr["username"].ToString();
                        obj.Password = dr["password"].ToString();

                        lst.Add(obj);
                    }

                    conn.Close();
                }
                //lst = await this.Query<LoginByUsernamePassword>().FromSql(sqlQuery, usernameParam, passwordParam).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Info.  
            return lst;
        }

        #endregion

    }
}

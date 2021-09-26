using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Models;
using Project.Models.DB;

namespace Project.Pages
{
    public class AfterLogInModel : PageModel
    {

        #region  Properties
        /// <summary>  
        private readonly db_corelogin databaseManager;


        db_corelogin _Context;
        public AfterLogInModel(db_corelogin databaseManagerContext)
        {
            try
            {
                // Settings.  
                this.databaseManager = databaseManagerContext;
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }
        }

        [BindProperty]
        public ToDoTask TodoTask { get; set; }

        #endregion

        #region On Get method.  

        /// <summary>  
        /// On Get method.  
        /// </summary>  
        public void OnGet()
        {
            try
            {
                if (!String.IsNullOrEmpty(Convert.ToString(HttpContext.Request.QueryString)))
                {               
                string idtask = Convert.ToString(HttpContext.Request.QueryString);
                string id = idtask.Split("=")[1];
                    //TodoTask = new ToDoTask(int.Parse(id));
                    TodoTask = this.databaseManager.SelectFromDb(int.Parse(id));
                }
                else
                {
                    TodoTask = new ToDoTask();
                    TodoTask.InsertDate = DateTime.Now;
                }
               
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }
        }

        #endregion
        #region Log Out method.  

        /// <summary>  
        /// POST: /Home/Index/LogOff  
        /// </summary>  
        /// <returns>Return log off action</returns>  
        public async Task<IActionResult> OnPostLogOff()
        {
            try
            {
                // Setting.  
                var authenticationManager = Request.HttpContext;

                // Sign Out.  
                await authenticationManager.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch (Exception ex)
            {
                // Info  
                throw ex;
            }

            // Info.  
            return this.RedirectToPage("/Index");
        }


        public async Task<IActionResult> OnPostSave(int? id)
        {
            try
            {
           // Setting.  
                var authenticationManager = Request.HttpContext;

                var task =  new ToDoTask();
                this.TodoTask.InsertDate = DateTime.Now;
                if (!ModelState.IsValid)
                {
                    return Page(); // return page
                }
                if (id == null)
                {
                    
                    var loginInfo = await this.databaseManager.saveInDb(this.TodoTask);
                }
                else
                {
                    var loginInfo = await this.databaseManager.UpdateRow(this.TodoTask);
                }
   

                return RedirectToPage("TaskList");
            }
            catch (Exception ex)
            {
                // Info  
                throw ex;
            }

            // Info.  
            return this.RedirectToPage("/Index");
        }

        #endregion


    }
}

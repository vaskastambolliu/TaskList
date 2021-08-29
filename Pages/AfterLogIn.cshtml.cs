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
        private readonly db_coreloginContext databaseManager;


        db_coreloginContext _Context;
        public AfterLogInModel(db_coreloginContext databaseManagerContext)
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
                TodoTask = new ToDoTask();
                TodoTask.InsertDate = DateTime.Now;
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
                if (!ModelState.IsValid)
                {
                    return Page(); // return page
                }
                this.TodoTask.InsertDate = DateTime.Now;
                var loginInfo = await this.databaseManager.saveInDb(this.TodoTask);

                return RedirectToPage("AfterLogIn");
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

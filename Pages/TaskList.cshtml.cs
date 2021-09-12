using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Models;
using Project.Models.DB;

namespace Project.Pages
{
    public class TaskListModel : PageModel
    {

        private readonly db_corelogin databaseManager;
        public TaskListModel(db_corelogin databaseManagerContext)
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

        public List<ToDoTask> ToDoList { get; set; }
      
        public async Task OnGetAsync()
        {
            ToDoList = await this.databaseManager.ListFromDb();

            
        }

        public async Task OnGetDelete()
        {
            ToDoList = await this.databaseManager.ListFromDb();


        }

        public async Task<IActionResult> OnPostNewTask()
        {
            try
            {
               // Verification.  
               
                    // Home Page.  
                    return this.RedirectToPage("AfterLogIn");
               
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }

            // Info.  
            return this.Page();


        }
    }
}

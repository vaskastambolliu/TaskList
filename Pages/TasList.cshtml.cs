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
    public class TasListModel : PageModel
    {
      
        db_coreloginContext _Context;
        public TasListModel(db_coreloginContext databasecontext)
        {
            _Context = databasecontext;
        }

        public List<ToDoTask> ToDoList { get; set; }
        public void OnGet()
        {
            var data = (from dolist in _Context.ToDoTask
                        select dolist).ToList();

            ToDoList = data;
        }
    }
}

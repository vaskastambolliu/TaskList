using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class ToDoTask
    {
        [Key]
        public int IdTask { get; set; }
        public string TaskDescription { get; set; }
        public bool Finished { get; set; }
        public bool InProgress { get; set; }
        public DateTime InsertDate { get; set; }
    }
}

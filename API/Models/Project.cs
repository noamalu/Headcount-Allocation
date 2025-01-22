using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Project
    {
        public string? ProjectName{get;set;}

        public int ProjectId{get;set;}

        public string? Description{get;set;}

        public DateTime Deadline{get;set;}

        public int RequiredHours{get;set;}

    }
}
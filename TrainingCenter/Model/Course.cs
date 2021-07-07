using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenter.Model
{
    public class Course
    {
        public int CourseId { get; set; }
        [StringLength(100)]
        public string Title { get; set; }
        public string Description { get; set; }
        public Account LeadingTeacher { get; set; }
        [StringLength(100)]
        public string Status { get; set; }
    }
}

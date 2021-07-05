using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenter.Model
{
    class CourseStudents
    {
        public int CourseStudentsId { get; set; }
        public Course Course { get; set; }
        public Account Student { get; set; }

        [StringLength(100)]
        public string Status { get; set; }
    }
}

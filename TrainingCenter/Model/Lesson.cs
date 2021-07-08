using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenter.Model
{
    public class Lesson
    {
        public int LessonId { get; set; }
        public Course CourseLesson { get; set; }
        [StringLength(100)]
        public string RoomNr { get; set; }
        public DateTime LessonStart { get; set; }
        public DateTime LessonEnd { get; set; }

    }
}

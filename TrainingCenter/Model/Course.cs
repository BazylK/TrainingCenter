﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenter.Model
{
    class Course
    {
        public int CourseId { get; set; }
        [StringLength(100)]
        public string Title { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        public Account LeadingTeacher { get; set; }
        [StringLength(100)]
        public string Status { get; set; }
    }
}
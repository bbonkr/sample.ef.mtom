using System;
using System.Collections.Generic;

namespace Sample.Entities
{
    /// <summary>
    /// Student entity
    /// </summary>
    public class Student
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public virtual List<Enrollment> Enrollments { get; set; }
    }
}

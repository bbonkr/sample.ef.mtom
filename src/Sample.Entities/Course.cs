using System.Collections.Generic;

namespace Sample.Entities
{
    /// <summary>
    /// Course entity
    /// </summary>
    public class Course
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public virtual List<Enrollment> Enrollments { get; set; }

        public virtual List<Student> Students { get; set; }
    }
}

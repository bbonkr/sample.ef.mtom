namespace Sample.Entities
{
    /// <summary>
    /// Student is enrolled this course.
    /// </summary>
    public class Enrollment
    {
        public long StudentId { get; set; }

        public long CourseId { get; set; }

        public virtual Student Student { get; set; }

        public virtual Course Course { get; set; }
    }
}

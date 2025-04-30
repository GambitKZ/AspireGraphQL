namespace ServerPart.Models;

public class CoursePlan
{
    public int Id { get; set; }
    public string Name { get; set; }

    // Courses
    public Guid CourseId { get; set; }
    public Course Course { get; set; }

    // Teachers
    public Guid TeacherId { get; set; }
    public Teacher Teacher { get; set; }

    // Students
    //public Guid StudentId { get; set; }
    //public Student Student { get; set; }
    public ICollection<Student> Students { get; set; } = new List<Student>();
}

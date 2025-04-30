namespace ServerPart.Models;

public class Student
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public GenderEnum Gender { get; set; }

    public int CoursePlanId { get; set; }
    public CoursePlan CoursePlan { get; set; }
}

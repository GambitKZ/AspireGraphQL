namespace ServerPart.Models
{
    public class Teacher
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public GenderEnum Gender { get; set; }
    }
}

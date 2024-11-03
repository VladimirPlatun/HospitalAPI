
namespace HospitalAPI.Model
{
    public class Patient
    {
        public Guid Id { get; set; } 

        public NameInfo Name { get; set; } 

        public Gender Gender { get; set; } 

        public DateTime BirthDate { get; set; } 

        public bool Active { get; set; } 
    }
}


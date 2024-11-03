using System.Text.Json.Serialization;

namespace HospitalAPI.Model
{
    public class NameInfo
    {
        public Guid Id { get; set; } 

        public string UseO { get; set; }

        public string Family { get; set; }

        public string FirstName { get; set; }

        public string Patronymic { get; set; }

        public Guid PatientId { get; set; } // Внешний ключ на Patient

        public Patient? Patient { get; set; } // Навигационное свойство для связи с Patient
    }
}

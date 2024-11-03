using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleAppHospital.Service;
using HospitalAPI.Model;

namespace ConsoleAppHospital.Service
{
    public class PatientGenerator
    {
        private static readonly string[] Families = { "Ivanov", "Petrov", "Sidorov", "Smirnov", "Fedorov" };
        private static readonly string[] FirstNames = { "Alexey", "Ivan", "Petr", "Ekaterina", "Maria" };
        private static readonly string[] Patronymics = { "Alekseevich", "Ivanovich", "Petrovna", "Sergeevna", "Viktorovna" };
        private static readonly Random Random = new Random();
        public static async Task GenerateAndSendPatients()
        {
            //string apiUrl = Environment.GetEnvironmentVariable("API_URL");
            string apiUrl = "https://hospital_api:3306/api/Patients";


            for (int i = 0; i < 100; i++)
            {
                var nameInfo = new NameInfo
                {
                    UseO = "official",
                    Family = Families[Random.Next(Families.Length)],
                    FirstName = FirstNames[Random.Next(FirstNames.Length)],
                    Patronymic = Patronymics[Random.Next(Patronymics.Length)]
                };
          
                var patient = new Patient
                {
                    Name = nameInfo,
                    Gender = (Gender)Random.Next(0, 4),
                    BirthDate = DateTime.Now.AddYears(-Random.Next(1, 10)),
                    Active = Random.Next(0, 4) > 0
                };

                try
                {
                    var response = await ApiCaller.Post(apiUrl, patient);
                    Console.WriteLine($"Successfully created patient {i + 1}: {response} ");
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Failed to create patient {i + 1}: {ex.Message} {apiUrl}");
                }
            }
        }

    }
}

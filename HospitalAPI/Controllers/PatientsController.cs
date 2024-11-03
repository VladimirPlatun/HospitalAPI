using HospitalAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace HospitalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить список всех пациентов.
        /// </summary>
        /// <remarks>
        /// Этот метод возвращает полный список пациентов, хранящихся в базе данных, с подробной информацией.
        /// </remarks>
        /// <returns>Коллекция пациентов.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            return Ok(await _context.Patients.Include(p => p.Name).ToListAsync());
        }

        /// <summary>
        /// Получить пациента по идентификатору.
        /// </summary>
        /// <param name="id">Уникальный идентификатор пациента GUID.</param>
        /// <remarks>
        /// Этот метод используется для получения информации о конкретном пациенте.
        /// </remarks>
        /// <returns>Данные пациента с указанным идентификатором или сообщение об ошибке, если пациент не найден.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(Guid id)
        {
            var patient = await _context.Patients.Include(p => p.Name).FirstOrDefaultAsync(p => p.Id == id);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        /// <summary>
        /// Создать нового пациента.
        /// </summary>
        /// <param name="patient">Данные нового пациента, включая обязательные поля, такие как фамилия и дата рождения.</param>
        /// <remarks>
        /// Этот метод добавляет нового пациента в базу данных и возвращает его данные после успешного создания. Пример:
        /// { 
        ///  "name": {
        ///    "useO": "official",
        ///    "family": "Doe",
        ///    "firstName": "John",
        ///    "patronymic": "Johnovich"
        ///  },
        ///  "gender": 0,
        ///  "birthDate": "2024-10-31T14:29:30.640Z",
        ///  "active": true
        ///}
        /// </remarks>
        /// <returns>Созданный пациент.</returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        public async Task<ActionResult<Patient>> CreatePatient(Patient patient)
        {
            try
            {
                if (string.IsNullOrEmpty(patient.Name?.Family) || patient.BirthDate == default)
                {
                    return BadRequest("Family name and BirthDate are required.");
                }

                patient.Id = Guid.NewGuid();
                patient.Name.Id = Guid.NewGuid();
                patient.Active = true;

                patient.Name.PatientId = patient.Id;

                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patient);
            }
            catch (Exception ex)
            {
                // Логируем ошибку
                Debug.WriteLine(ex, "Error creating patient.");
                return StatusCode(500, "Internal server error.");
            }
        }


        /// <summary>
        /// Обновить информацию о существующем пациенте.
        /// </summary>
        /// <param name="id">Идентификатор пациента, которого нужно обновить.</param>
        /// <param name="updatedPatient">Новые данные для обновления пациента.</param>
        /// <remarks>
        /// Метод обновляет данные о пациенте, такие как фамилия, пол, дата рождения и статус активности.Пример:
        /// id: 6d8d8e6c-6c30-45bd-b27a-80180476a994
        /// { 
        ///  "name": {
        ///    "useO": "official",
        ///    "family": "test",
        ///    "firstName": "John",
        ///    "patronymic": "Johnovich"
        ///  },
        ///  "gender": 0,
        ///  "birthDate": "2024-10-31T14:29:30.640Z",
        ///  "active": true
        ///}
        /// </remarks>
        /// <returns>Код 204 (No Content) при успешном обновлении или ошибка, если пациент не найден.</returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(Guid id, Patient updatedPatient)
        {
            var patient = await _context.Patients.Include(p => p.Name).FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(updatedPatient.Name?.Family) || updatedPatient.BirthDate == default)
            {
                return BadRequest("Family name and BirthDate are required.");
            }
            patient.Name.Family = updatedPatient.Name.Family;
            patient.Name.FirstName = updatedPatient.Name.FirstName; 
            patient.Name.Patronymic = updatedPatient.Name.Patronymic; 
            patient.Gender = updatedPatient.Gender;
            patient.BirthDate = updatedPatient.BirthDate;
            patient.Active = updatedPatient.Active;

            await _context.SaveChangesAsync();

            return NoContent();
        }


        /// <summary>
        /// Удалить пациента по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор пациента, которого нужно удалить.</param>
        /// <remarks>
        /// Этот метод удаляет пациента из базы данных, если он найден.
        /// </remarks>
        /// <returns>Код 204 (No Content) при успешном удалении или ошибка, если пациент не найден.</returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(Guid id)
        {
            var patient = await _context.Patients.Include(p => p.Name).FirstOrDefaultAsync(p => p.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        /// <summary>
        /// Поиск по дате рождения пациента.
        /// </summary>
        /// <param name="birthDate"></param>
        /// <remarks>
        /// Этот метод позволяет найти пользователя по дате рождения используя специальные прекфиксы:
        /// eq (равно): точное совпадение с указанной датой и временем.
        /// ne(не равно) : все даты, кроме указанной.
        /// lt(меньше чем): все даты до указанной.
        /// gt(больше чем): все даты после указанной.
        /// le(меньше или равно): все даты до и включая указанную.
        /// ge(больше или равно): все даты после и включая указанную.
        /// sa(начинается после): находит диапазоны, которые начинаются после указанной даты.
        /// eb(заканчивается до): находит диапазоны, которые заканчиваются до указанной даты.
        /// ap(приблизительно): включает даты вблизи указанной, разница определяется системой.
        /// Пример 1: Поиск по дате 2019-08-30 (Patient: Alexey Sidorov)
        ///Точное совпадение: eq2019-08-30
        ///До указанной даты: lt2019-08-30
        ///После указанной даты: gt2019-08-30
        ///Пример 2: Поиск по дате 2020-01-15 (Patient: Ivan Ivanov)
        ///Не равно дате: ne2020-01-15
        ///Меньше или равно дате: le2020-01-15
        ///Больше или равно дате: ge2020-01-15
        ///Пример 3: Поиск по дате 2023-02-18 (Patient: Ekaterina Smirnova)
        ///После указанной даты: sa2023-02-18 (найти пациентов, родившихся после 18 февраля 2023 года)
        ///До указанной даты: eb2023-02-18 (найти пациентов, родившихся до 18 февраля 2023 года)
        ///Пример 4: Поиск по дате 2021-05-20 (Patient: Maria Petrova)
        ///Точное совпадение: eq2021-05-20
        ///Период около даты: ap2021-05-20 (найдет пациентов, родившихся примерно в день этой даты)
        /// </remarks>
        /// <returns>Код 404 (BadRequest) при написании неправильного префикса или формата даты. Код 200 (Ok) возвращает пользователя если такой был найден с датой рождения, если пользователь был не обнаружен выведет пустой список.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]      
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Patient>>> SearchByBirthDate([FromQuery] string birthDate)
        {
            var match = Regex.Match(birthDate, @"^(?<prefix>(eq|ne|lt|gt|ge|le|sa|eb|ap))?(?<date>\d{4}(-\d{2})?(-\d{2})?(T\d{2}:\d{2}(:\d{2})?(\.\d{1,4})?(Z|([+-]\d{2}:\d{2})))?)$");

            if (!match.Success)
            {
                return BadRequest("Invalid date format. Please use the correct format with optional prefixes.");
            }

            var prefix = match.Groups["prefix"].Value;
            var dateStr = match.Groups["date"].Value;
            if (!DateTimeOffset.TryParse(dateStr, out var targetDate))
            {
                return BadRequest("Invalid date format.");
            }

            IQueryable<Patient> query = _context.Patients.Include(p => p.Name);

            switch (prefix)
            {
                case "eq":
                    query = query.Where(p => p.BirthDate.Date == targetDate.Date);
                    break;
                case "ne":
                    query = query.Where(p => p.BirthDate.Date != targetDate.Date);
                    break;
                case "lt":
                    query = query.Where(p => p.BirthDate < targetDate);
                    break;
                case "gt":
                    query = query.Where(p => p.BirthDate > targetDate);
                    break;
                case "ge":
                    query = query.Where(p => p.BirthDate >= targetDate);
                    break;
                case "le":
                    query = query.Where(p => p.BirthDate <= targetDate);
                    break;
                case "sa":
                    query = query.Where(p => p.BirthDate > targetDate);
                    break;
                case "eb":
                    query = query.Where(p => p.BirthDate < targetDate);
                    break;
                case "ap":
                    var rangeStart = targetDate.AddDays(-1);
                    var rangeEnd = targetDate.AddDays(1);
                    query = query.Where(p => p.BirthDate >= rangeStart && p.BirthDate <= rangeEnd);
                    break;
                default:
                    query = query.Where(p => p.BirthDate.Date == targetDate.Date);
                    break;
            }

            var result = await query.ToListAsync();
            return Ok(result);
        }

        /*
        Пример 1: Поиск по дате 2019-08-30 (Patient: Alexey Sidorov)
        Точное совпадение: eq2019-08-30
        До указанной даты: lt2019-08-30
        После указанной даты: gt2019-08-30
        Пример 2: Поиск по дате 2020-01-15 (Patient: Ivan Ivanov)
        Не равно дате: ne2020-01-15
        Меньше или равно дате: le2020-01-15
        Больше или равно дате: ge2020-01-15
        Пример 3: Поиск по дате 2023-02-18 (Patient: Ekaterina Smirnova)
        После указанной даты: sa2023-02-18 (найти пациентов, родившихся после 18 февраля 2023 года)
        До указанной даты: eb2023-02-18 (найти пациентов, родившихся до 18 февраля 2023 года)
        Пример 4: Поиск по дате 2021-05-20 (Patient: Maria Petrova)
        Точное совпадение: eq2021-05-20
        Период около даты: ap2021-05-20 (найдет пациентов, родившихся примерно в день этой даты)
        */
    }
}

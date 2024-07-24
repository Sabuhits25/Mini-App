using Mini_App;
using Newtonsoft.Json;

public class Program
{
    private static string filePath = "database.json";
    private static List<Classroom> classrooms = new();
    private static bool exit;

    public static void Main()
    {
        Directory.CreateDirectory(@"C:\Users\Zenbook\OneDrive\İş masası");
        string classroomsPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "classrooms.json");
        string result;

        using (StreamReader sr = new StreamReader(classroomsPath))
        {
            result = sr.ReadToEnd();
        }

        classrooms = JsonConvert.DeserializeObject<List<Classroom>>(result);
        if (classrooms == null)
            classrooms = new List<Classroom>();

        while (!exit)
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Classroom yarat");
            Console.WriteLine("2. Student yarat");
            Console.WriteLine("3. Butun Telebeleri ekrana cixart");
            Console.WriteLine("4. Secilmis sinifdeki telebeleri ekrana cixart");
            Console.WriteLine("5. Telebe sil");
            Console.WriteLine("6. Cixis");
            Console.Write("Seciminizi edin: ");
            var choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        CreateClassroom();
                        break;
                    case "2":
                        CreateStudent();
                        break;
                    case "3":
                        DisplayAllStudents();
                        break;
                    case "4":
                        DisplayClassroomStudents();
                        break;
                    case "5":
                        DeleteStudent();
                        break;
                    case "6":
                        var json = JsonConvert.SerializeObject(classrooms);
                        using (StreamWriter sw = new StreamWriter(classroomsPath))
                        {
                            sw.WriteLine(json);
                        }
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Yanlis secim etdiniz. Yeniden cehd edin.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Xəta baş verdi: {ex.Message}");
            }

            SaveData();
        }
    }

    private static void CreateClassroom()
    {
        Console.Write("Sinif adi (2 boyuk herf 3 reqem): ");
        var name = Console.ReadLine();
        while (!name.IsValidClassroomName())
        {
            Console.WriteLine("Yanlis format. Yeniden cehd edin.");
            name = Console.ReadLine();
        }

        Console.WriteLine("Sinif tipi (Backend = 0, Frontend = 1): ");
        var typeInput = Console.ReadLine();
        if (!Enum.TryParse<ClassroomType>(typeInput, out var type))
        {
            Console.WriteLine("Yanlis sinif tipi. Yeniden cehd edin.");
            return;
        }

        classrooms.Add(new Classroom(name, type));
        Console.WriteLine("Sinif yaradildi.");
    }

    private static void CreateStudent()
    {
        Console.Write("Sinif ID: ");
        if (!int.TryParse(Console.ReadLine(), out var classroomId))
        {
            Console.WriteLine("Yanlis ID format.");
            return;
        }

        var classroom = FindClassroomById(classroomId);
        if (classroom == null)
        {
            throw new ClassroomNotFoundException("Sinif tapilmadi.");
        }

        Console.Write("Telebenin adi: ");
        var name = Console.ReadLine();
        while (!name.IsValidName())
        {
            Console.WriteLine("Yanlis format. Yeniden cehd edin.");
            name = Console.ReadLine();
        }

        Console.Write("Telebenin soyadi: ");
        var surname = Console.ReadLine();
        while (!surname.IsValidSurname())
        {
            Console.WriteLine("Yanlis format. Yeniden cehd edin.");
            surname = Console.ReadLine();
        }

        var student = new Student(name, surname);
        classroom.AddStudent(student);
        Console.WriteLine("Telebe yaradildi.");
    }

    private static void DisplayAllStudents()
    {
        foreach (var classroom in classrooms)
        {
            Console.WriteLine($"Sinif: {classroom.Name}");
            foreach (var student in classroom.GetStudents())
            {
                Console.WriteLine($"Id: {student.Id}, Ad: {student.Name}, Soyad: {student.Surname}");
            }
        }
    }

    private static void DisplayClassroomStudents()
    {
        Console.Write("Sinif ID: ");
        if (!int.TryParse(Console.ReadLine(), out var classroomId))
        {
            Console.WriteLine("Yanlis ID format.");
            return;
        }

        var classroom = FindClassroomById(classroomId);
        if (classroom == null)
        {
            throw new ClassroomNotFoundException("Sinif tapilmadi.");
        }

        Console.WriteLine($"Sinif: {classroom.Name}");
        foreach (var student in classroom.GetStudents())
        {
            Console.WriteLine($"Id: {student.Id}, Ad: {student.Name}, Soyad: {student.Surname}");
        }
    }

    private static void DeleteStudent()
    {
        Console.Write("Sinif ID: ");
        if (!int.TryParse(Console.ReadLine(), out var classroomId))
        {
            Console.WriteLine("Yanlis ID format.");
            return;
        }

        var classroom = FindClassroomById(classroomId);
        if (classroom == null)
        {
            throw new ClassroomNotFoundException("Sinif tapilmadi.");
        }

        Console.Write("Telebenin ID: ");
        if (!int.TryParse(Console.ReadLine(), out var studentId))
        {
            Console.WriteLine("Yanlis ID format.");
            return;
        }

        classroom.DeleteStudent(studentId);
        Console.WriteLine("Telebe silindi.");
    }

    private static Classroom FindClassroomById(int id)
    {
        return classrooms.FirstOrDefault(classroom => classroom.Id == id);
    }

    private static void SaveData()
    {
        var json = JsonConvert.SerializeObject(classrooms, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    private static List<Classroom> LoadData()
    {
        if (!File.Exists(filePath))
        {
            return new List<Classroom>();
        }

        var json = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<List<Classroom>>(json);
    }
}
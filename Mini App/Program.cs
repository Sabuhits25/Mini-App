using Mini_App;

public class Program
{
    private static List<Classroom> classrooms = new List<Classroom>();

    public static void Main()
    {
        bool exit = false;
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
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Yanlis secim etdiniz. Yeniden cehd edin.");
                    break;
            }
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
        ClassroomType type = (ClassroomType)Enum.Parse(typeof(ClassroomType), typeInput);

        classrooms.Add(new Classroom(name, type));
        Console.WriteLine("Sinif yaradildi.");
    }

    private static void CreateStudent()
    {
        Console.Write("Sinif ID: ");
        var classroomId = int.Parse(Console.ReadLine());

        var classroom = classrooms.Find(c => c.Id == classroomId);
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
        var classroomId = int.Parse(Console.ReadLine());

        var classroom = classrooms.Find(c => c.Id == classroomId);
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
        var classroomId = int.Parse(Console.ReadLine());

        var classroom = classrooms.Find(c => c.Id == classroomId);
        if (classroom == null)
        {
            throw new ClassroomNotFoundException("Sinif tapilmadi.");
        }

        Console.Write("Telebenin ID: ");
        var studentId = int.Parse(Console.ReadLine());

        classroom.DeleteStudent(studentId);
        Console.WriteLine("Telebe silindi.");
    }
}
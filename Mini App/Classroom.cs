namespace Mini_App
{
    public class Classroom
    {
        private static int _idCounter = 0;
        private List<Student> students;

        public int Id { get; private set; }
        public string Name { get; set; }
        public ClassroomType Type { get; set; }
        public int Limit { get; private set; }

        public Classroom(string name, ClassroomType type)
        {
            Id = ++_idCounter;
            Name = name;
            Type = type;
            students = new List<Student>();

            if (type == ClassroomType.Backend)
            {
                Limit = 20;
            }
            else if (type == ClassroomType.Frontend)
            {
                Limit = 15;
            }
        }

        public void AddStudent(Student student)
        {
            if (students.Count < Limit)
            {
                students.Add(student);
            }
            else
            {
                throw new InvalidOperationException("Classroom limit reached.");
            }
        }

        public Student FindStudentById(int id)
        {
            return students.Find(student => student.Id == id);
        }

        public void DeleteStudent(int id)
        {
            var student = FindStudentById(id);
            if (student == null)
            {
                throw new StudentNotFoundException($"Student with id {id} not found.");
            }
            students.Remove(student);
        }

        public List<Student> GetStudents()
        {
            return students;
        }
    }
}

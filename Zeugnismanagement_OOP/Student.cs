namespace Zeugnismanagement_OOP
{
    public class Student
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public Fach[] Faecher { get; set; }
        public int AbsenceDays { get; set; }
        public int ExcusedAbsenceDays { get; set; }
        public int UnexcusedAbsenceDays => AbsenceDays - ExcusedAbsenceDays;
    }
}
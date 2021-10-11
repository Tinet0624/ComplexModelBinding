using System.ComponentModel.DataAnnotations;

namespace ComplexModelBinding.Models
{
    public class Course
    {
        [Key]
        public int Id {  get; set; }
        public string Title {  get; set; }
        public string Description {  get; set; }

        public Instructor Teacher {  get; set; }
    }

    public class CourseCreateViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public List<Instructor> AvailableTeachers { get; set; }

        public Instructor ChosenTeacher {  get; set; }
    }
}

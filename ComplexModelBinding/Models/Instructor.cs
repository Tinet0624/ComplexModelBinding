using System.ComponentModel.DataAnnotations;

namespace ComplexModelBinding.Models
{
    public class Instructor
    {
        [Key]
        public int Id {  get; set; }
        public string FullName {  get; set; }
    }
}

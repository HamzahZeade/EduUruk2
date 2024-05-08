using System.ComponentModel.DataAnnotations.Schema;

namespace EduUruk.Models.Entities
{
    [Table("Test")]
    public class Test : _GlobalColumn
    {

        public Guid Id { get; set; }
        public string Title { get; set; }
        //public string UserID { get; set; }
        //[ForeignKey("UserID")]
        //public virtual User User { get; set; }

        public int Mark { get; set; }
        public List<Question> Questions { get; set; }
    }
}

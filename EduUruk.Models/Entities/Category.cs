namespace EduUruk.Models.Entities
{
	public class Category
	{
		public Guid Id { get; set; }

		public string Name { get; set; }


		public ICollection<Video> Videos { get; set; }
	}
}

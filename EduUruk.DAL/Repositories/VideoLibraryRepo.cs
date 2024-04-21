using EduUruk.DAL.EnitityDAL;
using EduUruk.Models.Entities;

namespace EduUruk.DAL.Repositories
{
    public class VideoLibraryRepo : DBProvider
    {
        private readonly ApplicationDbContext _DbContext;
        public VideoLibraryRepo(ApplicationDbContext DbContext)
        {
            _DbContext = DbContext;
        }
        public List<Video> GetAll()
        {
            var res = _DbContext.Videos.ToList();

            return res;
        }

        public Video FindById(Guid id)
        {
            var res = _DbContext.Videos.Find(id);

            return res;
        }



    }
}

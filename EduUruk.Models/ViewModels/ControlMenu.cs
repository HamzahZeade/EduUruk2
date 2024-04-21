

namespace EduUruk.Models.ViewModels
{
    public class ControlMenu
    {
        public int MenuID { get; set; }

        public Nullable<int> GroupID { get; set; }

        public string MenuTitle { get; set; }

        public string MenuLink { get; set; }

        public string MenuIcon { get; set; }

        public Nullable<int> GroupOrder { get; set; }

        public List<ControlMenu> ChildLinks { get; set; }

        public ControlMenu()
        {
            ChildLinks = new List<ControlMenu>();
        }
    }

}

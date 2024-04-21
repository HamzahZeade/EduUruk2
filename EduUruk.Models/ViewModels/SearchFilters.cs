

namespace EduUruk.Models.ViewModels
{
    public class SearchFilters
    {
        public string queryType { get; set; }
        #region Properties

        /// <summary>
        /// Gets or sets StartIndex. 
        /// </summary>
        public int StartIndex
        {
            get;
            set;
        }
        //------------------------------------------

        /// <summary>
        /// Gets or sets  PageLenght. 
        /// </summary>
        public int PageLength
        {
            get;
            set;
        }
        //------------------------------------------

        /// <summary>
        /// Gets or sets SortColumn. 
        /// </summary>
        public string SortColumn
        {
            get;
            set;
        }
        //------------------------------------------

        /// <summary>
        /// Gets or sets SortDirection. 
        /// </summary>
        public string SortDirection
        {
            get;
            set;
        }
        //------------------------------------------

        public string SearchString
        {
            get;
            set;
        }

        public int UserID
        {
            get;
            set;
        }
        public Guid EventId
        {
            get;
            set;
        }
        public int CountryID
        {
            get;
            set;
        }
        public int? FormID { get; set; }
        public int? VisitFormTypeID { get; set; }
        #endregion
    }

}
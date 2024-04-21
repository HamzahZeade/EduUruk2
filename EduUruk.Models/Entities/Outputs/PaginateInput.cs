namespace EduUruk.Models.Enitities.Outputs
{
    public class PaginateInput
    {
        public int StartIndex;
        public int PageLength;
    }


    public class PagerInput
    {
        #region Properties
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        #endregion Properties

        #region Constructors
        public PagerInput()
        {

        }

        public PagerInput(int startIndex, int pageLength)
        {
            PageIndex = startIndex;
            PageSize = pageLength;
        }
        #endregion Constructors

    }
}

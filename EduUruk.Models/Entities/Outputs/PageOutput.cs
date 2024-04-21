namespace EduUruk.Models.Enitities.Outputs
{
    public class PageOutput<T>
    {
        public T Data;
        public int PageIndex;
        public int PageLength;
        public int RecordsFiltered;
        public int TotalRecords;

        public PageOutput()
        {

        }
        public PageOutput(T data, int startIndex, int pageLength, int totalRecords)
        {
            Data = data;
            PageIndex = startIndex;
            PageLength = pageLength;
            TotalRecords = totalRecords;
        }

        public int PageSize { get; set; }
    }
}

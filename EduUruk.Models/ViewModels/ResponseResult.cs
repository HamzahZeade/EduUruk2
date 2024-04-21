


namespace EduUruk.Models.ViewModels
{
    public class ResponseResult
    {
        public int Id
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public string URL
        {
            get;
            set;
        }

        public string BtnClass
        {
            get;
            set;
        }

        public string BtnText
        {
            get;
            set;
        }

        public bool Close
        {
            get;
            set;
        }
        public Object newObj
        {
            get;
            set;
        }
        public Guid newObjId
        {
            get;
            set;
        }
    }

    public class ResponseResultAPI
    {
        public string Status
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }
    }
}


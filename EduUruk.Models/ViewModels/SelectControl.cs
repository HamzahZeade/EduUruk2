

namespace EduUruk.Models.ViewModels
{
    public class SelectControl
    {
        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }
        /// <summary>
        /// Gets or sets Value. 
        /// </summary>
        public string Value
        {
            get;
            set;
        }
        //------------------------------------------

        /// <summary>
        /// Gets or sets  Text. 
        /// </summary>
        public string Text
        {
            get;
            set;
        }
        //------------------------------------------
    }
}

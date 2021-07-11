using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Payroll.ViewModels
{
    public class Notification
    {
        public Notification(string type, string title, string message, string redirect )
        {
            Type = type;
            Title = title;
            Message = message;
            Redirect = redirect;
        }
        [Required]
        public string Title { set; get; }
        [Required]
        public string Message { set; get; }
        public string Icon 
        {
            get
            {
                if (Type == "success")
                {
                    return "fa fa-check";
                }
                else if(Type == "danger") 
                {
                    return "fa fa-times";
                }
                else
                {
                    return "fa fa-user";
                }
            }
        }
        [Required]
        public string Type { set; get; }
        [DefaultValue(null)]
        public string Redirect { set; get; }
    }
}

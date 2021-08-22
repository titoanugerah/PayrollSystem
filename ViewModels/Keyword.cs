using System;

namespace Payroll.ViewModels
{
    public class Keyword
    {
        public Keyword(int id, string key)
        {
            Id = id;
            Key = Standarize(key);

        }
        public int Id { set; get; }
        public string Key { set; get; }

        private string Standarize(object keyword)
        {
            string value = null;
            if (keyword != null)
            {
                value = keyword.ToString().ToLower().Replace(" ", string.Empty);
            }
            return value;
        }
    }
}

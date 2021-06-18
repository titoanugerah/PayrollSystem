namespace Payroll.Models
{
    public class Position : Audit
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Remark { set; get; }
    }
}

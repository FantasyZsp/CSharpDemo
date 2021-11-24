using SqlSugar;

namespace TestProject.SqlSugar
{
    public class Customer
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public string Name { get; set; }

        public string Email { get; set; }
        public int? CardId { get; set; }
        public string Mark { get; set; }
        public string Config { get; set; }
    }
}
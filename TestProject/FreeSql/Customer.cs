using FreeSql.DataAnnotations;

namespace TestProject.FreeSql
{
    public class Customer
    {
        [Column(IsIdentity = true)] public uint? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int? CardId { get; set; }
        public string Mark { get; set; }
        public string Config { get; set; }
    }
}
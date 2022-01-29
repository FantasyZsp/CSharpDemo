using Newtonsoft.Json;
using SqlSugar;

namespace TestProject.SqlSugar
{
    [SugarTable("customer_sugar")]
    public class CustomerSugar
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public string Name { get; set; }

        public string Email { get; set; }
        public int? CardId { get; set; }
        public string Mark { get; set; }
        public string Config { get; set; }


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
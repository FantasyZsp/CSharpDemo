using SqlSugar;

namespace TestProject.SqlSugar
{
    [SugarTable("card_sugar")]
    public class CardSugar
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int? CardId { get; set; }

        public string CardNo { get; set; }
        public string Mark { get; set; }
    }
}
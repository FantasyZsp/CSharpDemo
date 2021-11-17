using System;
using FreeSql.DataAnnotations;

namespace TestProject.FreeSql
{
    public class Student
    {
        [Column(IsIdentity = true)] public string StuId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
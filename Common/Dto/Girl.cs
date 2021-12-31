namespace Common.Dto
{
    public class Girl
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public uint Age { get; set; }

        public Girl(string id, string name, uint age)
        {
            Id = id;
            Name = name;
            Age = age;
        }

        public Girl()
        {
        }
    }
}
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


    public class Animal
    {
        public string Name { get; set; }
    }

    public class Dog : Animal
    {
        public string NickName { get; set; }
    }

    public class Cat : Animal
    {
        public string LovelyName { get; set; }
    }
}
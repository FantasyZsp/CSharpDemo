namespace TestProject.BaseApi.Models;

public class Kid
{
    public string Name { get; set; }
    public int Age { get; set; }
    public SexEnum Sex { get; set; }
}

public enum SexEnum
{
    MM,
    GG
}
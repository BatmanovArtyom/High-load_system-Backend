namespace UserService.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Password { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    
    
}
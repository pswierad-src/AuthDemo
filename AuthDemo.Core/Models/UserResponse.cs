namespace AuthDemo.Core.Models;

public class UserResponse
{
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
}
namespace AuthDemo.Mongo.Documents;

public class UserDocument : IDocument
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateAdded { get; } = DateTime.Now;
    public DateTime? DateModified { get; set; }
    
    public string PasswordHash { get; set; }
}
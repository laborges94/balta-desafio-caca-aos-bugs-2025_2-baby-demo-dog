namespace BugStore.Responses.Customers;

public class Update
{
    public string OldName { get; set; } = string.Empty;
    public string OldEmail { get; set; } = string.Empty;
    public string OldPhone { get; set; } = string.Empty;
    public DateTime OldBirthDate { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
}
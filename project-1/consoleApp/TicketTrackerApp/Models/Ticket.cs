
namespace Models;
public class Ticket
{

    public int Id { get; set; } = 0; 
    public string Description { get; set;}
    public string Name { get; set; } = "Not set"; 
    public decimal Amount { get; set; }
    public string Status { get; set; } = "pending";

    public int EmployeeId { get; set; }

    public Ticket(int id, string description, string name, decimal amount, string status, int employeeId){
        Id = id; 
        Description = description;
        Name = name; 
        Amount = amount; 
        Status = status; 
        EmployeeId = employeeId; 
    }
    public Ticket(string description, decimal amount, int employeeId)
    {
        Description = description;
        Amount = amount;
        EmployeeId = employeeId;

    }

    

    public override string ToString()
    {
        string amountStr = string.Format("{0:0.00}", Amount);
        return $"Ticket Id:{Id} - {Name} - {Description} - ${amountStr} - {Status}";
    }

}
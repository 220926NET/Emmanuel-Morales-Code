
public class TicketDto{
     public int Id { get; set; } = 0; 
    public string Description { get; set;}
    public string Name { get; set; } = "Not set"; 
    public decimal Amount { get; set; }
    public string Status { get; set; } = "pending";

    public int EmployeeId { get; set; }

    public TicketDto(int id, string description, string name, decimal amount, string status, int employeeId){
        Id = id; 
        Description = description;
        Name = name; 
        Amount = amount; 
        Status = status; 
        EmployeeId = employeeId; 
    }
  
}
class Ticket {

    public int Id {get; set;} = 0;
    public string Description {get; set; }

    public string? Name {get; set;}
    public int Amount {get; set;}

    public string Status {get; set;} = "Pending";

    public int EmployeeId {get; set;}


    public Ticket(string description, int amount, int employeeId ){
        Description = description;
        Amount = amount;
        EmployeeId = employeeId; 
        
    }

    public override string ToString(){

        return $"{Id}- {Name} - {Description} - ${Amount} - {Status}";
    }
    
}
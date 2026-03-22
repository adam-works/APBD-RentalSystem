namespace EquipmentRental.App.Models;

public abstract class User
{
    public Guid Id { get; private set; } = Guid.NewGuid(); // Unikalny ID [cite: 28]
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public abstract int MaxRentalLimit { get; } // Limit zależny od typu [cite: 43, 44]

    protected User(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}

public class Student : User
{
    public Student(string firstName, string lastName) : base(firstName, lastName) { }
    public override int MaxRentalLimit => 2; // Limit dla studenta [cite: 43]
}

public class Employee : User
{
    public Employee(string firstName, string lastName) : base(firstName, lastName) { }
    public override int MaxRentalLimit => 5; // Limit dla pracownika [cite: 44]
}
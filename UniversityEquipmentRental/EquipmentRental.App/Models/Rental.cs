namespace EquipmentRental.App.Models;

public class Rental
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public User Renter { get; set; }
    public Equipment RentedEquipment { get; set; }
    public DateTime RentalDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; } // null, jeśli jeszcze nie zwrócono

    public bool IsOverdue => ReturnDate == null && DateTime.Now > DueDate;
}
using System;

namespace EquipmentRental.App.Models;

public class Rental
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    
    // Kto wypożycza
    public User Renter { get; set; }
    
    // Co wypożycza
    public Equipment RentedEquipment { get; set; }
    
    // Daty
    public DateTime RentalDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; } // null oznacza, że sprzęt jest wciąż wypożyczony

    // Właściwość pomocnicza
    public bool IsOverdue => ReturnDate == null && DateTime.Now > DueDate;

    public Rental(User renter, Equipment rentedEquipment, DateTime rentalDate, DateTime dueDate)
    {
        Renter = renter;
        RentedEquipment = rentedEquipment;
        RentalDate = rentalDate;
        DueDate = dueDate;
    }
}
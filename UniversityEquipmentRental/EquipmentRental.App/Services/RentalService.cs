using System;
using System.Collections.Generic;
using System.Linq;
using EquipmentRental.App.Models;

namespace EquipmentRental.App.Services;

public class RentalService
{
    private readonly List<Rental> _rentals = new();
    
    // Reguła kary: Stała kwota + stawka dzienna
    private const decimal BasePenalty = 10m;
    private const decimal DailyPenaltyRate = 5m;

    public void RentEquipment(User user, Equipment equipment, int days, DateTime? rentalDate = null)
    {
        if (equipment == null) throw new ArgumentNullException(nameof(equipment));
        if (user == null) throw new ArgumentNullException(nameof(user));
        if (days <= 0) throw new ArgumentException("Rental duration must be positive.", nameof(days));

        // 1. Sprawdź dostępność sprzętu
        if (!equipment.IsAvailable)
        {
            Console.WriteLine($"[ERROR] Equipment '{equipment.Name}' is not available.");
            return;
        }

        // 2. Sprawdź limit użytkownika
        var activeRentalsCount = _rentals.Count(r => r.Renter.Id == user.Id && r.ReturnDate == null);
        if (activeRentalsCount >= user.MaxRentalLimit)
        {
            Console.WriteLine($"[ERROR] User '{user.FirstName} {user.LastName}' reached rental limit ({user.MaxRentalLimit}).");
            return;
        }

        // 3. Utwórz wypożyczenie
        // Jeśli podano datę wypożyczenia (np. symulacja przeszłości), użyj jej. W przeciwnym razie użyj obecnej.
        var startDate = rentalDate ?? DateTime.Now;
        var dueDate = startDate.AddDays(days);
        
        var rental = new Rental(user, equipment, startDate, dueDate);

        _rentals.Add(rental);
        equipment.IsAvailable = false; 
        
        Console.WriteLine($"[SUCCESS] Rented '{equipment.Name}' to {user.FirstName}. Due: {rental.DueDate:d}");
    }

    public void ReturnEquipment(User user, Equipment equipment)
    {
        var rental = _rentals.FirstOrDefault(r => r.Renter.Id == user.Id && r.RentedEquipment.Id == equipment.Id && r.ReturnDate == null);
        
        if (rental == null)
        {
            Console.WriteLine("[ERROR] No active rental found.");
            return;
        }

        rental.ReturnDate = DateTime.Now;
        equipment.IsAvailable = true; 

        // Oblicz karę
        if (rental.ReturnDate > rental.DueDate)
        {
            var overdueDays = (rental.ReturnDate.Value - rental.DueDate).Days;
            if (overdueDays == 0 && rental.ReturnDate > rental.DueDate) overdueDays = 1;
            
            var penalty = BasePenalty + (overdueDays * DailyPenaltyRate);
            Console.WriteLine($"[RETURN] Late! Penalty: {penalty:C}");
        }
        else
        {
            Console.WriteLine("[RETURN] On time. No penalty.");
        }
    }

    public IEnumerable<Rental> GetActiveRentals(User user)
    {
        return _rentals.Where(r => r.Renter.Id == user.Id && r.ReturnDate == null);
    }

    public IEnumerable<Rental> GetOverdueRentals()
    {
        return _rentals.Where(r => r.ReturnDate == null && DateTime.Now > r.DueDate);
    }

    public void GenerateReport()
    {
        Console.WriteLine("\n--- Rental Report ---");
        Console.WriteLine($"Total active: {_rentals.Count(r => r.ReturnDate == null)}");
        Console.WriteLine($"Overdue: {GetOverdueRentals().Count()}");
        foreach (var rental in _rentals)
        {
            var status = rental.ReturnDate == null ? (DateTime.Now > rental.DueDate ? "[OVERDUE]" : "[Active]") : "[Returned]";
            Console.WriteLine($"{status} {rental.Renter.FirstName} -> {rental.RentedEquipment.Name} (Due: {rental.DueDate:d})");
        }
        Console.WriteLine("---------------------\n");
    }
}
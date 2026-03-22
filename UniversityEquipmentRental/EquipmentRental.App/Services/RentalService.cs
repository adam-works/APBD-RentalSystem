using System;
using System.Collections.Generic;
using System.Linq;
using EquipmentRental.App.Models;

namespace EquipmentRental.App.Services;

public class RentalService
{
    private readonly List<Rental> _rentals = new();
    private readonly List<User> _users = new();
    private readonly List<Equipment> _equipment = new();

    // Reguła kary: Stała kwota + stawka dzienna
    private const decimal BasePenalty = 10m;
    private const decimal DailyPenaltyRate = 5m;

    public void AddUser(User user) => _users.Add(user);
    public void AddEquipment(Equipment equipment) => _equipment.Add(equipment);

    public void RentEquipment(User user, Equipment equipment, int days)
    {
        if (equipment == null) throw new ArgumentNullException(nameof(equipment));
        if (user == null) throw new ArgumentNullException(nameof(user));
        if (days <= 0) throw new ArgumentException("Rental duration must be positive.", nameof(days));

        // 1. Sprawdź dostępność sprzętu
        if (!equipment.IsAvailable)
        {
            throw new InvalidOperationException($"Equipment '{equipment.Name}' is not available.");
        }

        // 2. Sprawdź limit użytkownika
        var activeRentalsCount = _rentals.Count(r => r.Renter.Id == user.Id && r.ReturnDate == null);
        if (activeRentalsCount >= user.MaxRentalLimit)
        {
            throw new InvalidOperationException($"User '{user.FirstName} {user.LastName}' has reached their rental limit of {user.MaxRentalLimit}.");
        }

        // 3. Utwórz wypożyczenie (poprawiony konstruktor)
        var rental = new Rental(user, equipment, DateTime.Now, DateTime.Now.AddDays(days));

        _rentals.Add(rental);
        equipment.IsAvailable = false; // Zaktualizuj stan sprzętu
        
        Console.WriteLine($"Successfully rented '{equipment.Name}' to {user.FirstName} {user.LastName}. Due: {rental.DueDate:d}");
    }

    public decimal ReturnEquipment(User user, Equipment equipment)
    {
        var rental = _rentals.FirstOrDefault(r => r.Renter.Id == user.Id && r.RentedEquipment.Id == equipment.Id && r.ReturnDate == null);
        
        if (rental == null)
        {
            throw new InvalidOperationException("No active rental found for this user and equipment.");
        }

        rental.ReturnDate = DateTime.Now;
        equipment.IsAvailable = true; // Sprzęt wraca do puli

        // Oblicz karę
        if (rental.ReturnDate > rental.DueDate)
        {
            var overdueDays = (rental.ReturnDate.Value - rental.DueDate).Days;
            // Jeśli spóźnienie jest mniejsze niż 1 dzień (ale po czasie), liczymy jako 1 dzień
            if (overdueDays == 0 && rental.ReturnDate > rental.DueDate) overdueDays = 1;
            
            var penalty = BasePenalty + (overdueDays * DailyPenaltyRate);
            Console.WriteLine($"Equipment returned late! Penalty: {penalty:C}");
            return penalty;
        }

        Console.WriteLine("Equipment returned on time. No penalty.");
        return 0m;
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
        Console.WriteLine($"Total active rentals: {_rentals.Count(r => r.ReturnDate == null)}");
        Console.WriteLine($"Overdue rentals: {GetOverdueRentals().Count()}");
        foreach (var rental in _rentals)
        {
            var status = rental.ReturnDate == null ? (DateTime.Now > rental.DueDate ? "[OVERDUE]" : "[Active]") : "[Returned]";
            Console.WriteLine($"{status} {rental.Renter.FirstName} rented {rental.RentedEquipment.Name} (Due: {rental.DueDate:d})");
        }
        Console.WriteLine("---------------------\n");
    }
}
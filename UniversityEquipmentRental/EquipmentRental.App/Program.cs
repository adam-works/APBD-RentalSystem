using System;
using EquipmentRental.App.Models;
using EquipmentRental.App.Services;

namespace EquipmentRental.App
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== University Equipment Rental System ===");

            // 1. Inicjalizacja serwisu
            var rentalService = new RentalService();

            // 2. Dodanie użytkowników
            var student = new Student("Jan", "Kowalski");
            var employee = new Employee("Dr.", "Nowak");

            // 3. Dodanie sprzętu
            var laptop = new Laptop("Dell XPS 15", "i7-12700H", 32);
            var projector = new Projector("Epson EB-U42", "1920x1200", 3600);
            var camera = new Camera("Canon EOS R5", 45, true);
            var backupLaptop = new Laptop("ThinkPad T14", "i5-1135G7", 16);

            Console.WriteLine("\n--- Scenario 1: Standard Rental ---");
            rentalService.RentEquipment(student, laptop, 7); // OK

            Console.WriteLine("\n--- Scenario 2: Renting Unavailable Equipment ---");
            rentalService.RentEquipment(employee, laptop, 3); // Fail (already rented)

            Console.WriteLine("\n--- Scenario 3: User Limit Check (Student Max 2) ---");
            rentalService.RentEquipment(student, projector, 2); // OK (2nd item)
            rentalService.RentEquipment(student, camera, 5);    // Fail (3rd item > limit 2)

            Console.WriteLine("\n--- Scenario 4: Overdue Return Simulation ---");
            // Symulacja: Wypożyczono 10 dni temu na 5 dni (spóźnienie 5 dni)
            var pastDate = DateTime.Now.AddDays(-10);
            rentalService.RentEquipment(employee, camera, 5, pastDate); 

            Console.WriteLine("\n--- Generating Report (Before Returns) ---");
            rentalService.GenerateReport();

            Console.WriteLine("\n--- Scenario 5: Returns ---");
            // Zwrot w terminie
            rentalService.ReturnEquipment(student, laptop); 
            
            // Zwrot spóźniony (z karą)
            rentalService.ReturnEquipment(employee, camera);

            Console.WriteLine("\n--- Final Report ---");
            rentalService.GenerateReport();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
# University Equipment Rental System

Aplikacja konsolowa w C# symulująca system wypożyczalni sprzętu uczelnianego. Projekt realizuje założenia programowania obiektowego, czystej architektury i zasad SOLID.

## Uruchomienie

1. Otwórz projekt w środowisku programistycznym (np. Visual Studio, Rider).
2. Przejdź do katalogu projektu `EquipmentRental.App`.
3. Uruchom aplikację. Program automatycznie wykona scenariusz testowy zdefiniowany w metodzie Main.

## Struktura Projektu i Decyzje Architektoniczne

Projekt został podzielony na warstwy, aby oddzielić logikę biznesową od danych i interfejsu użytkownika:

- Models/: Zawiera klasy domenowe (User, Equipment, Rental). Są to proste obiekty przechowujące stan.
- Services/: Zawiera logikę biznesową (RentalService). Odpowiada za procesowanie wypożyczeń, sprawdzanie limitów i naliczanie kar.
- Program.cs: Warstwa prezentacji. Służy wyłącznie do inicjalizacji systemu i przeprowadzenia demonstracji działania.

### Zastosowanie zasad SOLID

1. Single Responsibility Principle (SRP):
   - Klasa RentalService odpowiada wyłącznie za logikę wypożyczeń. Nie zajmuje się wyświetlaniem danych ani ich przechowywaniem na stałe.
   - Klasy User i Equipment odpowiadają tylko za reprezentację danych.

2. Open/Closed Principle (OCP):
   - System jest otwarty na rozszerzenia. Dodanie nowego typu sprzętu (np. Drone) wymaga jedynie stworzenia nowej klasy dziedziczącej po Equipment, bez konieczności modyfikacji istniejącego kodu w serwisie.

3. Liskov Substitution Principle (LSP):
   - Wszędzie tam, gdzie system oczekuje obiektu User, możemy przekazać instancję klasy Student lub Employee. Serwis traktuje je w ten sam sposób, respektując ich unikalne limity (polimorfizm).

## Reguły Biznesowe

Zaszyte w logice aplikacji (RentalService oraz modele):

1. Limity wypożyczeń:
   - Student: maksymalnie 2 aktywne wypożyczenia.
   - Pracownik: maksymalnie 5 aktywnych wypożyczeń.
   - Weryfikacja następuje w metodzie RentEquipment.

2. Kary za opóźnienie:
   - Wzór: Opłata Podstawowa (10 PLN) + (Liczba dni spóźnienia * 5 PLN).
   - Kara jest naliczana w momencie zwrotu w metodzie ReturnEquipment.

3. Dostępność:
   - Nie można wypożyczyć sprzętu, który ma status IsAvailable = false.

## Scenariusz Demonstracyjny

Metoda Main realizuje następujący przepływ:
1. Rejestracja użytkowników (Student, Pracownik).
2. Rejestracja sprzętu (Laptop, Projektor, Kamera).
3. Próba poprawnego wypożyczenia.
4. Próba wypożyczenia niedostępnego sprzętu (obsługa błędu).
5. Próba przekroczenia limitu przez studenta (obsługa błędu).
6. Symulacja zwrotu po terminie i naliczenie kary.
7. Generowanie raportu końcowego o stanie wypożyczalni.
namespace EquipmentRental.App.Models;

public abstract class Equipment
{
    public Guid Id { get; private set; } = Guid.NewGuid(); // Generowane przez system
    public string Name { get; set; }
    public bool IsAvailable { get; set; } = true; // Status dostępności

    protected Equipment(string name) => Name = name;
}

// Przykład typu sprzętu z 2 polami specyficznymi
public class Laptop : Equipment
{
    public string Processor { get; set; }
    public int RamSizeGb { get; set; }

    public Laptop(string name, string processor, int ramSizeGb) : base(name)
    {
        Processor = processor;
        RamSizeGb = ramSizeGb;
    }
}
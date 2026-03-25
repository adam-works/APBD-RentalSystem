using System;

namespace EquipmentRental.App.Models;

public abstract class Equipment
{
    public Guid Id { get; private set; } = Guid.NewGuid(); // Generowane przez system
    public string Name { get; set; }
    public bool IsAvailable { get; set; } = true; // Status dostępności

    protected Equipment(string name) => Name = name;
}

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

public class Projector : Equipment
{
    public string MaxResolution { get; set; }
    public int BrightnessLumens { get; set; }

    public Projector(string name, string maxResolution, int brightnessLumens) : base(name)
    {
        MaxResolution = maxResolution;
        BrightnessLumens = brightnessLumens;
    }
}

public class Camera : Equipment
{
    public int Megapixels { get; set; }
    public bool HasOpticalZoom { get; set; }

    public Camera(string name, int megapixels, bool hasOpticalZoom) : base(name)
    {
        Megapixels = megapixels;
        HasOpticalZoom = hasOpticalZoom;
    }
}
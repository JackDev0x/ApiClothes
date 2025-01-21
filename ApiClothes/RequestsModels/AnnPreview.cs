using ApiClothes.DtoModels;

public class AnnPreview
{
    public object Id { get; set; }
    public object Slug { get; set; }
    public object Brand { get; set; }
    public object Model { get; set; }
    public object Description { get; set; }
    public object summary { get; set; }
    public UserDto User { get; set; }
    public object Price { get; set; }
    public object Power { get; set; }
    public string? Engine { get; set; }
    public object FuelType { get; set; }
    public object Mileage { get; set; }
    public object ProductionYear { get; set; }
    public List<int> LikedBy { get; set; }
    public object Images { get; set; }
}
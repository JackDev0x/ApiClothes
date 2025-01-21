public class FilterRequest
{
    public List<string>? Brands { get; set; }
    public List<string>? Models { get; set; }
    public int? MinYear { get; set; }
    public int? MaxYear { get; set; }
    public int? MinMileage { get; set; }
    public int? MaxMileage { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public List<string>? BodyTypes { get; set; }
    public int? MinPower { get; set; }
    public int? MaxPower { get; set; }

    public List<int>? MultimediaFeatures { get; set; }
    public List<int>? SafetyFeatures { get; set; }
    public List<int>? DriverAssistanceSystemsFeatures { get; set; }
    public List<int>? PerformanceFeatures { get; set; }
    public List<int>? OtherFeatures { get; set; }
}
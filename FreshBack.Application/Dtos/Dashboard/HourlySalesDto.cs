namespace FreshBack.Application.Dtos.Dashboard;

public class HourlySalesDto
{
    public List<HourlySalesPoint> Points { get; set; } = [];
}

public class HourlySalesPoint
{
    public int Hour { get; set; }
    public string Label => $"{Hour:D2}:00";
    public decimal Revenue { get; set; }
}
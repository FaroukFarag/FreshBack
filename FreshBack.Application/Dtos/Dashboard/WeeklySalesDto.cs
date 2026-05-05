namespace FreshBack.Application.Dtos.Dashboard;

public class WeeklySalesDto
{
    public List<WeeklySalesPoint> Points { get; set; } = default!;
}

public class WeeklySalesPoint
{
    public int DayIndex { get; set; }
    public DateOnly Date { get; set; }
    public string DayNameAr { get; set; } = default!;
    public string DayNameEn { get; set; } = default!;
    public decimal Revenue { get; set; }
}

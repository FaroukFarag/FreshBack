namespace FreshBack.Application.Dtos.Dashboard;

public class RevenueDto
{
    public decimal Today { get; set; }
    public decimal ThisWeek { get; set; }
    public decimal ThisMonth { get; set; }
    public decimal PercentageComparedToYesterday { get; set; }
    public decimal PercentageComparedToLastWeek { get; set; }
    public decimal PercentageComparedToLastMonth { get; set; }
}

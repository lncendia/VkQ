namespace VkQ.Infrastructure.DataStorage.Models.Reports.Base;

public abstract class ReportModel : IModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public UserModel User { get; set; } = null!;
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsSucceeded { get; set; }
    public string? Message { get; set; }
    public bool IsStarted { get; set; }

    public List<ReportElementModel> ReportElementsList { get; set; } = new();
}
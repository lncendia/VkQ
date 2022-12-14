using VkQ.Domain.Abstractions;
using VkQ.Domain.Reposts.BaseReport.Events;
using VkQ.Domain.Reposts.BaseReport.Exceptions;
using VkQ.Domain.Users.Entities;

namespace VkQ.Domain.Reposts.BaseReport.Entities;

public abstract class Report : AggregateRoot
{
    protected Report(User user)
    {
        if (!user.IsSubscribed) throw new UserSubscribeException(user.Id, user.Name);
        if (!user.HasVk || !user.Vk!.IsActive()) throw new UserVkException(user.Id);
        if (!user.Vk.ProxyId.HasValue) throw new UserVkException(user.Id);
        UserId = user.Id;
    }


    public Guid UserId { get; }
    public DateTimeOffset CreationDate { get; } = DateTimeOffset.Now;
    public DateTimeOffset? StartDate { get; private set; }
    public DateTimeOffset? EndDate { get; private set; }
    public bool IsCompleted { get; private set; }
    public bool IsSucceeded { get; private set; }
    public string? Message { get; private set; }
    public bool IsStarted => StartDate.HasValue;

    protected readonly List<ReportElement> ReportElementsList = new();

    protected void Start() => StartDate = DateTimeOffset.Now;

    protected void Succeed()
    {
        EndDate = DateTimeOffset.Now;
        IsCompleted = true;
        IsSucceeded = true;
        AddDomainEvent(new ReportFinishedEvent(Id, true));
    }

    protected void Fail(string message)
    {
        EndDate = DateTimeOffset.Now;
        IsCompleted = true;
        IsSucceeded = false;
        Message = message;
        AddDomainEvent(new ReportFinishedEvent(Id, false));
    }
}
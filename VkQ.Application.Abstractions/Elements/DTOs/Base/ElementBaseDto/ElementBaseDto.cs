﻿namespace VkQ.Application.Abstractions.Elements.DTOs.Base.ElementBaseDto;

public abstract class ReportBaseDto
{
    protected ReportBaseDto(ReportBaseBuilder builder)
    {
        Id = builder.Id ?? throw new ArgumentNullException(nameof(builder.Id));
        CreationDate = builder.CreationDate ?? throw new ArgumentNullException(nameof(builder.CreationDate));
        StartDate = builder.StartDate;
        EndDate = builder.EndDate;
        IsStarted = builder.IsStarted;
        IsCompleted = builder.IsCompleted;
        IsSucceeded = builder.IsSucceeded;
        Message = builder.Message;
    }

    public Guid Id { get; }
    public DateTimeOffset CreationDate { get; }
    public DateTimeOffset? StartDate { get; }
    public DateTimeOffset? EndDate { get; }
    public bool IsStarted { get; }
    public bool IsCompleted { get; }
    public bool IsSucceeded { get; }
    public string? Message { get; }
}
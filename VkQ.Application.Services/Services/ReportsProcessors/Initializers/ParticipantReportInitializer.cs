﻿using VkQ.Application.Abstractions.ReportsProcessors.ServicesInterfaces;
using VkQ.Domain.Abstractions.UnitOfWorks;
using VkQ.Domain.Participants.Specification;
using VkQ.Domain.Reposts.ParticipantReport.Entities;

namespace VkQ.Application.Services.Services.ReportsProcessors.Initializers;

public class ParticipantReportInitializer : IReportInitializerUnit<ParticipantReport>
{
    private readonly IUnitOfWork _unitOfWork;

    public ParticipantReportInitializer(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task InitializeReportAsync(ParticipantReport report, CancellationToken token)
    {
        var participants =
            await _unitOfWork.ParticipantRepository.Value.FindAsync(
                new ParticipantsByUserIdSpecification(report.UserId));
        report.Start(participants);
    }
}
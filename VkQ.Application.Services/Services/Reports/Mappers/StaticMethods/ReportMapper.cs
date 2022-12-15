﻿using VkQ.Application.Abstractions.Elements.DTOs.Base.PublicationElementBaseDto;
using VkQ.Application.Abstractions.ReportsQuery.DTOs.Base.PublicationReportBaseDto;
using VkQ.Domain.Reposts.BaseReport.Entities.Publication;

namespace VkQ.Application.Services.Services.Reports.Mappers.StaticMethods;

internal class ReportMapper
{
    public static void InitReportBuilder(PublicationReportBaseBuilder builder, PublicationReport report)
    {
        builder.WithLinkedUsers(report.LinkedUsers)
            .WithHashtag(report.Hashtag)
            .WithId(report.Id)
            .WithCreationDate(report.CreationDate);
        if (report.SearchStartDate != null) builder.WithSearchStartDate(report.SearchStartDate.Value);

        if (!report.IsStarted) return;
        builder.WithPublications(report.Publications.Select(x => new PublicationDto(x.Id, x.ItemId, x.OwnerId)))
            .WithDates(report.StartDate!.Value, report.EndDate)
            .WithStatus(report.IsCompleted, report.IsSucceeded);

        if (!string.IsNullOrEmpty(report.Message)) builder.WithMessage(report.Message);
    }

    public static void InitElementBuilder(PublicationReportElementBaseBuilder builder, PublicationReportElement element)
    {
        builder.WithAccepted(element.IsAccepted)
            .WithVip(element.Vip)
            .WithParticipantId(element.ParticipantId)
            .WithLikeChatName(element.LikeChatName)
            .WithName(element.Name)
            .WithVkId(element.VkId);
    }
}
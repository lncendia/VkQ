using System.Diagnostics.CodeAnalysis;
using VkQ.Domain.Links.Entities;
using VkQ.Domain.Reposts.BaseReport.Entities;
using VkQ.Domain.Reposts.BaseReport.Exceptions;
using VkQ.Domain.Reposts.PublicationReport.DTOs;
using VkQ.Domain.Reposts.PublicationReport.Exceptions;
using VkQ.Domain.Users.Entities;

namespace VkQ.Domain.Reposts.PublicationReport.Entities;

public abstract class PublicationReport : Report
{
    protected PublicationReport(User user, string hashtag, DateTimeOffset? searchStartDate = null,
        IReadOnlyCollection<Link>? coAuthors = null) : base(user)
    {
        Hashtag = hashtag;
        SearchStartDate = searchStartDate;
        if (coAuthors == null) return;
        if (coAuthors.Count > 3) throw new TooManyLinksException();
        foreach (var l in coAuthors)
        {
            if (!l.IsConfirmed) throw new ArgumentException(null, nameof(coAuthors));
            if (l.User1Id == user.Id) _linkedUsersList.Add(l.User2Id);
            else if (l.User2Id == user.Id) _linkedUsersList.Add(l.User1Id);
            else throw new ArgumentException(null, nameof(coAuthors));
        }
    }

    private readonly List<Guid> _linkedUsersList = new();
    public List<Guid> LinkedUsers => _linkedUsersList.ToList();
    public string Hashtag { get; }
    public DateTimeOffset? SearchStartDate { get; }
    public int Process { get; protected set; }

    protected List<Publication> PublicationsList = new();
    public List<Publication> Publications => PublicationsList.ToList();

    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    private void LoadPublications(IEnumerable<PublicationDto> dtos)
    {
        if (!dtos.Any()) throw new PublicationsListEmptyException();
        PublicationsList = dtos.Select(dto => new Publication(dto.ItemId, dto.OwnerId)).ToList();
    }

    ///<exception cref="ReportAlreadyCompletedException">Report already completed</exception>
    ///<exception cref="ElementsListEmptyException">elements collection is empty</exception>
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    private void LoadElements(IEnumerable<PublicationReportElement> elements)
    {
        if (!elements.Any()) throw new ElementsListEmptyException();
        ReportElementsList.AddRange(elements);
    }

    protected void Start(IEnumerable<PublicationDto> dtos, IEnumerable<PublicationReportElement> elements)
    {
        LoadPublications(dtos);
        LoadElements(elements);
        base.Start();
    }
}
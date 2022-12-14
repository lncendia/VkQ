namespace VkQ.Application.Abstractions.ReportsProcessors.DTOs;

public class PublicationDto
{
    public PublicationDto(long publicationId, long ownerId)
    {
        PublicationId = publicationId;
        OwnerId = ownerId;
    }

    public long PublicationId { get;  }
    public long OwnerId { get;  }
}
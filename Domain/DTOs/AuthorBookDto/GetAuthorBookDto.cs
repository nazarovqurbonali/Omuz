namespace Domain.DTOs.AuthorBookDto;

public class GetAuthorBookDto
{
    public int  Id { get; set; }
    public int AuthorId { get; set; }
    public int BookId { get; set; }
    public DateTimeOffset CreateAt { get; set; }
    public DateTimeOffset  UpdateAt { get; set; }
}

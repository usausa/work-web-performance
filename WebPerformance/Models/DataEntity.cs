namespace WebPerformance.Models;

using Smart.Data.Accessor.Attributes;

[Name("DATA")]
public class DataEntity
{
    [Key]
    [Name("ID")]
    public string Id { get; set; } = default!;

    [Name("NAME")]
    public string Name { get; set; } = default!;
}

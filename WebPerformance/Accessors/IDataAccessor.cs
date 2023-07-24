namespace WebPerformance.Accessors;

using System.Data.Common;
using Smart.Data.Accessor.Attributes;
using Smart.Data.Accessor.Builders;

using WebPerformance.Models;

[DataAccessor]
public interface IDataAccessor
{
    [QueryFirstOrDefault]
    ValueTask<DataEntity?> QueryAsync(string id);

    [Count(typeof(DataEntity))]
    public ValueTask<int> CountAsync();

    [Insert]
    ValueTask InsertAsync(DbTransaction tx, DataEntity entity);
}

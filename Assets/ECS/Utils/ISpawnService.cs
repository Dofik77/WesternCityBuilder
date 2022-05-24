namespace ECS.Utils
{
    public interface ISpawnService<in TEntity, out TObject>
    {
        TObject SpawnPrefab(TEntity entity);
    }
}
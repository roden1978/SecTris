public interface IStorage
{
    public object Load(object defaultData, string fileName);
    public void Save(object data, string fileName);
}
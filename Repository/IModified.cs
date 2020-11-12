namespace Repository
{
    public interface IModified
    {
        bool Add<IdType>(dynamic entety, Table table, string pKName);
    }
}

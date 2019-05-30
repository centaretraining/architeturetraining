namespace LegacyApp.Ordering.Domain
{
    public interface IGetCartQuery
    {
        GetCartQueryResult GetCartById(int cartId);
    }
}

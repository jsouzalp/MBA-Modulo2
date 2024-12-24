namespace FinPlanner360.Domains.Entities
{
    public class DomainInput<T> where T : class
    {
        public T Input { get; set; }
    }
}

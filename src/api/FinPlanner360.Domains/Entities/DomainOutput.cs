using FinPlanner360.Entities;

namespace FinPlanner360.Domains.Entities
{
    public class DomainOutput<T> 
    {
        public bool Success { get => Errors == null || !Errors.Any(); }
        public string Message { get; set; }
        public T Output { get; set; }
        public IEnumerable<ErrorBase> Errors { get; set; } = null;
    }
}
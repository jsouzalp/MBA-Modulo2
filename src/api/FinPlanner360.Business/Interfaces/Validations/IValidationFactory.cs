namespace FinPlanner360.Busines.Interfaces.Validations
{
    public interface IValidationFactory<TI> where TI : class
    {
        Task<bool> ValidateAsync(TI input);
    }
}

namespace FinPlanner360.Busines.Interfaces.Validations
{
    public interface IValidationFactory<TI> where TI : class
    {
        //Task<(bool Success, ICollection<string> Errors)> ValidateAsync(TI input);
        Task<bool> ValidateAsync(TI input);
    }
}

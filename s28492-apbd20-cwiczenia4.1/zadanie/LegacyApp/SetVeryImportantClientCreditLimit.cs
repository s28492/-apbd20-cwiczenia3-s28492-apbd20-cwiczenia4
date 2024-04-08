namespace LegacyApp;

public class SetVeryImportantClientCreditLimit: ISetCreditLimit
{
    public int CalculateCreditLimit(User user)
    {
        user.HasCreditLimit = false;
        return Int32.MaxValue;
    }
}
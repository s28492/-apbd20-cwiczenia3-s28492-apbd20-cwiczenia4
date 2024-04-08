using System;

namespace LegacyApp
{
    public class UserService
    {
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {


            if (!(IsFullNameGiven(firstName, lastName) && IsEmailSyntaxCorrect(email))) return false;
            
            var now = DateTime.Now;
            int age = CalculateUserAge(now, dateOfBirth);

            if (!IsUserAnAdult(age)) return false;

            var clientRepository = new ClientRepository();
            var client = clientRepository.GetById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            var userCreditService = new UserCreditService();
            userCreditService.SetCreditLimit(client.Type, user);

            IsCredditLimitCorrect(user, 500);

            UserDataAccess.AddUser(user);
            return true;
        }

        private static bool IsFullNameGiven(string firstName, string lastName)
        {
            return !string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName);
        }

        private static bool IsEmailSyntaxCorrect(string email)
        {
            return email.Contains("@") || email.Contains(".");
        }

        private static int CalculateUserAge(DateTime now, DateTime dateOfBirth)
        {
            return  IsUserBeforeBirthdayThisYear(now, dateOfBirth) ? now.Year - dateOfBirth.Year - 1 : now.Year - dateOfBirth.Year;
        }

        private static bool IsUserBeforeBirthdayThisYear(DateTime now, DateTime dateOfBirth)
        {
            return now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day);
        }

        private static bool IsUserAnAdult(int age)
        {
            return age > 21;
        }

        private static bool IsCredditLimitCorrect(User user, double minimumLimit)
        {
            return !(user.HasCreditLimit && user.CreditLimit < minimumLimit);
        }
    }
}

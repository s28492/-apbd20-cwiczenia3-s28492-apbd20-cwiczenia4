using System;
using System.Collections.Generic;
using System.Threading;

namespace LegacyApp
{
    public class UserCreditService : IDisposable
    {
        /// <summary>
        /// Simulating database
        /// </summary>
        private readonly Dictionary<string, int> _database =
            new Dictionary<string, int>()
            {
                {"Kowalski", 200},
                {"Malewski", 20000},
                {"Smith", 10000},
                {"Doe", 3000},
                {"Kwiatkowski", 1000}
            };
        
        public void Dispose()
        {
            //Simulating disposing of resources
        }

        /// <summary>
        /// This method is simulating contact with remote service which is used to get info about someone's credit limit
        /// </summary>
        /// <returns>Client's credit limit</returns>
        internal int GetCreditLimit(string lastName, DateTime dateOfBirth)
        {
            Waiting.WaitABit(3000);
            
            if (_database.ContainsKey(lastName))
                return _database[lastName];

            throw new ArgumentException($"Client {lastName} does not exist");
        }

        internal void SetCreditLimit(string clientType, User user)
        {
            int creditLimit = GetCreditLimit(user.LastName, user.DateOfBirth);
            ISetCreditLimit setLimit;
            switch (clientType)
            {
                case "VeryImportantClient":
                    setLimit = new SetVeryImportantClientCreditLimit();
                    creditLimit = setLimit.CalculateCreditLimit(user);
                    break;
                case "ImportantClient":
                    setLimit = new SetImportantClientCreditLimit();
                    creditLimit = setLimit.CalculateCreditLimit(user);
                    break;
                default:
                    setLimit = new SetNotImportantCkientCreditLimit();
                    creditLimit = setLimit.CalculateCreditLimit(user);
                    break;
                
            }
            user.CreditLimit = creditLimit;
        }
        
    }
}
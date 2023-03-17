using GameBotAlpha.Data.Models;
using GameBotAlpha.Data.Repositories;

namespace GameBotAlpha.Services
{
    public class Game : ConnectionStringContainer, IGame
    {
        private readonly IUserProfileRepository _userProfileRepository;
        public Game()
        {
            _userProfileRepository = new UserProfileRepository(_connectionString);
        }

        public bool HasStarted(string discordUid)
        {
            return _userProfileRepository.HasStarted(discordUid);
        }

        public void Reset(string discordUid)
        {
            _userProfileRepository.Reset(discordUid);
        }

        public Dictionary<string, object> Sell(string discordUid)
        {
            UserProfile profile = _userProfileRepository.GetById(discordUid);
            Dictionary<string, object> result = new()
            {
                { "StartingBalance", profile.Balance },
                { "ItemName", profile.Generator.Item.Name },
                { "ItemAmount", 0 },
                { "Profit", 0 },
                { "NewBalance", 0 }
            };

            //! Store the total amount of generations since the last sell.
            int generations = (profile.LastSell - DateTime.Now).Seconds / profile.Generator.SecondsPerGeneration;

            //! Store the amount of items sold.
            int itemAmount = 0;

            try
            {
                checked
                {
                    itemAmount = generations * profile.Generator.Amount;
                }
            }
            catch (OverflowException)
            {
                itemAmount = profile.Backpack.ItemLimit;
            }
            finally
            {
                itemAmount = itemAmount > profile.Backpack.ItemLimit ? profile.Backpack.ItemLimit : itemAmount;
                result["ItemAmount"] = itemAmount;
            }

            //! Stores the profit
            int profit = itemAmount * profile.Generator.Item.SellPrice;
            result["Profit"] = profit;

            //! Store the new balance, and update it in the databse
            int newBalance = profile.Balance + profit;
            result["NewBalance"] = newBalance;

            _userProfileRepository.SetBalance(discordUid, newBalance);

            return result;
        }

        public UserProfile GetProfile(string discordUid)
        {
            return _userProfileRepository.GetById(discordUid);
        }

        public Backpack Backpack(string discordUid)
        {
            return _userProfileRepository.GetById(discordUid).Backpack;
        }
    }
}

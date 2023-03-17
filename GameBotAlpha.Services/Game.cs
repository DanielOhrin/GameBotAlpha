using GameBotAlpha.Data.Enums;
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

        public void Start(string discordUid)
        {
            _userProfileRepository.Start(discordUid);
        }

        public bool HasStarted(string discordUid)
        {
            return _userProfileRepository.HasStarted(discordUid);
        }

        public KeyValuePair<bool, int> Upgrade(string discordUid, UpgradeTypes upgradeType)
        {
            /*! KeyValuePair:
             * bool: whether or not the upgrade happened
             * int: the price of the upgrade (if 0, then they are at the max upgrade for that item)
             */
            UserProfile profile = _userProfileRepository.GetById(discordUid);

            bool didSucceed = false;
            int priceOfUpgrade = _userProfileRepository.PriceOfUpgrade(discordUid, upgradeType);
            

            if (priceOfUpgrade > 0)
            {
                if (profile.Balance >= priceOfUpgrade)
                {
                    _userProfileRepository.Upgrade(discordUid, upgradeType);
                    didSucceed = true;
                }
            }

            return new KeyValuePair<bool, int>(didSucceed, priceOfUpgrade);
        }

        public KeyValuePair<int, Backpack> Backpack(string discordUid)
        {
            UserProfile profile = _userProfileRepository.GetById(discordUid);

            int generations = (DateTime.Now - profile.LastSell).Seconds / profile.Generator.SecondsPerGeneration;

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
            }

            return new KeyValuePair<int, Backpack>(itemAmount, profile.Backpack);
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
            int generations = (DateTime.Now - profile.LastSell).Seconds / profile.Generator.SecondsPerGeneration;

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

            _userProfileRepository.SetBalance(discordUid, newBalance, true);

            return result;
        }

        public UserProfile GetProfile(string discordUid)
        {
            return _userProfileRepository.GetById(discordUid);
        }
    }
}

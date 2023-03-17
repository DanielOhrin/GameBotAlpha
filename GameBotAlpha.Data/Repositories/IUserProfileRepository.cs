using GameBotAlpha.Data.Enums;
using GameBotAlpha.Data.Models;

namespace GameBotAlpha.Data.Repositories
{
    public interface IUserProfileRepository
    {
        public bool HasStarted(string discordUid);
        public void Start(string discordUid);
        public void Reset(string discordUid);
        public UserProfile GetById(string discordUid);
        public void SetBalance(string discordUid, int balance, bool isSell);
        public void Upgrade(string discordUid, UpgradeTypes upgradeType);
        public int PriceOfUpgrade(string discordUid, UpgradeTypes upgradeType);
    }
}

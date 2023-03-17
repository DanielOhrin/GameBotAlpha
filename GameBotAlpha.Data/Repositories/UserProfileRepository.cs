using System.Data;

using GameBotAlpha.Data.Enums;
using GameBotAlpha.Data.Models;

using Microsoft.Data.SqlClient;

namespace GameBotAlpha.Data.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(string connectionString) : base(connectionString) { }

        public void Start(string discordUid)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO dbo.UserProfile (DiscordUid)
                        VALUES (@DiscordUid)
                    ";

                    cmd.Parameters.AddWithValue("@DiscordUid", discordUid);
                    
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Reset(string discordUid)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE dbo.UserProfile
                        SET DateDeleted = GETDATE()
                        WHERE DiscordUid = @DiscordUid
                            AND DateDeleted IS NULL                    
                    ";
                    
                    cmd.Parameters.AddWithValue("@DiscordUid", discordUid);

                    cmd.ExecuteNonQuery();

                    Start(discordUid);
                }
            }
        }

        public UserProfile GetById(string discordUid)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT up.Id UserProfileId, up.DiscordUid, up.DateCreated, up.Balance, up.GeneratorId, up.LastSell, up.BackpackId
                                g.[Name] GeneratorName, g.ItemId, g.Amount, g.SPG,
                                i.[Name] ItemName, i.SellPrice,
                                bp.[Name] BackpackName, bp.BuyPrice BackpackBuyPrice, bp.ItemLimit
                        FROM dbo.UserProfile up
                        LEFT JOIN Generator g ON g.Id = up.GeneratorId 
                        LEFT JOIN Item i ON i.Id = g.ItemId
                        WHERE up.DiscordUid = @DiscordUid 
                            AND DateDeleted IS NULL
                    ";

                    cmd.Parameters.AddWithValue("@DiscordUid", discordUid);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        UserProfile profile = null!;

                        if (reader.Read())
                        {
                            profile = new()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                                DiscordUid = reader.GetString(reader.GetOrdinal("DiscordUid")),
                                DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                                Balance = reader.GetInt32(reader.GetOrdinal("Balance")),
                                GeneratorId = reader.GetInt32(reader.GetOrdinal("GeneratorId")),
                                BackpackId = reader.GetInt32(reader.GetOrdinal("BackpackId")),
                                LastSell = reader.GetDateTime(reader.GetOrdinal("LastSell")),
                                Generator = new()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("GeneratorId")),
                                    Name = reader.GetString(reader.GetOrdinal("GeneratorName")),
                                    Amount = reader.GetInt32(reader.GetOrdinal("Amount")),
                                    SecondsPerGeneration = reader.GetInt32(reader.GetOrdinal("SPG")),
                                    ItemId = reader.GetInt32(reader.GetOrdinal("ItemId")),
                                    Item = new()
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("ItemId")),
                                        Name = reader.GetString(reader.GetOrdinal("ItemName")),
                                        SellPrice = reader.GetInt32(reader.GetOrdinal("SellPrice"))
                                    }
                                },
                                Backpack = new()
                                { 
                                    Id = reader.GetInt32(reader.GetOrdinal("BackpackId")),
                                    Name = reader.GetString(reader.GetOrdinal("BackpackName")),
                                    BuyPrice = reader.GetInt32(reader.GetOrdinal("BackpackBuyPrice")),
                                    ItemLimit = reader.GetInt32(reader.GetOrdinal("ItemLimit"))
                                }
                            };
                        }

                        return profile;
                    }
                }
            }
        }

        public void SetBalance(string discordUid, int balance)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE dbo.UserProfile
                        SET Balance = @Balance
                        WHERE DiscordUid = @DiscordUid
                            AND DateDeleted IS NULL                    
                    ";

                    cmd.Parameters.AddWithValue("@DiscordUid", discordUid);
                    cmd.Parameters.AddWithValue("@Balance", balance);

                    cmd.ExecuteNonQuery();

                    Start(discordUid);
                }
            }
        }

        public void Upgrade(string discordUid, UpgradeTypes upgradeType)
        {
            string itemToUpgrade = Enum.GetName(typeof(UpgradeTypes), upgradeType) ?? "INVALID ITEM";

            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "dbo.Upgrade";

                    cmd.Parameters.AddWithValue("@DiscordUid", discordUid);
                    cmd.Parameters.AddWithValue("@UpgradeType", itemToUpgrade);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        
        public int PriceOfUpgrade(string discordUid, UpgradeTypes upgradeType)
        {
            string itemToUpgrade = Enum.GetName(typeof(UpgradeTypes), upgradeType) ?? "INVALID ITEM";

            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "dbo.PriceOfUpgrade";

                    cmd.Parameters.AddWithValue("@DiscordUid", discordUid);
                    cmd.Parameters.AddWithValue("@UpgradeType", itemToUpgrade);

                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public bool HasStarted(string discordUid)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id
                        FROM dbo.UserProfile
                        WHERE DiscordUid = @DiscordUid
                            AND DateDeleted IS NULL
                    ";

                    cmd.Parameters.AddWithValue("@DiscordUid", discordUid);

                    return cmd.ExecuteScalar() != null;
                }
            }
        }
    }
}

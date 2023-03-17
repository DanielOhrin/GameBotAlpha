use [GamePoc]
GO

DROP PROCEDURE IF EXISTS dbo.Upgrade
GO

CREATE PROCEDURE dbo.Upgrade @DiscordUid nvarchar(max), @UpgradeType nvarchar(50)
AS

-- We are checking this in the app, but still adding this error just in case.
IF @UpgradeType NOT IN ('Generator', 'Backpack')
	THROW 50000, 'Invalid upgrade type.', 16

-- In the app, we check that the user CAN upgrade their generator/backack AND that they have sufficient funds.
DECLARE @UpgradePrice INT;

IF @UpgradeType = 'Generator'
BEGIN
	SET @UpgradePrice = (
		SELECT g.BuyPrice
		FROM dbo.UserProfile up
		LEFT JOIN dbo.Generator g ON g.Id = up.GeneratorId + 1
		WHERE up.DiscordUid = @DiscordUid
			AND up.DateDeleted IS NULL
	)

	UPDATE dbo.UserProfile
	SET Balance -= @UpgradePrice,
		GeneratorId += 1
	WHERE DiscordUid = @DiscordUid
		AND DateDeleted IS NULL
END
ELSE IF @UpgradeType = 'Backpack'
BEGIN
	SET @UpgradePrice = (
		SELECT bp.BuyPrice
		FROM dbo.UserProfile up
		LEFT JOIN dbo.Backpack bp ON bp.Id = up.BackpackId + 1
		WHERE up.DiscordUid = @DiscordUid
			AND up.DateDeleted IS NULL
	)

		UPDATE dbo.UserProfile
	SET Balance -= @UpgradePrice,
		BackpackId += 1
	WHERE DiscordUid = @DiscordUid
		AND DateDeleted IS NULL
END
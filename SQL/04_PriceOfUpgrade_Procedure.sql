use [GamePoc]
GO

DROP PROCEDURE IF EXISTS dbo.PriceOfUpgrade
GO

CREATE PROCEDURE dbo.PriceOfUpgrade @DiscordUid nvarchar(max), @UpgradeType nvarchar(50)
AS

-- We are checking this in the app, but still adding this error just in case.
IF @UpgradeType NOT IN ('Generator', 'Backpack')
	THROW 50000, 'Invalid upgrade type.', 16

-- In the app, we check that the user CAN upgrade their generator/backack AND that they have sufficient funds.
DECLARE @UpgradePrice INT

IF @UpgradeType = 'Generator'
BEGIN
	SET @UpgradePrice = (
		SELECT COALESCE(g.BuyPrice, 0)
		FROM dbo.UserProfile up
		LEFT JOIN dbo.Generator g ON g.Id = up.GeneratorId + 1
		WHERE up.DiscordUid = @DiscordUid
			AND up.DateDeleted IS NULL
	)

	SELECT @UpgradePrice;
END
ELSE IF @UpgradeType = 'Backpack'
BEGIN
	SET @UpgradePrice = (
		SELECT COALESCE(bp.BuyPrice, 0)
		FROM dbo.UserProfile up
		LEFT JOIN dbo.Backpack bp ON bp.Id = up.BackpackId + 1
		WHERE up.DiscordUid = @DiscordUid
			AND up.DateDeleted IS NULL
	)

	SELECT @UpgradePrice;
END
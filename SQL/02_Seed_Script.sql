use [GamePoc]
GO

INSERT INTO dbo.Item ([Name], SellPrice) VALUES ('Dirt', 1), ('Cobblestone', 2), ('Iron', 4), ('Gold', 7), ('Diamond', 10), ('Emerald', 15)
GO

INSERT INTO dbo.Generator ([Name], BuyPrice, SPG, Amount, ItemId) VALUES ('Dirt Sieve', 0, 1, 1, 1),
('Cobblestone Generator', 25, 1, 1, 2),
('Iron Mine', 200, 1, 1, 3),
('Gold Mine', 2500, 1, 1, 4),
('Diamond Quarry', 7500, 1, 1, 5),
('Mine of Kings', 25000, 1, 1, 6)
GO

INSERT INTO dbo.Backpack ([Name], BuyPrice, ItemLimit) VALUES ('Torn Bindle', 0, 5),
('Repaired Bindle', 500, 15),
('Reinforced Bindle', 3000, 50),
('Leather Knapsack', 8000, 90),
('Bear Skin Tote', 15000, 150)
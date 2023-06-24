IF NOT EXISTS (SELECT * FROM Aircrafts)
BEGIN
INSERT INTO Aircrafts ([Name]) VALUES
('Boeing737-800'),
('Boeing787-8');
end
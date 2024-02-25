CREATE TABLE Achievements (
    Name VARCHAR(50) NOT NULL,
    Description VARCHAR(150) NOT NULL,
    Gamerscore INT NOT NULL,
    UnlockTime DATETIME(6) NOT NULL,
    PRIMARY KEY (Name ASC)
);
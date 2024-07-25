USE test;

-- Country table (already provided)
CREATE TABLE DC_Country (
    CountryId INT IDENTITY(1,1) PRIMARY KEY,
    CountryName NVARCHAR(50) NOT NULL
);

-- State table (already provided)
CREATE TABLE DC_State (
    StateId INT IDENTITY(1,1) PRIMARY KEY,
    CountryId INT NOT NULL,
    StateName NVARCHAR(50) NOT NULL,
    FOREIGN KEY (CountryId) REFERENCES DC_Country(CountryId)
);

-- New City table
CREATE TABLE DC_City (
    CityId INT IDENTITY(1,1) PRIMARY KEY,
    StateId INT NOT NULL,
    CityName NVARCHAR(50) NOT NULL,
    FOREIGN KEY (StateId) REFERENCES DC_State(StateId)
);

-- Insert sample cities (you can add more as needed)
INSERT INTO DC_City (StateId, CityName) VALUES 
(1, 'Los Angeles'), (1, 'San Francisco'), -- California
(2, 'Miami'), (2, 'Orlando'), -- Florida
(3, 'New York City'), (3, 'Buffalo'), -- New York
(4, 'Houston'), (4, 'Dallas'), -- Texas
(5, 'Calgary'), (5, 'Edmonton'), -- Alberta
(6, 'Vancouver'), (6, 'Victoria'), -- British Columbia
(7, 'Toronto'), (7, 'Ottawa'), -- Ontario
(8, 'Montreal'), (8, 'Quebec City'), -- Quebec
(9, 'London'), (9, 'Manchester'), -- England
(10, 'Edinburgh'), (10, 'Glasgow'), -- Scotland
(11, 'Cardiff'), (11, 'Swansea'), -- Wales
(12, 'Belfast'), (12, 'Derry'), -- Northern Ireland
(13, 'Sydney'), (13, 'Newcastle'), -- New South Wales
(14, 'Brisbane'), (14, 'Gold Coast'), -- Queensland
(15, 'Melbourne'), (15, 'Geelong'), -- Victoria
(16, 'Perth'), (16, 'Fremantle'), -- Western Australia
(17, 'New Delhi'), (17, 'Gurgaon'), -- Delhi
(18, 'Mumbai'), (18, 'Pune'), -- Maharashtra
(19, 'Bangalore'), (19, 'Mysore'), -- Karnataka
(20, 'Chennai'), (20, 'Coimbatore'); -- Tamil Nadu	
-- Crear la base de datos
CREATE DATABASE HibikiDB;

-- Mostrar las bases de datos existentes
SHOW DATABASES;

-- Usar la base de datos recién creada
USE HibikiDB;

-- Crear la tabla Artista
CREATE TABLE Artista (
    CantanteId INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(255) NOT NULL,
    OyentesMensuales INT NOT NULL,
    Descripcion TEXT NOT NULL
);

-- Insertar algunos datos iniciales en la tabla Artista
INSERT INTO Artista (Nombre, OyentesMensuales, Descripcion)
VALUES 
('Artista 1', 5000000, 'Descripción del Artista 1'),
('Artista 2', 3000000, 'Descripción del Artista 2'),
('Artista 3', 8000000, 'Descripción del Artista 3');

-- Crear la tabla Album
CREATE TABLE Album (
    AlbumId INT AUTO_INCREMENT PRIMARY KEY,
    ArtistId INT NOT NULL,
    Name VARCHAR(255) NOT NULL,
    ReleaseDate DATE,
    Image VARCHAR(255) NOT NULL,
    FOREIGN KEY (ArtistId) REFERENCES Artista(CantanteId)
);

-- Insertar algunos datos iniciales en la tabla Album
INSERT INTO Album (ArtistId, Name, ReleaseDate, Image)
VALUES 
(1, 'Album 1', '2020-01-01', 'image1.jpg'),
(2, 'Album 2', '2021-02-02', 'image2.jpg'),
(1, 'Album 3', '2022-03-03', 'image3.jpg'),
(3, 'Album 4', '2019-07-15', 'image4.jpg'),
(2, 'Album 5', '2021-09-20', 'image5.jpg');

-- Consultar todos los datos de la tabla Album
SELECT * FROM Album;

-- Consultar los álbumes de un artista específico (por ejemplo, ArtistId = 1)
SELECT * 
FROM Album
WHERE ArtistId = 1;

-- Consultar los álbumes ordenados por fecha de lanzamiento (más antiguos primero)
SELECT * 
FROM Album
ORDER BY ReleaseDate ASC;

-- Consultar los nombres y las imágenes de los álbumes sin duplicados
SELECT DISTINCT Name, Image 
FROM Album;

-- Consultar los álbumes de un artista junto con el nombre del artista
SELECT a.Name AS AlbumName, ar.Nombre AS ArtistName, a.ReleaseDate, a.Image
FROM Album a
JOIN Artista ar ON a.ArtistId = ar.CantanteId;

-- Consultar el número total de álbumes por artista
SELECT ar.Nombre AS ArtistName, COUNT(a.AlbumId) AS TotalAlbums
FROM Album a
JOIN Artista ar ON a.ArtistId = ar.CantanteId
GROUP BY ar.Nombre;

-- Consultar los artistas con más de 1 álbum
SELECT ar.Nombre AS ArtistName, COUNT(a.AlbumId) AS TotalAlbums
FROM Album a
JOIN Artista ar ON a.ArtistId = ar.CantanteId
GROUP BY ar.Nombre
HAVING COUNT(a.AlbumId) > 1;

-- Consultar los álbumes lanzados en un rango de fechas
SELECT * 
FROM Album
WHERE ReleaseDate BETWEEN '2020-01-01' AND '2022-12-31';

-- Consultar los álbumes ordenados por nombre del álbum
SELECT * 
FROM Album
ORDER BY Name ASC;

-- Consultar los artistas con el número total de oyentes mensuales y el total de álbumes
SELECT ar.Nombre AS ArtistName, ar.OyentesMensuales, COUNT(a.AlbumId) AS TotalAlbums
FROM Artista ar
JOIN Album a ON a.ArtistId = ar.CantanteId
GROUP BY ar.Nombre, ar.OyentesMensuales
ORDER BY ar.OyentesMensuales DESC;

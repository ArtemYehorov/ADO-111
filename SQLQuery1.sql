SELECT d.Name, SUM(s.Count)
FROM Sales s JOIN Managers m 
ON s.IdManager = m.Id 
JOIN Products p ON s.IdProduct = p.Id 
JOIN Departments d ON m.IdMainDep = d.Id 
WHERE CAST(s.Moment AS DATE) = '2023.02.18'
GROUP BY d.Name 
Order by Sum(s.count) desc



SELECT Name
FROM Departments
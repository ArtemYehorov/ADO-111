SELECT m.Surname, d.Name, ds.Name
From Managers m LEFT JOIN Departments d on m.IdMainDep = d.Id
LEFT JOIN Departments ds on m.IdSecDep = ds.Id

-- 1.	Написать запрос, который возвращает интервалы для одинаковых Id

SELECT dt1.ID, dt1.DT AS sd, dt2.DT AS ed FROM dates AS dt1
INNER JOIN dates AS dt2
ON dt1.ID = dt2.ID
WHERE dt1.dt < dt2.Dt AND 
	(
     SELECT COUNT(*) FROM dates AS dt3
     WHERE 
     	dt1.dt < dt3.dt AND
     	dt3.dt < dt2.dt AND
     	dt1.Id = dt3.Id
    ) = 0

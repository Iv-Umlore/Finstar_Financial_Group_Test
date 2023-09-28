/*
Clients - клиенты
(
	Id bigint, -- Id клиента
	ClientName nvarchar(200) -- Наименование клиента
);
ClientContacts - контакты клиентов
(
	Id bigint, -- Id контакта
	ClientId bigint, -- Id клиента
	ContactType nvarchar(255), -- тип контакта
	ContactValue nvarchar(255) --  значение контакта
);
*/

-- 1.	Написать запрос, который возвращает наименование клиентов и кол-во контактов клиентов
SELECT clt.ClientName, count(contact.Id) as CNT 
	FROM Clients as clt
	LEFT JOIN ClientContacts as contact
	ON clt.Id = contact.ClientId
	GROUP BY clt.ClientName
	ORDER By clt.Id

-- 2.	Написать запрос, который возвращает список клиентов, у которых есть более 2 контактов
SELECT subReq.ClientName, subReq.CNT 
FROM (
	SELECT clt.Id, clt.ClientName, count(contact.Id) AS CNT 
		FROM Clients AS clt
		LEFT JOIN ClientContacts AS contact
		ON clt.Id = contact.ClientId
		GROUP BY clt.ClientName
) AS subReq
WHERE subReq.CNT > 2
ORDER BY subReq.Id


-- Fetch product name, price and category name.
SELECT ProductName, CategoryName, UnitPrice FROM Products
JOIN Categories ON Products.CategoryID = Categories.CategoryID
ORDER BY CategoryName, ProductName

-- Sort all customers by amount of orders descending.
SELECT CompanyName, COUNT(Orders.OrderID) AS OrderCount FROM Customers
JOIN Orders ON Orders.CustomerID = Customers.CustomerID
GROUP BY CompanyName
ORDER BY OrderCount DESC

-- Fetch all employees and the territories they are responsible for.
SELECT FirstName + ' ' + LastName AS EmployeeName, TerritoryDescription as Territory FROM Employees
JOIN EmployeeTerritories ON Employees.EmployeeID = EmployeeTerritories.EmployeeID
JOIN Territories ON EmployeeTerritories.TerritoryID = Territories.TerritoryID
ORDER BY LastName, FirstName, TerritoryDescription


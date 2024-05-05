How to Run this project local machine
1. System Requirements
   1.1 Visual Studio 2022 or above.
   1.2 SQL Server 2017 or above.
2. Application Configuration
   2.1 Change SQL Connection String which locate in Assignment.API->appsettings.json
   2.2 Run this 'update-database'  command in visual studio package console manager.
   2.3 Run Procedure.sql on your sql server which locate in Assignment.DataAccess->SQLScript->Procedure.sql
   2.4 Go to Assignment.WebApp->appsetting.json and change port number of base url.
   2.5 Rebuild all project and run Assignment.WebApp and Assignment.API.
   2.6 Now you register once user to asscess the Assignment.WebApp go to swagger and open register endpoints and registed.
   2.7 After register you now to access WebApp.
3. You can also change token expier time and session expier time. Token expier time locate in Assignment.API->Controllers->AccountController and goto line number 106. Session expire locate in Assignment.WebApp->Program.cs and goto line 9.
4. If you have any issues let me know
   

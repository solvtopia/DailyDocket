<% 
    'declare the variables 
    Dim Connection
    Dim ConnString
    Dim Recordset
    Dim SQL
    Dim Attorneys
    Dim FileRecords
    Dim MonthlyCharge
    Dim ClientCharge

    Attorneys = 0
    FileRecords = 0
    MonthlyCharge = 0
    ClientCharge = 0

    'define the connection string, specify database driver
    ConnString="DRIVER={SQL Server};SERVER=mssql.solvtopia.com;UID=solvt_master;PWD=9dR00g326d;DATABASE=solvtopia_DailyDocket;"

'    SQL = "SELECT 'Attorneys' AS [Type], Count([ID]) AS [TotalCount] FROM [Attorneys] UNION SELECT 'FileRecords' AS [Type], Count([ID]) AS [TotalCount] FROM [FileRecords];"
'
'    'create an instance of the ADO connection and recordset objects
'    Set Connection = Server.CreateObject("ADODB.Connection")
'    Set Recordset = Server.CreateObject("ADODB.Recordset")
'
'    'Open the connection to the database
'    Connection.Open ConnString
'
'    'Open the recordset object executing the SQL statement and return records 
'    Recordset.Open SQL,Connection
'
'    'first of all determine whether there are any records 
'    If Recordset.EOF Then 
'        Response.Write("No records returned.") 
'    Else 
'        'if there are records then loop through the fields 
'        Do While NOT Recordset.Eof   
'            If Recordset("Type") = "Attorneys" Then
'                Attorneys = Recordset("TotalCount")
'            ElseIf Recordset("Type") = "FileRecords" Then
'                FileRecords = Recordset("TotalCount")
'            End If
'            Recordset.MoveNext     
'        Loop
'    End If
'
'    'close the connection and recordset objects to free up resources
'    Recordset.Close
'    Connection.Close

    SQL = "EXEC [procPricing] 0, 1;"

    'create an instance of the ADO connection and recordset objects
    Set Connection = Server.CreateObject("ADODB.Connection")
    Set Recordset = Server.CreateObject("ADODB.Recordset")

    'Open the connection to the database
    Connection.Open ConnString

    'Open the recordset object executing the SQL statement and return records 
    Recordset.Open SQL,Connection

    'first of all determine whether there are any records 
    If Recordset.EOF Then 
        Response.Write("No records returned.") 
    Else 
        'if there are records then loop through the fields 
        Do While NOT Recordset.Eof   
            MonthlyCharge = Recordset("UserAmount")
            Recordset.MoveNext     
        Loop
    End If

    'close the connection and recordset objects to free up resources
    Recordset.Close
    Connection.Close

    Set Recordset=nothing
    Set Connection=nothing

    SQL = "EXEC [procPricing] 0, 2;"

    'create an instance of the ADO connection and recordset objects
    Set Connection = Server.CreateObject("ADODB.Connection")
    Set Recordset = Server.CreateObject("ADODB.Recordset")

    'Open the connection to the database
    Connection.Open ConnString

    'Open the recordset object executing the SQL statement and return records 
    Recordset.Open SQL,Connection

    'first of all determine whether there are any records 
    If Recordset.EOF Then 
        Response.Write("No records returned.") 
    Else 
        'if there are records then loop through the fields 
        Do While NOT Recordset.Eof   
            ClientCharge = Recordset("UserAmount")
            Recordset.MoveNext     
        Loop
    End If

    'close the connection and recordset objects to free up resources
    Recordset.Close
    Connection.Close

    Set Recordset=nothing
    Set Connection=nothing

    Response.Write Attorneys & "|" & FileRecords & "|" & MonthlyCharge & "|" & ClientCharge
%>
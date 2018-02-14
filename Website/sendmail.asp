<%
    Function URLDecode(ByVal What)
        'URL decode Function
        '2001 Antonin Foller, PSTRUH Software, http://www.motobit.com
        Dim Pos, pPos

        'replace + To Space
        What = Replace(What, "+", " ")

        on error resume Next
        Dim Stream: Set Stream = CreateObject("ADODB.Stream")
        If err = 0 Then 'URLDecode using ADODB.Stream, If possible
            on error goto 0
            Stream.Type = 2 'String
            Stream.Open

            'replace all %XX To character
            Pos = InStr(1, What, "%")
            pPos = 1
            Do While Pos > 0
                Stream.WriteText Mid(What, pPos, Pos - pPos) + _
                Chr(CLng("&H" & Mid(What, Pos + 1, 2)))
                pPos = Pos + 3
                Pos = InStr(pPos, What, "%")
            Loop
            Stream.WriteText Mid(What, pPos)

            'Read the text stream
            Stream.Position = 0
            URLDecode = Stream.ReadText

            'Free resources
            Stream.Close
        Else 'URL decode using string concentation
            on error goto 0
            'UfUf, this is a little slow method. 
            'Do Not use it For data length over 100k
            Pos = InStr(1, What, "%")
            Do While Pos>0 
                What = Left(What, Pos-1) + _
                Chr(Clng("&H" & Mid(What, Pos+1, 2))) + _
                Mid(What, Pos+3)
                Pos = InStr(Pos+1, What, "%")
            Loop
            URLDecode = What
        End If
    End Function


    Dim name
    name = URLDecode(Request.QueryString("name"))
    Dim email
    email = URLDecode(Request.QueryString("email"))
    Dim message
    message = URLDecode(Request.QueryString("msg"))
    Dim subject
    subject = URLDecode(Request.QueryString("subject"))
    Dim body
    body = "A new contact request has been received from DailyCourtDocket.com ...<br/><br/>Name: " & name & "<br/>Email: " & email & "<br/>Message: " & message

    if email <> "" and message <> "" then
        Set Mail = CreateObject("CDO.Message")

        'This section provides the configuration information for the remote SMTP server.

        'Send the message using the network (SMTP over the network).
        Mail.Configuration.Fields.Item ("http://schemas.microsoft.com/cdo/configuration/sendusing") = 2 

        Mail.Configuration.Fields.Item ("http://schemas.microsoft.com/cdo/configuration/smtpserver") ="ml01.anaxanet.com"
        Mail.Configuration.Fields.Item ("http://schemas.microsoft.com/cdo/configuration/smtpserverport") = 25

        'Use SSL for the connection (True or False)
        Mail.Configuration.Fields.Item ("http://schemas.microsoft.com/cdo/configuration/smtpusessl") = False 

        Mail.Configuration.Fields.Item ("http://schemas.microsoft.com/cdo/configuration/smtpconnectiontimeout") = 60

        'If your server requires outgoing authentication, uncomment the lines below and use a valid email address and password.
        'Basic (clear-text) authentication
        'Mail.Configuration.Fields.Item ("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate") = 1 
        'Your UserID on the SMTP server
        'Mail.Configuration.Fields.Item ("http://schemas.microsoft.com/cdo/configuration/sendusername") ="sales@utilitywizards.com"
        'Mail.Configuration.Fields.Item ("http://schemas.microsoft.com/cdo/configuration/sendpassword") ="9dR00g326d!"

        Mail.Configuration.Fields.Update

        'End of remote SMTP server configuration section

        Mail.Subject=subject
        Mail.From="sales@utilitywizards.com"
        Mail.To="james@solvtopia.com"
        Mail.HTMLBody=body

        Mail.Send
        Set Mail = Nothing
    end if

    response.Write("Subject: " & subject & "<br/>")
    response.Write("Name: " & name & "<br/>")
    response.Write("Email: " & email & "<br/>")
    response.Write("Message: " & message & "<br/>")

    response.Redirect("index.html")
%>
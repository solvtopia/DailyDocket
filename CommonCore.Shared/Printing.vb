Imports System.IO
Imports System.Text
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.pdf.parser

Public Class Printing
    Public Shared Function HtmlToPdf(ByVal url As Uri, ByVal dest As String) As String
        Dim htmlText As String = Common.ScrapeUrl(url.ToString, Enums.ScrapeType.ReturnAll)
        Return HtmlToPdf(htmlText, dest)
    End Function
    Public Shared Function HtmlToPdf(ByVal htmlText As String, ByVal dest As String) As String
        Dim retVal As String = ""

        Dim document As New Document()

        Try
            'Create a byte array that will eventually hold our final PDF
            Dim bytes As [Byte]()

            Using ms = New MemoryStream()
                Using doc = New Document()
                    Using writer = PdfWriter.GetInstance(doc, ms)
                        doc.Open()
                        Using srHtml = New StringReader(htmlText)
                            iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, srHtml)
                        End Using

                        doc.Close()
                    End Using
                End Using
                bytes = ms.ToArray()
            End Using

            System.IO.File.WriteAllBytes(dest, bytes)

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
            retVal = ex.ToString
        End Try

        Return retVal
    End Function

    Public Shared Function PdfToText(fileName As String) As String
        Dim text As New StringBuilder()

        Try
            If File.Exists(fileName) Then
                Dim pdfReader As New PdfReader(fileName)

                For page As Integer = 1 To pdfReader.NumberOfPages
                    Dim strategy As ITextExtractionStrategy = New SimpleTextExtractionStrategy()
                    Dim currentText As String = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy)

                    currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.[Default], Encoding.UTF8, Encoding.[Default].GetBytes(currentText)))
                    text.Append(currentText)
                Next

                pdfReader.Close()
            End If

        Catch ex As Exception
        End Try

        Return text.ToString()
    End Function
End Class

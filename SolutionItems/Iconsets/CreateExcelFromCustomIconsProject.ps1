function CreateSvg([System.Object]$icon)
{
   [System.XML.XMLDocument]$oXMLDocument=New-Object System.XML.XMLDocument
   [System.XML.XMLElement]$oXMLRoot=$oXMLDocument.CreateElement("svg")
   $oXMLDocument.appendChild($oXMLRoot) | Out-Null

   $oXMLRoot.SetAttribute("xmlns","http://www.w3.org/2000/svg")
   $oXMLRoot.SetAttribute("height","1000")
   $oXMLRoot.SetAttribute("width","1000")

   $icon.paths | % {$i = 0}{ 
      [System.XML.XMLElement]$oXMLPath=$oXMLDocument.CreateElement("path")
      [System.XML.XMLElement]$oXMLSystem=$oXMLRoot.appendChild($oXMLPath) | Out-Null

      $path = $icon.paths[$i]
      $style = ""

      If ([bool]($icon.PSObject.Properties.name -match "attrs")) {
          $style = "fill:{0}" -f $icon.attrs[$i].fill
      }

      $oXMLPath.SetAttribute("d",$path)

       $oXMLPath.SetAttribute("style",$style)
      $i++
   }

   return $oXMLDocument.OuterXml -replace '"',"'"

}


$excel = New-Object -ComObject excel.application
$excel.visible = $false

$workbook = $excel.Workbooks.Add()

$worksheet = $workbook.Worksheets.Item(1)
$worksheet.Name = 'Icons'

$row = 1

#create the column headers
$worksheet.Cells.Item($row,1) = 'name'
$worksheet.Cells.Item($row,2) = 'svg'

$json = Get-Content .\Mavim-Tree-Icons-v0.2.json| ConvertFrom-Json

$json.iconSets.icons | ForEach-Object { 
   $row++

   $worksheet.Cells.Item($row,1) = $_.tags[0] 
   $worksheet.Cells.Item($row,2) = (CreateSvg -icon $_)
}


$usedRange = $worksheet.UsedRange
$usedRange.EntireColumn.AutoFit() | Out-Null

#$range = $worksheet.Range("A1", "B$row")
#$range.Name = "Icons"

$workbook.SaveAs("C:\Users\rno\Desktop\test.xlsx")
$excel.Quit()


Write-Host("To be able to use the Excel file in PowerApps we need to add all the rows to a named table.")
Write-Host("I do not know how to do this in PowerShell, so we need to do this by hand.")
Write-Host("Open the generated Excel file and select all the cells with value")
Write-Host("Then go to the Insert tab and click on the Table item in the ribbon")
Write-Host("Then in the Table Design tab we need to change the Table Name to icons")


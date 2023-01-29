$location = Get-Location
$writer = New-Object System.IO.StreamWriter(Join-Path $location "../docs/binary-specification.md")

Add-Type -LiteralPath (Join-Path $location "../src/Oligopoly/bin/Release/net6.0/Oligopoly.dll")
Add-Type -LiteralPath (Join-Path $location "../src/Oligopoly.Writers/bin/Release/net6.0/Oligopoly.Writers.dll")

function Write-Specification {
    param (
        $model
    )

    $writer.WriteLine($model.GetType().Name)

    $htmlFragmentWriter = New-Object Oligopoly.Writers.HtmlFragmentWriter ($writer, $true)

    $htmlFragmentWriter.Write($model)
    $htmlFragmentWriter.Dispose()
    $writer.WriteLine()
    $writer.WriteLine()
}

$writer.WriteLine("# Binary specification")
$writer.WriteLine("## Description")
$writer.WriteLine("This is the specification for the fast, lightweight binary serialization format used to store _machine-readable_ Oligopoly data files. This specialized, raw format allows a high degree of flexibility while avoiding the serialization and storage overhead of structured data. It is both minimalistic and portable, although it requires more code, testing, and maintenance and is not generalizable.")

Write-Specification (New-Object Oligopoly.Board)
Write-Specification (New-Object Oligopoly.Group)
Write-Specification (New-Object Oligopoly.Squares.Square)
Write-Specification (New-Object Oligopoly.Squares.StartSquare)
Write-Specification (New-Object Oligopoly.Squares.StreetSquare)

$writer.Dispose()

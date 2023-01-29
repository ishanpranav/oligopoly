$location = Get-Location
$jsonPath = Join-Path $location "../data/board.json"
$jsonContent = Get-Content -Raw $jsonPath
$source = $jsonContent | ConvertFrom-Json

Add-Type -LiteralPath (Join-Path $location "../src/Oligopoly/bin/Release/net6.0/Oligopoly.dll")

$squares = New-Object Oligopoly.Squares.Square[] ($source.squares.Count)

function New-Square {
    param (
        $square
    )

    switch ($square.type) {
        "Start" { 
            return [Oligopoly.Squares.Square]::Start
        }

        "Street" {
            return New-Object Oligopoly.Squares.StreetSquare ($square.name, $square.cost, [int[]] $square.rents)
        }

        "Card" {
            return New-Object Oligopoly.Squares.CardSquare
        }
        
        "Tax" {
            return New-Object Oligopoly.Squares.TaxSquare ($square.name, $square.cost)
        }

        "Railroad" {
            return New-Object Oligopoly.Squares.RailroadSquare ($square.name)
        }

        Default {
            throw "Square type $($square.type) is out of range."
        }
    }
}

for ($i = 0; $i -lt $squares.Count; $i++) {
    $squares[$i] = New-Square $source.squares[$i]
}

$groups = [System.Array]::Empty[Oligopoly.Group]()
$board = New-Object Oligopoly.Board ($source.railroadCost, $squares, [int[]] $source.railroadRents, $groups)
$datPath = Join-Path $location "../data/board.dat"
$output = [System.IO.File]::Create($datPath)
$writer = (New-Object Oligopoly.Writers.BinaryWriter ($output))

$writer.WriteVersion()
$writer.Write($board)
$writer.Dispose()
$output.Dispose()

$jsonLength = 0

foreach ($symbol in [char[]]$jsonContent) {
    if ([char]::IsWhiteSpace($symbol)) {
        continue;
    }

    $jsonLength++
}

$datLength = $(((Get-Item $datPath).length))

Write-Output "JavaScript Object Notation`t(*.json):`t$jsonLength bytes"
Write-Output "Binary Data`t`t`t(*.dat):`t$datLength bytes"
Write-Output $("Ratio:`t`t`t`t`t`t{0:n2} : 1" -f ($datLength / $jsonLength))
Write-Output $("Multiplier:`t`t`t`t`t{0:n2}x" -f $($jsonLength / $datLength))

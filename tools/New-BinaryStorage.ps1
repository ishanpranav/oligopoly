$location = Get-Location
$jsonPath = Join-Path $location "../data/board.json"
$jsonContent = Get-Content -Raw $jsonPath

Test-Json $jsonContent -SchemaFile (Join-Path $location '../docs/board-schema.json')

$source = $jsonContent | ConvertFrom-Json

Add-Type -LiteralPath (Join-Path $location "../src/Oligopoly/bin/Release/net6.0/Oligopoly.dll")

$squares = New-Object Oligopoly.Squares.Square[] ($source.squares.Count)
$utilityRents = [int[]] $source.utilityRents
$railroadRents = [int[]] $source.railroadRents

function New-Square {
    param (
        $square
    )

    switch ($square.type) {
        "None" {
            return [Oligopoly.Squares.Square]::Empty
        }
        
        "Jail" {
            return [Oligopoly.Squares.Square]::Jail
        }

        "Police" {
            return [Oligopoly.Squares.Square]::Police
        }
        
        "Card" {
            return New-Object Oligopoly.Squares.CardSquare ($square.deck)
        }

        "Utility" {
            return New-Object Oligopoly.Squares.UtilitySquare ($square.name, $source.utilityCost, $utilityRents)
        }

        "Railroad" {
            return New-Object Oligopoly.Squares.RailroadSquare ($square.name, $source.railroadCost, $railroadRents)
        }
        
        "Street" {
            return New-Object Oligopoly.Squares.StreetSquare ($square.name, $square.cost, [int[]] $square.rents)
        }

        "Tax" {
            return New-Object Oligopoly.Squares.TaxSquare ($square.name, $square.cost)
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
$datPath = Join-Path $location "../data/board.dat"
$output = [System.IO.File]::Create($datPath)
$writer = New-Object Oligopoly.Writers.BinaryWriter ($output)

$writer.Write((New-Object Oligopoly.Board ((New-Object Oligopoly.BoardSettings ($source.maxImprovements)), $squares, $groups)))
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
Write-Output $("Compression Ratio:`t`t`t`t{0:n2} : 1" -f ($jsonLength / $datLength))
Write-Output $("Space Saving:`t`t`t`t`t{0:p2}" -f $(1 - ($datLength / $jsonLength)))

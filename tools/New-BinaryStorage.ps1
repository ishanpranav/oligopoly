$location = Get-Location
$jsonPath = Join-Path $location "../data/board.json"
$jsonContent = Get-Content -Raw $jsonPath

Test-Json $jsonContent -SchemaFile (Join-Path $location '../docs/board-schema.json')

$source = $jsonContent | ConvertFrom-Json

Add-Type -LiteralPath (Join-Path $location "../src/Oligopoly/bin/Release/net6.0/Oligopoly.dll")

$squares = New-Object Oligopoly.Squares.Square[] ($source.squares.Count)
$groups = New-Object Oligopoly.Group[] ($source.groups.Count + 2)
$utilityRents = [int[]] $source.utilityInfo.rents
$railroadRents = [int[]] $source.railroadInfo.rents
$groups[0] = New-Object Oligopoly.Group (0, $source.utilityInfo.name, $source.utilityInfo.cost)
$groups[1] = New-Object Oligopoly.Group (1, $source.railroadInfo.name, $source.railroadInfo.cost)

for ($i = 0; $i -lt $source.groups.Count; $i++) {
    $id = $i + 2;
    $group = $source.groups[$i]
    $groups[$id] = New-Object Oligopoly.Group ($id, $group.name, $group.improvementCost)
}

function New-Square {
    param (
        $square
    )

    switch ($square.type) {
        "none" {
            return New-Object Oligopoly.Squares.EmptySquare ($square.name)
        }
        
        "jail" {
            return New-Object Oligopoly.Squares.JailSquare ($square.name)
        }

        "police" {
            return New-Object Oligopoly.Squares.PoliceSquare ($square.name)
        }
        
        "card" {
            return New-Object Oligopoly.Squares.CardSquare ($square.deck)
        }

        "utility" {
            return New-Object Oligopoly.Squares.UtilitySquare ($square.name, $utilityRents, $groups[0])
        }

        "railroad" {
            return New-Object Oligopoly.Squares.RailroadSquare ($square.name, $railroadRents, $groups[1])
        }
        
        "street" {
            return New-Object Oligopoly.Squares.StreetSquare ($square.name, [int[]] $square.rents, $groups[[int] $square.group + 1], $square.cost)
        }

        "tax" {
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

$datPath = Join-Path $location "../data/board.dat"
$output = [System.IO.File]::Create($datPath)
$writer = New-Object Oligopoly.Writers.BinaryWriter ($output)

$writer.Write((New-Object Oligopoly.Game ($source.maxImprovements, $squares, $groups)))
$writer.Dispose()
$output.Dispose()

$jsonLength = 0

foreach ($symbol in [char[]] $jsonContent) {
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

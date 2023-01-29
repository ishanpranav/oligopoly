$location = Get-Location
$source = Get-Content -Raw (Join-Path $location "../data/board.json") | ConvertFrom-Json

Add-Type -LiteralPath (Join-Path $location "../src/Oligopoly/bin/Release/net6.0/Oligopoly.dll")

$squares = New-Object Oligopoly.Squares.Square[] ($source.squares.Count)

function New-Square {
    param (
        $square
    )

    $type = [Oligopoly.Squares.SquareType] $square.type

    switch ($type) {
        Start { 
            return New-Object Oligopoly.Squares.StartSquare
        }

        Street {
            return New-Object Oligopoly.Squares.StreetSquare ($square.name, $square.cost, $square.improvementCost, [int[]] $square.rents)
        }

        Default {
            throw "Square type '$type' is out of range."
        }
    }
}

for ($i = 0; $i -lt $squares.Count; $i++) {
    $squares[$i] = New-Square $source.squares[$i]
}

$groups = [System.Array]::Empty[Oligopoly.Group]()
$board = New-Object Oligopoly.Board ($squares, $groups)
$output = [System.IO.File]::Create((Join-Path $location "../data/board.dat"))
$writer = (New-Object Oligopoly.Writers.BinaryWriter ($output))

$writer.WriteVersion()
$writer.Write($board)
$writer.Dispose()
$output.Dispose()

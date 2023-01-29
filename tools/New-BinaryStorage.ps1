$location = Get-Location
$source = Get-Content -Raw (Join-Path $location "../data/board.json") | ConvertFrom-Json

Add-Type -LiteralPath (Join-Path $location "../src/Oligopoly/bin/Release/net6.0/Oligopoly.dll")

$squares = New-Object Oligopoly.Squares.Square[] ($source.squares.Count)

function New-Square {
    param (
        $square
    )

    switch ([Oligopoly.Squares.SquareType]$square.type) {
        Oligopoly.Squares.SquareType.Start { 
            return New-Object Oligopoly.Squares.StartSquare
        }

        Default {
            return New-Object Oligopoly.Squares.StartSquare
        }
    }
}

for ($i = 0; $i -lt $squares.Count; $i++) {
    $squares[$i] = New-Square $source.squares[$i]

    Write-Output $squares[$i]
}

$groups = [System.Array]::Empty[Oligopoly.Group]()
$board = New-Object Oligopoly.Board ($squares, $groups)
$output = [System.IO.File]::Create((Join-Path $location "../data/board.dat"))

(New-Object Oligopoly.Writers.BinaryWriter ($output)).Write($board)

$output.Dispose()

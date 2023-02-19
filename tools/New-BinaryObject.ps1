Add-Type -Path "../src/Oligopoly/bin/Debug/net7.0/Oligopoly.dll"
Add-Type -Path "../src/Oligopoly/bin/Debug/net7.0/MessagePack.Annotations.dll"

$paths = @{
    "../data/board.json" = [Oligopoly.Board]
}
$jsonSerializerOptions = New-Object System.Text.Json.JsonSerializerOptions
$jsonSerializerOptions.PropertyNameCaseInsensitive = $true

[Environment]::CurrentDirectory = Get-Location

foreach ($path in $paths.Keys) {
    $path
    try {
        $inputStream = [System.IO.File]::OpenRead($path)
        
        $board =  [System.Text.Json.JsonSerializer]::Deserialize($input, $paths[$path], $jsonSerializerOptions)
    }
    finally {
        $inputStream.Close()
    }

    try {
        $outputStream = [System.IO.File]::Create([System.IO.Path]::ChangeExtension($path, "bin"))

        [Oligopoly.OligopolySerializer]::Write($fileStream, $board)
    }
    finally {
        $outputStream.Close()
    }
}

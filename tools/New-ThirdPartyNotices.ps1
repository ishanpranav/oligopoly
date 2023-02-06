$path = "../THIRD-PARTY-NOTICES.md"

Remove-Item -Path $path

function Out-ThirdPartyNotices {
    param ($value)

    Out-File -FilePath $path -Append -InputObject $value
}

Out-ThirdPartyNotices "# Third-Party Notices

Oligopoly by Ishan Pranav

Copyright (c) 2023 Ishan Pranav

Oligopoly was created for research purposes and is inspired by the classic
MONOPOLY board game. It takes heavy inspiration from open-source MONOPOLY
implementations and is not intended for commercial use. This is free software.
For more details, please see the [license](LICENSE.txt).

This software uses third-party libraries or other resources that may be
distributed under licenses different than the software.

The attached notices are provided for informational purposes only.

________________________________________________________________________________"

$json = Get-Content licenses.json -Raw | ConvertFrom-Json -AsHashtable

foreach ($dependency in $json.dependencies | Sort-Object -Property "index") {
    Out-ThirdPartyNotices ""
    Out-ThirdPartyNotices ("**" + $dependency.title + "**")
    Out-ThirdPartyNotices ("- Source: " + $dependency.repositoryUrl)

    if ($null -ne $dependency.author) {
        Out-ThirdPartyNotices ("- Author: " + $dependency.author)
    }

    $license = $json.licenses[$dependency.license]

    Out-ThirdPartyNotices ("- License: [" + $license.title + "](#" + $dependency.license + ")")
}

foreach ($key in $json.licenses.Keys | Sort-Object) {
    $license = $json.licenses[$key]

    Out-ThirdPartyNotices ""
    Out-ThirdPartyNotices "________________________________________________________________________________"
    Out-ThirdPartyNotices ""
    Out-ThirdPartyNotices ("<em><a id='" + $key + "'>" +  $license.title + "</a></em>")
    Out-ThirdPartyNotices ""
    Out-ThirdPartyNotices "``````"
    Out-ThirdPartyNotices $json.licenses[$key].text
    Out-ThirdPartyNotices "``````"
}

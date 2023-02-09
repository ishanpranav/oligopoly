$path = "../THIRD-PARTY-NOTICES.md"

if ((Test-Path $path) -eq $true) {
    Remove-Item -Path $path
}

function Out-ThirdPartyNotices {
    param ($value)

    Out-File -FilePath $path -Append -InputObject $value
}

function Out-Dependencies {
    param ($dependencies)

    foreach ($dependency in $dependencies | Sort-Object -Property "index") {
        Out-ThirdPartyNotices ""
        
        $out = "__" + $dependency.title + "__&emsp;"
        
        foreach ($format in $dependency.formats) {
            $out += "*" + $format + "*&ensp;"
        }
    
        Out-ThirdPartyNotices $out
    
        if ($null -ne $dependency.author) {
            Out-ThirdPartyNotices ("- Author: " + $dependency.author)
        }
    
        if ($null -ne $dependency.designer) {
            Out-ThirdPartyNotices ("- Designer: " + $dependency.designer)
        }
    
        if ($dependency.source.StartsWith("https://github.com/")) {
            Out-ThirdPartyNotices ("- Source: [" + $dependency.source.Substring(19) + "](" + $dependency.source + ")")
        }
        
        $license = $json.licenses[$dependency.license]
        $licenseTitleSeparatorIndex = $license.title.IndexOf(" - ")
    
        if ($licenseTitleSeparatorIndex -eq -1) {
            $licenseTitle = $license.title
        } else {
            $licenseTitle = $license.title.Substring(0, $licenseTitleSeparatorIndex)
        }
    
        Out-ThirdPartyNotices ("- License: [" + $licenseTitle + "](#" + $dependency.license + ')')
    }    

    Out-ThirdPartyNotices ""
}

$json = Get-Content "../docs/licenses.json" -Raw | ConvertFrom-Json -AsHashtable

Out-ThirdPartyNotices "Third-Party Notices
===================
Oligopoly by Ishan Pranav

Copyright (c) 2023 Ishan Pranav

Oligopoly was created for research purposes and is inspired by the classic
MONOPOLY board game. It takes heavy inspiration from open-source MONOPOLY
implementations and is not intended for commercial use. This is free software.
For more details, please see the [license](LICENSE.txt).

This software uses third-party libraries or other resources that may be
distributed under licenses different than the software.

The attached notices are provided for informational purposes only.

Dependencies
------------
This section contains notices for binary dependencies redistributed alongside
the application."
Out-Dependencies $json.dependencies
Out-ThirdPartyNotices "References
----------
This section contains references to parts of the source code based on or
inspired by third-party open-source software."
Out-Dependencies $json.references
Out-ThirdPartyNotices "Resources
---------
This section contains attributions for helpful resources used to assist in the
development of this software."
Out-Dependencies $json.tools
Out-ThirdPartyNotices "Licenses
--------
This section contains licenses provided by third-party software vendors."

foreach ($key in $json.licenses.Keys | Sort-Object) {
    $license = $json.licenses[$key]

    Out-ThirdPartyNotices ""
    Out-ThirdPartyNotices "________________________________________________________________________________"
    Out-ThirdPartyNotices ""
    Out-ThirdPartyNotices ("_<a id='" + $key + "'>")
    Out-ThirdPartyNotices $license.title
    Out-ThirdPartyNotices "</a>_"
    Out-ThirdPartyNotices ""
    Out-ThirdPartyNotices "``````"
    Out-ThirdPartyNotices $license.text
    Out-ThirdPartyNotices "``````"
}

$path = "../THIRD-PARTY-NOTICES.md"

if ((Test-Path $path) -eq $true) {
    Remove-Item -Path $path
}

function Out-ThirdPartyNotices {
    param ($value)

    Out-File -FilePath $path -Append -InputObject $value
}

function Convert-MarkdownString {
    param ($value)

    return $value.Replace('#', "\#")
}

$hashtable = @{}

function Out-Dependencies {
    param ($dependencies)

    foreach ($dependency in $dependencies) {
        if ($null -eq $dependency.index) {
            $dependency.index = $dependency.author
        }
    }

    foreach ($dependency in $dependencies | Sort-Object -Property "index") {
        Out-ThirdPartyNotices ""
        
        $out = "### " + (Convert-MarkdownString $dependency.title) + "&emsp;<sub><sup>"
        
        foreach ($format in $dependency.formats) {
            $out += '*' + (Convert-MarkdownString $format) + "*&ensp;"
        }
    
        Out-ThirdPartyNotices ($out + "</sup></sub>")
    
        if ($null -ne $dependency.author) {
            Out-ThirdPartyNotices ("- Author: " + $dependency.author)
        }
    
        if ($null -ne $dependency.designer) {
            Out-ThirdPartyNotices ("- Designer: " + $dependency.designer)
        }
    
        if ($null -ne $dependency.source) {
            $source = $dependency.source

            if ($source.StartsWith("https://github.com/")) {
                $source = $source.Substring(19)
            }

            if ($source.StartsWith("https://en.wikipedia.org/wiki/")) {
                $source = $source.Substring(30);
            }

            Out-ThirdPartyNotices ("- Source: [" + $source + "](" + $dependency.source + ")")
        }

        if ($null -ne $dependency.license) {
            $index = $dependency.license.IndexOf('_')

            if ($index -eq -1) {
                $key = $dependency.license
            }
            else {
                $key = $dependency.license.SubString(0, $index)
            }

            $license = $json.licenses[$key]

            if ($null -eq $dependency.copyright) {
                $dependency.copyright = ""
            }

            if (-Not ($hashtable.ContainsKey($dependency.license))) {
                $hashtable.Add($dependency.license, [string]::Format($license.text, $dependency.copyright))
            }

            Out-ThirdPartyNotices ("- License: [" + $license.title + "](#" + $dependency.license + ')')
        }

        if ($null -ne $dependency.resource) {
            Out-ThirdPartyNotices ("See [here](" + $dependency.resource + ") for the resource included in the repository.")
        }

        if ($null -ne $dependency.notices) {
            Out-ThirdPartyNotices ""
            Out-ThirdPartyNotices ("For more information about this software, please see its [third-party notices](" + $dependency.notices + ").")
        }
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

The attached notices are provided for informational purposes only. Please
create a new GitHub issue if a required notice is missing. 

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
This section contains attributions for helpful resources that assisted in the
development of this software."
Out-Dependencies $json.tools
Out-ThirdPartyNotices "Licenses
--------"

foreach ($key in $hashtable.Keys | Sort-Object) {
    $licenseText = $hashtable[$key]
    $index = $key.IndexOf('_')

    if ($index -eq -1) {
        $license = $key
    }
    else {
        $license = $key.Substring(0, $index)
    }

    Out-ThirdPartyNotices ""
    Out-ThirdPartyNotices ("_<a id='" + $key + "'>")
    Out-ThirdPartyNotices $json.licenses[$license].title
    Out-ThirdPartyNotices "</a>_"
    Out-ThirdPartyNotices ""
    Out-ThirdPartyNotices "``````"
    Out-ThirdPartyNotices $licenseText
    Out-ThirdPartyNotices "``````"
    Out-ThirdPartyNotices "________________________________________________________________________________"
}

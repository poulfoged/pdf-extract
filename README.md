![](https://raw.githubusercontent.com/poulfoged/pdf-extract/master/logo.png) &nbsp; ![](https://ci.appveyor.com/api/projects/status/prwp3j290469ntpb/branch/master?svg=true) &nbsp; ![](http://img.shields.io/nuget/v/pdf-extract.svg?style=flat)
#PDF Extract  

As part of integration-testing I needed to extract text from PDF's - all existing solutions was either too cumbersome or had a wierd API.

PDF Extract works by executing an external executable (Win64 only!) - but is fully self-contained and only exposes streams to the outside world.

Internally it uses [Xpdf][http://www.foolabs.com/xpdf] internally. 

## How to
To use extract text simply use provided extractor-class (here from a file):

```c#
using (var pdf = File.OpenRead("my.pdf"))
using (var extractor = new Extractor())
{
    var extractedText = extractor.ExtractToString();
}

```

Or extract from/to a stream

```c#
using (var extractor = new Extractor())
{
    using (var rawTextStream = extractor.ExtractText(pdfStream))
        /// ...
}

```
## Install

Simply add the Nuget package:

`PM> Install-Package elasticsearch-inside`

## Requirements

You'll need .NET Framework 4.5.1 or later to use the precompiled binaries.

## License

PDF Extract is licensed under the GNU General Pulbic License (GPL), version 2
or 3 similar to Xpdf.




![](https://raw.githubusercontent.com/poulfoged/pdf-extract/master/icon.png) &nbsp; ![](https://ci.appveyor.com/api/projects/status/72o2g3k11t5j8k5e?svg=true) &nbsp; ![](http://img.shields.io/nuget/v/pdf-extract.svg?style=flat)
#PDF Extract  

As part of integration-testing I needed to extract text from PDF's - all existing solutions was either too cumbersome or had a wierd API.

PDF Extract works by executing an external executable (Win64 only!) - but is fully self-contained and only exposes streams to the outside world.

Internally it uses [Xpdf](http://www.foolabs.com/xpdf). 

## How to
To extract text simply use provided extractor-class (here from a file):

```c#
using (var pdfStream = File.OpenRead("my.pdf"))
using (var extractor = new Extractor())
{
    var extractedText = extractor.ExtractToString(pdfStream);
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

`PM> Install-Package pdf-extract`

## Requirements

You'll need .NET Framework 4.5.1 or later on 64 bit Windows to use the precompiled binaries.

## License

PDF Extract is licensed under the GNU General Pulbic License (GPL), version 2
or 3 similar to Xpdf.




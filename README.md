## Cewe2pdf

This commandline program converts **CEWE FOTOWELT** `.mcfx` photobook project files to high quality `.pdf` documents.
It's still an incomplete version with lots of missing features, see the [Known Issues](#known-issues) section below, but for books containing images and text it gets the job done.

If you encounter a bug or the generated pdf contains errors please [report an Issue](https://github.com/stfnk/Cewe2pdf/issues) Describe the problem and attach the `cewe2pdf.log` file, located next to the `Cewe2pdf.exe`.
_This file may contain file- and foldernames from your computer. Open it in Notepad to review its content._

### `.mcfx` support, 2025
Recently Cewe updated the fileformat of the Designer Software - now ending in `.mcfx` instead of the old `.mcf`. 
From version 0.4.0 onwards, `Cewe2pdf` only supports these new files. For old, regular `.mcf`files, use previous releases.

#### Some findings on the new format
Opposed to the old version, the new format actually contains the image data itself, images are no longer stored in a separate directory. An mcfx file is basically just a sqlite3 database with a custom file extension. The database consists of 3 columns: `Data`, `Filename` and `LastModified`. For the pdf conversion, only Data and Filename are relevant. All image files, as well as the old xml-style mcf file are stored in this database. The image paths from the xml now point to Data entries in the database and can be accessed by name. To support the new fileformat, the old mcfParser was refactored to use the in-memory version of the mcf hierarchy, extracted from the database. To access the database, a new Mcfx class was introduce that wraps the sqlite commands to retrieve the required data for the conversion. In its initial state it supports extracting image files in various formats, as well as the mcf description.

## Installation
Head over to the [release](https://github.com/stfnk/Cewe2pdf/releases) section and download the latest build for your platform.</br>
**Note:** *Currently only Windows Binaries provided.*</br>

To run, this program requires the `.NET Core 3.1` runtime installed on your System. Get the appropriate version from:</br>
https://dotnet.microsoft.com/download/dotnet-core/3.1
</br>(`.NET Core Runtime 3.1.5` is probably enough, but any version should work.)

## Usage
This is a commandline only program, there is no graphical interface to interact with. [Detailed Instructions](#detailed-instructions-for-windows)

### I know what I'm doing:
Conversion with default settings, generates the pdf next to the mcfx file:

    Cewe2pdf <pathTo.mcfx>

List options:

    Cewe2pdf --help

### Detailed instructions for Windows
Extract all files from the downloaded `.zip` to any location on your computer, for example your Desktop. Press **WindowsKey**+**R** to open the _Run_ Dialog. Type `cmd` and hit **Enter**. A commandprompt should open up.
Navigate to the folder that contains the program, if you extracted the `.zip` to your Desktop, type:</br>

    cd Desktop\Cewe2pdf

and hit **Enter**. It should now look like this:</br>

    C:\Users\<username>\Desktop\Cewe2pdf>

now you need to get the path to your `.mcf` file. Navigate to it in Windows Explorer, Right click on the `.mcf`, click `Properties` and under the `Security` tab, copy the full file path.

Back in the commandline, type `Cewe2pdf.exe "` paste the filename with **Ctrl**+**V** and add another `"`. It should now look like this:
    
    Cewe2pdf.exe "C:\Users\username\Documents\MyPhotobook.mcf"

Now hit **Enter** and conversion should start. This may take several minutes.
Once finished, the `.pdf` is located right next to the `Cewe2pdf.exe`, in this case on your Desktop in the Cewe2pdf folder, named `MyPhotobook-converted.pdf`

### Known Issues
_Currently only the following features are supported!_
* Images
* Text Boxes
* Image Borders

This works for my usecase, please report missing elements [here](https://github.com/stfnk/Cewe2pdf/issues).</br>
Please also attach the `cewe2pdf.log` file, which is located next to the `Cewe2pdf.exe` _This file may contain file- and foldernames from your computer. Open it in Notepad to review its content._

## Development
This program is written in C# for the .NET Core 3.1 runtime and should build on any platform. It uses [iTextSharp 5](https://github.com/itext/itextsharp/) for pdf rendering, and `System.Drawing.Commons` for image loading.</br>
`mcfx`files are sqlite3 databases, queried using the `System.Data.SQLite` API.
`.mcf` files are just plain XML files, parsed using the C# native `System.Xml` API.</br>
Photobook backgrounds are stored as `.webp` images in the CEWE installation directory, loaded thorugh the excellent [WebP Wrapper](https://github.com/JosePineiro/WebP-wrapper).

## Releases

**Changelog**

    v0.4.0
    [added] support new .mcfx file format, old mcf files are not longer supported, use an older release for those
    [fixed] improve text handling to extract properly from the embedded html - introduced in a newer Cewe version
    [changed] cleaned up some internal design id handling, instead of resorting to a predefined resource list, scan installation directory on deman
 
    v0.3.0
    [added] new background system, loading color directly from .webp files
    [fixed] multiline text with same color, did not inherit color from previous lines
    
    v0.2.0
    [added] support for page numbers, inlcuding font, color & size
    [added] support for colored, bold, italic, underlined text
    [fixed] keep images resolution consistent

    v0.1.0 Initial Release.
    * images
    * image borders
    * background colors
    * basic text boxes

**Roadmap**
* more configuration options _some actually already exist, just not exposed to commandline_

## License
The Code in this repository is licensed under the MIT License, but note that `iTextSharp` uses the AGPL License.

    MIT License

    Copyright (C) 2025 Stefan Kreller

    Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

if (args.Length != 2) {
    Console.Error.WriteLine("usage: zip-dir <dir> <out.zip>");
    return 2;
}
string //
    source = args[0],
    destination = args[1];
List<string> files = [.. Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories).Order()];
Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(destination))!);
using FileStream output = new(destination, FileMode.Create);
using ZipArchive archive = new(output, ZipArchiveMode.Create);
foreach (string full in files) {
    string entry = Path.GetRelativePath(source, full).Replace('\\', '/');
    archive.CreateEntryFromFile(full, entry, CompressionLevel.Optimal);
}
return 0;

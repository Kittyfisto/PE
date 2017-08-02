# PE

[![Build status](https://ci.appveyor.com/api/projects/status/c3x146obbblqdf6l?svg=true)](https://ci.appveyor.com/project/Kittyfisto/pe)

A library to parse PE (portable executable) images.


# Example

    var header = PortableExecutable.ReadHeader("foo.dll");
    Console.WriteLine(header.FileHeader.Characteristics); // Prints IMAGE_FILE_EXECUTABLE_IMAGE

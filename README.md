# PE

A library to parse PE (portable executable) images.


# Example

    var header = PortableExecutable.ReadHeader("foo.dll");
    Console.WriteLine(header.FileHeader.Characteristics); // Prints IMAGE_FILE_EXECUTABLE_IMAGE, IMAGE_FILE_LARGE_ADDRESS_AWARE

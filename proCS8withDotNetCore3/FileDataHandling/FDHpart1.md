# File I/O and Object Serialization

## Exploring the System.IO Namespace

Key Members of the `System.IO` Namespace

| Nonabstract I/O Class types | Meaning in Life
| ---------------------------- | ----------------
| BinaryReader <br> BinaryWriter | These classes allow you to store and retrieve primitive data types (integers, Booleans, strings, and whatnot) as a binary value.
| BufferedStream | This class provides temporary storage for a stream of bytes that you can commit to storage at a later time.
| Directory <br> DirectoryInfo | This class provides detailed information regarding the drives that a given machine uses.
| File <br> FileInfo | You use these classes to manipulate a machine’s set of files.
| FileStream | This class gives you random file access (e.g., seeking capabilities) with data represented as a stream of bytes.
| FileSystemWatcher | This class allows you to monitor the modification of external files in a specified directory.
| MemoryStream | This class provides random access to streamed data stored in memory rather than in a physical file.
| Path | This class performs operations on System.String types that contain file or directory path information in a platform-neutral manner.
| StreamWrtier/Reader | You use these classes to store (and retrieve) textual information to (or from) a file. These types do not support random file access.
| StringWriter/Reader | Like the StreamReader/StreamWriter classes, these classes also work with textual information. However, the underlying storage is a string buffer rather than a physical file.

<br>

## The Directory(Info) and File(Info) Types

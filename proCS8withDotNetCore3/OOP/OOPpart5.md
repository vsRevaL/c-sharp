# Object-Oriented Programming with C#, Chapter 9

<br>

# Understanding Object Lifetime

## Summary

The garbage collector will run only when it is unable to acquire the necessary memory from the managed heap (or when the developer calls GC.Collect()).

When a collection does occur, you can rest assured that Microsoft’s collection algorithm has been optimized by the use of object generations, secondary threads for the purpose of object finalization, and a managed heap dedicated to hosting large objects.

The garbage collector using the System.GC class type. The only time you will really need to do so is when you are building `finalizable` or `disposable` class types that operate on unmanaged resources.
Recall that `finalizable` types are classes that have provided a destructor (effectively overriding the Finalize() method) to clean up unmanaged resources at the time of garbage collection.

Disposable objects, on the other hand, are classes (or non-ref structures) that implement the IDisposable interface, which should be called by the object user when it is finished using said objects.

The official “disposal” pattern blends both approaches.

There's a generic class named Lazy<>, you can use this class to delay the creation of an expensive (in terms of memory consumption) object until the caller actually requires it.
By doing so, you can help reduce the number of objects stored on the managed heap and also ensure expensive objects are created only when actually required by the caller.


<br>
<br>
<br>
<br>
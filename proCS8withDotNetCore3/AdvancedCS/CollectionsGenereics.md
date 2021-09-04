# Advanced C# Programming

<br>

# Collections and Generics

- The Basic Mechanics of Inheritance

<br>

## The Motivation for Collection Classes

```cs
using System.Collections.Generic
```

Unlike a simple C# array, collection classes are built 
to dynamically resize themselves on the fly as you insert or remove items. 
- Nongeneric collections (primarily found in the System.Collections namespace)
- Generic collections (primarily found in the System.Collections.Generic
namespace)

Useful Types of System.Collections:

| System.Collections Class | Meaning | Key Implemented Interfaces
| ------------------------ | ------- | ----------------
| ArrayList | a dynamically sized collection of objects listed in sequential order | IList, ICollection, IEnumerable, ICloneable
| BitArray | | ICollection, IEnumerable, ICloneable
| Hashtable | key-value pairs | IDictionary, ICollection, IEnumerable, IClonable
| Queue | std first-in, first-out collection
| SortedList | sorted key-value pairs | IDictionary, ICollection,IEnumerable, and ICloneable
| Stack | last-in, first-out stack providing push and pop | ICollection, IEnumerable and ICloneable

<br>

Key Interfaces Supported by Classes of System.Collections:

| System.Collections Interface | Meaning
| ---------------------------- | --------
| ICollection | Defines general characteristics (e.g., size, enumeration, and thread safety) for all nongeneric collection types
| ICloneable | Allows the implementing object to return a copy of itself to the caller
| IDictionary | ows a nongeneric collection object to represent its contents using key-value pair
| IEnumerable | Returns an object implementing the IEnumerator interface (see next table entry)
| IEnumerator | Enables foreach-style iteration of collection item
| IList | Provides behavior to add, remove, and index items in a sequential list of objects

<br>

## The Problems of Nongeneric Collections

- performance issuess
- safety type issues

in the other hand:

- Generics provide better performance because they do not result in boxing or unboxing penalties when storing value types.
- Generics are type-safe because they can contain only the type of type you specify.
- Generics greatly reduce the need to build custom collection types because you specify the “type of type” when creating the generic container.

<br>

## The Role of Generic Type Parameters

■ **Note** Only classes, structures, interfaces, and delegates can be written generically; enum types cannot.

<br>

## Working with the Stack<T> Class

[ in process.. ]

<br>
<br>
<br>
<br>
<br>
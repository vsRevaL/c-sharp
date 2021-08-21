# Core C# Programming Constructs, Part 1 - Chaper 3

## Working with String

### Strings Are Immutable

One of the interesting aspects of System.String is that after you assign a string object with its initial value, 
the character data cannot be changed. At first glance, this might seem like a flat-out lie, given that you are 
always reassigning strings to new values and because the System.String type defines a number of methods 
that appear to modify the character data in one way or another (such as uppercasing and lowercasing). 
However, if you look more closely at what is happening behind the scenes, you will notice the methods of the 
string type are, in fact, returning you a new string object in a modified format.

```cs
static void StringsAreImmutable()
{
 // Set initial string value.
 string s1 = "This is my string.";
 Console.WriteLine("s1 = {0}", s1);
 // Uppercase s1?
 string upperString = s1.ToUpper();
 Console.WriteLine("upperString = {0}", upperString);
 // Nope! s1 is in the same format!
 Console.WriteLine("s1 = {0}", s1);
}
```

---
```cs
s1 = This is my string.
upperString = THIS IS MY STRING.
s1 = This is my string.
```

---

The same law of immutability holds true when you use the C# assignment operator.

```cs
static void StringsAreImmutable2()
{
 string s2 = "My other string";
 s2 = "New string value";
}
```

Simply put, the ldstr opcode of the CIL loads a new string object on the managed 
heap. The previous string object that contained the value "My other string" will eventually be garbage 
collected.

### The System.Text.StringBuilder Type

What is unique about the StringBuilder is that when you call members of this type, you are directly 
modifying the object’s internal character data (making it more efficient), not obtaining a copy of the data in 
a modified format. When you create an instance of the StringBuilder, you can supply the object’s initial 
startup values via one of many constructors. 

If you append more characters than the specified limit, the StringBuilder object will copy its data into 
a new instance and grow the buffer by the specified limit.

<br>
<br>

---
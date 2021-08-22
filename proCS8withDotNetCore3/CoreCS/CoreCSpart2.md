# Core C# Programming Constructs, Part 2 -Chapter 4

## Summary

- Arrays
- Methods
- Method parameters
- enum
- Structure
- Value types, Reference Types,
- Nullabe types, Tuples

## Understanding C# Arrays

### Implicitly Typed Local Arrays 

Recall that the var keyword 
allows you to define a variable, whose underlying type is determined by the compiler. In a similar vein, the 
var keyword can be used to define implicitly typed local arrays. Using this technique, you can allocate a 
new array variable without specifying the type contained within the array itself (note you must use the new
keyword when using this approach).


```cs
static void DeclareImplicitArrays()
{
 Console.WriteLine("=> Implicit Array Initialization.");
 // a is really int[].
 var a = new[] { 1, 10, 100, 1000 };
 Console.WriteLine("a is a: {0}", a.ToString());
 // b is really double[].
 var b = new[] { 1, 1.5, 2, 2.5 };
 Console.WriteLine("b is a: {0}", b.ToString());
 // c is really string[].
 var c = new[] { "hello", null, "world" };
 Console.WriteLine("c is a: {0}", c.ToString());
 Console.WriteLine();
}

```

Of course, just as when you allocate an array using explicit C# syntax, the items in the array’s 
initialization list must be of the same underlying type (e.g., all ints, all strings, or all SportsCars). Unlike 
what you might be expecting, an implicitly typed local array does not default to System.Object; thus, the 
following generates a compile-time error:

```cs
// Error! Mixed types!
var d = new[] {1, "one", 2, "two", false}
```

### Defining an Array of Objects

```cs
static void ArrayOfObjects()
{
 Console.WriteLine("=> Array of Objects.");
 // An array of objects can be anything at all.
 object[] myObjects = new object[4];
 myObjects[0] = 10;
 myObjects[1] = false;
 myObjects[2] = new DateTime(1969, 3, 24);
 myObjects[3] = "Form & Void";
 foreach (object obj in myObjects)
 {
 // Print the type and value for each item in array.
 Console.WriteLine("Type: {0}, Value: {1}", obj.GetType(), obj);
 }
 Console.WriteLine();
}
```

```
=> Array of Objects.
Type: System.Int32, Value: 10
Type: System.Boolean, Value: False
Type: System.DateTime, Value: 3/24/1969 12:00:00 AM
Type: System.String, Value: Form & Void
```

<br><br>

## Methods

Methods are defined by an access modifier, return type (or 
void for no return type), and may or may not take parameters. A method that returns a value to the caller is 
commonly referred to as a function, while methods that do not return a value are commonly returned to as 
methods.

```cs
// Recall that static methods can be called directly
// without creating a class instance.
class Program
{
 // static returnType MethodName(parameter list) { /* Implementation */ }
 static int Add(int x, int y)
 {
 return x + y;
 }
}
```

### Expression-Bodied Members

```cs
static int Add(int x, int y) => x + y;
```

This is what is commonly referred to as syntactic sugar, meaning that the generated IL is no different. It’s 
just another way to write the method. Some find it easier to read, and others don’t, so the choice is yours (or 
your team’s) which style you prefer.

### Local Functions (New 7.0)

A new feature introduced in C# 7.0 is the ability to create methods within methods, referred to officially as 
local functions. A local function is a function declared inside another function.

```cs
static int AddWrapper(int x, int y)
{
    //Do some validation here
    return Add();
    int Add()
    {
    return x + y;
    }
}
```

### Static Local Functions (New 8.0)

An improvement to local functions that was introduced in C# 8 is the ability to declare a local function 
as static. In the previous example, the local Add() function was referencing the variables from the main 
function directly. This could cause unexpected side effects, since the local function can change the values of 
the variables.
To see this in action, create a new method call AddWrapperWithSideEffect(), as shown here:

```cs
static int AddWrapperWithSideEffect(int x, int y)
{
    //Do some validation here
    return Add();
    int Add()
    {
    x += 1;
    return x + y;
    }
}
```

Of course, this example is so simple, it probably wouldn’t happen in real code. To prevent this type 
of mistake, add the static modifier to the local function. This prevents the local function from accessing 
the main method variables directly, and this causes the compiler exception CS8421 A static local function 
cannot contain a reference to ‘<variable name>’.

```cs
static int AddWrapperWithSideEffect(int x, int y){
    return Add(x,y);

    static int Add(int x, int y){
        x += 1;
        return x + y;
    }
}
```

### Method Parameters

| Parameter Modifier | Meaning
| ------------------ | -------
| (None) | Reference types without modifier always passed in by reference
| `out` | Output parameters must be assigned by the method being called and, therefore, are passed by reference. If the called method fails to assign output parameters, you are issued a compiler error. 
| `ref` | The value is initially assigned by the caller and may be optionally modified by the called method (as the data is also passed by reference). No compiler error is generated if the called method fails to assign a ref parameter. 
| `in` | New in C# 7.2, the in modifier indicates that a ref parameter is read-only by the called method.
| `params` | This parameter modifier allows you to send in a variable number of arguments as a single logical parameter. A method can have only a single params modifier, and it must be the final parameter of the method. In reality, you might not need to use the params modifier all too often; however, be aware that numerous methods within the base class libraries do make use of this C# language feature.

<br>

When a parameter does not have a modifier, the behavior for value types is to pass in the parameter by value 
and for reference types is to pass in the parameter by reference.

The default manner in which a reference type parameter is sent into a function is by reference for its 
properties, but by value for itself. 

---
■ **Note** Even though the string datatype is technically a reference type, as discussed in Chapter 3, it’s a 
special case. When a string parameter does not have a modifier, it is passed in by value

---

### The `out` Modifier (Updated 7.0)

Methods that have been defined to take output parameters 
(via the out keyword) are under obligation to assign them to an appropriate value before exiting the method 
scope (if you fail to do so, you will receive compiler errors).

```cs
        static void Main(string[] args)
        {
            int ans;
            Add(5, 6, out ans);
            Console.WriteLine(ans); // 11
        }

        static void Add(int x, int y, out int ans)
        {
            ans = x + y;
        }
```

Calling a method with output parameters also requires the use of the out modifier. However, the local 
variables that are passed as output variables are not required to be assigned before passing them in as 
output arguments (if you do so, the original value is lost after the call). The reason the compiler allows you to 
send in seemingly unassigned data is because the method being called must make an assignment. To call the 
updated Add method, create a variable of type int, and use the out modifier in the call, like this:

Starting with C# 7.0, out parameters do not need to be declared before using them. In other words, they 
can be declared inside the method call, like this:

```cs
        static void Main(string[] args)
        {
            Console.WriteLine("***** Fun with Methods *****");
            FillTheseValues(out int i, out string str, out bool b);
            Console.WriteLine("Int is: {0}", i);
            Console.WriteLine("String is: {0}", str);
            Console.WriteLine("Boolean is: {0}", b);
        }

        // Returning multiple output parameters.
        static void FillTheseValues(out int a, out string b, out bool c)
        {
            a = 9;
            b = "Enjoy your string.";
            c = true;
        }
```

Always remember that a method that defines output parameters must assign the parameter to a valid 
value before exiting the method scope.

```cs
static void ThisWontCompile(out int a)
{
 Console.WriteLine("Error! Forgot to assign output arg!");
}
```

### The `ref` Modifier

```cs
static void Main(string[] args)
{
    Console.WriteLine("***** Fun with Methods *****");
    int i = 0;
    increment(ref i);
    Console.WriteLine(i);
}

static void increment(ref int i)
{
    i++;
}
```

 Note the distinction between output and reference parameters:

- Output parameters do not need to be initialized before they are passed to the method. 
The reason for this is that the method must assign output parameters before exiting.

- Reference parameters must be initialized before they are passed to the method. 
The reason for this is that you are passing a reference to an existing variable. If you 
don’t assign it to an initial value, that would be the equivalent of operating on an 
unassigned local variable.

```cs
        static void Main(string[] args)
        {
            Console.WriteLine("***** Fun with Methods *****");
            string str1 = "Flip";
            string str2 = "Flop";
            Console.WriteLine("Before: {0}, {1} ", str1, str2);
            SwapStrings(ref str1, ref str2);
            Console.WriteLine("After: {0}, {1} ", str1, str2);
            Console.ReadLine();
        }

        // Reference parameters.
        public static void SwapStrings(ref string s1, ref string s2)
        {
            string tempStr = s1;
            s1 = s2;
            s2 = tempStr;
        }
```

---
```cs
Before: Flip, Flop
After: Flop, Flip
```
---

### The `in` Modifier (New 7.2)

The in modifier passes a value by reference (for both value and reference types) and prevents the called 
method from modifying the values. This clearly states a design intent in your code, as well as potentially 
reducing memory pressure. When value types are passed by value, they are copied (internally) by the called 
method. If the object is large (such as a large struct), the extra overhead of making a copy for local use can 
be significant. Also, even when reference types are passed without a modifier, they can be modified by the 
called method. Both of these issues can be resolved using the in modifier.

```cs
        static int AddReadOnly(in int x, in int y)
        {
            //Error CS8331 Cannot assign to variable 'in int' because it is a readonly variable
            x = 10000;
            y = 88888;
            int ans = x + y;
            return ans;
        }
```

### The `params` modifier

C# supports the use of parameter arrays using the params keyword. The params keyword allows you to pass 
into a method a variable number of identically typed parameters (or classes related by inheritance) as a 
single logical parameter. As well, arguments marked with the params keyword can be processed if the caller 
sends in a strongly typed array or a comma-delimited list of items. Yes, this can be confusing! To clear things 
up, assume you want to create a function that allows the caller to pass in any number of arguments and 
return the calculated average.

If you were to prototype this method to take an array of doubles, this would force the caller to first define 
the array, then fill the array, and finally pass it into the method. However, if you define CalculateAverage to 
take a params of double[] data types, the caller can simply pass a comma-delimited list of doubles. The .NET 
Core runtime will automatically package the set of doubles into an array of type double behind the scenes.

```cs
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("***** Fun with Methods *****");

            // Pass in a comma-delimited list of doubles...
            double average;
            average = CalculateAverage(4.0, 3.2, 5.7, 64.22, 87.2);
            Console.WriteLine("Average of data is: {0}", average);
            
            // ...or pass an array of doubles.
            double[] data = { 4.0, 3.2, 5.7 };
            average = CalculateAverage(data);
            Console.WriteLine("Average of data is: {0}", average);
            
            // Average of 0 is 0!
            Console.WriteLine("Average of data is: {0}", CalculateAverage());
            Console.ReadLine();
        }

        // Reference parameters.
        // Return average of "some number" of doubles.
        static double CalculateAverage(params double[] values)
        {
            Console.WriteLine("You sent me {0} doubles.", values.Length);
            double sum = 0;
            if (values.Length == 0)
            {
                return sum;
            }
            for (int i = 0; i < values.Length; i++)
            {
                sum += values[i];
            }
            return (sum / values.Length);
        }
    }
```

### Defining Optional Parameters

```cs
 EnterLogData("Oh no! Grid can't find data"); // owner = "Programmer"
 EnterLogData("Oh no! I can't find the payroll data", "CFO"); // owner = "CFO"

static void EnterLogData(string message, string owner = "Programmer")
{
    Console.Beep();
    Console.WriteLine("Error: {0}", message); 
    Console.WriteLine("Owner of Error: {0}", owner);
}
```

```cs
// Error! The default value for an optional arg must be known
// at compile time!
static void EnterLogData(string message, string owner = "Programmer", DateTime timeStamp = 
DateTime.Now)
{
    Console.Beep();
    Console.WriteLine("Error: {0}", message);
    Console.WriteLine("Owner of Error: {0}", owner);
    Console.WriteLine("Time of Error: {0}", timeStamp);
}
```

### Understanding Method Overloading

```cs
class Program
{
 static void Main(string[] args)
 {
        Console.WriteLine("***** Fun with Method Overloading *****\n");
        //Calls int version of Add()
        Console.WriteLine(Add(10, 10));
        // Calls long version of Add()
        Console.WriteLine(Add(900_000_000_000, 900_000_000_000));
        // Calls double version of Add()
        Console.WriteLine(Add(4.3, 4.4));
 }
 // Overloaded Add() method.
 static int Add(int x, int y)
    { return x + y; }
 static double Add(double x, double y)
    { return x + y; }
 static long Add(long x, long y)
    { return x + y; }
} 
```

The caller can now simply invoke Add() with the required arguments, and the compiler is happy to 
comply, given that the compiler is able to resolve the correct implementation to invoke with the provided 
arguments.

<br><br>

## Uderstanding the `enum` Type

```cs
// A custom enumeration
enum EmpTypeEnum{
    Manager, // = 0
    Grunt, // = 1
    Contractor, // = 2
    VicePresident // = 3
}
```

```cs
// Begin with 102.
enum EmpTypeEnum
{
 Manager = 102,
 Grunt, // = 103
 Contractor, // = 104
 VicePresident // = 105
}
```

```cs
// Elements of an enumeration need not be sequential!
enum EmpType
{
 Manager = 10,
 Grunt = 1,
 Contractor = 100,
 VicePresident = 9
}
```

### Controlling the Underlying Storage for an enum

```cs
// This time, EmpTypeEnum maps to an underlying byte.
enum EmpTypeEnum : byte
{
 Manager = 10,
 Grunt = 1,
 Contractor = 100,
 VicePresident = 9
}
```

```cs
// Compile-time error! 999 is too big for a byte!
enum EmpTypeEnum : byte
{
 Manager = 10,
 Grunt = 1,
 Contractor = 100,
 VicePresident = 999
}
```

### Declaring enum Variables

```cs
enum EmpTypeEnum : int
{
    Manager = 10,
    Grunt = 1,
    Contractor = 100,
    VicePresident = 999
}

class Program
{


    static void Main(string[] args)
    {
        Console.WriteLine("**** Fun with Enums *****");
        // Make an EmpTypeEnum variable.
        EmpTypeEnum emp = EmpTypeEnum.Contractor;
        AskForBonus(emp);
    }
    // Enums as parameters.
    static void AskForBonus(EmpTypeEnum e)
    {
        switch (e)
        {
            case EmpTypeEnum.Manager:
                Console.WriteLine("How about stock options instead?");
                break;
            case EmpTypeEnum.Grunt:
                Console.WriteLine("You have got to be kidding...");
                break;
            case EmpTypeEnum.Contractor:
                Console.WriteLine("You already get enough cash...");
                break;
            case EmpTypeEnum.VicePresident:
                Console.WriteLine("VERY GOOD, Sir!");
                break;
        }
    }
}
```

```cs
static void ThisMethodWillNotCompile()
{
 // Error! SalesManager is not in the EmpTypeEnum enum!
 EmpTypeEnum emp = EmpTypeEnum.SalesManager;
 // Error! Forgot to scope Grunt value to EmpTypeEnum enum!
 emp = Grunt;
}
```

```cs
    static void Main(string[] args)
    {
        Console.WriteLine("**** Fun with Enums *****");
        // Make a contractor type.
        EmpTypeEnum emp = EmpTypeEnum.Contractor;
        AskForBonus(emp);
        // Print storage for the enum.
        Console.WriteLine("EmpTypeEnum uses a {0} for storage",
        Enum.GetUnderlyingType(emp.GetType()));
        Console.ReadLine();
    }
}
```

`ToString()`

```cs
    static void Main(string[] args)
    {
        EmpTypeEnum emp = EmpTypeEnum.Contractor;

        // Prints out "emp is a Contractor".
        Console.WriteLine("emp is a {0}.", emp.ToString());
        Console.ReadLine();
    }
```

To get the value:

```cs
    static void Main(string[] args)
    {
        EmpTypeEnum emp = EmpTypeEnum.Contractor;

        // Prints out "emp is a Contractor".
        Console.WriteLine("emp is a {0}.", (int)emp);
        Console.ReadLine();
    }
```

```cs
        // Get all name-value pairs for incoming parameter.
        Array enumData = Enum.GetValues(e.GetType());
        Console.WriteLine("This enum has {0} members.", enumData.Length);
        
        // Now show the string name and associated value, using the D format
        // flag (see Chapter 3).
        for (int i = 0; i < enumData.Length; i++)
        {
            Console.WriteLine("Name: {0}, Value: {0:D}",
            enumData.GetValue(i));
        }
```

<br><br>

## Understanding the Structure (aka Value Type)

---
■ Note a structure as a “lightweight class type,” given that 
structures provide a way to define a type that supports encapsulation but cannot be used to build a family of 
related types. When you need to build a family of related types through inheritance, you will need to make use 
of class types.

---

```cs
struct Point
{
    public int X;
    public int Y;

    // Add 1 to the (X, Y) position.
    public void Increment()
    {
        X++; Y++;
    }

    // Subtract 1 from the (X, Y) position.
    public void Decrement()
    {
        X--; Y--;
    }

    // Display the current postion.
    public void Display()
    {
        Console.WriteLine($"X = {X}, Y = {Y}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Point myPoint;
        myPoint.X = 349;
        myPoint.Y = 76;
        myPoint.Display();
        // Adjust the X and Y values.
        myPoint.Increment();
        myPoint.Display();
    }
}
```

---
```cs
X = 349, Y = 76
X = 350, Y = 77
```

---

### Creating Structure Variables

```cs
// Error! Did not assign Y value.
Point p1;
p1.X = 10;
p1.Display();

// OK! Both fields assigned before use.
Point p2;
p2.X = 10;
p2.Y = 10;
p2.Display();
```

```cs
// Set all fields to default values
// using the default constructor.
Point p1 = new Point();
// Prints X=0,Y=0.
p1.Display();
```

### Readonly Structs

Structs can also be marked as readonly if there is a need for them to be immutable. Immutable objects must 
be set up at construction and, because they cannot be changed, can be more performant. When declaring a 
struct as readonly, all of the properties must also be readonly. But, you might ask, how can a property be set 
(as all properties must be on a struct) if it’s read-only? The answer is that the value must be set during the 
construction of the struct.

```cs
readonly struct ReadOnlyPoint
{
    // Fields of the structure.
    public int X { get; }
    public int Y { get; }
    // Display the current position and name.
    public void Display()
    {
        Console.WriteLine($"X = {X}, Y = {Y}");
    }
    public ReadOnlyPoint(int xPos, int yPos, string name)
    {
        X = xPos;
        Y = yPos;
    }
}
```

### Readonly members

```cs
struct PointWithReadOnly
{
    // Fields of the structure.
    public int X;
    public readonly int Y;
    public readonly string Name;
    // Display the current position and name.
    public readonly void Display()
    {
        Console.WriteLine($"X = {X}, Y = {Y}, Name = {Name}");
    }
    // A custom constructor.
    public PointWithReadOnly(int xPos, int yPos, string name)
    {
        X = xPos;
        Y = yPos;
        Name = name;
    }
}

class Program
{
    static void Main(string[] args)
    {
        PointWithReadOnly p3 = new PointWithReadOnly(50, 60, "Point w/RO");
        p3.Display();
    }
}
```

### `ref` Structs (New 7.2)

This requires all instances of the 
struct to be stack allocated and cannot be assigned as a property of another class. The technical reason for 
this is that ref structs cannot referenced from the heap.

There are some additional limitations of ref structs:

- They cannot be assigned to a variable of type object, dynamic, or an interface type.
- They cannot implement interfaces.
- They cannot be used as a property of a non-ref struct.
- They cannot be used in async methods, in iterators, lambda expressions, or local 
functions.

### Disposable ref Structs (New 8.0)

`ref` structs (and readonly ref structs) cannot implement an interface and 
therefore cannot implement IDisposable. New in C# 8.0, ref structs and readonly ref structs can be made 
disposable by adding a public void Dispose() method

```cs
ref struct DisposableRefStruct
{
    public int X;
    public readonly int Y;
    public readonly void Display()
    {
        Console.WriteLine($"X = {X}, Y = {Y}");
    }
    // A custom constructor.
    public DisposableRefStruct(int xPos, int yPos)
    {
        X = xPos;
        Y = yPos;
        Console.WriteLine("Created!");
    }
    public void Dispose()
    {
        //clean up any resources here
        Console.WriteLine("Disposed!");
    }
}

class Program
{
    static void Main(string[] args)
    {
        var s = new DisposableRefStruct(50, 60);
        s.Display();
        s.Dispose();
    }
}
```

<br><br>

## Understanding Value Types and Reference Types

Unlike arrays, strings, or enumerations, C# structures do not have an identically named representation in 
the .NET Core library (i.e., there is no System.Structure class) but are implicitly derived from System.
ValueType. The role of System.ValueType is to ensure that the derived type (e.g., any structure) is allocated 
on the stack, rather than the garbage-collected heap. Simply put, data allocated on the stack can be created 
and destroyed quickly, as its lifetime is determined by the defining scope. Heap-allocated data, on the other 
hand, is monitored by the .NET Core garbage collector and has a lifetime that is determined by a large 
number of factors

Given that value types are using value-based semantics, the lifetime of a structure (which includes all 
numerical data types [int, float], as well as any enum or structure) is predictable. When a structure variable 
falls out of the defining scope, it is removed from memory immediately.

### Value Types, Reference Types, and the Assignment Operator

When you assign one value type to another, a member-by-member copy of the field data is achieved. In the 
case of a simple data type such as System.Int32, the only member to copy is the numerical value. However, 
in the case of your Point, the X and Y values are copied into the new structure variable.

```cs
    static void ValueTypeAssignment()
    {
        Console.WriteLine("Assigning value types\n");
        Point p1 = new Point(10, 10);
        Point p2 = p1;
        // Print both points.
        p1.Display();
        p2.Display();
        // Change p1.X and print again. p2.X is not changed.
        p1.X = 100;
        Console.WriteLine("\n=> Changed p1.X\n");
        p1.Display();
        p2.Display();
    }
```

---
```cs
Assigning value types
X = 10, Y = 10
X = 10, Y = 10
=> Changed p1.X
X = 100, Y = 10
X = 10, Y = 10
```

---

### Value Types Containing Reference Types

```cs
class ShapeInfo
{
    public string InfoString;
    public ShapeInfo(string info)
    {
        InfoString = info;
    }
}

struct Rectangle
{
    // The Rectangle structure contains a reference type member.
    public ShapeInfo RectInfo;
    public int RectTop, RectLeft, RectBottom, RectRight;
    public Rectangle(string info, int top, int left, int bottom, int right)
    {
        RectInfo = new ShapeInfo(info);
        RectTop = top; RectBottom = bottom;
        RectLeft = left; RectRight = right;
    }
    public void Display()
    {
        Console.WriteLine("String = {0}, Top = {1}, Bottom = {2}, " +
        "Left = {3}, Right = {4}",
        RectInfo.InfoString, RectTop, RectBottom, RectLeft, RectRight);
    }
}

class Program
{
    static void Main(string[] args)
    {
        ValueTypeContainingRefType();
    }
    static void ValueTypeContainingRefType()
    {
        // Create the first Rectangle.
        Console.WriteLine("-> Creating r1");
        Rectangle r1 = new Rectangle("First Rect", 10, 10, 50, 50);
        // Now assign a new Rectangle to r1.
        Console.WriteLine("-> Assigning r2 to r1");
        Rectangle r2 = r1;
        // Change some values of r2.
        Console.WriteLine("-> Changing values of r2");
        r2.RectInfo.InfoString = "This is new info!";
        r2.RectBottom = 4444;
        // Print values of both rectangles.
        r1.Display();
        r2.Display();
    }
}
```

At this point, you have contained a reference type within a value type. The million-dollar question now 
becomes “what happens if you assign one Rectangle variable to another?”

---
```cs
-> Creating r1
-> Assigning r2 to r1
-> Changing values of r2
String = This is new info!, Top = 10, Bottom = 50, Left = 10, Right = 50
String = This is new info!, Top = 10, Bottom = 4444, Left = 10, Right = 50
```

---

As you can see, when you change the value of the informational string using the r2 reference, the r1
reference displays the same value. By default, when a value type contains other reference types, assignment 
results in a copy of the references. In this way, you have two independent structures, each of which contains 
a reference pointing to the same object in memory (i.e., a shallow copy). When you want to perform a deep 
copy, where the state of internal references is fully copied into a new object, one approach is to implement 
the ICloneable interface

### Passing Reference Types by Value

```cs
class Person
{
    public string personName;
    public int personAge;
    // Constructors.
    public Person(string name, int age)
    {
        personName = name;
        personAge = age;
    }
    public Person() { }
    public void Display()
    {
        Console.WriteLine("Name: {0}, Age: {1}", personName, personAge);
    }
}

class Program{

    static void Main(string[] args)
    {
        // Passing ref-types by value.
        Person fred = new Person("Fred", 23);
        Console.WriteLine("\nBefore by value call, Person is:");
        fred.Display();
        
        SendAPersonByValue(fred);
        Console.WriteLine("\nAfter by value call, Person is:");
        fred.Display();
    }

    static void SendAPersonByValue(Person p)
    {
        // Change the age of "p"?
        p.personAge = 90;
        
        //Will the caller see this reassignment?
        p = new Person("Nikki", 99); // (u can't do this without the ref modifier)
    }
}    

```

---
```cs
Before by value call, Person is:
Name: Fred, Age: 23
After by value call, Person is:
Name: Fred, Age: 99
```
---

SendAPersonByValue() method is pointing to the same object as the caller, it is possible to alter the 
object’s state data. What is not possible is to reassign what the reference is pointing to.

### Passing Reference Types by Reference

However when using the ref keyword:

```cs
    ...
    Person fred = new Person("Fred", 23);
    SendAPersonByReference(ref fred);
    ...
}    

static void SendAPersonByReference(ref Person p)
{
        // Change some data of "p".
        p.personAge = 555;

        // "p" is now pointing to a new object on the heap!
        p = new Person("Nikki", 999);
}
```

---
```cs
Before by ref call, Person is:
Name: Fred, Age: 23
After by ref call, Person is:
Name: Nikki, Age: 999
```
---

So keep in mind that:

- If a reference type is passed by reference, the callee may change the values of the 
object’s state data, as well as the object it is referencing.

- If a reference type is passed by value, the callee may change the values of the object’s 
state data but not the object it is referencing.

### Final Details Regarding Value Types and Reference Types

| Intriguing Question | Value Type | Reference Type 
| ------------------- | ---------- | --------------
| Where are objects allocated? | On the stack | on the managed heap
| How is variable represented? | vars are local copies | vars are pointing to the memory occupied by the allocated instance
| What is the base type? | Implicitly extends `System.ValueType` | Can derive from any other type (except System.ValueType), as long as that type is not “sealed”
| Can this type function as a base to other types? | No. Value types are always sealed and cannot be inherited from. | Yes. If the type is not sealed
| What is the default parameter-passing behavior? | Vars are passed by value | For reference types, the reference is copied by value.
| Can this type override `System.Object.Finalize()`? | No | Yes, indirectly
| Can I define constructors for this type? | Yes, ur custom constructor must all have arguments (?) | But, of course!
| When do vars of this type die? | When they fall out of the defining scope. | When the object is garbage collected

Despite their differences, value types and reference types both have the ability to implement interfaces 
and may support any number of fields, methods, overloaded operators, constants, properties, and events.

<br><br>

## Understanding C# Nullable Types

- Value types cannot be set to `null`!

```cs
int myInt = null; // Compiler errors
```

- C# supports *nullable data types*

```cs
    static void Main(string[] args)
    {
        //int i = null;
        int? i = null;
        Console.WriteLine(i); // nothing
        i++;
        Console.WriteLine(i); // nothing
        i = 0;
        i++;
        Console.WriteLine(i); //
        i = null;
    }
```

### Nullable value types

```cs
static void LocalNullableVariablesUsingNullable()
{
 // Define some local nullable types using Nullable<T>.
 Nullable<int> nullableInt = 10;
 Nullable<double> nullableDouble = 3.14;
 Nullable<bool> nullableBool = null;
 Nullable<char> nullableChar = 'a';
 Nullable<int>[] arrayOfNullableInts = new Nullable<int>[10];
}
```

### Nullable Reference Types

`Page 200->`

<br><br>

## Tuples (New/Update 7.0)

- Tuples are lightweight data structures that contain multiple fields.

- In C# 7, tuples use the new ValueTuple data type instead of reference types, potentially saving 
significant memory. The ValueTuple data type creates different structs based on the number of properties 
for a tuple. An additional feature added in C# 7 is that each property in a tuple can be assigned a specific 
name (just like variables), greatly enhancing the usability.

- There are two important considerations for tuples:
    * the fields are not validated.
    * You cannot define your own methods.

```cs
(string, int, string) values = ("a", 5, "c");
var values2 = ("a", 5, "c");

Console.WriteLine($"First item: {values.Item1}");
Console.WriteLine($"Second item: {values.Item2}");
Console.WriteLine($"Third item: {values.Item3}");

//Specific names can also be added:
(string FirstLetter, int TheNumber, string SecondLetter) valuesWithNames = ("a", 5, "c");
var valuesWithNames2 = (FirstLetter: "a", TheNumber: 5, SecondLetter: "c");

```

### Tuple Equality/Inequality (New 7.3)

An added feature in C# 7.1 is the tuple equality (==) and inequality (!=). When testing for inequality, 
the comparison operators will perform implicit conversions on datatypes within the tuples, including 
comparing nullable and non-nullable tuples and/or properties. 

### Tuples As Method Return Values

```cs
static (int a,string b,bool c) FillTheseValues()
{
 return (9,"Enjoy your string.",true);
}

var samples = FillTheseValues();
Console.WriteLine($"Int is: {samples.a}");
Console.WriteLine($"String is: {samples.b}");
Console.WriteLine($"Boolean is: {samples.c}");
```

```cs
//Switch expression with Tuples
static string RockPaperScissors(string first, string second)
{
    return (first, second) switch
    {
        ("rock", "paper") => "Paper wins.",
        ("rock", "scissors") => "Rock wins.",
        ("paper", "rock") => "Paper wins.",
        ("paper", "scissors") => "Scissors wins.",
        ("scissors", "rock") => "Rock wins.",
        ("scissors", "paper") => "Scissors wins.",
        (_, _) => "Tie.",
    };
}
```


## Next chapter: OOP

<br>
<br>
<br>
<br>
<br>
<br>
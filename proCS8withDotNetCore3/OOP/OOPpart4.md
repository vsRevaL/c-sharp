# Object-Oriented Programming with C#, Chapter 8

<br>

# Working with Interfaces

- Understanding Interface Types
- Defining Custom Interfaces
- Implementing an Interface
- Invoking Interface Members at the Object Level
- Default Implementations
- Static Constructors and Members
- Interfaces As Parameters
- Interfaces As Return Values
- Arrays of Interface Types
- Explicit Interface Implementation
- Designing Interface Hierarchies
- The IEnumerable and IEnumerator Interfaces
- The ICloneable Interface
- The IComparable Interface

## Understanding Interface Types

- a named set is abstract members
- they don't provide a default implementation

■ Note By convention, .NET interfaces are prefixed with a capital letter I. When you are creating your own custom interfaces, it is considered a best practice to do the same.

<br>

### Interface Types vs Abstract Classes

- interfaces can't have nonstatic constructors
- class can implement multiple interfaces

Given this definition, only members that extend CloneableType are able to support the Clone()
method. If you create a new set of classes that do not extend this base class, you can’t gain this polymorphic interface. Also, you might recall that C# does not support multiple inheritance for classes. Therefore, if you wanted to create a MiniVan that is-a Car and is-a CloneableType, you are unable to do so

```cs
public abstract class CloneableType
{
    // Only derived types can support this "polymorphic interface"
    // Classes in other hierarchies have no access to this abstract member
    public abstract object Clone();
}

// Nope! Multiple inheritance is not possible in C#
// for classes.
public class MiniVan : Car, CloneableType
{
}
```

Interfaces come to the rescue.
After an interface has been defined, it can be implemented by any class or structure, in any hierarchy, and within any namespace or any assembly (written in any .NET Core programming language).

```cs
public interface ICloneableType
{
    object Clone();
}


public class MiniVan : Car, ICloneableType
{
    public object Clone()
    {
        throw new NotImplementedException();
    }
}
```

If you examine the .NET Core base class libraries, you’ll find that a large number of seemingly unrelated types (System.Array, System.Data.SqlClient.SqlConnection, System.OperatingSystem, System.String, etc.) all implement this interface. Although these types have no common parent (other than System.
Object), you can treat them polymorphically via the ICloneable interface type.
For example, if you had a method named CloneMe that took an ICloneable interface parameter, you could pass this method any object that implements said interface. Consider the following simple example that implements the CloneMe method as a local function:

```cs
    static void Main(string[] args)
    {
        string myStr = "Hello";
        OperatingSystem unixOS = new OperatingSystem(PlatformID.Unix, new Version());

        CloneMe(myStr);
        CloneMe(unixOS);
    }

    static void CloneMe(ICloneable c)
    {
        object theClone = c.Clone();
        Console.WriteLine("Your clone is a: {0}", theClone.GetType().Name);
    }
```

<br>

## Defining Custom Interfaces

```cs
public interface IPointy
{
    //Implicitly public and abstract
    byte GetNumberOfPoints();
}
```

```cs
// Ack! Errors abound!
    public interface IPointy
    {
        // Error! Interfaces cannot have data fields!
        public int numbOfPoints;
        // Error! Interfaces do not have nonstatic constructors!
        public IPointy() { numbOfPoints = 0; }
    }
```

Interface types are also able to define any number of property prototypes. For example, we could update the IPointy interface to use a read-write property (commented out) and a read-only property. The Points property replaces the GetNumberOfPoints method:

```cs
// The pointy behavior as a read-only property.
public interface IPointy
{
    // Implicitly public and abstract.
    //byte GetNumberOfPoints();
    // A read-write property in an interface would look like:
    //string PropName { get; set; }
    // while a write-only property in an interface would be:
    byte Points { get; }
}
```

■ Note Interface types can also contain event

```cs
// Ack! Illegal to allocate interface types.
static void Main(string[] args)
{
    IPointy p = new IPointy(); // Compiler error!
}
```

<br>

## Implementing an Interface

```cs
// This class derives from System.Object and
// implements a single interface.
public class Pencil : IPointy
{...}

// This class also derives from System.Object
// and implements a single interface.
public class SwitchBlade : object, IPointy
{...}

// This class derives from a custom base class
// and implements a single interface.
public class Fork : Utensil, IPointy
{...}

// This struct implicitly derives from System.ValueType and
// implements two interfaces.
public struct PitchFork : ICloneable, IPointy
{...}
```

```cs
class Triagnle : Shape, IPointy
{
    public Triangle() {}
    ...
    //Ipointy implementation
    //public byte Points { get {return 3;} }
    public byte Points => 3;
}
```

<br>

## Invoking Interface Members at the Object Level

Now that you have some classes that support the IPointy interface, the next question is how you interact with the new functionality. The most straightforward way to interact with functionality supplied by a given interface is to invoke the members directly from the object level (

```cs
static void Main(string[] args)
{
    Console.WriteLine("***** Fun with Interfaces *****\n");
    // Call Points property defined by IPointy.
    Hexagon hex = new Hexagon();
    Console.WriteLine("Points: {0}", hex.Points);
}
```

Other times, however, you might not be able to determine which interfaces are supported by a given type. For example, suppose you have an array containing 50 Shape-compatible types, only some of which support IPointy. Obviously, if you attempt to invoke the Points property on a type that has not implemented IPointy,you would receive an error. So, how can you dynamically determine whether a class or structure supports the correct interface?

```cs
static void Main(string[] args)
{
    ...
    // Catch a possible InvalidCastException.
    Circle c = new Circle("Lisa");
    IPointy itfPt = null;
    try
    {
        itfPt = (IPointy)c;
        Console.WriteLine(itfPt.Points);
    }
    catch (InvalidCastException e)
    {
        Console.WriteLine(e.Message);
    }
}
```

While you could use try/catch logic and hope for the best, it would be ideal to determine which interfaces are supported before invoking the interface members in the first place. Let’s see two ways of doing so

### Obtaining Interface References: The as Keyword

```cs
static void Main(string[] args)
{
    ...
    // Can we treat hex2 as IPointy?
    Hexagon hex2 = new Hexagon("Peter");
    IPointy itfPt2 = hex2 as IPointy;
    if (itfPt2 != null)
    {
        Console.WriteLine("Points: {0}", itfPt2.Points);
    }
    else
    {
        Console.WriteLine("OOPS! Not pointy...");
    }
}
```

### Obtaining Interface References: The is Keyword (Updated 7.0)

```cs
static void Main(string[] args)
{
    Console.WriteLine("***** Fun with Interfaces *****\n");
    ...
    if(hex2 is IPointy itfPt3)
    {
        Console.WriteLine("Points: {0}", itfPt3.Points);
    }
    else
    {
        Console.WriteLine("OOPS! Not pointy...");
    }
}
```

<br>

## Default Implementations (New 8.0)

C# 8.0 added the ability for interface methods and properties to have a default implementation.

```cs
interface IRegularPointy : IPointy
{
    int SideLength { get; set; }
    int NumberOfSides { get; set; }
    int Perimeter => SideLength * NumberOfSides;
}

class Square : Shape, IRegularPointy
{
    public Square() { }
    public Square(string name) : base(name) { }
    //Draw comes from the Shape base class
    public override void Draw()
    {
        Console.WriteLine("Drawing a square");
    }

    //This comes from the IPointy interface
    public byte Points => 4;
    //These come from the IRegularPointy interface
    public int SideLength { get; set; }
    public int NumberOfSides { get; set; }
    //Note that the Perimeter property is not implemented
}
```

The Perimeter property, defined on the IRegularPointy interface, is not defined in the Square class, making it inaccessible from an instance of Square.

```cs
var sq = new Square("Boxy") {
    NumberOfSides = 4,
    SideLength = 4
};
 sq.Draw();

 //This won’t compile
 Console.WriteLine($"{sq.PetName} has {sq.NumberOfSides} of length {sq.SideLength} and a perimeter of {sq.Perimeter}");
```

Instead, the Square instance must be explicitly cast to the IRegularPointy interface (since that is where the implementation lives), and then the Perimeter property can be accessed.

```cs
Console.WriteLine($"{sq.PetName} has {sq.NumberOfSides} of length {sq.SideLength} and a perimeter of {((IRegularPointy)sq).Perimeter}");
```

One option to get around this problem is to always code to the interface of a type. Change the definition of the Square instance to IRegularPointy instead of Square, like this:

```cs
IRegularPointy sq = new Square("Boxy") {NumberOfSides = 4, SideLength = 4};
```

The problem with this approach (in our particular case) is that the Draw method and PetName property are not defined on the interface, causing compilation errors.

<br>

## Static Constructors and Members (New 8.0)

These function the same as static members on class definitions, but defined on interfaces.

```cs
interface IRegularPointy : IPointy
{
    int SideLength { get; set; }
    int NumberOfSides { get; set; }
    int Perimeter => SideLength * NumberOfSides;

    //Static members are also allowed in C# 8
    static string ExampleProperty { get; set; }
    static IRegularPointy() => ExampleProperty = "Foo";
}
```

Static constructors have to be parameterless and can only access static properties and methods. To access the interface static property, add the following code to the Main method:

```cs
Console.WriteLine($"Example property: {IRegularPointy.ExampleProperty}");

IRegularPointy.ExampleProperty = "Updated";

Console.WriteLine($"Example property: {IRegularPointy.ExampleProperty}");
```

Notice how the static property must be invoked from the interface and not an instance variable.

<br>

## Interfaces As Parameters

```cs
// Models the ability to render a type in stunning 3D.
public interface IDraw3D
{
    void Draw3D();
}
```

Next, assume that two of your three shapes (ThreeDCircle and Hexagon) have been configured to support this new behavior

```cs

// Circle supports IDraw3D.
class ThreeDCircle : Circle, IDraw3D
{
    ...
    public void Draw3D() 
        => Console.WriteLine("Drawing Circle in 3D!");
}

// Hexagon supports IPointy and IDraw3D.
class Hexagon : Shape, IPointy, IDraw3D
{
    ...
    public void Draw3D() 
        => Console.WriteLine("Drawing Hexagon in 3D!");
}
```

If you now define a method taking an IDraw3D interface as a parameter, you can effectively send in any object implementing IDraw3D.

```cs
// I'll draw anyone supporting IDraw3D.
static void DrawIn3D(IDraw3D itf3d)
{
    Console.WriteLine("-> Drawing IDraw3D compatible type");
    itf3d.Draw3D();
}
```

```cs
static void Main(string[] args)
{
    Shape[] myShapes = { 
        new Hexagon(),
        new Circle(),
        new Triangle("Joe"),
        new Circle("JoJo")
    };

    for (int i = 0; i < myShapes.Length; i++)
    {
        // Can I draw you in 3D?
        if (myShapes[i] is IDraw3D s)
        {
            DrawIn3D(s);
        }
    }
}
```

<br>

## Interfaces As Return Values

Interfaces can also be used as method return values. For example, you could write a method that takes an array of Shape objects and returns a reference to the first item that supports IPointy.

```cs
static IPointy FindFirstPointyShape(Shape[] shapes)
{
    foreach (Shape s in shapes)
    {
        if (s is IPointy ip)
        {
            return ip;
        }
    }
    return null;
}
```

<br>

## Arrays of Interface Types

```cs
 // This array can only contain types that
 // implement the IPointy interface.
 IPointy[] myPointyObjects = {
     new Hexagon(),
     new Knife(),
     new Triangle(),
     new Fork(),
     new PitchFork()
 };

 foreach(IPointy i in myPointyObjects)
 {
    Console.WriteLine("Object has {0} points.", i.Points);
 }
```

<br>

## Explicit Interface Implementation

As shown earlier in this chapter, a class or structure can implement any number of interfaces. Given this, there is always the possibility you might implement interfaces that contain identical members and, therefore, have a name clash to contend with.

```cs
// Draw image to a form.
public interface IDrawToForm
{
    void Draw();
}

// Draw to buffer in memory.
public interface IDrawToMemory
{
    void Draw();
}

// Render to the printer.
public interface IDrawToPrinter
{
    void Draw();
}
```

Notice that each interface defines a method named Draw(), with the identical signature. If you now want to support each of these interfaces on a single class type named Octagon, the compiler would allow the following definition:

```cs
class Octagon : IDrawToForm, IDrawToMemory, IDrawToPrinter
{
    public void Draw()
    {
        // Shared drawing logic.
        Console.WriteLine("Drawing the Octagon...");
    }
}
```

```cs
 // Shorthand notation if you don't need
 // the interface variable for later use.
 ((IDrawToPrinter)oct).Draw();

 // Could also use the "is" keyword.
 if (oct is IDrawToMemory dtm)
 {
    dtm.Draw();
 }
```

When you implement several interfaces that have identical members, you can resolve this sort of name clash using explicit interface implementation syntax. Consider the following update to the Octagon type:

```cs
class Octagon : IDrawToForm, IDrawToMemory, IDrawToPrinter
{
    // Explicitly bind Draw() implementations
    // to a given interface.
    // returnType InterfaceName.MethodName(params){}
    void IDrawToForm.Draw()
    {
        Console.WriteLine("Drawing to form...");
    }
    void IDrawToMemory.Draw()
    {
        Console.WriteLine("Drawing to memory...");
    }
    void IDrawToPrinter.Draw()
    {
        Console.WriteLine("Drawing to a printer...");
    }
}
```

Note that when using this syntax, you do not supply an access modifier; explicitly implemented members are automatically private.

```cs
// Error! No access modifier!
public void IDrawToForm.Draw()
{
    Console.WriteLine("Drawing to form...");
}
```

**explicitly implemented members are always implicitly private**

these members are no longer available from the object level

```cs
static void Main(string[] args)
{
    Octagon oct = new Octagon();

    // We now must use casting to access the Draw()
    // members.
    IDrawToForm itfForm = (IDrawToForm)oct;
    itfForm.Draw();

    // Shorthand notation if you don't need
    // the interface variable for later use.
    ((IDrawToPrinter)oct).Draw();

    // Could also use the "is" keyword.
    if (oct is IDrawToMemory dtm)
    {
        dtm.Draw();
    }
}
```

<br>

## Designing Interface Hierarchies

Prior to C# 8, derived interfaces never inherit true implementation. Rather, a derived interface simply extends its own definition with additional abstract members. In C# 8, derived interfaces inherit the default implementations as well as extend the definition, and potentially add new default implementations.

```cs
public interface IDrawable
{
    void Draw();
}

public interface IAdvancedDraw : IDrawable
{
    void DrawInBoundingBox(int top, int left, int bottom, int right);
    void DrawUpsideDown();
}
```

Given this design, if a class were to implement IAdvancedDraw, it would now be required to implement every member defined up the chain of inheritance (specifically, the Draw(), DrawInBoundingBox(), and DrawUpsideDown() methods).

```cs
public class BitmapImage : IAdvancedDraw
{
    public void Draw()
    {
        Console.WriteLine("Drawing...");
    }

    public void DrawInBoundingBox(int top, int left, int bottom, int right)
    {
        Console.WriteLine("Drawing in a box...");
    }

    public void DrawUpsideDown()
    {
        Console.WriteLine("Drawing upside down!");
    }
}
```

Now, when you use the BitmapImage, you are able to invoke each method at the object level (as they are all public), as well as extract a reference to each supported interface explicitly via casting.

```cs
static void Main(string[] args)
{
    Console.WriteLine("***** Simple Interface Hierarchy *****");
    
    // Call from object level.
    BitmapImage myBitmap = new BitmapImage();
    myBitmap.Draw();
    myBitmap.DrawInBoundingBox(10, 10, 100, 150);
    myBitmap.DrawUpsideDown();
    
    // Get IAdvancedDraw explicitly.
    if (myBitmap is IAdvancedDraw iAdvDraw)
    {
        iAdvDraw.DrawUpsideDown();
    }
}
```

<br>

### Interface Hierarchies with Default Implementations (New 8.0)

```cs
public interface IDrawable
{
    void Draw();
    int TimeToDraw() => 5;
}
```

```cs
static void Main(string[] args)
{
    if (myBitmap is IAdvancedDraw iAdvDraw)
    {
        iAdvDraw.DrawUpsideDown();
        Console.WriteLine($"Time to draw: {iAdvDraw.TimeToDraw()}");
    }
}
```

Not only does this code compile, but it outputs a value of 5 for the TimeToDraw method. This is because default implementations automatically carry forward to descendant interfaces. Casting the BitMapImage to the IAdvancedDraw interface provides access to the TimeToDraw method, even though the BitMapImage instance does not have access to the default implementation. If a downstream interface wants to provide its own default implementation, it must hide the upstream implementation.

```cs
public interface IAdvancedDraw : IDrawable
{
    void DrawInBoundingBox(int top, int left, int bottom, int right);
    void DrawUpsideDown();

    new int TimeToDraw() => 15;
}
```

### Multiple Inheritance with Interface Types

Unlike class types, an interface can extend multiple base interfaces, allowing you to design some powerful and flexible abstractions.

```cs
// Multiple inheritance for interface types is A-okay.
interface IDrawable
{
    void Draw();
}

interface IPrintable
{
    void Print();
    void Draw(); // <-- Note possible name clash here!
}

// Multiple interface inheritance. OK!
interface IShape : IDrawable, IPrintable
{
    int GetNumberOfSides();
}
```

At this point, the million-dollar question is “If you have a class supporting IShape, how many methods will it be required to implement?” The answer: it depends.

If you’d have specific implementations for each Draw() method (which in this case would make the most sense), you can resolve the name clash using explicit interface implementation, as shown in the following Square type:

```cs
class Square : IShape
{
    //Using explicit implementation to handle member name clash.
    void IPrintable.Draw()
    {   
        // Draw to printer ..
    }

    void IDrawable.Draw()
    {   
        // Draw to screen ..
    }

    public void Print()
    {   
        // Print ..
    }
}
```

To summarize the story thus far, remember that interfaces can be extremely useful in the following cases:

- You have a single hierarchy where only a subset of the derived types supports a common behavior.

- You need to model a common behavior that is found across multiple hierarchies with no common parent class beyond `System.Object`

<br>
<br>

## The IEnumerable and IEnumerator Interfaces

Recall that C# supports a keyword named foreach that allows you to iterate over the contents of any array type.

Any type supporting a method named GetEnumerator() can be evaluated by the foreach construct.

```cs
// This interface informs the caller
// that the object's items can be enumerated.
public interface IEnumerable
{
    IEnumerator GetEnumerator();
}

```

```cs
// This interface allows the caller to
// obtain a container's items.
public interface IEnumerator
{
    bool MoveNext (); // Advance the internal position of the cursor.
    object Current { get;} // Get the current item (read-only property).
    void Reset (); // Reset the cursor before the first member.
}    
```

```cs
using System.Collections;
...
public class Garage : IEnumerable
{
    // System.Array already implements IEnumerator!
    private Car[] carArray = new Car[4];
    public Garage()
    {
        carArray[0] = new Car("FeeFee", 200);
        carArray[1] = new Car("Clunker", 90);
        carArray[2] = new Car("Zippy", 30);
        carArray[3] = new Car("Fred", 30);
    }

    // Return the array object's IEnumerator.
    public IEnumerator GetEnumerator()
    => carArray.GetEnumerator();
}
```

### Building Iterator Methods with the yield Keyword

There's an alternative way to build types that work with the foreach loop via iterators.

```cs
public class Garage : IEnumerable
{
    ...
    // Iterator method.
    public IEnumerator GetEnumerator()
    {
        foreach (Car c in carArray)
        {
            yield return c;
        }
    }
}
```

When the yield return statement is reached, the current location in the container is stored, and execution is restarted from this location the next time the iterator is called.

Iterator methods are not required to use the foreach keyword to return its contents. It is also permissible to define this iterator method as follows:

```cs
public IEnumerator GetEnumerator()
{
    yield return carArray[0];
    yield return carArray[1];
    yield return carArray[2];
    yield return carArray[3];
}
```

#### Guard Clauses with Local Functions (New 7.0)

None of the code in the GetEnumerator() method is executed until the first time that the items are iterated over (or any element is accessed). That means if there is an exception prior to the yield statement, it will not get thrown when the method is first called, but only when the first MoveNext is called.
To test this, update the GetEnumerator method to this:

```cs
public IEnumerator GetEnumerator()
{
    //This will not get thrown until MoveNext() is called
    throw new Exception("This won't get called");
    foreach (Car c in carArray)
    {
        yield return c;
    }
}
```

### Building a Named Iterator

When building a named iterator, be aware that the method will return the 
IEnumerable interface, rather than the expected IEnumerator-compatible type.

```cs
public IEnumerable GetTheCars(bool returnReversed)
{
    //do some error checking here
    return ActualImplementation();
    IEnumerable ActualImplementation()
    {
        // Return the items in reverse.
        if (returnReversed)
        {
            for (int i = carArray.Length; i != 0; i--)
            {
                yield return carArray[i - 1];
            }
        }
        else
        {
            // Return the items as placed in the array.
            foreach (Car c in carArray)
            {
                yield return c;
            }
        }
    }
}
```

```cs
static void Main(string[] args)
{
    Console.WriteLine("***** Fun with the Yield Keyword *****\n");
    Garage carLot = new Garage();
    // Get items using GetEnumerator().
    foreach (Car c in carLot)
    {
        Console.WriteLine("{0} is going {1} MPH",
        c.PetName, c.CurrentSpeed);
    }
    Console.WriteLine();
    // Get items (in reverse!) using named iterator.
    foreach (Car c in carLot.GetTheCars(true))
    {
        Console.WriteLine("{0} is going {1} MPH",
        c.PetName, c.CurrentSpeed);
    }
}
```

<br>

## The ICloneable Interface

System.Object defines a method named MemberwiseClone(). This method is used to obtain a shallow copy of the current object. Object users do not call this method directly, as it is protected. However, a given object may call this method itself during the cloning process.

```cs
// A class named Point.
public class Point
{
    public int X { get; set; }
    public int Y { get; set; }
    public Point(int xPos, int yPos) { X = xPos; Y = yPos; }
    public Point() { }

    // Override Object.ToString().
    public override string ToString() => $"X = {X}; Y = {Y}";
}
```

When you want to give your custom type the ability to return an identical copy of itself to the caller, you may implement the standard ICloneable interface.
As shown at the start of this chapter, this type defines a single method named Clone().

```cs
public interface ICloneable
{
    public abstract object Clone();
}
```

The basic functionality tends to be the same: copy the values of your member variables into a new object instance of the same type and return it to the user.

```cs
// The Point now supports "clone-ability."
public class Point : ICloneable
{
    public int X { get; set; }
    public int Y { get; set; }
    public Point(int xPos, int yPos) { X = xPos; Y = yPos; }
    public Point() { }
    
    // Override Object.ToString().
    public override string ToString() => $"X = {X}; Y = {Y}";
    
    // Return a copy of the current object.
    public object Clone() => new Point(this.X, this.Y);
}
```

```cs
 // Notice Clone() returns a plain object type.
 // You must perform an explicit cast to obtain the derived type.
 Point p3 = new Point(100, 100);
 Point p4 = (Point)p3.Clone();
```

While the current implementation of Point fits the bill, you can streamline things just a bit. Because the Point type does not contain any internal reference type variables, you could simplify the implementation of the Clone() method as follows:

```cs
// Copy each field of the Point member by member.
public object Clone() => this.MemberwiseClone();
```

Be aware, however, that if the Point did contain any reference type member variables, MemberwiseClone() would copy the references to those objects (i.e., a shallow copy). If you want to support a true deep copy, you will need to create a new instance of any reference type variables during the cloning process. Let’s see an example next.

### A More Elaborate Cloning Example

Now assume the Point class contains a reference type member variable of type PointDescription.

```cs
// This class describes a point.
public class PointDescription
{
    public string PetName { get; set; }
    public Guid PointID { get; set; }
    public PointDescription()
    {
        PetName = "No-name";
        PointID = Guid.NewGuid();
    }
}
```

```cs
public class Point : ICloneable
{
    public int X { get; set; }
    public int Y { get; set; }
    public PointDescription desc = new PointDescription();
    public Point(int xPos, int yPos, string petName)
    {
        X = xPos; Y = yPos;
        desc.PetName = petName;
    }
    public Point(int xPos, int yPos)
    {
        X = xPos; Y = yPos;
    }
    public Point() { }
    
    // Override Object.ToString().
    public override string ToString()
    => $"X = {X}; Y = {Y}; Name = {desc.PetName};\nID = {desc.PointID}\n";
    
    // Return a copy of the current object.
    public object Clone() => this.MemberwiseClone();
}
```

Notice that you did not yet update your Clone() method. Therefore, when the object user asks for a clone using the current implementation, a shallow (member-by-member) copy is achieved.

```cs
static void Main(string[] args)
{
    Console.WriteLine("***** Fun with Object Cloning *****\n");
    ...
    Console.WriteLine("Cloned p3 and stored new Point in p4");
    Point p3 = new Point(100, 100, "Jane");
    Point p4 = (Point)p3.Clone();
    Console.WriteLine("Before modification:");
    Console.WriteLine("p3: {0}", p3);
    Console.WriteLine("p4: {0}", p4);
    p4.desc.PetName = "My new Point";
    p4.X = 9;
    Console.WriteLine("\nChanged p4.desc.petName and p4.X");
    Console.WriteLine("After modification:");
    Console.WriteLine("p3: {0}", p3);
    Console.WriteLine("p4: {0}", p4);
}
```

Notice in the following output that while the value types have indeed been changed, the internal reference types maintain the same values, as they are “pointing” to the same objects in memory (specifically, note that the pet name for both objects is now “My new Point”).

----
***** Fun with Object Cloning *****
Cloned p3 and stored new Point in p4

Before modification:

p3: X = 100; Y = 100; Name = Jane;

ID = 133d66a7-0837-4bd7-95c6-b22ab0434509

<br>

p4: X = 100; Y = 100; Name = Jane;

ID = 133d66a7-0837-4bd7-95c6-b22ab0434509

<br>

Changed p4.desc.petName and p4.X

After modification:

p3: X = 100; Y = 100; Name = My new Point;

ID = 133d66a7-0837-4bd7-95c6-b22ab0434509

<br>

p4: X = 9; Y = 100; Name = My new Point;

ID = 133d66a7-0837-4bd7-95c6-b22ab0434509

----

To have your Clone() method make a complete deep copy of the internal reference types, you need to configure the object returned by MemberwiseClone() to account for the current point’s name (the System.Guid type is in fact a structure, so the numerical data is indeed copied). Here is one possible implementation:

```cs
// Now we need to adjust for the PointDescription member.
public object Clone()
{
    // First get a shallow copy.
    Point newPoint = (Point)this.MemberwiseClone();
    
    // Then fill in the gaps.
    PointDescription currentDesc = new PointDescription();
    currentDesc.PetName = this.desc.PetName;
    newPoint.desc = currentDesc;
    return newPoint;
}
```

To summarize the cloning process, if you have a class or structure that contains nothing but value types, implement your Clone() method using MemberwiseClone().

However, if you have a custom type that maintains other reference types, you might want to create a new object that takes into account each reference type member variable in order to get a “deep copy.”

<br>

## The IComparable Interface

The System.IComparable interface specifies a behavior that allows an object to be sorted based on some specified key. Here is the formal definition:

```cs
// This interface allows an object to specify its
// relationship between other like objects.
public interface IComparable
{
    int CompareTo(object o);
}
```

■ Note The generic version of this interface (`IComparable<T>`) provides a more type-safe manner to handle comparisons between objects.

```cs
public class Car
{
    ...
    public int CarID { get; set; }
    public Car(string name, int currSp, int id)
    {
        CurrentSpeed = currSp;
        PetName = name;
        CarID = id;
    }
    ...
}
```

Now assume you have an array of Car objects as follows:

```cs
static void Main(string[] args)
{
    // Make an array of Car objects.
    Car[] myAutos = new Car[5];
    myAutos[0] = new Car("Rusty", 80, 1);
    myAutos[1] = new Car("Mary", 40, 234);
    myAutos[2] = new Car("Viper", 40, 34);
    myAutos[3] = new Car("Mel", 40, 4);
    myAutos[4] = new Car("Chucky", 40, 5);
}
```

The System.Array class defines a static method named Sort(). When you invoke this method on an array of intrinsic types (int, short, string, etc.), you are able to sort the items in the array in numeric/
alphabetic order, as these intrinsic data types implement IComparable. However, what if you were to send an array of Car types into the Sort() method as follows?

```cs
// Sort my cars? Not yet! RuntimeException
Array.Sort(myAutos);
```

If you run this test, you would get a runtime exception, as the Car class does not support the necessary interface. When you build custom types, you can implement IComparable to allow arrays of your types to be sorted. When you flesh out the details of CompareTo(), it will be up to you to decide what the baseline of the ordering operation will be. For the Car type, the internal CarID seems to be the logical candidate.

```cs
// The iteration of the Car can be ordered
// based on the CarID.
public class Car : IComparable
{
    // IComparable implementation.
    int IComparable.CompareTo(object obj)
    {
        if (obj is Car temp)
        {
            if (this.CarID > temp.CarID)
            {
                return 1;
            }
            if (this.CarID < temp.CarID)
            {
                return -1;
            }
            return 0;
        }
        throw new ArgumentException("Parameter is not a Car!");
    }
}
```

As you can see, the logic behind CompareTo() is to test the incoming object against the current instance based on a specific point of data. The return value of CompareTo() is used to discover whether this type is less than, greater than, or equal to the object it is being compared with

| Return Value | Description
| ------------ | ------------
| Any number less than zero | This instance comes before the specified object in the sort order.
| Zero | This instance is equal to the specified object.
| Any number greater than zero |This instance comes after the specified object in the sort order.

<br>

```cs
// Exercise the IComparable interface.
static void Main(string[] args)
{
    // Make an array of Car objects.
    ...
    
    // Display current array.
    Console.WriteLine("Here is the unordered set of cars:");
    foreach (Car c in myAutos)
    {
        Console.WriteLine("{0} {1}", c.CarID, c.PetName);
    }
    
    // Now, sort them using IComparable!
    Array.Sort(myAutos);
    Console.WriteLine();
    
    // Display sorted array.
    Console.WriteLine("Here is the ordered set of cars:");
    foreach (Car c in myAutos)
    {
        Console.WriteLine("{0} {1}", c.CarID, c.PetName);
    }
    Console.ReadLine();
}
```

### Specifying Multiple Sort Orders with IComparer

Now, what if you wanted to build a Car that could be sorted by ID as well as by pet name? If this is the type of behavior you are interested in, you need to make friends with another standard interface named IComparer, defined within the System.Collections namespace as follows:

```cs
// A general way to compare two objects.
interface IComparer
{
    int Compare(object o1, object o2);
}
```

■ Note The generic version of this interface (`IComparer<T>`) provides a more type-safe manner to handle comparisons between objects.

Unlike the IComparable interface, IComparer is typically not implemented on the type you are trying to sort (i.e., the Car). Rather, you implement this interface on any number of helper classes, one for each sort order (pet name, car ID, etc.). Currently, the Car type already knows how to compare itself against other cars based on the internal car ID. Therefore, allowing the object user to sort an array of Car objects by pet name will require an additional helper class that implements IComparer. Here’s the code (be sure to import the System.Collections namespace in the code file):

```cs
// This helper class is used to sort an array of Cars by pet name.
public class PetNameComparer : IComparer
{
    // Test the pet name of each object.
    int IComparer.Compare(object o1, object o2)
    {
        if (o1 is Car t1 && o2 is Car t2)
        {
            return string.Compare(t1.PetName, t2.PetName,
            StringComparison.OrdinalIgnoreCase);
        }
        else
        {
            throw new ArgumentException("Parameter is not a Car!");
        }
    }
}
```

The object user code is able to use this helper class. System.Array has a number of overloaded Sort()
methods, one that just happens to take an object implementing IComparer.

```cs
static void Main(string[] args)
{
    ...
    // Now sort by pet name.
    Array.Sort(myAutos, new PetNameComparer());
    
    // Dump sorted array.
    Console.WriteLine("Ordering by pet name:");
    foreach(Car c in myAutos)
    {
        Console.WriteLine("{0} {1}", c.CarID, c.PetName);
    }
}
```

### Custom Properties and Custom Sort Types

Custom Properties and Custom Sort Types
It is worth pointing out that you can use a custom static property to help the object user along when sorting your Car types by a specific data point. Assume the Car class has added a static read-only property named SortByPetName that returns an instance of an object implementing the IComparer interface (PetNameComparer, in this case; be sure to import System.Collections).

```cs
// We now support a custom property to return
// the correct IComparer interface.
public class Car : IComparable
{
    ...
    // Property to return the PetNameComparer.
    public static IComparer SortByPetName
        => (IComparer)new PetNameComparer();
}
```

The object user code can now sort by pet name using a strongly associated property, rather than just “having to know” to use the stand-alone PetNameComparer class type.

```cs
// Sorting by pet name made a bit cleaner.
Array.Sort(myAutos, Car.SortByPetName);
```

Ideally, at this point you not only understand how to define and implement your own interfaces but also understand their usefulness. 

<br>
<br>
<br>
<br>
<br>
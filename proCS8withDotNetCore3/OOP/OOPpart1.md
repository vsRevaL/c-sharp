# Object-Oriented Programming with C#, Chapter 5

<br>

# Understanding Encapsulation

- Introducing the C# Class Type
- Allocation Objects with the new Keyword
- Understanding Constructors
- Defining Custom Constructors
- The Role of the this Keyword
- Understanding the static Keyword
- Defining the Pillars of OOP
- C# Access Modifiers (Updated 7.2)
- The First Pillar: C#'s Encapsulation
- Automatic Properties
- Understanding Object Initialization Syntax
- Working with Constant Field Data

<br>

## Introducing the C# Class Type

As far as the .NET platform is concerned, the most fundamental programming construct is the class type. 
Formally, a class is a user-defined type that is composed of field data (often called member variables) and 
members that operate on this data (such as constructors, properties, methods, events, etc.). Collectively, 
the set of field data represents the “state” of a class instance (otherwise known as an object). The power of 
object-oriented languages, such as C#, is that by grouping data and related functionality in a unified class 
definition, you are able to model your software after entities in the real world.

- Field data is `private` by default

<br>

## Allocation Objects with the new Keyword

- objects must be allocated into memory using the new keyword, or complier error

```cs
// Car c; //
// c.speed = 50; Compiler: That's illegal

Car c = new Car(); // or Car c; c = new Car();
c.speed = 50;
```

<br>

## Understanding Constructors

- called indirectly when creating object using `new` keyword
- never has return value



### The Role of the Default Constructor

- never takes arguments
- default constructor ensures that all field data of the class is setto an appropriate default value

```cs
class Car{
    public string brand;

    public Car(string brand){
        this.brand = brand;
    }
}
```

<br>

## Defining Custom Constructors

- what makes one constructor different from another (in the eyes of the C# compiler) is the number of and/or type of constructor arguments.

- as soon as you define a custom constructor with any number of parameters, the default constructor is silently removed from the class and is no longer available.

<br>

## Constructors As Expression-Bodied Members (New 7.0)

```cs
class Car
{
    public string brand;
    public Car(string brand) => this.brand = brand;
}
```

<br>

## The Role of the `this` Keyword

C# supplies a this keyword that provides access to the current class instance. One possible use of the this
keyword is to resolve scope ambiguity, which can arise when an incoming parameter is named identically 
to a data field of the class.



### Chaining Constructor Calls Using `this`

Another use of the this keyword is to design a class using a technique termed constructor chaining. This 
design pattern is helpful when you have a class that defines multiple constructors. Given that constructors 
often validate the incoming arguments to enforce various business rules, it can be quite common to find 
redundant validation logic within a class’s constructor set.

A cleaner approach is to designate the constructor that takes the greatest number of arguments as the 
“master constructor” and have its implementation perform the required validation logic. The remaining 
constructors can make use of the this keyword to forward the incoming arguments to the master 
constructor and provide any additional parameters as necessary. 

```cs
class Motorcycle
{
    public int driverIntensity;
    public string driverName;

    public Motorcycle() { }
    public Motorcycle(int intensity) : this(intensity, "") { }
    public Motorcycle(string name) : this(0, name) { }

    public Motorcycle(int intensity, string name)
    {
        if(intensity > 10)
        {
            intensity = 10;
        }
        this.driverIntensity = intensity;
        this.driverName = name;
    }
}
```


### Observing Constructor Flow

- once a constructor passes arguments to the designated master constructor (and that constructor has processed the data), the constructor invoked originally by the caller will finish executing any remaining code statements.

```cs
class Car
{
    public string brand;
    public int speed;
    public Car(int speed) : this(speed, "Ferrari")
    {
        Console.WriteLine("I'm in the Car(int) ctor");
    }

    public Car(int speed, string brand)
    {
        Console.WriteLine("I'm in the master ctor");
        this.speed = speed;
        this.brand = brand;
    }
}
```

---
`I'm in the master ctor` <br>
`I'm in the Car(int) ctor`

--- 

<br>

## Understanding the `static` Keyword

- A C# class may define any number of static members, which are declared using the static keyword. When you do so, the member in question must be invoked directly from the class level, rather than from an object reference variable. 

```cs
Console c = new Console() // comp error
c.WriteLine("I can't be printed..");
```

```cs
Console.WriteLine("Much better. Thanks!"); // Correct
```

- While any class can 
define static members, they are quite commonly found within utility classes.

- By definition, a utility class is a 
class that does not maintain any object-level state and is not created with the new keyword. Rather, a utility class exposes all functionality as class-level (aka static) members.

the static keyword can be applied to the following:
- Data of a class
- Methods of a class
- Properties of a class
- A constructor
- The entire class definition
- In conjunction with the C# `using` keyword 



### Defining Static Field Data

- Most of the time when designing a class, you define data as instance-level data or, said another way, as 
nonstatic data. When you define instance-level data, you know that every time you create a new object, the 
object maintains its own independent copy of the data. In contrast, when you define static data of a class, the 
memory is shared by all objects of that category.

```cs
// A simple savings account class.
class SavingsAccount
{
    // Instance-level data.
    public double currBalance;
    
    // A static point of data.
    public static double currInterestRate = 0.04;
    public SavingsAccount(double balance)
    {
        currBalance = balance;
    }
}

class Program{
    static void Main(string[] args){
        SavingsAccount s1 = new SavingsAccount(50);
        SavingsAccount s2 = new SavingsAccount(100);
        SavingsAccount s3 = new SavingsAccount(10000.75);
    }
}
```

**Static data is allocated once and shared among all instances of the class**


---
```cs
s1.currInterestRate -> 0.04;
s2.currInterestRate -> 0.04;
s2.currInterestRate -> 0.04;
```

---

### Defining Static Methods

```cs
// A simple savings account class.
class SavingsAccount
{
    // Instance-level data.
    public double currBalance;
    
    // A static point of data.
    public static double currInterestRate = 0.04;

    public SavingsAccount(double balance) => this.currBalance = balance;
    
    // Static members to get/set interest rate.
    public static void SetInterestRate(double newRate) => currInterestRate = newRate; // can't use this.currInterestRate here
    public static double GetInterestRate() => currInterestRate; // neither here
}
```
---
■ Note It is a compiler error for a static member to reference nonstatic members in its implementation. On a related note, it is an error to use the this keyword on a static member because this implies an object!

---

### Defining Static Constructors

A typical constructor is used to set the value of an object’s instance-level data at the time of creation. However, 
what would happen if you attempted to assign the value of a static point of data in a typical constructor? You 
might be surprised to find that the value is reset each time you create a new object!

```cs
class SavingsAccount
{
    public double currBalance;
    public static double currInterestRate;
    
    // Notice that our constructor is setting
    // the static currInterestRate value.
    public SavingsAccount(double balance)
    {
        currInterestRate = 0.04; // This is static data!
        currBalance = balance;
    }

    public static double GetInterestRate() => currInterestRate;
    public static void SetInterestRate(double newRate) => currInterestRate = newRate;
}

class Program{
    static void Main(string[] args){
            SavingsAccount s1 = new SavingsAccount(50);
            Console.WriteLine("Interest Rate is: {0}", SavingsAccount.GetInterestRate());
            SavingsAccount.SetInterestRate(0.08);
            SavingsAccount s2 = new SavingsAccount(100);
            Console.WriteLine("Interest Rate is: {0}", SavingsAccount.GetInterestRate());
    }
}
```

If you executed the previous Main() method, you would see that the currInterestRate variable is reset 
each time you create a new SavingsAccount object, and it is always set to 0.04. Clearly, setting the value of 
static data in a normal instance-level constructor sort of defeats the whole purpose.

---
```cs
Interest Rate is: 0.04
Interest Rate is: 0.04
```
---

For this reason, C# allows you to define a static constructor, which allows you to safely set the values of 
your static data.

```cs
    ...
    // A static constructor!
    static SavingsAccount()
    {
        Console.WriteLine("In static ctor!");
        currInterestRate = 0.04;
    }
    ...
```
After compiling again:

---
```cs
In static ctor!
Interest Rate is: 0.04
Interest Rate is: 0.08
```
---

Simply put, a static constructor is a special constructor that is an ideal place to initialize the values of 
static data when the value is not known at compile time

**Few points of interest regarding static constructors**:

- A given class may define only a single static constructor. In other words, the static 
constructor cannot be overloaded.

- A static constructor does not take an access modifier and cannot take any 
parameters.

- A static constructor executes exactly one time, regardless of how many objects of the 
type are created.

- The runtime invokes the static constructor when it creates an instance of the class or 
before accessing the first static member invoked by the caller.

- The static constructor executes before any instance-level constructors.


### Defining Static Classes

It is also possible to apply the static keyword directly on the class level. When a class has been defined as 
static, it is not creatable using the new keyword, and it can contain only members or data fields marked with 
the static keyword. If this is not the case, you receive compiler errors.


### Importing Static Members via the C# `using` Keyword

```cs
// Import the static members of Console and DateTime.
using static System.Console;
using static System.DateTime;
```

With these “static imports,” the remainder of your code file is able to directly use the static members of 
the Console and DateTime class, without the need to prefix the defining class. For example, you could update 
your utility class like so:

```cs
WriteLine("Finally..");
```

However, be aware that overuse of static import statements could result in potential confusion. First, 
what if multiple classes define a WriteLine() method? The compiler is confused and so are others reading 
your code.

<br>

## Defining the Pillars of OOP

All object-oriented languages (C#, Java, C++, Visual Basic, etc.) must contend with three core principles, 
often called the pillars of object-oriented programming (OOP):

- Encapsulation: How does this language hide an object’s internal implementation 
details and preserve data integrity?

- Inheritance: How does this language promote code reuse?

- Polymorphism: How does this language let you treat related objects in a similar way?

### The Role of Encapsulation

```cs
// Assume this class encapsulates the details of opening and closing a database.
DatabaseReader dbReader = new DatabaseReader();
dbReader.Open(@"C:\AutoLot.mdf");
// Do something with data file and close the file.
dbReader.Close();
```

Ideally, 
an object’s state data should be specified using either the private, internal, or protected keyword. In this 
way, the outside world must ask politely in order to change or obtain the underlying value. 

### The Role of Inheritance

- In essence, inheritance allows you to extend the behavior of 
a base (or parent) class by inheriting core functionality into the derived subclass (also called a child class). 

- System.Object is always the topmost parent in any class hierarchy.

- When you have 
classes related by this form of inheritance, you establish “is-a” relationships between types. The “is-a” 
relationship is termed inheritance.

- There is another form of code reuse in the world of OOP: the containment/delegation model also 
known as the “has-a” relationship or aggregation. This form of reuse is not used to establish parent-child 
relationships. Rather, the “has-a” relationship allows one class to define a member variable of another class 
and expose its functionality (if required) to the object user indirectly:

```cs
class Radio
{
    public void Power(bool turnOn)
    {
        Console.WriteLine("Radio on: {0}", turnOn);
    }
}

class Car
{
    private Radio meinRadio = new Radio();

    public void TurnOnRadio(bool onOff)
    {
        meinRadio.Power(onOff);
    }
}
```


### The Role of Polymorphism

This trait captures a language’s ability to treat related objects in a similar manner. Specifically, this tenant of an object-oriented language allows a base class to define a 
set of members (formally termed the polymorphic interface) that are available to all descendants. A class’s 
polymorphic interface is constructed using any number of virtual or abstract members

In a nutshell, a virtual member is a member in a base class that defines a default implementation that 
may be changed (or more formally speaking, overridden) by a derived class. In contrast, an abstract method
is a member in a base class that does not provide a default implementation but does provide a signature.

When a class derives from a base class defining an abstract method, **it must be overridden** by a derived 
type. In either case, when derived types override the members defined by a base class, they are essentially 
redefining how they respond to the same request.

```cs
    static void Main(string[] args)
    {
        Shape[] myShapes = new Shape[3];
        myShapes[0] = new Hexagon();
        myShapes[1] = new Circle();
        myShapes[2] = new Hexagon();
        foreach (Shape s in myShapes)
        {
            // Use the polymorphic interface!
            s.Draw();
        }
        Console.ReadLine();
    }
```

<br>

## C# Access Modifiers (Updated 7.2)

| C# Access Modifier | May Be Applied To | Meaning in Life
| ----------------- | ----------------- | ----------------
| public | Types or type members | Can be accessed even from Mars
| private | Type members or nested types | Can be accessed only by the class (or structure) that defines the item
| protected | Type members or nested types | Can be used by the class defining them and any child class. They cannot be accessed from outside the inheritance chain.
| internal | Types or type members | only within the current assembly, other assemblies can be explicitly granted permission to see the internal items.
| protected internal | Type members or nested types | within the defining assembly, within the defining class, and by derived classes inside or outside of the defining assembly
| private protected | Type members or nested types | accessible within the defining class and by derived classes in the same assembly.

<br>

### The Default Access Modifiers

By default, type members are implicitly private, while types are implicitly internal.

```cs
// An internal class with a private default constructor
class Radio{
    Radio(){}
}
```

or

```cs
internal class Radio{
    private Radio(){}
}
```

<br>

To allow other parts of a program to invoke members of an object, you must define them with the 
public keyword (or possibly with the protected keyword)

```cs
public class Radio{
    public Radio(){}
}
```

### Access Modifiers and Nested Types

private, protected, protected internal, and private protected access modifiers can be applied to a nested type.

```cs
public class A {
    private class B {
    }
}
```

Here, it is permissible to apply the private access modifier on the nested type. However, non-nested 
types (such as the A) can be defined only with the public or internal modifiers.

```cs
private class A { // compiler: Wait, thats illegal
}
```

<br>

## The First Pillar: C#'s Encapsulation

The concept of encapsulation revolves around the notion that an object’s data should not be directly 
accessible from an object instance. Rather, class data is defined as private. If the object user wants to alter 
the state of an object, it does so indirectly using public members. 

Encapsulation provides a way to preserve the integrity of an object’s state data. Rather than defining 
public fields (which can easily foster data corruption), you should get in the habit of defining private data, 
which is indirectly manipulated using one of two main techniques.

- You can define a pair of public accessor (get) and mutator (set) methods.
- You can define a public property.

```cs
class Employee
{
    // Field data.
    private string _empName;

    // Accessor (get method).
    public string GetName() => _empName;
    // Mutator (set method).
    public void SetName(string name)
    {
        // Do a check on incoming value
        // before making assignment.
        if (name.Length > 15)
        {
            Console.WriteLine("Error! Name length exceeds 15 characters!");
        }
        else
        {
            _empName = name;
        }
    }
}
```

### Encapsulation Using Properties

.NET Core 
languages prefer to enforce data encapsulation state data using properties. Properties 
are just a container for “real” accessor and mutator methods, named get and set, respectively. 

 C# property is composed by defining a get scope (accessor) and set scope (mutator) directly within 
the property itself.

```cs
// The 'int' represents the type of data this property encapsulates.
public int Id // Note lack of parentheses.
{
 get { return _empId; }
 set { _empID = value; }
}
```

```cs
class A
{
    private int _age;
    public int Age { 
        get { return _age; } 
        set
        {
            if(value < 0)
            {
                //throw new Exception("Invalid age");
                Console.WriteLine("Sorry playa, invalid age");
            }
            else
            {
                _age = value;
            }
        }
    }
}

class Solution
{
    public static void Main(string[] args)
    {
        A a = new A();
        a.Age = -5;
        Console.WriteLine(a.Age);
    }
}
```

### Properties As Expression-Bodied Members (New 7.0)

Both syntaxes compile down to the same IL, so which syntax you use is completely up to you.

```cs
class A
{
    private int _age;
    public int Age
    {
        get => _age;
        set => _age = value;
    }
}
```

### Using Properties Within a Class Definition

**You can use the properties in ctor to avoid code duplication!**

```cs
class A
{
    private int _age;

    public A(int _age)
    {
        Age = _age; // HERE!
    }

    public int Age
    {
        get { return _age; }
        set
        {
            if (value < 0)
            {
                //throw new Exception("Invalid age");
                Console.WriteLine("Sorry! Invalid range");
            }
            else
            {
                _age = value;
            }
        }
    }
}

class Solution
{
    public static void Main(string[] args)
    {
        A a = new A(-5); // Sorry! Invalid range.
        Console.WriteLine(a.Age);
    }
}
```

### Read-Only and Write-Only Properties

When encapsulating data, you might want to configure a read-only property. To do so, simply omit the set
block. Likewise, if you want to have a write-only property, omit the get block.

```cs
class A
{
    private int _age;

    public A(int _age)
    {
        Age = _age; // OOPS! This is no longer possible if the property is read only. 
    }

    public int Age
    {
        get { return _age; }
    }
}
```

### Mixing Private and Public Get/Set Methods on Properties

```cs
class A
{
    private int _age;

    public A(int _age)
    {
        Age = _age; // Now this is valid!!!
    }

    public int Age
    {
        get { return _age; }
        private set { Age = value; } // Bcs of this line 
    }
}
```

### Defining Static Properties

```cs
class A
{
    private static int _age;

    public A(int _age)
    {
        Age = _age;
    }

    public static int Age
    {
        get { return _age; }
        private set { Age = value; }
    }
}
```

If you want to use this property in place of the previous static methods, you could update your Main() method as so:

```cs
Console.WriteLine($"Age is {A.Age}");
```

## Automatic Properties

```cs
class Car
{
    private string carName = "";
    public string PetName
    {
        get { return carName; }
        set { carName = value; }
    }
}
```

Ahh man, it's just a waste of code lines, in this cases you better do this:

```cs
class Car
{
    // Automatic properties! No need to define backing fields.
    public string PetName { get; set; }
    public int Speed { get; set; }
    public string Color { get; set; }
}
```

but be careful:

```cs
// Read-only property? This is OK!
public int MyReadOnlyProp { get; }

// Write only property? Nah, Error!
// it's not possible to define a write-only 
property.
public int MyWriteOnlyProp { set; }
```

### Interacting with Automatic Properties

```cs
class Garage
{
    // The hidden int backing field is set to zero!
    public int NumberOfCars { get; set; }
    
    // The hidden Car backing field is set to null!
    public Car MyAuto { get; set; }
}

class Solution
{
    public static void Main(string[] args)
    {
        Garage g = new Garage();
        
        // OK, prints default value of zero.
        Console.WriteLine("Number of Cars: {0}", g.NumberOfCars);
        
        // Runtime error! Backing field is currently null!
        Console.WriteLine(g.MyAuto.PetName);

    }
}
```

To solve this problem, you could update the class constructors to ensure the object comes to life in a 
safe manner. Here’s an example:

```cs
public Garage()
{
    MyAuto = new Car();
    NumberOfCars = 1;
}
```

### Initialization of Automatic Properties

```cs
class Garage
{
    public int NumberOfCars { get; set; } = 5;
}

class Solution
{
    public static void Main(string[] args)
    {
        Garage g = new Garage();
        Console.WriteLine(g.NumberOfCars); // 5
    }
}
```

## Understanding Object Initialization Syntax

```cs
Point finalPoint = new Point { X = 30, Y = 30 };
```

### Initializing Data with Initialization Syntax

The “has-a” relationship 
allows you to compose new classes by defining member variables of existing classes.

```cs
class Point
{
    public int X { get; set; }
    public int Y { get; set; }
    public Point(int xVal, int yVal)
    {
        X = xVal;
        Y = yVal;
    }
    public Point() { }
    public void DisplayStats()
    {
        Console.WriteLine("[{0}, {1}]", X, Y);
    }
}

class Rectangle
{
    private Point topLeft = new Point();
    private Point bottomRight = new Point();
    public Point TopLeft
    {
        get { return topLeft; }
        set { topLeft = value; }
    }
    public Point BottomRight
    {
        get { return bottomRight; }
        set { bottomRight = value; }
    }
    public void DisplayStats()
    {
        Console.WriteLine("[TopLeft: {0}, {1}, {2} BottomRight: {3}, {4}, {5}]",
    }
}
```

## Working with Constant Field Data

C# offers the const keyword to define constant data, which can never change after the initial assignment.

```cs
class MyMathClass
{
    public const double PI = 3.14;
}
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("***** Fun with Const *****\n");
        Console.WriteLine("The value of PI is: {0}", MyMathClass.PI);
        // Error! Can't change a constant!
        // MyMathClass.PI = 3.1444;
        Console.ReadLine();
    }
}
```


- **constant fields of a class are implicitly static.**
 
- Regardless of where you define a constant piece of data, the one point to always remember is that **the 
initial value assigned to the constant must be specified at the time you define the constant**.

```cs
class MyMathClass
{
    // Try to set PI in ctor?
    public const double PI;
    public MyMathClass()
    {
        // Not possible- must assign at time of declaration.
        PI = 3.14;
    }
}
```

### Read-Only Fields

**Like a constant**, a read-only field cannot be changed after the initial assignment 
compile-time error. However, unlike a constant, **the value assigned to a read-only field can be determined at 
runtime** and, therefore, can legally be assigned within the scope of a **constructor but nowhere else**.

```cs
class MyMathClass
{
    // Read-only fields can be assigned in ctors,
    // but nowhere else.
    public readonly double PI;
    public MyMathClass()
    {
        PI = 3.14;
    }
}
```

<br>
<br>
<br>
<br>
<br>
<br>

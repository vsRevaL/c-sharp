# Object-Oriented Programming with C#, Chapter 6

<br>

# Understanding Inheritance and Polymorphism

- The Basic Mechanics of Inheritance
- The Second Pillar of OOP: The Details of Inheritance
- Programming for Containment/Delegation
- The Third Pillar of OOP: C#’s Polymorphic Support
- Understanding Base Class/Derived Class Casting Rules
- The Super Parent Class: System.Object


<br>

## The Basic Mechanics of Inheritance

- "is-a", "has-a" relatinships

```cs
class Car
{
    public readonly int MaxSpeed;
    private int _currSpeed;

    public Car(int max)
    {
        MaxSpeed = max;
    }
    public Car()
    {
        MaxSpeed = 55;
    }
    public int Speed
    {
        get { return _currSpeed; }
        set
        {
            _currSpeed = value;
            if (_currSpeed > MaxSpeed)
            {
                _currSpeed = MaxSpeed;
            }
        }
    }
}

// MiniVan "is-a" Car.
class MiniVan : Car
{
}
```

MiniVan objects now have access to each public 
member defined within the parent class.

---
■ **Note** Although constructors are typically defined as public, a derived class never inherits the constructors 
of a parent class. Constructors are used to construct only the class that they are defined within, although they 
can be called by a derived class through constructor chaining. 

---

- inheritance preserves encapsulation; therefore, the following code results in a 
compiler error, as private members can never be accessed from an object reference:

```cs
// Error! Can't access private members!
 myVan.currSpeed = 55;
```

### Regarding Multiple Base Classes

- It is not possible to create a class type that directly derives from two or more base classes 

### The sealed Keyword

- When you mark a class as 
sealed, the compiler will not allow you to derive from this type. For example, assume you have decided that 
it makes no sense to further extend the MiniVan class.

```cs
sealed class MiniVan : Car
{
}

// Error! Cannot extend
// a class marked with the sealed keyword!
class DeluxeMiniVan : MiniVan
{
}
```

<br>

## The Second Pillar of OOP: The Details of Inheritance

### Controlling Base Class Creation with the base Keyword

```cs
sealed class MiniVan : Car
{
    private int _size;
    public int Size { get { return _size; } set { _size = value; } }
    public MiniVan() : base() {}
}


class Program
{
    public static void Main(string[] args)
    {
        Car mv = new MiniVan();
        ((MiniVan)mv).Size = 5;
        System.Console.WriteLine(mv.MaxSpeed);
    }
}
```
--- 
■ **Note** You may use the base keyword whenever a subclass wants to access a public or protected member 
defined by a parent class. Use of this keyword is not limited to constructor logic.

---

### Keeping Family Secrets: The protected Keyword

---
■ **Note** Convention is that protected members are named Pascal-Cased (EmpName) and not UnderscoreCamel-Case (_empName).

---

The benefit of defining protected members in a base class is that derived types no longer have to 
access the data indirectly using public methods or properties.

Understand that as far as the object user is concerned, protected data is regarded as private (as 
the user is “outside” the family). Therefore, the following is illegal:

```cs
static void Main(string[] args)
{
 // Error! Can't access protected data from client code.
 Employee emp = new Employee();
 emp.empName = "Fred";
}
```

<br>

## Programming for Containment/Delegation

However, exposing the functionality of 
the contained object to the outside world requires delegation. Delegation is simply the act of adding public 
members to the containing class that use the contained object’s functionality.

```cs
class A
{
    private int _size;

    public int Size { get { return _size; } set { _size = value; } }
}

class B
{
    protected A a = new A();

    public B()
    {
    }

    public int GetASize() => a.Size;
}

class Program
{
    public static void Main(string[] args)
    {
        A a = new A();
        a.Size = 5;
        System.Console.WriteLine(a.Size);
    }
}
```

### Understanding Nested Type Definition

It is possible to define a type (enum, class, 
interface, struct, or delegate) directly within the scope of a class or structure. When you have done so, the 
nested (or “inner”) type is considered a member of the nesting (or “outer”) class and in the eyes of the 
runtime can be manipulated like any other member (fields, properties, methods, and events).

```cs
public class OuterClass
{
    // A public nested type can be used by anybody.
    public class PublicInnerClass
    { 
    }
    // A private nested type can only be used by members
    // of the containing class.
    private class PrivateInnerClass
    {
    }
}
```

- Nested types allow you to gain complete control over the access level of the inner 
type because they may be declared privately (recall that non-nested classes cannot 
be declared using the private keyword).

- Because a nested type is a member of the containing class, it can access private 
members of the containing class.

- Often, a nested type is useful only as a helper for the outer class and is not intended 
for use by the outside world.

```cs
class Outer
{
    private class InnerPrivate { }
    public class InnerPublic { }
}

class Program
{
    public static void Main(string[] args)
    {
        Outer.InnerPublic ip; // OK
        //Outer.InnerPrivate ip; NOT OK bcs of private
    }
}
```

The nesting process can be as “deep” as you require.

<br>

## The Third Pillar of OOP: C#’s Polymorphic Support

### The virtual and override Keywords

```cs
class Employe
{
    public virtual int GetBonus() { return 5; }
}

class Salesman : Employe
{
    public override int GetBonus()
    {
        return 10;
       // return base.GetBonus();
       // each overridden method is free to leverage the default behavior using the base keyword
    }
}

class Program
{
    public static void Main(string[] args)
    {
        Employe emp = new Employe();
        System.Console.WriteLine(emp.GetBonus()); // 5
        Salesman salesman = new Salesman();
        System.Console.WriteLine(salesman.GetBonus()); // 10
    }
}
```

---
■ **Note** Methods that have been marked with the virtual keyword are (not surprisingly) termed virtual 
methods

---

### Sealing Virtual Members

```cs
class Salesman : Employe
{
    public override sealed int GetBonus()
    {
        return base.GetBonus();
    }
}

class OverrideGetBonus : Salesman
{
    public override int GetBonus() // comp error, method is sealed
    {
        return 0;
    }
}
```

### Understanding Abstract Classes



```cs
// What exactly does this mean?
Employee X = new Employee();
```
In this example, the only real purpose of the Employee base class is to define common members for all 
subclasses. In all likelihood, you did not intend anyone to create a direct instance of this class, reason being 
that the Employee type itself is too general of a concept. For example, if I were to walk up to you and say “I’m 
an employee!”, I would bet your first question to me would be “What kind of employee are you?” “Are you a 
consultant, trainer, admin assistant, copyeditor, or White House aide?”

```cs
abstract class Employe
{
    private int _salary;
    public abstract int GetBonus();
    public int GetSalary() { return _salary; }
}

class Salesman : Employe
{
    public override int GetBonus()
    {
        return 5;
    }
}

...
// Error! Cannot create an instance of an abstract class!
Employee X = new Employee()
```

### Understanding the Polymorphic Interface

When a class has been defined as an abstract base class (via the abstract keyword), it may define any 
number of abstract members.

---
■ Note Abstract methods can be defined only in abstract classes. If you attempt to do otherwise, you will be 
issued a compiler error.

---

### Understanding Member Shadowing

C# provides a facility that is the logical opposite of method overriding, termed shadowing. Formally 
speaking, if a derived class defines a member that is identical to a member defined in a base class, the 
derived class has shadowed the parent’s version.

```cs
class Circle
{
    public void Draw()
    {
        Console.WriteLine("Drawing a Circle");
    }
}
//You figure that a ThreeDCircle “is-a” Circle, so you derive from your existing Circle type.
class ThreeDCircle : Circle
{
    public void Draw() // public new void Draw() will hide the inherited property
    {
        Console.WriteLine("Drawing a 3D Circle");
    }
}
```

---
```cs
'ThreeDCircle.Draw()' hides inherited member 'Circle.Draw()'. To make the current member 
override that implementation, add the override keyword. Otherwise add the new keyword.
```

---

<br>

## Understanding Base Class/Derived Class Casting Rules

```cs
static void CastingExamples()
{
    // A Manager "is-a" System.Object, so we can
    // store a Manager reference in an object variable just fine.
    object frank = new Manager("Frank Zappa", 9, 3000, 40000, "111-11-1111", 5);
    
    // A Manager "is-an" Employee too.
    Employee moonUnit = new Manager("MoonUnit Zappa", 2, 3001, 20000, "101-11-1321", 1);
    
    // A PtSalesPerson "is-a" SalesPerson.
    SalesPerson jill = new PtSalesPerson("Jill", 834, 3002, 100000, "111-12-1119", 90);
}
```

The first law of casting between class types is that when two classes are related by an “is-a” relationship, 
it is always safe to store a derived object within a base class reference. Formally, this is called an implicit cast, 
as “it just works” given the laws of inheritance. This leads to some powerful programming constructs.

```cs
object frank = new Manager("Frank Zappa", 9, 3000, 40000, "111-11-1111", 5);
// Error!
GivePromotion(frank);
```

The problem is that you are attempting to pass in a variable that is not declared as an Employee but 
a more general System.Object. Given that object is higher up the inheritance chain than Employee, the 
compiler will not allow for an implicit cast, in an effort to keep your code as type-safe as possible.

Even though you can figure out that the object reference is pointing to an Employee-compatible class 
in memory, the compiler cannot, as that will not be known until runtime. You can satisfy the compiler by 
performing an explicit cast. 

```cs
// OK!
GivePromotion((Manager)frank);
```

### The C# as Keyword

Be aware that explicit casting is evaluated at **runtime**, **not compile time**.

```cs
// Ack! You can't cast frank to a Hexagon, but this compiles fine!
object frank = new Manager();
Hexagon hex = (Hexagon)frank;
```

However, you would receive a runtime error, or, more formally, a runtime exception. 

```cs
object frank = new Manager();
Hexagon hex;
try
{
    hex = (Hexagon)frank;
}
catch (InvalidCastException ex)
{
    Console.WriteLine(ex.Message);
}
```

C# provides the as keyword to quickly determine at runtime whether a given type is compatible with 
another. When you use the as keyword, you are able to determine compatibility by checking against a null
return value. Consider the following:

```cs
// Use "as" to test compatibility.
object[] things = new object[4];
things[0] = new Hexagon();
things[1] = false;
things[2] = new Manager();
things[3] = "Last thing";
foreach (object item in things)
{
    Hexagon h = item as Hexagon;
    if (h == null)
    {
        Console.WriteLine("Item is not a hexagon");
    }
    else
    {
        h.Draw();
    }
}
```

### The C# is Keyword (Updated 7.0)

In addition to the as keyword, the C# language provides the is keyword to determine whether two items are 
compatible. Unlike the as keyword, however, the is keyword returns false, rather than a null reference, if 
the types are incompatible. 

Here, you are performing a runtime check to determine what the incoming base class reference is 
actually pointing to in memory:

```cs
static void GivePromotion(Employee emp)
{
    Console.WriteLine("{0} was promoted!", emp.Name);
    //Check if is SalesPerson, assign to variable s
    if (emp is SalesPerson s) // SalesPerson s => no need to cast !!!
    {
        Console.WriteLine("{0} made {1} sale(s)!", s.Name,
        s.SalesNumber);
    }
    //Check if is Manager, if it is, assign to variable m
    else if (emp is Manager m)
    {
        Console.WriteLine("{0} had {1} stock options...",
        m.Name, m.StockOptions);
    }
}
```

### Discards with the is Keyword (New 7.0)

The is keyword can also be used in conjunction with the discard variable placeholder. If you want to create a 
catchall in your if or switch statement, you can do so as follows:

```cs
if (obj is var _)
{
//do something
}
```

```cs
if (emp is SalesPerson s)
{
    Console.WriteLine("{0} made {1} sale(s)!", s.Name, s.SalesNumber);
    Console.WriteLine();
}
//Check if is Manager, if it is, assign to variable m
else if (emp is Manager m)
{
    Console.WriteLine("{0} had {1} stock options...", m.Name, m.StockOptions);
    Console.WriteLine();
}
else if (emp is var _)
{
    Console.WriteLine("Unable to promote {0}. Wrong employee type", emp.Name);
    Console.WriteLine();
}
```


```cs
switch (emp)
{
    case SalesPerson s when s.SalesNumber > 5:
        Console.WriteLine("{0} made {1} sale(s)!", emp.Name,
        s.SalesNumber);
        break;
    case Manager m:
        Console.WriteLine("{0} had {1} stock options...",
        emp.Name, m.StockOptions);
        break;
    case Employee _:
        Console.WriteLine("Unable to promote {0}. Wrong employee type", emp.Name);
        break;
}
```

<br>

## The Super Parent Class: System.Object

```cs
// Here we are explicitly deriving from System.Object.
class Car : object
{...}
```

A rundown of the functionality provided by some of the methods you’re most likely 
to use:

- By default, this method returns true only if the items being compared 
refer to the same item in memory. Thus, Equals() is used to compare 
object references, not the state of the object. Typically, this method is 
overridden to return true only if the objects being compared have the 
same internal state values (i.e., value-based semantics).

| Instance Method of Object Class | Meaning ing Life 
| ------------------------------- | ---------------- 
| `Equals()` | By default, this method returns true only if the items being compared refer to the same item in memory. Thus, Equals() is used to compare object references, not the state of the object. Typically, this method is overridden to return true only if the objects being compared have the same internal state values (i.e., value-based semantics). <br> Be aware that if you override Equals(), you should also override GetHashCode(), as these methods are used internally by Hashtabletypes to retrieve subobjects from the container.
| `GetHashCode()` | This method returns an int that identifies a specific object instance.
| `ToString()` | This method returns a string representation of this object, using the <namespace>.<type name> format (termed the fully qualified name). This method will often be overridden by a subclass to return a tokenized string of name/value pairs that represent the object’s internal state, rather than its fully qualified name.
| `GetType()` | This method returns a Type object that fully describes the object you are currently referencing. In short, this is a Runtime Type Identification (RTTI) method available to all objects.
| `MemberwiseClone()` | This method exists to return a member-by-member copy of the current object, which is often used when cloning an object.

```cs
class Program
{
    static void Main(string[] args)
    {
        Person p1 = new Person();
        
        // Use inherited members of System.Object.
        Console.WriteLine("ToString: {0}", p1.ToString());
        Console.WriteLine("Hash code: {0}", p1.GetHashCode());
        Console.WriteLine("Type: {0}", p1.GetType());
        
        // Make some other references to p1.
        Person p2 = p1;
        object o = p2;
        
        // Are the references pointing to the same object in memory?
        if (o.Equals(p1) && p2.Equals(o))
        {
            Console.WriteLine("Same instance!");
        }
    }
}
```

---
```cs
ToString: ObjectOverrides.Person //  ObjectOverrides is the namespace
Hash code: 58225482
Type: ObjectOverrides.Person
Same instance!
```

---

### Overriding System.Object.ToString()

```cs
public override string ToString() => 
$"[First Name: {FirstName}; Last Name: {LastName}; Age: {Age}";
```

However, always remember that a proper ToString() override should also account for 
any data defined up the chain of inheritance.

```cs
class A
{
    public override string ToString()
    {
        return "A";
    }
}

class B : A
{
}

class Program
{
    static void Main(string[] args)
    {
        A a = new A();
        Console.WriteLine(a); // A
        B b = new B();
        Console.WriteLine(b); // A
    }
}
```

### Overriding System.Object.Equals()

```cs
public override bool Equals(object obj)
{
    if (!(obj is Person temp))
    {
        return false;
    }
    if (temp.FirstName == this.FirstName
    && temp.LastName == this.LastName
    && temp.Age == this.Age)
    {
        return true;
    }
    return false;
}
```

### Overriding System.Object.GetHashCode()

```cs
// Assume we have an SSN property as so.
class Person
{
    public string SSN { get; } = "";
    public Person(string fName, string lName, int personAge,
    string ssn)
    {
        FirstName = fName;
        LastName = lName;
        Age = personAge;
        SSN = ssn;
    }
    
    // Return a hash code based on unique string data.
    public override int GetHashCode() => SSN.GetHashCode();
}
```

### The Static Members of System.Object

```cs
static void StaticMembersOfObject()
{
    // Static members of System.Object.
    Person p3 = new Person("Sally", "Jones", 4);
    Person p4 = new Person("Sally", "Jones", 4);
    Console.WriteLine("P3 and P4 have same state: {0}", object.Equals(p3, p4));
    Console.WriteLine("P3 and P4 are pointing to same object: {0}",
    object.ReferenceEquals(p3, p4));
}
```

---
```cs
P3 and P4 have the same state: True
P3 and P4 are pointing to the same object: False
```

---

<br>
<br>
<br>
<br>
<br>
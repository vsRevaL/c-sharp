# Object-Oriented Programming with C#, Chapter 6

<br>

# Understanding Inheritance and Polymorphism

- The Basic Mechanics of Inheritance
- The Second Pillar of OOP: The Details of Inheritance
- Programming for Containment/Delegation
- The Third Pillar of OOP: C#’s Polymorphic Support


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


<br>
<br>
<br>
<br>
<br>
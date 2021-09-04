# Delegates, Events, and Lambda Expressions

- Delegate Type
- Sending Object State Notifications Using Delegates
- Understanding Generic Delegates
- Understanding C# Events
- Lambda Expressions

## Delegate Type

Specifically, a delegate 
maintains three important pieces of information:

- The address ot the method on wich it makes calls
- The parameters (if any) of this method
- The return type (if any) of this method

■ **Note** .NET Core delegates can point to either static or instance methods.

### Defining a Delegate Type in C#

```cs
    // This delegate can point to any method,
    // taking two integers and returning an integer.
    public delegate int BinaryOp(int x, int y);
```

### The System.MulticastDelegate and System.Delegate Base Classes

Select Members of System.MulticastDelegate/System.Delegate

| Method | Meaning
| ------ | -------
| Method | This property returns a System.Reflection.MethodInfo object that represents details of a static method maintained by the delegate
| Target | It the method to be called is defined at the object level, Target returns an object that represents the method maintained by the delegate. It it's `null` than the method to be called is a static member
| Combine() | This static method adds a method to the list maintained by the delegate. In C# you trigger this method using the overloaded += operator as shorthand notation
| GetInvocationList() | returns an array of System.Delegate objects, each representing a particular method that may be invoked.
| Remove(), RemoveAll() | These static methods remove a method (or all methods) from the delegate's invocation list. In C# the Remove() method can be called indirectly using the overloaded -= operator.



### The Simplest Possible Delegate Example

```cs
namespace SimpleDelegate
{
    // This delegate can point to any method,
    // taking two integers and returning an integer.
    public delegate int BinaryOp(int x, int y);
    
    // This class contains methods BinaryOp will
    // point to.
    public class SimpleMath
    {
        public static int Add(int x, int y) => x + y;
        public static int Subtract(int x, int y) => x - y;
    }
    class Program
    {
        static void Main(string[] args)
        {
            // Create a BinaryOp delegate object that
            // "points to" SimpleMath.Add().
            BinaryOp b = new BinaryOp(SimpleMath.Add);
            
            // Invoke Add() method indirectly using delegate object.
            Console.WriteLine("10 + 10 is {0}", b(10, 10));
        }
    }
}
```

### Investigating a Delegate Object

```cs
static void DisplayDelegateInfo(Delegate delObj)
{
    // Print the names of each member in the
    // delegate's invocation list.
    foreach (Delegate d in delObj.GetInvocationList())
    {
        Console.WriteLine("Method Name: {0}", d.Method);
        Console.WriteLine("Type Name: {0}", d.Target);
    }
}
```
---
```cs
Method Name: Int32 Add(Int32, Int32)
Type Name:
```

---

We can't see the Target because the delegate is pointing to a `static` method

If we remove the static keyword and do the following:

```cs
            SimpleMath sm = new SimpleMath();
            BinaryOp obj = new BinaryOp(sm.Add);
            DisplayDelegateInfo(obj);
```
---
```cs
Method Name: Int32 Add(Int32, Int32)
Type Name: SimpleDelegate.SimpleMath
```

---

<br>

## Sending Object State Notifications Using Delegates

```cs
public class Car
{
    // Internal state data.
    public int CurrentSpeed { get; set; }
    public int MaxSpeed { get; set; } = 100;
    public string PetName { get; set; }

    // Is the car alive or dead?
    private bool _carIsDead;

    // Class constructors.
    public Car() { }
    public Car(string name, int maxSp, int currSp)
    {
        CurrentSpeed = currSp;
        MaxSpeed = maxSp;
        PetName = name;
    }

    // 1) Define a delegate type.
    public delegate void CarEngineHandler(string msgForCaller);
    // 2) Define a member variable of this delegate.
    private CarEngineHandler _listOfHandlers;
    // 3) Add registration function for the caller.
    public void RegisterWithCarEngine(CarEngineHandler methodToCall)
    {
        _listOfHandlers = methodToCall;
    }

    // 4) Implement the Accelerate() method to invoke the delegate's
    // invocation list under the correct circumstances.
    public void Accelerate(int delta)
    {
        // If this car is "dead", send a dead message.
        if (_carIsDead)
        {
            _listOfHandlers?.Invoke("Sorry, this car is dead..");
        }
        else
        {
            CurrentSpeed += delta;
            // Is this car "almost dead"?
            if (10 == (MaxSpeed - CurrentSpeed))
            {
                _listOfHandlers?.Invoke("You better slow down playa");
            }
            if (CurrentSpeed >= MaxSpeed)
            {
                _carIsDead = true;
            }
            else
            {
                Console.WriteLine("CurrentSpeed = {0}", CurrentSpeed);
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        // First, make a Car object.
        Car c1 = new Car("SlugBug", 100, 10);

        // Now, tell the car which method to call
        // when it wants to send us messages.
        c1.RegisterWithCarEngine(new Car.CarEngineHandler(OnCarEngineEvent));

        // Speed up (this will trigger the events).
        Console.WriteLine("***** Speeding up *****");
        for (int i = 0; i < 6; i++)
        {
            c1.Accelerate(20);
        }
    }
    // This is the target for incoming events.
    public static void OnCarEngineEvent(string msg)
    {
        Console.WriteLine("\n*** Message From Car Object ***");
        Console.WriteLine("=> {0}", msg);
        Console.WriteLine("********************\n");
    }
}
```

---
```cs
***** Delegates as event enablers *****
***** Speeding up *****
CurrentSpeed = 30
CurrentSpeed = 50
CurrentSpeed = 70
***** Message From Car Object *****
=> Careful buddy! Gonna blow!
***********************************
CurrentSpeed = 90
***** Message From Car Object *****
=> Sorry, this car is dead...
***********************************
```
---

### Enabling Multicasting

```cs
    // Now with multicasting support!
    // Note we are now using the += operator, not
    // the assignment operator (=).
    public void RegisterWithCarEngine(CarEngineHandler methodToCall)
    {
        _listOfHandlers += methodToCall;
    }
```

### Removing Targets from a Delegate’s Invocation List

```cs
public class Car
{
...
 public void UnRegisterWithCarEngine(CarEngineHandler methodToCall)
    {
        _listOfHandlers -= methodToCall;
    }
}
```

```cs
// Unregister from the second handler.
c1.UnRegisterWithCarEngine(handler2);
// We won't see the "uppercase" message anymore!
Console.WriteLine("***** Speeding up *****");
for (int i = 0; i < 6; i++)
{
    c1.Accelerate(20);
}
```

### Method Group Conversion Syntax

C# provides a shortcut termed method group conversion. This feature allows you 
to supply a direct method name, rather than a delegate object, when calling methods that take delegates as 
arguments.

```cs
// Register the simple method name.
 c2.RegisterWithCarEngine(OnCarEngineEvent);
```

<br>

## Understanding Generic Delegates

For example, assume you 
want to define a delegate type that can call any method returning void and receiving a single parameter. If 
the argument in question may differ, you could model this using a type parameter. To illustrate, consider the 
following code:

```cs
using System;

// This generic delegate can represent any method
// returning void and taking a single parameter of type T.
public delegate void MyGenericDelegate<T>(T arg);

class Program
{
    static void Main(string[] args)
    { 
        // Register targets.
        MyGenericDelegate<string> strTarget = new MyGenericDelegate<string>(StringTarget);
        strTarget("Some string data");
        
        //Using the method group conversion syntax
        MyGenericDelegate<int> intTarget = IntTarget;
        intTarget(9);
    }
    static void StringTarget(string arg)
    {
        Console.WriteLine("arg in uppercase is: {0}", arg.ToUpper());
    }
    static void IntTarget(int arg)
    {
        Console.WriteLine("++arg is: {0}", ++arg);
    }
}
```

### The Generic Action<> and Func<> Delegates

In many cases, you simply want “some 
delegate” that takes a set of arguments and possibly has a return value other than void. In these cases, you 
can use the framework’s built-in Action<> and Func<> delegate types.
It can take up to **16 parameters**.

```cs
class Program
{
    static void Main(string[] args)
    {
        Action<string, ConsoleColor, int> actionTarget = DisplayMessage;
        actionTarget("Action Message!", ConsoleColor.Yellow, 5);
    }

    // This is a target for the Action<> delegate.
    static void DisplayMessage(string msg, ConsoleColor txtColor, int printCount)
    {
        // Set color of console text.
        ConsoleColor previous = Console.ForegroundColor;
        Console.ForegroundColor = txtColor;
        for (int i = 0; i < printCount; i++)
        {
            Console.WriteLine(msg);
        }
        // Restore color.
        Console.ForegroundColor = previous;
    }
}
```

Action<> delegate type can point only to methods that take a void return value. If 
you want to point to a method that does have a return value (and don’t want to bother writing the custom 
delegate yourself), you can use Func<>.

The generic Func<> delegate can point to methods that (like Action<>) take up to **16 parameters** and a 
custom return value.

Be aware that the final type parameter of Func<> is **always the return value** of the method.

```cs
class Program
{
    static void Main(string[] args)
    {
        Func<int, int, int> funcTarget = Add;
        Console.WriteLine(funcTarget(50,50)); // 100
    }

    // This is a target for the Action<> delegate.
    static int Add(int x, int y)
    {
        return x + y;
    }
}
```

<br>

## Understanding C# Events

### The C# event Keyword

When the compiler processes the event keyword, you are 
automatically provided with registration and unregistration methods, as well as any necessary member 
variables for your delegate types. These delegate member variables are always declared private, and, 
therefore, they are not directly exposed from the object firing the event. To be sure, the event keyword can 
be used to simplify how a custom class sends out notifications to external objects.

Defining an event is a two-step process. First, you need to define a delegate type (or reuse an existing 
one) that will hold the list of methods to be called when the event is fired. Next, you declare an event (using 
the C# event keyword) in terms of the related delegate type.

```cs
public class Car
{
    // This delegate works in conjunction with the
    // Car's events.
    public delegate void CarEngineHandler(string msg);
    // This car can send these events.
    public event CarEngineHandler Exploded;
    public event CarEngineHandler AboutToBlow;
    ...
     public void Accelerate(int delta)
    {
        // If the car is dead, fire Exploded event.
        if (_carIsDead)
        {
            Exploded?.Invoke("Sorry, this car is dead...");
        }
        else
        {
            CurrentSpeed += delta;
            // Almost dead?
            if (10 == MaxSpeed - CurrentSpeed)
            {
                AboutToBlow?.Invoke("Careful buddy! Gonna blow!");
            }
            // Still OK!
            if (CurrentSpeed >= MaxSpeed)
            {
                _carIsDead = true;
            }
            else
            {
                Console.WriteLine("CurrentSpeed = {0}", CurrentSpeed);
            }
        }
    }
}
```

### Listening to Incoming Events

When you want to register with an event, follow the 
pattern shown here:

```cs
// NameOfObject.NameOfEvent += new RelatedDelegate(functionToCall);
Car.CarEngineHandler d = new Car.CarEngineHandler(CarExplodedEventHandler);
myCar.Exploded += d;

// OR

Car.CarEngineHandler d = CarExplodedEventHandler;
myCar.Exploded += d;
```

```cs
using System;

public class Car
{
    // This delegate works in conjunction with the
    // Car's events.
    public delegate void CarEngineHandler(string msg);
    private bool _carIsDead = false;
    public void setCarIsDead(bool l) => _carIsDead = l;
    public int CurrentSpeed { get; set; } = 10;
    public int MaxSpeed { get; set; } = 100;
    // This car can send these events.
    public event CarEngineHandler Exploded;
    public event CarEngineHandler AboutToBlow;
    public void Accelerate(int delta)
    {
        // If the car is dead, fire Exploded event.
        if (_carIsDead)
        {
            Exploded?.Invoke("Sorry, this car is dead...");
        }
        else
        {
            CurrentSpeed += delta;
            // Almost dead?
            if (10 == MaxSpeed - CurrentSpeed)
            {
                AboutToBlow?.Invoke("Careful buddy! Gonna blow!");
            }
            // Still OK!
            if (CurrentSpeed >= MaxSpeed)
            {
                _carIsDead = true;
            }
            else
            {
                Console.WriteLine("CurrentSpeed = {0}", CurrentSpeed);
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Car c1 = new Car();

        // Register event handlers.
        c1.AboutToBlow += CarAboutToBlow;
        c1.AboutToBlow += CarAlmostDoomed;
        c1.Exploded += CarExploded;

        Console.WriteLine("***** Speeding up *****");
        for (int i = 0; i < 6; i++)
        {
            c1.Accelerate(20);
        }
        // Remove CarExploded method
        // from invocation list.
        //c1.Exploded -= CarExploded;
        Console.WriteLine("\n***** Speeding up *****");
        c1.CurrentSpeed = 10;
        c1.setCarIsDead(false);
        c1.Exploded -= CarExploded;
        for (int i = 0; i < 6; i++)
        {
            c1.Accelerate(20);
        }
    }

    public static void CarAboutToBlow(string msg)
    {
        Console.WriteLine(msg);
    }

    public static void CarAlmostDoomed(string msg)
    {
        Console.WriteLine($"=> Critical Message from Car: {msg}");
    }

    public static void CarExploded(string msg)
    {
        Console.WriteLine(msg);
    }
```

### Creating Custom Event Arguments

The System.EventArgs
base class represents an event that is not sending any custom information.

```cs
public delegate void CarEngineHandler(object sender, CarEventArgs e);
```

Here, when firing the events from within the Accelerate() method, you would now need to supply a 
reference to the current Car (via the this keyword) and an instance of the CarEventArgs type. For example, 
consider the following partial update:

```cs
 // If the car is dead, fire Exploded event.
 if (carIsDead)
 {
 Exploded?.Invoke(this, new CarEventArgs("Sorry, this car is dead..."));
 }
```

```cs
public class CarEventArgs
{
    public readonly string msg;
    public CarEventArgs(string message)
    {
        msg = message;
    }
}

public class Car
{
    // This delegate works in conjunction with the
    // Car's events.
    public delegate void CarEngineHandler(object sender, CarEventArgs e);
    private bool _carIsDead = false;
    public void setCarIsDead(bool l) => _carIsDead = l;
    public int CurrentSpeed { get; set; } = 10;
    public int MaxSpeed { get; set; } = 100;
    // This car can send these events.
    public event CarEngineHandler Exploded;
    public event CarEngineHandler AboutToBlow;
    public void Accelerate(int delta)
    {
        // If the car is dead, fire Exploded event.
        if (_carIsDead)
        {
            Exploded?.Invoke(this, new CarEventArgs("Sorry, this car is dead..."));
        }
        else
        {
            CurrentSpeed += delta;
            // Almost dead?
            if (10 == MaxSpeed - CurrentSpeed)
            {
                AboutToBlow?.Invoke(this, new CarEventArgs("Careful buddy! Gonna blow!"));
            }
            // Still OK!
            if (CurrentSpeed >= MaxSpeed)
            {
                _carIsDead = true;
            }
            else
            {
                Console.WriteLine("CurrentSpeed = {0}", CurrentSpeed);
            }
        }
    }
}
```

### The Generic EventHandler<T> Delegate

```cs
public class Car
{
    ...
    public event EventHandler<CarEventArgs> Exploded;
    public event EventHandler<CarEventArgs> AboutToBlow;
}

static void Main(string[] args)
{
    Console.WriteLine("***** Prim and Proper Events *****\n");
    // Make a car as usual.
    Car c1 = new Car("SlugBug", 100, 10);
    // Register event handlers.
    c1.AboutToBlow += CarIsAlmostDoomed;
    c1.AboutToBlow += CarAboutToBlow;
    EventHandler<CarEventArgs> d = CarExploded;
    c1.Exploded += d;
    ...
}

```

<br>

## Understanding C# Anonymous Methods

As you have seen, when a caller wants to listen to incoming events, it must define a custom method in a class 
(or structure) that matches the signature of the associated delegate:

```cs
class Program
{
    static void Main(string[] args)
    {
        SomeType t = new SomeType();
        
        // Assume "SomeDelegate" can point to methods taking no
        // args and returning void.
        t.SomeEvent += new SomeDelegate(MyEventHandler);
    }
    
    // Typically only called by the SomeDelegate object.
    public static void MyEventHandler()
    {
        // Do something when event is fired.
    }
}
```

It is possible to associate an event directly to a block of code statements at the 
time of event registration. Formally, such code is termed an anonymous method.

```cs
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("***** Anonymous Methods *****\n");
        Car c1 = new Car();
        // Register event handlers as anonymous methods.
        c1.AboutToBlow += delegate
        {
            Console.WriteLine("Eek! Going too fast!");
        };

        c1.AboutToBlow += delegate (object sender, CarEventArgs e)
        {
            Console.WriteLine($"Message from Car: {e.msg}");
        };

        // This will eventually trigger the events.
        for (int i = 0; i < 6; i++)
        {
            c1.Accelerate(20);
        }
    }
}
```

The basic syntax of an anonymous method 
matches the following pseudocode:

```cs
class Program
{
    static void Main(string[] args)
    {
        SomeType t = new SomeType();
        t.SomeEvent += delegate (optionallySpecifiedDelegateArgs)
        { /* statements */ };
    }
}
```

When handling the first AboutToBlow event within the previous Main() method, notice that you are not 
specifying the arguments passed from the delegate.

```cs
        c1.AboutToBlow += delegate
        {
            Console.WriteLine("Eek! Going too fast!");
        };
```

You are not required to receive the incoming arguments sent by a specific event. 
However, if you want to make use of the possible incoming arguments, you will need to specify the 
parameters prototyped by the delegate type

### Accessing Local Variables

Anonymous methods can acces local vars, they're termed *outer variables*. A few important points about them:
- An anonymous method cannot access ref or out parameters of the defining method.
- An anonymous method cannot have a local variable with the same name as a local 
variable in the outer method.
- An anonymous method can access instance variables (or static variables, as 
appropriate) in the outer class scope.
- An anonymous method can declare local variables with the same name as outer 
class member variables (the local variables have a distinct scope and hide the outer 
class member variables).

```cs
class Program
{
    static void Main(string[] args)
    {
        int aboutToBlowCounter = 0;
        Car c = new Car();

        // Register event handlers as anonymous methods.
        c.AboutToBlow += delegate
        {
            aboutToBlowCounter++;
            Console.WriteLine("Going too fast mate!");
        };

        c.AboutToBlow += delegate (object sender, CarEventArgs e)
        {
            aboutToBlowCounter++;
            Console.WriteLine("Critical Message from Car: {0}", e.msg);
        };
        
        // This will eventually trigger the events.
        for (int i = 0; i < 6; i++)
        {
            c.Accelerate(20);
        }

        Console.WriteLine("AboutToBlow event was fired {0} times.", aboutToBlowCounter);
    }
}
```

<br>

## Lambda Expressions

C# supports the ability to handle events “inline” by assigning a block of code statements 
directly to an event using anonymous methods, rather than building a stand-alone method to be called by 
the underlying delegate.

```cs
// Method of the System.Collections.Generic.List<T> 
class.public List<T> FindAll(Predicate<T> match)
```

As you can see, this method returns a new List<T> that represents the subset of data.
This delegate type can 
point to any method returning a bool and takes a single type parameter as the only input parameter

```cs
// This delegate is used by FindAll() method
// to extract out the subset.
public delegate bool Predicate<T>(T obj);
```

Example:

```cs
class Program
{
    static void Main(string[] args)
    {
        TraditionalDelegateSyntax();
    }
    static void TraditionalDelegateSyntax()
    {
        // Make a list of integers.
        List<int> list = new List<int>();
        list.AddRange(new int[] { 20, 1, 4, 8, 9, 44 });

        // Call FindAll() using traditional delegate syntax.
        Predicate<int> callback = IsEvenNumber;
        List<int> evenNumbers = list.FindAll(callback);
        
        Console.WriteLine("Here are your even numbers:");
        foreach (int evenNumber in evenNumbers)
        {
            Console.Write("{0}\t", evenNumber);
        }
    }

    // Target for the Predicate<> delegate.
    static bool IsEvenNumber(int i)
    {
        // Is it an even number?
        return (i % 2) == 0;
    }
}
```

OR for cleaner code:

```cs
// Now, use an anonymous method.
List<int> evenNumbers = list.FindAll(delegate (int i) { return i % 2 == 0; });
```

Want to be more cleaner?

Lambda expressions can be used to **simplify** the call to FindAll() **even more**. When you use lambda syntax, there is no trace of the underlying delegate object whatsoever.

```cs
// Now, use a C# lambda expression.
List<int> evenNumbers = list.FindAll(i => i % 2 == 0);
```

With LINQ query syntax:

```cs
var evenNumbers = from x in list where x % 2 == 0 select x;
```

### Dissecting a Lambda Expression

A lambda expression is written by first defining a parameter list, followed by the => token (C#’s token for the 
lambda operator found in the lambda calculus), followed by a set of statements (or a single statement) that 
will process these arguments. From a high level, a lambda expression can be understood as follows:

`ArgumentsToProcess => StatementsToProcessThem`

Within the LambdaExpressionSyntax() method, things break down like so:

```cs
// "i" is our parameter list.
// "(i % 2) == 0" is our statement set to process "i".
List<int> evenNumbers = list.FindAll(i => i % 2 == 0);
```

### Processing Arguments Within Multiple Statements

You can build lamba expressions using multiple statement blocks.

```cs
    static void LambdaExpressionSyntax()
    {
        // Make a list of integers.
        List<int> list = new List<int>();
        list.AddRange(new int[] { 20, 1, 4, 8, 9, 44 });
        
        // Now process each argument within a group of
        // code statements.
        List<int> evenNumbers = list.FindAll((i) =>
        {
            Console.WriteLine("value of i is currently: {0}", i);
            bool isEven = ((i % 2) == 0);
            return isEven;
        });

        Console.WriteLine("Here are your even numbers:");
        foreach (int evenNumber in evenNumbers)
        {
            Console.WriteLine(evenNumber);
        }
    }
```

### Lambda Expressions with Multiple (or Zero) Parameters

```cs
public class SimpleMath
{
    public delegate void MathMessage(string msg, int result);
    private MathMessage _mmDelegate;
    public void SetMathHandler(MathMessage target)
    {
        _mmDelegate = target;
    }

    public void Add(int x, int y)
    {
        _mmDelegate?.Invoke("Adding has completed!", x + y);
    }
}
```

Notice that the MathMessage delegate type is expecting two parameters. To represent them as a lambda 
expression, the Main() method might be written as follows:

```cs
class Program
{
    static void Main(string[] args)
    {
        SimpleMath sm = new SimpleMath();
        sm.SetMathHandler((string msg, int result) =>
        {
            Console.WriteLine("Message: " + msg + "\nResult: " + result);
        });

        sm.Add(50, 50);
    }
}
```

Finally, if you are using a lambda expression to interact with a delegate taking no parameters at all, you 
may do so by supplying a pair of empty parentheses as the parameter.

```cs
public delegate string VerySimpleDelegate();
class Program
{
    static void Main(string[] args)
    {
        VerySimpleDelegate d = new VerySimpleDelegate(() => "Enjoy your string!");
        Console.WriteLine(d());
    }
}
```

### Retrofitting the CarEvents Example Using Lambda Expressions

Here is a simplified version of that project’s Program
class, which makes use of lambda expression syntax (rather than the raw delegates) to hook into each event 
sent from the Car object:

```cs
using System;

public class CarEventArgs
{
    public readonly string msg;
    public CarEventArgs(string msg) => this.msg = msg;
}

public class Car
{
    private bool _carIsDead = false;
    public int CurrentSpeed { get; set; } = 10;
    public int MaxSpeed { get; set; } = 100;

    public delegate void CarEngineHandler(object sender, CarEventArgs e);
    public event CarEngineHandler Exploded;
    public event CarEngineHandler AboutToBlow;

    public void Accelerate(int delta)
    {
        if (_carIsDead)
        {
            Exploded?.Invoke(this, new CarEventArgs("Sry, this car is dead"));
        }
        else
        {
            CurrentSpeed += delta;
            if (10 == MaxSpeed - CurrentSpeed)
            {
                AboutToBlow?.Invoke(this, new CarEventArgs("Careful buddy, gonna blow!"));
            }

            if (CurrentSpeed >= MaxSpeed)
            {
                _carIsDead = true;
            }
            else
            {
                Console.WriteLine("Current speed= " + CurrentSpeed);
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Car c1 = new Car();
        // Hook into events with lambdas
        c1.AboutToBlow += (sender, e) => { Console.WriteLine(e.msg); };
        c1.Exploded += (sender, e) => { Console.WriteLine(e.msg); };

        //Speed up (this will trigger the events)
        Console.WriteLine("\n***** Speeding up *****");
        for(int i = 0; i < 6; i++)
        {
            c1.Accelerate(20);
        }
    }
}
```

### Lambdas and Expression-Bodied Members (Updated 7.0)

```cs
static class SimpleMath
{
    public static int Add(int x, int y) => x + y;
    public static void PrintSum(int x, int y) => Console.WriteLine(Add(x,y));
}

class Program
{
    static void Main(string[] args)
    {
        SimpleMath.PrintSum(7, 1); // 8
    }
}
```

<br>
<br>
<br>
<br>
<br>
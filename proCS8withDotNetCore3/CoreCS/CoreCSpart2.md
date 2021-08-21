# Core C# Programming Constructs, Part 2 - Chaper 4

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

## Understanding Method Overloading

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

## Uderstanding the `enum` Type

// To-do

<br>
<br>
<br>
<br>
<br>
<br>
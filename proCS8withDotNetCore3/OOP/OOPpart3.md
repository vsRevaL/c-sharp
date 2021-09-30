# Object-Oriented Programming with C#, Chapter 7

<br>

# Understanding Structured Exception Handling

## Errors, Bugs and Exceptions

**Bugs**

- Errors made by the programmer, eg. memory leak in.

**User error**

- Caused by the individual running your app, eg. wrong input

**Exception**

- Runtime anomalies that are difficult, if not impossible, to account for while programming your application. Eg. connecting to database that no longer exists, opening corrupted XML file.

<br>

## The Role of .NET Exception Handling

### The Building Blocks of .NET Exception Handling

Programming with structured exception handling involves the use of four interrelated entities:

- A class type that represents the details of the exception
- A member that throws an instance of the exception class to the caller under the 
correct circumstances
- A block of code on the caller’s side that invokes the exception-prone member
- A block of code on the caller’s side that will process (or catch) the exception, should 
it occur

Keywords: `try, catch, throw, finally, and when`


## Example of Custom Exception

```cs
[Serializable]
public class CarIsDeadException : ApplicationException
{
    public CarIsDeadException(string cause, DateTime time)
        : this(cause, time, string.Empty) { }
    public CarIsDeadException(string cause, DateTime time, string message)
        : this(cause, time, string.Empty, null) { }
    public CarIsDeadException(string cause, DateTime time, string message, System.Exception inner)
    : base(message, inner)
    {
        CauseOfError = cause;
        ErrorTimeStamp = time;
    }
    protected CarIsDeadException(string cause, DateTime time, SerializationInfo info, StreamingContext context)
    : base(info, context)
    {
        CauseOfError = cause;
        ErrorTimeStamp = time;
    }

    // Any additional custom properties and data members...
}

// Throw the custom CarIsDeadException.
public void Accelerate(int delta)
{
    // ...
    throw new CarIsDeadException($"{Car} has overheated!", "You have a lead foot", DateTime.Now)
    {
        HelpLink = "http://www.CarsRUs.com",
    };
    // ...
}
```

<br>
<br>
<br>
<br>
<br>
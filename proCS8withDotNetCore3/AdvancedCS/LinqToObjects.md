# Some notes about LINQ: 2020.07.28

Language INtegrated Query

LINQ is the name for a set of technologies based on the integration of query capabilities directly into the C# language

Before LINQ:
- Object syntax -> Objects
- XML syntax -> XML
- Entity syntax -> Entity
- Dataset syntax -> Dataset

Each of those above had their own syntax, with LINQ we can use the LINQ syntax, so you don't have to learn all of the different syntax types.


## Difference between Query & Method Syntax

```cs
class Program
{
    static void Main(string[] args)
    {
        // Step 1: Getting data source
        List<int> nums = new List<int> { 1, 2, 3, 4, 5, 6 };

        // Step 2: Writing query - Query syntax
        IEnumerable<int> query = from i in nums where i % 2 == 0 select i;

        // Step 2: Writing query - Method syntax
        IEnumerable<int> methodQuery = nums.Where(i => i % 2 == 0);

        // Step 3: Executing query
        foreach(int number in query)
        {
            Console.WriteLine(number);
        }
    }
}
```

Comparison

|  | Query Syntax | Method Syntax
| -------------------------------- | ------------ | --------------
| List of LINQ Operators supported | Limited | Supports all |
| 'select' clause in syntax | Mandatory | Optional
| Ease of use | Simple to read & write | A bit difficult
| LINQ Operators | All in lowercase | Starts with uppercase 
| Compile time action | Converted to Method syntax | Not applicable

References:
- https://en.wikipedia.org/wiki/Language_Integrated_Query
- https://www.youtube.com/watch?v=KWe-sZ10bIc&list=PLb__S-rkKheyoCVIfhWzrA8C_J_Wd2sx_


<br>
<br>
<br>
<br>
<br>
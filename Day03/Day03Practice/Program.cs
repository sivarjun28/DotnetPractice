// See https://aka.ms/new-console-template for more information

//Arrays
//Declaration And Intialization
using System.Text;

int[] numbers = new int[5];
int[] numbers2 = { 1, 2, 3, 4, 5 };
int[] numbers3 = new int[] { 8, 9, 7, 6, 5, 2 };

//Arrays Properties 
System.Console.WriteLine(numbers.Length);
System.Console.WriteLine(numbers.Rank);

//Accessing 
numbers[0] = 10;
numbers[1] = 11;
numbers[2] = 12;
numbers[3] = 13;
numbers[4] = 14;
int first = numbers[0];

//Iterating

foreach (int num in numbers)
{
    System.Console.WriteLine(num);
}
for (int i = 0; i < numbers3.Length; i++)
{
    System.Console.WriteLine(numbers3[i]);
}

//Multi Dimensional Array
int[,] matrix1 = new int[3, 4]; //3 rows 4 colums
int[,] matrix2 = {
                {1,2,3,4},
                {5,5,6,7,},
                {7,8,9,0}
                };
matrix1[2, 2] = 21;
int val = matrix2[2, 3];
System.Console.WriteLine(val);

//dimensions
System.Console.WriteLine(matrix2.GetLength(0));
System.Console.WriteLine(matrix2.GetLength(1));

//Iterate
for (int i = 0; i < matrix2.GetLength(0); i++)
{
    for (int j = 0; j < matrix2.GetLength(1); j++)
    {
        System.Console.Write($"{matrix2[i, j]} ");
    }
    System.Console.WriteLine();
}

//Jagged Arrays
int[][] jagged = new int[3][];
jagged[0] = new int[] { 1, 3 };
jagged[1] = new int[] { 7, 3, 4, 5 };
jagged[2] = new int[] { 1 };

int val2 = jagged[1][1];

//Iterate
foreach (int[] row in jagged)
{
    System.Console.WriteLine(string.Join(",", row));
}
System.Console.WriteLine(jagged.GetLength(0));
// System.Console.WriteLine(jagged.GetLength(1));

//Array Methods 
int[] newArr = { 7, 8, 4, 12, 87, 1, 0 };

Array.Sort(newArr);
Array.Reverse(newArr);

int index = Array.IndexOf(newArr, 12);
System.Console.WriteLine(index);
bool exists = Array.Exists(newArr, n => n > 5);
System.Console.WriteLine(exists);

int[] copy = new int[newArr.Length];
Array.Copy(newArr, copy, newArr.Length);


int? firstLarge = Array.Find(newArr, n => n > 5);
System.Console.WriteLine(firstLarge);
int[] allLarge = Array.FindAll(newArr, n => n > 5);


Array.Resize(ref newArr, 10);
System.Console.WriteLine(newArr.Length);

//Advanced String methods

String greet = " Hello Sivarjun! ";
System.Console.WriteLine(greet.Trim());
System.Console.WriteLine(greet.TrimStart());
System.Console.WriteLine(greet.TrimEnd());

//case conversion
System.Console.WriteLine(greet.ToLower());
System.Console.WriteLine(greet.ToUpper());

//searching
System.Console.WriteLine(greet.Contains("Hello"));
System.Console.WriteLine(greet.StartsWith(" "));
System.Console.WriteLine(greet.EndsWith("jun"));
System.Console.WriteLine(greet.IndexOf("Hello"));
System.Console.WriteLine(greet.LastIndexOf("n"));

//substring
System.Console.WriteLine(greet.Substring(1, 6));
System.Console.WriteLine(greet.Substring(8));

//replace
System.Console.WriteLine(greet.Replace("Hello", "hi"));
System.Console.WriteLine(greet.Replace(" ", ""));

//splitting 
string csv = "apple, bananana, orange";
string[] fruits = csv.Split(", ");


string sentence = "Iam working in VSC";
string[] split = sentence.Split(' ');

//withoptions
string data = "a,,b,,,c";
string[] parts = data.Split(',', StringSplitOptions.RemoveEmptyEntries);


//string formatting
System.Console.WriteLine(string.Format("Name: {0}, Age: {1}", "Arjun", 30));

//interpolation
string name = "Arjun";
int age = 23;
System.Console.WriteLine($"Name: {name}, Age: {age}");

//Allignment

System.Console.WriteLine($"{"Left",-10}");

System.Console.WriteLine($"{"Right",10}");

//Formating numbers
double price = 12345.89;
System.Console.WriteLine($"{price:C}");
System.Console.WriteLine($"{price:F2}");
System.Console.WriteLine($"{price:N}");
System.Console.WriteLine($"{price:P}");

//date formats
DateTime now = DateTime.Now;
System.Console.WriteLine($"{now:yyyy-mm-dd}");
System.Console.WriteLine($"{now:HH:mm:ss}");
System.Console.WriteLine($"{now:dddd, MMMM dd, yyyy}");

//StringBuilder Deep Dive
StringBuilder sb = new StringBuilder();
sb.Append("Hello ");
sb.Append("World");
sb.Append("!");
System.Console.WriteLine(sb.ToString());//HelloWorld!

//chaining
sb.Append(" One")
.Append(" Two")
.Append(" Three");

sb.Insert(0, "Start:");
System.Console.WriteLine(sb.ToString());//Start:Hello World! One Two Three

sb.Replace("Hello", "Hi");
sb.Remove(0, 6);
System.Console.WriteLine(sb.ToString());//Hi World! One Two Three

StringBuilder sb2 = new StringBuilder(100);
System.Console.WriteLine(sb2.Capacity);//10
System.Console.WriteLine(sb2.Length);//0

//List - Most Common Collection

List<int> num1 = new List<int>();
List<String> names = new List<string> { "Arjun", "siva", "sumanth" };

//Add elements
num1.Add(12);
num1.Add(13);
num1.Add(14);
num1.AddRange(new[] { 15, 16, 17 });
num1.Insert(0, 0);
foreach (int item in num1)
{
    System.Console.Write(" " + item);
}
int first1 = num1[0];//first val
int last1 = num1[num1.Count - 1];// last val
System.Console.WriteLine(first1);
System.Console.WriteLine(last1);

System.Console.WriteLine(num1.Remove(15));//removes value
num1.RemoveAt(0);

//searching
bool contains = name.Contains("Arjun");//true or false
System.Console.WriteLine(contains);
int ind = name.IndexOf("siva"); //index value
System.Console.WriteLine(ind);

//sort
num1.Sort();
num1.Reverse();

//Filtering 
List<int> evens = num1.FindAll(n => n % 2 == 0);
int? firstEven = num1.Find(n => n % 2 == 0);
foreach (int item in evens)
{
    System.Console.WriteLine(item);
}

//Dictionary<TKey, TValue>

Dictionary<string, int> ages = new Dictionary<string, int>();
ages.Add("Arjun", 23);
ages.Add("Gopi", 26);
ages.Add("sumanth", 24);
foreach (var item in ages)
{
    System.Console.WriteLine(item);
}


Dictionary<string, string> capitals = new Dictionary<string, string>
{
    {"India", "New Delhi"},
    {"Japan", "Tokyo"},
    {"USA", "Washington"}
};

// Or with collection initializer (C# 6+)
var scores = new Dictionary<string, int>
{
    ["Arjun"] = 89,
    ["Siva"] = 78,
    ["Sumanth"] = 98
};

//Add
ages["krishna"] = 26;
ages.Add("santhosh", 23);

int krishnaAge = ages["krishna"];
System.Console.WriteLine(krishnaAge);

// Safe get
if (ages.TryGetValue("Arjun", out int age1))
{
    System.Console.WriteLine(age1);
}

//// Check existence
bool hasSiva = scores.ContainsKey("Siva");
System.Console.WriteLine(hasSiva);
bool hasScore = scores.ContainsValue(89);
System.Console.WriteLine(hasScore);

//
ages.Remove("krishna");

//Iterate 
foreach (KeyValuePair<string, int> pair in ages)
{
    System.Console.WriteLine($"{pair.Key}: {pair.Value}");
}
//keys ans values 
foreach (string name1 in ages.Keys)
{
    System.Console.WriteLine(name1);
}

//Hashset in C#

HashSet<int> set = new HashSet<int> { 1, 2, 3, 4, 5 };
set.Add(6);
set.Add(2);

//remove
set.Remove(1);
System.Console.WriteLine(set);


foreach (int se in set)
{
    System.Console.WriteLine(se);
}

bool has3 = set.Contains(3);
System.Console.WriteLine(has3);
HashSet<int> set1 = new HashSet<int> { 1, 2, 3, 4, 5 };
HashSet<int> set2 = new HashSet<int> { 2, 3, 4, 8, 9 };

// Union: All elements that are in either set1 or set2
var unionResult = set1.Union(set2);
Console.WriteLine("Union:");
foreach (var item in unionResult)
{
    Console.Write(item + " ");
}
Console.WriteLine();

// Intersect: Elements that are in both set1 and set2
var intersectResult = set1.Intersect(set2);
Console.WriteLine("Intersect:");
foreach (var item in intersectResult)
{
    Console.Write(item + " ");
}
Console.WriteLine();

// Except: Elements in set1 but not in set2
var exceptResult = set1.Except(set2);
Console.WriteLine("Except:");
foreach (var item in exceptResult)
{
    Console.Write(item + " ");
}
Console.WriteLine();

// // Check before add (Dictionary)
// if (!dict.ContainsKey(key))
//     dict[key] = value;

// // Or use TryAdd (C# 10+)
// dict.TryAdd(key, value);

// // List to Dictionary
// List<Person> people = GetPeople();
// Dictionary<int, Person> peopleById = people.ToDictionary(p => p.Id);

// // Remove while iterating (use ToList() to avoid modification during iteration)
// foreach (var item in list.ToList())
// {
//     if (condition)
//         list.Remove(item);
// }\













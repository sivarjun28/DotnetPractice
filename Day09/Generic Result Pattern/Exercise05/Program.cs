// See https://aka.ms/new-console-template for more information


Result<string> ParseInt(string input)
{
    if(int.TryParse(input, out int value))
        return Result<string>.Success(input);
    return Result<String>.Failure("Invalid number");
}
Result<int> ConverToInt(string input)
{
    if(int.TryParse(input, out int value))
        return Result<int>.Success(value);
    return Result<int>.Failure("Convcersion Failed");
}

Result<int> MultipleByTwo(int number)
{
    return Result<int>.Success(number * 2);
}

var result = ParseInt("42")
            .Bind(ConverToInt)
            .Bind(MultipleByTwo);
string output = result.Match(
    onSuccess : value => $"Success {value}",
    onFailure: error => $"Error {error}"
);
System.Console.WriteLine(output);

/*
Create:
1. Result<T> - Success or Failure
2. Extension methods for Result<T>
3. Railway-oriented programming style

Features:
- Success/Failure states
- Error messages
- Map, Bind, Match operations
*/

public class Result<T>
{
    public T ? Value{get; }
    public string Error{get;}
    public bool IsSuccess{get;}
    public bool IsFailure => IsSuccess;


    private Result(T value)
    {
        Value = value;
        Error = string.Empty;
        IsSuccess = true;
    }

    private Result(string error)
    {
        Value = default;
        Error = error;
        IsSuccess = false;
    }
    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(string error) => new(error);

    public Result<TOutput> Map<TOutput> (Func<T, TOutput> transform)
    {
        return IsSuccess
            ? Result<TOutput>.Success(transform(Value!))
            : Result<TOutput>.Failure(Error);
    }

    //flat Map
    public Result<TOutput> Bind<TOutput> (Func<T, Result<TOutput>> transform)
    {
        return IsSuccess
            ? transform(Value!)
            : Result<TOutput>.Failure(Error);
    }

    public TOutput Match<TOutput>(Func<T, TOutput> onSuccess, Func<string, TOutput> onFailure)
    {
        return IsSuccess
            ? onSuccess(Value!)
            : onFailure(Error);
    }
}



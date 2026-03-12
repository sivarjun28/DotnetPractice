using System;
namespace Exercise04
{
    public interface IPipleLineStep<TIn, TOut>
    {
        TOut Process(TIn input);
    }

    public class PipeLine<T>
    {
        private Func<T, T> pipeline = input => input;

        public PipeLine<T> AddStep(Func<T, T> step)
        {
            Func<T, T> currentPipeLine = pipeline;
            pipeline = input => step(currentPipeLine(input));
            return this;
        }

        public T Execute(T input)
        {
            return pipeline(input);
        }
    }

    public class TransformPipeline
    {
        public static PipelineBuilder<TInput> Create<TInput>(TInput input)
        {
            return new PipelineBuilder<TInput>(input);
        }
    }
    public class PipelineBuilder<T>
    {
        private T current;
        public PipelineBuilder(T initial)
        {
            current = initial;
        }

        public PipelineBuilder<TNext> Then<TNext>(Func<T, TNext> transform)
        {
            TNext next = transform(current);
            return new PipelineBuilder<TNext>(next);
        }

        public T GetResult() => current;
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            string csvData = "John,30,john@example.com";

            var result = TransformPipeline.Create(csvData)
                .Then(csv => csv.Split(','))              // string -> string[]
                .Then(parts => new
                {
                    Name = parts[0],
                    Age = int.Parse(parts[1]),
                    Email = parts[2]
                })                                         // string[] -> anonymous
                .Then(user => $"User: {user.Name}, Age: {user.Age}")  // anonymous -> string
                .GetResult();

            Console.WriteLine(result);
        }
    }
}

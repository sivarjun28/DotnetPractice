using System;
namespace Exercise04
{
    public interface IPipleLineStep<TIn, TOut>
    {
        TOut Process(TIn input);
    }

    //Pipeline Class
    public class PipeLine<T>
    {
        private Func<T, T> pipeline = input => input;

        public PipeLine<T> AddStep(Func<T, T> step)
        {
            Func<T,T> currentPipeLine = pipeline;
            pipeline = input => step(currentPipeLine(input));
            return this;
        }

        public T Execute(T input)
        {
            return pipeline(input);
        }
    }
}

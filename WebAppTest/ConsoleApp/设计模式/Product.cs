using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp
{
    public class Product
    {
        IList<string> parts = new List<string>();

        public void Add(string part) 
        {
            parts.Add(part);
        }

        public void show() 
        {
            Console.Write("-------------");
            foreach (var item in parts)
            {
                Console.WriteLine(item);
            }
        }
    }

    public abstract class Builder 
    {
        public abstract void BuildA();
        public abstract void BuildB();
        public abstract Product GetResult();
    }

    public class ConcreteBuilder1 : Builder 
    {
        private Product product = new Product();

        public override void BuildA()
        {
            product.Add("部件A");
        }

        public override void BuildB()
        {
            product.Add("部件B");
        }

        public override Product GetResult()
        {
            return product;
        }
    }

    public class ConcreteBuilder2 : Builder
    {
        private Product product = new Product();

        public override void BuildA()
        {
            product.Add("部件X");
        }

        public override void BuildB()
        {
            product.Add("部件Y");
        }

        public override Product GetResult()
        {
            return product;
        }
    }

    public class Director 
    {
        public void Construct(Builder builder) 
        {
            builder.BuildA();
            builder.BuildB();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp
{
    public abstract class Prototype
    {
        private string id;

        public Prototype(string id) 
        {
            this.id = id;
        }

        public string Id { get { return id; } }

        public abstract Prototype Clone();
    }

    public class ConcretePrototypeClone : Prototype 
    {
        public ConcretePrototypeClone(string id) : base(id) { }

        public override Prototype Clone()
        {
            return (Prototype)this.MemberwiseClone();
        }
    }
}

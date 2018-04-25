﻿using System;
using Xunit;

namespace SpanJson.Tests
{
    public class InheritanceTests
    {
        public class Family
        {
            public Child Child { get; set; }
        }

        public abstract class Child
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        public class Daughter : Child
        {
            public string DaughterSpecific { get; set; }
        }

        public class Son : Child
        {
            public string SonSpecific { get; set; }
        }

        /// <summary>
        ///     TODO: disable deserialization in case of abstract class
        /// </summary>
        [Fact]
        public void Serialize()
        {
            var family = new Family {Child = new Daughter {DaughterSpecific = "Hello World", Name = "Daughter", Age = 5}};
            var serialized = JsonSerializer.Generic.SerializeToString(family);
            Assert.NotNull(serialized);
            Assert.Contains(nameof(Child.Age), serialized);
            Assert.Contains(nameof(Daughter.DaughterSpecific), serialized);

            var anotherFamily = new Family {Child = new Son {SonSpecific = "Hello World", Name = "Son", Age = 5}};
            serialized = JsonSerializer.Generic.SerializeToString(anotherFamily);
            Assert.NotNull(serialized);
            Assert.Contains(nameof(Child.Age), serialized);
            Assert.Contains(nameof(Son.SonSpecific), serialized);

            var ex = Assert.Throws<NotSupportedException>(() => JsonSerializer.Generic.Deserialize<Family>(serialized));
            Assert.NotEmpty(ex.Message);
        }
    }
}
﻿using NUnit.Framework;

namespace Oxygenize.Test
{
    public class OxygenizeTest
    {
        [Test]
        public void Should_Create_Instance_Of_Given_Type()
        {
            var instance = Oxygenize.For<TestClass>()
                            .WithStrategy(GenerationStrategy.Mixed)
                            .Instance;

            Assert.IsTrue(instance.GetType() == typeof(TestClass));
        } 
    }

    class TestClass
    {
    }
}
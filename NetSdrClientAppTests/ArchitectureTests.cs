﻿using NetArchTest.Rules;
using NUnit.Framework;

namespace NetSdrClientAppTests
{
    public class ArchitectureTests
    {
        [Test]
        public void UI_ShouldNotDependOnInfrastructureDirectly()
        {
            var assembly = typeof(NetSdrClientApp.NetSdrClient).Assembly;

            var result = Types.InAssembly(assembly)
                .That()
                .ResideInNamespace("NetSdrClientApp")
                .ShouldNot()
                .HaveDependencyOn("EchoServer")
                .GetResult();

            Assert.IsTrue(result.IsSuccessful);
        }
    }
}
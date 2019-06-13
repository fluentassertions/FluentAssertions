---
title: Introduction
permalink: /introduction
layout: single
classes: wide
sidebar:
  nav: "sidebar"
---

## Getting started ##

Fluent Assertions is a set of .NET extension methods that allow you to more naturally specify the expected outcome of a TDD or BDD-style unit test. This enables a simple intuitive syntax that all starts with the following `using` statement:

```c#
using FluentAssertions;
```

This brings a lot of extension methods into the current scope. For example, to verify that a string begins, ends and contains a particular phrase.

```c#
string actual = "ABCDEFGHI";
actual.Should().StartWith("AB").And.EndWith("HI").And.Contain("EF").And.HaveLength(9);
```

To verify that a collection contains a specified number of elements and that all elements match a predicate.

```c#
IEnumerable numbers = new[] { 1, 2, 3 };
numbers.Should().HaveCount(4, "because we thought we put four items in the collection");
```

The nice thing about the second failing example is that it will throw an exception with the message

> "Expected numbers to contain 4 item(s) because we thought we put four items in the collection, but found 3."

To verify that a particular business rule is enforced using exceptions.

```c#
var recipe = new RecipeBuilder()
                    .With(new IngredientBuilder().For("Milk").WithQuantity(200, Unit.Milliliters))
                    .Build();
Action action = () => recipe.AddIngredient("Milk", 100, Unit.Spoon);
action
                    .Should().Throw<RuleViolationException>()
                    .WithMessage("*change the unit of an existing ingredient*")
                    .And.Violations.Should().Contain(BusinessRule.CannotChangeIngredientQuantity);
```

One neat feature is the ability to chain a specific assertion on top of an assertion that acts on a collection or graph of objects.

```c#
dictionary.Should().ContainValue(myClass).Which.SomeProperty.Should().BeGreaterThan(0);
someObject.Should().BeOfType<Exception>().Which.Message.Should().Be("Other Message");
xDocument.Should().HaveElement("child").Which.Should().BeOfType<XElement>().And.HaveAttribute("attr", "1");
```

This chaining can make your unit tests a lot easier to read.

## Detecting Test Frameworks ##
Fluent Assertions supports a lot of different unit testing frameworks. Just add a reference to the corresponding test framework assembly to the unit test project. Fluent Assertions will automatically find the corresponding assembly and use it for throwing the framework-specific exceptions.

If, for some unknown reason, Fluent Assertions fails to find the assembly, and you're running under .NET 4.5 or a .NET Standard 2.0 project, try specifying the framework explicitly using a configuration setting in the project’s app.config. If it cannot find any of the supported frameworks, it will fall back to using a custom `AssertFailedException` exception class.

```xml
<configuration>
  <appSettings>
    <!-- Supported values: nunit, xunit, mstest, mspec, mbunit and gallio -->
    <add key="FluentAssertions.TestFramework" value="nunit"/>
  </appSettings>
</configuration>
```
Just add NuGet package "FluentAssertions" to your test project.

## Subject Identification ##
Fluent Assertions can use the C# code of the unit test to extract the name of the subject and use that in the assertion failure. Consider for instance this statement:

```csharp
string username = "dennis";
username.Should().Be("jonas");
```

This will throw a test framework-specific exception with the following message:

`Expected username to be "jonas", but "dennis" differs near 'd' (index 0)`.`

The way this works is that Fluent Assertions will try to traverse the current stack trace to find the line and column numbers as well as the full path to the source file. Since it needs the debug symbols for that, this will require you to compile the unit tests in debug mode, even on your build servers. Also, since only .NET Standard 2.0 and the full .NET Framework support getting direct access to the current stack trace, subject identification only works for the platforms targeting those frameworks.

Now, if you've built your own extensions that use Fluent Assertions directly, you can tell it to skip that extension code while traversing the stack trace. Consider for example the customer assertion:

```csharp
    public class CustomerAssertions
    {
        private readonly Customer customer;

        public CustomerAssertions(Customer customer)
        {
            this.customer = customer;
        }

        [CustomAssertion]
        public void BeActive(string because = "", params object[] becauseArgs)
        {
            customer.Active.Should().BeTrue(because, becauseArgs);
        }
    }
```

And it's usage:

```
myClient.Should().BeActive("because we don't work with old clients");
```

Without the `[CustomAssertion]` attribute, Fluent Assertions would find the line that calls `Should().BeTrue()` and treat the `customer` variable as the subject-under-test (SUT). But by applying this attribute, it will ignore this invocation and instead find the SUT by looking for a call to `Should().BeActive()` and use the `myClient` variable instead.




## Assertion Scopes ##

You can batch multiple assertions into an `AssertionScope` so that FluentAssertions throws one exception at the end of the scope with all failures.

E.g.

```csharp
using (new AssertionScope())
{
    5.Should().Be(10);
    "Actual".Should().Be("Expected");
}
```

The above will batch the two failures, and throw an exception at the point of disposing the `AssertionScope` displaying both errors.

E.g. Exception thrown at point of dispose contains:

    Expected value to be 10, but found 5.
    Expected string to be "Expected" with a length of 8, but "Actual" has a length of 6, differs near "Act" (index 0).
    
        at........
        
For more information take a look at the [AssertionScopeSpecs.cs](https://github.com/fluentassertions/fluentassertions/blob/master/Tests/Shared.Specs/AssertionScopeSpecs.cs) in Unit Tests.


# Rethought.Commands
The motivation behind this project is to simplify the building of commands.
Commands are defined as functions that can be invoked with a context.
The context is a specified generic type argument.
An example could be a context for a chat-application that contains the message,
the user and a channel from which the message was sent.

To build commands you can use the fluent extensions or manually construct commands.

Table of Contents
- [Rethought.Commands](#rethoughtcommands)
    - [Installation](#installation)
    - [Getting Started](#getting-started)
    - [Documentation](#documentation)
        - [Conditions](#conditions)
        - [Actions](#actions)
        - [Funcs](#funcs)
        - [Adapters](#adapters)
        - [Option](#option)
        - [TypeParser](#typeparser)
        - [Prototype](#prototype)
        - [All, Any and Enumerating](#all-any-and-enumerating)
    - [Todo](#todo)

## Installation

This project hasn't been uploaded to NuGet yet.

## Getting Started

Rethought.Commands on its own is not very useful. What makes it powerful is the immense support for extensions.

Currently, the following extensions are available:

- [Rethought.Commands.Discord.Net]()


## Documentation

This project will be documented in the form of samples. Detailed samples can be found at the respective repository of the extension.

Before we begin, we should think about our context. Most of the extensions will come with prebuilt contexts. For the sake of the example however, let's define one.

```csharp
public class MyContext
{
    public string Sender { get; set; }
    public string Message { get; set; }
}
```

As you can see, the context is often just a simple data holding object. In real-world scenarios, there will be much more data.

Next, let's take a look at our composition.

```csharp
public class Program
{
    public static async Task Main()
    {
        var asyncFuncBuilder = AsyncFuncBuilder<MyContext>.Create();

        var asyncFunc = asyncFuncBuilder
            .WithCondition(context => context.Sender == "Foo")
            .WithCondition(context => context.Message.Length > 0)
            .WithAction(context => Console.WriteLine($"{context.Sender}: {context.Message}"))
            .Build();

        await asyncFunc.InvokeAsync(new MyContext("Foo", "Hello World!"), CancellationToken.None);
    }
}
```

A lot is happening here. Let's break it down, step by step.

### Conditions
First we create an `AsyncActionBuilder` with our `TContext` (`MyContext`). This is where we configure our whole command tree.
In this simple example, we start by adding a condition such that the `Sender` must equal `"Foo"`.
It should be noted that there are other ways to accomplish this. While it may not fit this simplified example, if you often re-use the same condition
or use any of the additional packages, you might run across the following:

```csharp
public class MyCondition : ICondition<MyContext>
{
    public bool Satisfied(MyContext context)
    {
        return context.Sender == "Foo";
    }
}
```

This could then be used instead of the Func, like: `.WithCondition(new MyCondition())`.

As with many other components of the framework, there is also an Async variant of this. Respectively, an overload for `Func<TContext, Task<bool>>` and an interface `IAsyncCondition`.

### Actions

Looking back at the example, we called `.WithAction(context => Console.WriteLine($"{context.Sender}: {context.Message}"))`. Which is as simple as it looks.

Like previously, there is also a variant in the form of an interface.

```csharp
public class MyAction : IAction<MyContext>
{
    public void Invoke(MyContext context)
    {
        Console.WriteLine($"{context.Sender}: {context.Message}");
    }
}
```

### Funcs

There is an important distinction to make here. Funcs are types that have a return value. As such, "Async Actions" are Funcs. In the library, there are three different types of Funcs.

`IAsyncFunc` is the previously mentioned async variant of an `IAction`. It also comes with a `CancellationToken`.

The other two types are called `IResultFunc` and `IAsyncResultFunc`. The main difference here is that these two types also return a `Result`.

The `Result` is used for flow control and can have three values.
`Result.Aborted` is usually returned when the operation was forcefully aborted or there was an exception. 
`Result.Completed` is whenever the operation was successful.
`Result.None` is a bit more special and used when no match was found.

There are plenty of overloads for Actions and Funcs, accepting `System.Action`, `System.Func` or the library built-in Action and Func types. The Func ones come with options to not discard the `CancellationToken`.

### Adapters

Especially in more complex scenarios, these come in handy. Imagine you have the example context from before and now want to analyze it in order to analyze the toxicity of the message.

```csharp
.WithAdapter(new ToxicityTypeParser(), ConfigureToxicityBuilder)
```

The `ToxicityTypeParser` is responsible for parsing from `MyContext` (our source context) to `ToxicityContext` (our new context).

If you wish to preserve the source context, it's up to you.

```csharp
public class ToxicityContext
{
    public ToxicityContext(MyContext myContext, double toxicity)
    {
        MyContext = myContext;
        Toxicity = toxicity;
    }

    public MyContext MyContext { get; }

    public double Toxicity { get; }
}
```

The method `ConfigureToxicityBuilder` is responsible for taking the new `AsyncFuncBuilder` and continue configuring. In our case, all we are adding is an action to print the toxicity alongside the message.

```csharp
void ConfigureToxicityBuilder(AsyncFuncBuilder<ToxicityContext> toxicityBuilder)
{
    toxicityBuilder.WithAction(
        context => Console.WriteLine(
            $"{context.MyContext.Sender}: {context.MyContext.Message} had a toxicity value of {context.Toxicity}"));
}
```

### Option

This library makes heavy use of the Option-Pattern in favor of null checks.
You can find more information about the concrete framework used [here](https://github.com/nlkl/Optional). I recommend taking a glance at what that means prior to reading about Type Parsers.

### TypeParser

As stated in the previous section, these are used to parse from Type A to Type B.
However, there is one speciality in their return type.

In the current version these return `Option<Option<TOutput>>`. What might look confusing at first glance hopefully becomes clear on the second.

The first Option determines whether the parsing operation was aborted or not. Often when you use an Adapter you need additional user input for example.

The second Option determines whether the result `TOutput` is empty. In other words,
if the first Option was not empty, but the second one is, then the TypeParser was unable to parse.

I would recommend using the method `Flatten()` on the result and then you can do `TryGetValue(...)` to get access to the value.

If you want to check whether the parsing was aborted, do `TryGetValue` on the first Option.

### Prototype

You may want to continue building a command with an already existing one. To do that you can use `.WithPrototype(IAsyncResultFunc<TContext> asyncResultFunc)`.

Please note that currently it is not supported to continue building a command from a previous `AsyncFuncBuilder`.

### All, Any and Enumerating

Alone these probably aren't too powerful. But, a lot of the additional packages may make use of them. They behave very similar to LINQ. The operations accept either a collection of `Action<AsyncFuncBuilder<TContext>>`, `IAsyncResultFunc<TContext>` or `Func<IAsyncResultFunc<TContext>>` and a predicate. Additionally, they have a bool `shortCircuiting` which determines whether it will stop at the first occurrence of something in the predicate not being true, or will continue until the end and then return the result.

Enumerating does not accept a predicate! It is simply enumerating a collection of the three above mentioned types.

## Todo
- Various chat application implementations (Slack, ..)

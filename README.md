# Rethought.Commands
The motivation behind this project is to simplify the building of commands.
Commands are defined as functions that can be invoked with a context.
The context is a specified generic type argument.
An example could be context of a chat-application that contains the message,
the user and a channel from which the message was sent.

To build commands you can use the fluent extensions or manually construct commands.

Table of Content
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
            - [IAbortableTypeParser](#iabortabletypeparser)
            - [ITypeParser](#itypeparser)
        - [Prototype](#prototype)
        - [All, Any and Enumerating](#all-any-and-enumerating)
    - [To do](#to-do)

## Installation

This project hasn't been uploaded to NuGet yet.

## Getting Started

Rethought.Commands on its own is not very useful. What makes it powerful is the immense support for extension.

Currently there are the following extensions available:

- [Rethought.Commands.Discord.Net]() soon

## Documentation

This project will be documented in form of samples. Detailed samples can be found at the respective repository of the extension.

Before we even begin, we should think about our context. Most of the extensions will come with prebuilt contexts, but for the sake of the example we define one.

```csharp
public class MyContext
{
    public string Sender { get; set; }
    public string Message { get; set; }
}
```

As you can see the context is often just a simple data holding object. In real world scenarios there will be much more data.

Next lets take a look at our composition.

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

A lot is happening here, so let us inspect it more closely step by step.

### Conditions
First we create an `AsyncActionBuilder` with our `TContext` (`MyContext`). This is where we configure our whole command tree.
In this simple example we start by adding a condition such that the `Sender` must equal `"Foo"`.
But there are also other ways to do the exact same thing. Here it does not make much sense, because it is so simple, but if you often re-use the same condition
or use any of the additional packages you might run across the following.

```csharp
public class MyCondition : ICondition<MyContext>
{
    public bool Satisfied(MyContext context)
    {
        return context.Sender == "Foo";
    }
}
```

which could then be used instead of the Func, looking like that: `.WithCondition(new MyCondition())`.

As with many other components of the framework there also is an Async variant of this. Respectively an overload for `Func<TContext, Task<bool>>` and an interface `IAsyncCondition`.

### Actions

Looking back at the example we called `.WithAction(context => Console.WriteLine($"{context.Sender}: {context.Message}"))`. Which is as simple as it looks.

Like previously there is also a variant in form of an interface.

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

There is an important distinction to make here. Funcs are types that have a return value. And as such "Async Actions" are Funcs. In the library there are three different types of Funcs.

`IAsyncFunc` is the previously mentioned async variant of an `IAction`. It also comes with a `CancellationToken`.

The other two types are called `IResultFunc` and `IAsyncResultFunc`. The main difference here is that these two types also return a `Result`.

The `Result` is used for flow control and can have three values.
`Result.Aborted` is usually returned when the operation was forcefully aborted or there was some kind of exception. `Result.Completed` whenever the operation was successful.
`Result.None` is a bit more special and used when no match was found.

There are plenty of overloads for Actions and Funcs, accepting `System.Action`, `System.Func` or the library built-in Action and Func types. The Func ones come with options to not discard the `CancellationToken`.

### Adapters

Especially in more complex scenarios these come in handy. Imagine you have the example context from before and now want to analyze it in order to analyze the toxicity of the message.

```csharp
.WithAdapter(new ToxicityTypeParser(), ConfigureToxicityBuilder)
```

The `ToxicityTypeParser` is responsible to parse from `MyContext`, our previous context, to our new context `ToxicityContext`.

If you want to preserve the previous context is up to you.

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

The method `ConfigureToxicityBuilder` is responsible to take the new `AsyncFuncBuilder` and continue configuring. In our case all we are adding is an action to print the toxicity alongside the message.

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
You can find more information about the concrete framework used [here](https://github.com/nlkl/Optional). I recommend you to at least have a glance on what that means prior to reading about Type Parsers.

### TypeParser

Like in the previous section said, these are used to parse from Type A to Type B.
However, there are two types of TypeParsers.

#### IAbortableTypeParser

These return a bool and as out parameter `Option<TOutput>`. What might look confusing on the first glance hopefully becomes clear on the second.

The bool determines whether the parsing operation was aborted or not. It is aborted when the bool is false.

Then the Option determines whether the TypeParser `TOutput` succeeded parsing and contains the `TOutput`.

#### ITypeParser

These are not abortable. They only return an `Option<TOutput>` which means the parsing succeeded or not. There is no way to determine from an outer scope if the parsing was aborted or unsuccessful.

### Prototype

You may want to continue building a command with an already existing one. To do that you can use `.WithPrototype(IAsyncResultFunc<TContext> asyncResultFunc)`.

Please note that currently it is not supported to continue building a command from a previous `AsyncFuncBuilder`.

### All, Any and Enumerating

Alone these probably aren't too powerful. But a lot of the additional packages may make usage of them. They behave very similar to LINQ. The operations accept either a collection of `Action<AsyncFuncBuilder<TContext>>`, `IAsyncResultFunc<TContext>` or `Func<IAsyncResultFunc<TContext>>` and a predicate. All also has a bool `shortCircuiting` which determines whether it will stop at the first occurrence of something not being true from the predicate or will continue till the end and then return the result.

Enumerating does not accept a predicate and simply is enumerating a collection of the three above mentioned types.

## To do
- Various chat application implementations (Slack, ..)
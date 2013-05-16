**Pathoschild.DesignByContract** is a little aspect-oriented library which enables design-by-contract preconditions and postconditions. By annotating your code with attributes like `[NotNull]`, you can remove common validation code and provide helpful exception messages, making your code more robust and refactor-safe. These annotation attributes are also recognized by [ReSharper](http://www.jetbrains.com/resharper/) when it has an equivalent annotation, which gives you real-time feedback on contract violations as you type.

The library uses [PostSharp](http://www.sharpcrafters.com/) for its compile-time aspect weaving, which requires a [free license](http://www.sharpcrafters.com/purchase/compare) to build the annotated project. (A license is only required to build the project containing the annotations — there are no licensing requirements to reference the compiled project.)

##Usage
You can define a contract on code using annotation attributes like `[NotNull]` on return values, method arguments, and properties. Annotations on interfaces are inherited by their implementations. You enable contract validation by applying `[DesignedByContract]` to the class (or unit of code):

###Example
The following method *with* the contract annotations:
```c#
    [DesignedByContract]
    public class Sword
    {
        [return: NotNull, NotBlank]
        public string Hit([NotNull] string actor, [NotNull] string target)
        {
            return String.Format("{0} hit {1} with a sword!", actor, target);
        }
    }
```

is equivalent to this one *without*:

```c#
    public class Sword
    {
        public string Hit(string actor, string target)
        {
            if (actor == null) throw new ArgumentNullException("actor", "The value cannot be null for parameter 'actor' of method 'Sword::Hit'.");
            if (target == null) throw new ArgumentNullException("target", "The value cannot be null for parameter 'target' of method 'Sword::Hit'.");

            string value = String.Format("{0} hit {1} with a sword!", actor, target);

            if(value == null) throw new NullReferenceException("The return value cannot be null for method 'Sword::Hit'.");
            if(String.IsNullOrWhiteSpace(value)) throw new InvalidOperationException("The return value cannot be blank or consist entirely of whitespace for method 'Sword::Hit'.");

            return value;
        }
    }
```

###Available annotations
The following annotations are implemented out of the box. When a contract is violated, an annotation will by convention throw a descriptive message with the type `ParameterContractException` (which subclasses `ArgumentException`) or `ReturnValueContractException` (which subclasses `InvalidOperationException`).

* `[NotNull]` marks a value that cannot be `null`.
* `[NotBlank]` marks a string value that cannot be empty or only whitespace.
* `[NotEmpty]` marks a sequence value that cannot have zero elements.
* `[NotDefault]` marks a value that cannot be equal to the default value for its type (e.g., `null` for a reference type or 0 for an integer).
* `[HasType]` marks a value that must implement one of the specified types (with possible inheritance). This is intended to make code that must [unbox arguments](http://msdn.microsoft.com/en-us/library/yz2be5wk.aspx) more robust.

###Creating annotations
You can create new annotations by implementing [attributes](http://msdn.microsoft.com/en-us/library/z0w1kczw\(v=vs.80\).aspx) with any of the following interfaces (see an [example annotation](https://github.com/Pathoschild/Pathoschild.DesignByContract/blob/master/Pathoschild.DesignByContract/NotNullAttribute.cs)):

* [`IParameterPrecondition`](https://github.com/Pathoschild/Pathoschild.DesignByContract/blob/master/Pathoschild.DesignByContract/Framework/IParameterPrecondition.cs) checks that an input value to a method argument or property setter is valid.
* [`IReturnValuePrecondition`](https://github.com/Pathoschild/Pathoschild.DesignByContract/blob/master/Pathoschild.DesignByContract/Framework/IReturnValuePrecondition.cs) checks that a return value from a method or property getter is valid.

###Installation
You only need to do the following for uncompiled projects containing annotations. You can reference their compiled DLLs without knowing about annotations or PostSharp, so using these annotations shouldn't affect redistribution. (These steps assume you're using Visual Studio.)

1. Acquire a [free Starter License for PostSharp](http://www.sharpcrafters.com/purchase/compare).
2. Install the [`Pathoschild.DesignByContract` NuGet package](http://nuget.org/packages/Pathoschild.DesignByContract) for the projects that will contain annotations.

And that's it; you can now enforce code contracts throughout your code.

##Performance
###Runtime
Efficient runtime performance is a core design goal. The `[DesignedByContract]` aspect analyzes contracts at compile time and injects non-reflection validation logic into the method, so it's very fast at runtime.

For example, this is the decompiled source code generated by the `Sword` example above:

```c#
        [return: NotNull, NotBlank]
        public string Hit([NotNull] string actor, [NotNull] string target)
        {
          MethodExecutionArgs args = new MethodExecutionArgs(null, new Arguments<string, string>() { Arg0 = actor, Arg1 = target });
          Sword.<>z__Aspects.a1.OnEntry(args);
          string str = string.Format("{0} hit {1} with a sword!", actor, target);
          args.ReturnValue = str;
          Sword.<>z__Aspects.a1.OnSuccess(args);
          return str;
        }
```

###Build-time
The library analyses contracts at build time, so large projects or solutions with many projects will take longer to build.

##Partial trust compatibility
This package doesn't require full trust, and should work correctly in medium or partial trust scenarios.

##Future to-do
* Switch to an open-source AOP framework like [SheepAspect](http://sheepaspect.org).
* Support optionally enforcing [.NET Data Annotations](http://msdn.microsoft.com/en-us/library/system.componentmodel.dataannotations.aspx).
**Pathoschild.DesignByContract** is a little aspect-oriented library which enables design-by-contract preconditions and postconditions. By annotating your code with attributes like `[NotNull]`, you can eliminate boilerplate validation logic while enabling helpful exception messages and making your code much more robust and refactor-safe. These annotation attributes are also recognized by [ReSharper](http://www.jetbrains.com/resharper/) when it has an equivalent annotation, which gives you real-time feedback on contract violations as you type.

The library uses [PostSharp](http://www.sharpcrafters.com/) for its compile-time aspect weaving, a proprietary and commercial framework that requires a license for use (the [free Starter License](http://www.sharpcrafters.com/purchase/compare) is sufficient). A license is only required to build the project containing the annotations — there are no licensing requirements to reference the compiled project. The eventual goal is to migrate to an open-source alternative like [SheepAspect](http://sheepaspect.org) when it's mature enough.

**This is an early proof of concept, and shouldn't really be used in production yet. (We're using it in production, but we're _crazy_.)**

##Usage
This library lets you define a contract on code using annotation attributes like `[NotNull]`. You can generally apply these to methods, properties, return values, and parameters. You then enable contract validation by applying `[DesignedByContract]` to the class (or any other unit of code).

###Example
The following method *with* the contract annotations:

        [return: NotNullOrEmpty]
        public string Hit([NotNullOrEmpty] string actor, [NotNullOrEmpty] string target)
        {
            return String.Format("{0} hit {1} with a sword!", actor, target);
        }

is equivalent to this one *without*:

        public string Hit(string actor, string target)
        {
            if (actor == null) throw new ArgumentNullException("actor", "The value cannot be null for parameter 'actor' of method 'Sword::Hit'.");
            if (String.IsNullOrWhiteSpace(actor)) throw new ArgumentNullException("actor", "The value cannot be blank or consist entirely of whitespace for parameter 'actor' of method 'Sword::Hit'.");
            if (target == null) throw new ArgumentNullException("target", "The value cannot be null for parameter 'target' of method 'Sword::Hit'.");
            if (String.IsNullOrWhiteSpace(target)) throw new ArgumentNullException("target", "The value cannot be blank or consist entirely of whitespace for parameter 'target' of method 'Sword::Hit'.");

            string value = String.Format("{0} hit {1} with a sword!", actor, target);

            if(value == null) throw new NullReferenceException("The return value cannot be null for method 'Sword::Hit'.");
            if(String.IsNullOrWhiteSpace(value)) throw new InvalidOperationException("The return value cannot be blank or consist entirely of whitespace for method 'Sword::Hit'.");

            return value;
        }

###Available annotations
The following annotations are implemented out of the box. When a contract is violated, an annotation will generally throw a descriptive exception with the type `ArgumentException` (parameter value) or `InvalidOperationException` (return value).

* `[NotNull]` indicates that a value cannot be `null`. Unlike other annotations, it will throw `ArgumentNullException` (parameter value) or `NullReferenceException` (return value).
* `[NotBlank]` indicates that a value cannot be an empty string or one consisting entirely of whitespace.
* `[NotEmpty]` indicates that a value cannot be an empty sequence or string.
* `[HasType]` indicates that a value must implement one of several types (with possible inheritance). This is intended to make code that must [unbox arguments](http://msdn.microsoft.com/en-us/library/yz2be5wk.aspx) more robust.

The following convenience attributes are also available in the `Pathoschild.DesignByContract.Shorthand` namespace:

* `[NotNullOrBlank]` is equivalent to `[NotNull, NotBlank]`.
* `[NotNullOrEmpty]` is equivalent to `[NotNull, NotEmpty]`.

###Creating annotations
You can create new annotations simply by creating [attributes](http://msdn.microsoft.com/en-us/library/z0w1kczw\(v=vs.80\).aspx) that implement one or more of the following interfaces (see an [example annotation](https://github.com/Pathoschild/Pathoschild.DesignByContract/blob/master/Pathoschild.DesignByContract/NotNullAttribute.cs)):

* `IParameterPrecondition` checks that an input value to a method parameter or property setter is valid.
* `IReturnValuePrecondition` checks that a return value from a method or property getter is valid.

###Installation
You only need to do the following for uncompiled projects containing annotations. You can reference their compiled DLLs without knowing about annotations or PostSharp, so using these annotations shouldn't affect redistribution. (These steps assume you're using Visual Studio.)

1. [Purchase a license or request a free Starter License for PostSharp](http://www.sharpcrafters.com/purchase/compare).
2. Install PostSharp on your workstation, _or_ use the [NuGet library package manager](https://nuget.codeplex.com/wikipage?title=Getting%20Started) to install the PostSharp package for the projects that will contain annotations.
3. Download and compile `Pathoschild.DesignByContract`.
4. Reference `Pathoschild.DesignByContract.dll` from each project that will contain annotations.

And that's it; you can now apply annotations like `[NotNull]` throughout your code.

##Performance
Decent performance is a core design goal for this library — it was originally written for use in a fairly large web application. The `[DesignedByContract]` aspect does all its reflection at compile time and injects non-reflection validation logic into the method, so it's very fast at runtime.

For example, this is the decompiled source code generated by the `Sword` example above:

        [return: NotNullOrEmpty]
        public string Hit([NotNullOrEmpty] string actor, [NotNullOrEmpty] string target)
        {
          MethodExecutionArgs args = new MethodExecutionArgs(null, new Arguments<string, string>() { Arg0 = actor, Arg1 = target });
          Sword.<>z__Aspects.a1.OnEntry(args);
          string str = string.Format("{0} hit {1} with a sword!", actor, target);
          args.ReturnValue = str;
          Sword.<>z__Aspects.a1.OnSuccess(args);
          return str;
        }

##Future to-do

* Switch to an open-source AOP framework like [SheepAspect](http://sheepaspect.org) (when it's more mature).
* Provide a NuGet package for `Pathoschild.DesignByContract`, which would automate installation steps 2–4.

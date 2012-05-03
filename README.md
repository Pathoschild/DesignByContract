**Pathoschild.DesignByContract** is a little aspect-oriented library
which enables design-by-contract preconditions and postconditions on method
arguments and return values. By annotating methods with attributes like
`[NotNull]`, you can eliminate boilerplate validation logic while enabling
helpful exception messages and making your code much more robust and
refactor-safe. These annotation attributes are also recognized by
[ReSharper](http://www.jetbrains.com/resharper/) (if you use it), which
provides you with feedback on contract violations in realtime as you type code.

The library is built on top of [PostSharp](http://www.sharpcrafters.com/), a
proprietary and commercial framework that requires a license for use. However,
the library carefully only uses features available as part of the
[free Starter License](http://www.sharpcrafters.com/purchase/compare). The
eventual goal is to migrate to an open-source alternative like [SheepAspect](http://sheepaspect.org) when
it's mature enough.

**This is an early proof of concept, and shouldn't really be used in production yet.**

##Usage
This library lets you define a contract on any class using annotation
attributes like `[NotNull]`. You can generally apply these to methods,
properties, return values, and parameters. You then enable contract
validation by applying `[DesignedByContract]` to the class. (Optionally,
you can apply the `[DesignedByContract]` attribute to a method, property,
return value, or parameter; it'll trickle down logically.)

###Example
The following code block *with* the annotations:

        [DesignedByContract]
        public class Sword
        {
            [return: NotNull]
            public string Hit([NotNull] string actor, [NotNull] string target)
            {
                return String.Format("{0} hit {1} with a sword!", actor, target);
            }
        }

is equivalent to this one *without* the annotations:

        public class Sword
        {
            public string Hit(string actor, string target)
            {
                if (actor == null)
                    throw new ArgumentNullException("actor", "The value cannot be null for parameter 'actor' of method 'Sword::Hit'.");
                if (String.IsNullOrWhiteSpace(actor))
                    throw new ArgumentException("The value cannot be blank or consist entirely of whitespace for parameter 'actor' of method 'Sword::Hit'.", "actor");

                if (target == null)
                    throw new ArgumentNullException("target", "The value cannot be null for parameter 'target' of method 'Sword::Hit'.");
                if (String.IsNullOrWhiteSpace(target))
                    throw new ArgumentException("The value cannot be blank or consist entirely of whitespace for parameter 'target' of method 'Sword::Hit'.", "target");

                string value = String.Format("{0} hit {1} with a sword!", actor, target);
                
                if(value == null)
                    throw new NullReferenceException("The return value cannot be null for method 'Sword::Hit'.");
                if(String.IsNullOrWhiteSpace(value))
                    throw new InvalidOperationException("The return value cannot be blank or consist entirely of whitespace for method 'Sword::Hit'.");
                
                return value;
            }
        }

###Available annotations
The following annotations are currently implemented:

* `[NotNull]`:
  * on a method argument, throws `ArgumentNullException` if a `null` value is
    passed, or `ArgumentException` if a blank or whitespace-only value is passed.
  * on a method or method return value, throws `NullReferenceException` if a
    `null` value is returned, or `InvalidOperationException` if a blank or
    whitespace-only value is passed.

###Creating annotations
Contract annotations are simply [attributes](http://msdn.microsoft.com/en-us/library/z0w1kczw(v=vs.80).aspx)
that implement one or more of the following interfaces (in order of validation):
* `IMethodPrecondition` checks that a method may be invoked (e.g.:
  `[NotDisposed]`).
* `IParameterPrecondition` checks that an input value to a method parameter or
  property setter is valid (e.g.: `[NotNull]`).
* `IReturnValuePrecondition` checks that a return value from a method or
  property getter is valid (e.g.: `[NotNull]`).
* `IMethodPostcondition` checks that a method may be completed.

##Performance
Decent performance is a core design goal for this library � it was originally
written for use in a large multi-tenant enterprise application. The
`[DesignedByContract]` aspect does all its reflection at compile time and injects
simple validation code, so it's very fast at runtime.

For example, this is the (cleaned up) decompiled source code generated by the `Sword` example above:

        public string Hit([NotNull] string actor, [NotNull] string target)
        {
            MethodExecutionArgs args = new MethodExecutionArgs((object) null, (Arguments) new Arguments<string, string>() { Arg0 = actor, Arg1 = target });

            Sword.\u003C\u003Ez__Aspects.a0.OnEntry(args);
            string str = string.Format("{0} hit {1} with a sword!", actor, target);
            args.ReturnValue = str;

            Sword.\u003C\u003Ez__Aspects.a0.OnSuccess(args);
            return str;
        }

##Installation
**This is an early proof of concept, and shouldn't really be used in production yet.**

You only need to do the following for uncompiled projects containing
annotations. You can reference their compiled DLLs without knowing about
annotations or PostSharp, so using these annotations shouldn't affect
redistribution.

These steps assume you're using Visual Studio 2010; you may need to adapt these
instructions if you're not.

1. [Purchase a license or request a free Starter License for PostSharp](http://www.sharpcrafters.com/purchase/compare).
2. Using the [NuGet library package manager](https://nuget.codeplex.com/wikipage?title=Getting%20Started),
   install PostSharp for each project that will contain annotations.
3. Download and compile `Pathoschild.DesignByContract`.
4. Reference `Pathoschild.DesignByContract.dll` from each project that will
   contain annotations.

And that's it; you can now apply annotations like `[NotNull]` throughout your code.

##Future to-do
* Switch to an open-source AOP framework like [SheepAspect](http://sheepaspect.org)
  (when it's more mature).
* Provide a NuGet package for `Pathoschild.DesignByContract`, which would
  automate installation steps 2�4.
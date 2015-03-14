# TeraSoft's Documentation Conventions #

This page will act as a reference for our documentation conventions.

# Introduction #

Due to the fact that we need to unify our documentation conventions, it was decided that we will use the C# documentation conventions so that it would be compatible with the documentation generators if we decided to use them (to have something like [TeraSoft Documentation](http://terasoft-12.tk)).


# Details #
Here is a sample documentation for a method called **_foo_**

```
/// <summary>
/// This foo method does nothing useful!
/// </summary>
/// <remarks>
/// <para>AUTHOR: My Name </para>   
/// <para>DATE WRITTEN: Month, dd</para>
/// <para>DATE MODIFIED: Month, dd</para>
/// </remarks>
/// <param name="x">A parameter that has no meaning what so ever</param> 
/// <returns>An int that is identical to the parameter passed to the method</returns>

   int foo (int x)
   {
      return x;
   }

```

**_NB: The DATE WRITTEN and DATE MODIFIED parameters are optional and not required!_**
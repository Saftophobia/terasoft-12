# Introduction #

Code must be clean and each sentence should have ameaning so useless things should be removed so here is some hints for that.


# Details #


1)Remove the useless imports of classes that you do not use,some of them are created by default when you create a new class.However you may not need them so it should be removed.

for example if you have screen and only doing XNA work so why you need these default imports :
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

Also in XNA their is alot of imports you may not need.What you not need should be removed.

2)Do not  use "this." for useless things,it is already made by default in system so you do not need to write it.

3)Redundant methods should be  removed.Methods and variables that are not used should be removed.

4)redundant qualifiers like "Draw(Microsoft.Xna.Framework.GameTime gameTime)"  should be removed and it should be just like :

> Draw(GameTime gameTime)


5)Use var type for local variables instead of an explicit type ,ex : var x =1; var y = "hello" and so on...

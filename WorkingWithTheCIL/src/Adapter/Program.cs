/******************************************************************************
The MIT License (MIT)

Copyright (c) 2013 Curtis Schlak

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adapter
{
    class Program
    {
        /// <summary>
        /// The <see cref="Main"/> method demonstrates the runtime creation of
        /// an adapter class that adapts the <see cref="Target"/> class to the
        /// <see cref="IAddNumbers"/> interface.
        /// </summary>
        /// <param name="args">Not used.</param>
        static void Main(string[] args)
        {
            // This is the instance of the Target that we'll adapt to the
            // IAddNumbers interface. Note that it does not implement the
            // IAddNumbers interface.
            Target target = new Target();

            // The AdapterBuilder.Build method does all that magic of
            // creating a class using reflection and IL emission. It then
            // instantiates that class, sets target onto a field of the
            // object, and returns it as an IAddNumbers.
            IAddNumbers adder = AdapterBuilder.Build<IAddNumbers, Target>(target);

            // Use the newly created instance of the generated-at-runtime class
            // that will forward the method call to the instance of Target
            // created on line 44.
            int result = adder.Add(5, 6);

            // This prints out the result.
            Console.WriteLine("The result of adding 5 and 6: {0}", result);

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true);
        }
    }
}

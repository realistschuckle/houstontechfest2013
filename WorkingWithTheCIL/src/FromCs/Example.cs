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

namespace FromCs
{
    /// <summary>
    /// The <see cref="Example{TFirst,TSecond}"/> class exists to allow you to compile
    /// representative C# and, then, open the resulting assembly in
    /// an IL decompiler such as IlSpy «http://ilspy.net».
    /// </summary>
    /// <typeparam name="TFirst">Some type parameter</typeparam>
    /// <typeparam name="TSecond">Another type parameter</typeparam>
    public class Example<TFirst, TSecond>
    {
        /// <summary>
        /// The <see cref="GenericMethod"/> shows the use of class-
        /// and method-level type parameters in IL. It also shows
        /// the use of the initobj instruction to return a default
        /// value of type <typeparamref name="TThird"/>.
        /// </summary>
        /// <typeparam name="TThird">
        /// Yet another type parameter.
        /// </typeparam>
        /// <param name="first">Just some parameter.</param>
        /// <returns>A default value.</returns>
        public TThird GenericMethod<TThird>(TFirst first)
        {
            return default(TThird);
        }

        /// <summary>
        /// The <see cref="SwitchStrings"/> method demostrates how
        /// the C# compiler translates a switch case based on
        /// strings into the corresponding IL.
        /// </summary>
        /// <param name="s">Some parameter.</param>
        /// <returns>Who cares, really?</returns>
        public string SwitchStrings(string s)
        {
            switch (s)
            {
                case "100":
                    return "C";
                case "101":
                    return "CI";
                case "102":
                    return "CII";
                case "103":
                    return "CIII";
                default:
                    return "?";
            }
        }

        /// <summary>
        /// The <see cref="SwitchSequence"/> method demostrates how
        /// the C# compiler translates a switch case based on sequential
        /// integers into the corresponding IL.
        /// </summary>
        /// <param name="i">Some parameter</param>
        /// <returns>Who cares?</returns>
        public string SwitchSequence(int i)
        {
            switch(i)
            {
                case 100:
                    return "C";
                case 101:
                    return "CI";
                case 102:
                    return "CII";
                case 103:
                    return "CIII";
                default:
                    return "?";
            }
        }

        /// <summary>
        /// The <see cref="SwitchNonSequence"/> method demostrates how
        /// the C# compiler translates a switch case based on sequential
        /// and non-sequential integers into the corresponding IL.
        /// </summary>
        /// <param name="i">Some parameter</param>
        /// <returns>Who cares?</returns>
        public string SwitchNonSequence(int i)
        {
            switch (i)
            {
                case 100:
                    return "C";
                case 101:
                    return "C";
                case 102:
                    return "C";
                case 200:
                    return "CC";
                case 300:
                    return "CCC";
                case 400:
                    return "CD";
                default:
                    return "?";
            }
        }

        /// <summary>
        /// The <see cref="Seh"/> method demonstrates how the C# compiler
        /// translates try/catch(Exception)/catch/finally blocks into IL.
        /// </summary>
        /// <param name="s">Some parameter.</param>
        public void Seh(string s)
        {
            try
            {
                PrintManyStrings(s);
            }
            catch(NullReferenceException e)
            {
                Console.WriteLine("too null");
                throw e;
            }
            catch
            {
                Console.WriteLine("what happened?");
                throw;
            }
            finally
            {
                Console.WriteLine("Glad that's over.");
            }
        }

        /// <summary>
        /// The <see cref="Summary"/> method demonstrates how the C#
        /// compiler translates a while/continue/break loop into IL.
        /// </summary>
        /// <param name="vals">Some parameter.</param>
        /// <returns>Who cares?</returns>
        public int While(params int[] vals)
        {
            int sum = 0;
            int i = 0;
            while(true)
            {
                if(i % 2 == 0)
                {
                    continue;
                }
                if(i >= vals.Length)
                {
                    break;
                }
                sum += vals[i];
                i++;
            }
            return sum;
        }

        /// <summary>
        /// The <see cref="PrintManyStrings"/> and
        /// <see cref="RunPrintManyStrings"/> methods demonstrate how the
        /// C# compiler translates the params keyword into IL.
        /// </summary>
        /// <param name="stuff">Some parameters.</param>
        public void PrintManyStrings(params string[] stuff)
        {
            foreach(var junk in stuff)
            {
                Console.WriteLine(junk);
            }
        }

        /// <summary>
        /// The <see cref="PrintManyStrings"/> and
        /// <see cref="RunPrintManyStrings"/> methods demonstrate how the
        /// C# compiler translates the params keyword into IL.
        /// </summary>
        public void RunPrintManyStrings()
        {
            PrintManyStrings("1", "2", "3", "4", "5", "6");
        }
    }
}

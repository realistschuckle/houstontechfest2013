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
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Adapter
{
    /// <summary>
    /// The <see cref="AdapterBuilder"/> class creates a class at runtime in
    /// the current <see cref="AppDomain"/> that adapts the methods of an
    /// instance of an object to corresponding methods with the same
    /// signatures on an interface.
    /// </summary>
    /// <remarks>
    /// This implementation will consume memory with each adaptation. To
    /// make it "production ready," make a cache of adaptation definitions
    /// and reuse them.
    /// </remarks>
    public class AdapterBuilder
    {
        /// <summary>
        /// The <see cref="Build"/> method creates a class at runtime in
        /// the current <see cref="AppDomain"/> that adapts the methods of an
        /// instance of an object to corresponding methods with the same
        /// signatures on an interface.
        /// </summary>
        /// <typeparam name="TInterface">
        /// The interface to which you want to adapt
        /// <typeparamref name="TClass"/>.
        /// </typeparam>
        /// <typeparam name="TClass">
        /// The class to which you want want to adapt
        /// <typeparamref name="TInterface"/>.
        /// </typeparam>
        /// <param name="target">
        /// The instance of <typeparamref name="TClass"/> that this
        /// method adapts to <typeparamref name="TInterface"/>.
        /// </param>
        /// <returns>The adapted object.</returns>
        public static TInterface Build<TInterface, TClass>(TClass target)
        {
            // Get the types. We'll need them later.
            Type from = typeof(TInterface);
            Type to = typeof(TClass);

            // Get the type of object.
            Type objt = typeof(object);

            // Get the domain in which to build the new class
            AppDomain domain = AppDomain.CurrentDomain;

            // Define a new assembly to build the new class
            string sasmName = Guid.NewGuid().ToString();
            AssemblyName asmName = new AssemblyName(sasmName);
            AssemblyBuilderAccess access = AssemblyBuilderAccess.Run;

            AssemblyBuilder asm;
            asm = domain.DefineDynamicAssembly(asmName, access);

            // Define a module to build the new class
            ModuleBuilder mod = asm.DefineDynamicModule(sasmName);
            
            // Get the interfaces that this will implement
            Type[] ifaces = new Type[]{typeof(TInterface)};

            // Define a type (class)
            string typeName = "From" + from.Name + "To" + to.Name;
            TypeAttributes typeAttrs = TypeAttributes.Public
                                     | TypeAttributes.AutoLayout;
            TypeBuilder type = mod.DefineType(typeName, typeAttrs, objt, ifaces);

            // Define a public field in which to store target
            FieldBuilder field = type.DefineField("$TARGET", to, FieldAttributes.Public);

            // Get the methods from the interface
            MethodInfo[] methods = from.GetMethods();

            // For each method...
            foreach(MethodInfo method in methods)
            {
                // Get a reference to the target method
                Type[] argTypes = method.GetParameters()
                                        .Select(p => p.ParameterType)
                                        .ToArray();
                MethodInfo targetMethod;
                targetMethod = to.GetMethod(method.Name, argTypes);

                // Define a method on the proxy
                MethodAttributes methodAttrs = MethodAttributes.Public
                                             | MethodAttributes.Virtual
                                             | MethodAttributes.Final;
                MethodBuilder fromMethod;
                Type retType = method.ReturnType;
                fromMethod = type.DefineMethod(method.Name, methodAttrs, retType, argTypes);

                // Define method body
                ILGenerator ilgen = fromMethod.GetILGenerator();
                ilgen.Emit(OpCodes.Ldarg_0);
                ilgen.Emit(OpCodes.Ldfld, field);
                for(int i = 0; i < argTypes.Length; i += 1)
                {
                    ilgen.Emit(OpCodes.Ldarg, i + 1);
                }
                ilgen.EmitCall(OpCodes.Call, targetMethod, Type.EmptyTypes);
                ilgen.Emit(OpCodes.Ret);
            }

            // Finalize the defintion of the type so we can use it.
            Type finalType = type.CreateType();

            // Get the default constructor so we can instantiate the class.
            ConstructorInfo ctor = finalType.GetConstructor(Type.EmptyTypes);

            // Instantiate the class.
            object inst = ctor.Invoke(null);

            // Get the field that will hold a reference to the adapted object.
            FieldInfo instField = finalType.GetField("$TARGET");

            // Set the value of the field.
            instField.SetValue(inst, target);

            // Return the newly instantiated object cast as the interface.
            return (TInterface)inst;
        }
    }
}

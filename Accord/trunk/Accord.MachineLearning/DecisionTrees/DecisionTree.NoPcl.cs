// Accord Machine Learning Library
// The Accord.NET Framework
// http://accord-framework.net
//
// Copyright © César Souza, 2009-2013
// cesarsouza at gmail.com
//
//    This library is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Lesser General Public
//    License as published by the Free Software Foundation; either
//    version 2.1 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with this library; if not, write to the Free Software
//    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//

namespace Accord.MachineLearning.DecisionTrees
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.Serialization;

    public partial class DecisionTree
    {


        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            foreach (DecisionNode node in this)
            {
                node.Owner = this;
            }
        }

#if !NET35

        /// <summary>
        ///   Creates a .NET assembly (.dll) containing a static class of
        ///   the given name implementing the decision tree. The class will
        ///   contain a single static Compute method implementing the tree.
        /// </summary>
        /// 
        /// <param name="assemblyName">The name of the assembly to generate.</param>
        /// <param name="className">The name of the generated static class.</param>
        /// 
        public void ToAssembly(string assemblyName, string className)
        {
            ToAssembly(assemblyName, "Accord.MachineLearning.DecisionTrees.Custom", className);
        }

        /// <summary>
        ///   Creates a .NET assembly (.dll) containing a static class of
        ///   the given name implementing the decision tree. The class will
        ///   contain a single static Compute method implementing the tree.
        /// </summary>
        /// 
        /// <param name="assemblyName">The name of the assembly to generate.</param>
        /// <param name="moduleName">The namespace which should contain the class.</param>
        /// <param name="className">The name of the generated static class.</param>
        /// 
        public void ToAssembly(string assemblyName, string moduleName, string className)
        {
            AssemblyBuilder da = AppDomain.CurrentDomain.DefineDynamicAssembly(
                new AssemblyName(assemblyName), AssemblyBuilderAccess.Save);

            ModuleBuilder dm = da.DefineDynamicModule(moduleName, assemblyName);
            TypeBuilder dt = dm.DefineType(className);
            MethodBuilder method = dt.DefineMethod("Compute",
                MethodAttributes.Public | MethodAttributes.Static);

            // Compile the tree into a method
            ToExpression().CompileToMethod(method);

            dt.CreateType();
            da.Save(assemblyName);
        }
#endif

    }
}


﻿using System;
using System.Reflection;

namespace FluentAssertions.Common
{
    public static class CSharpAccessModifierExtensions
    {
        internal static CSharpAccessModifier GetCSharpAccessModifier(this MethodBase methodBase)
        {
            if (methodBase.IsPrivate)
            {
                return CSharpAccessModifier.Private;
            }

            if (methodBase.IsFamily)
            {
                return CSharpAccessModifier.Protected;
            }

            if (methodBase.IsAssembly)
            {
                return CSharpAccessModifier.Internal;
            }

            if (methodBase.IsPublic)
            {
                return CSharpAccessModifier.Public;
            }

            if (methodBase.IsFamilyOrAssembly)
            {
                return CSharpAccessModifier.ProtectedInternal;
            }

            return CSharpAccessModifier.InvalidForCSharp;
        }

        internal static CSharpAccessModifier GetCSharpAccessModifier(this FieldInfo fieldInfo)
        {
            if (fieldInfo.IsPrivate)
            {
                return CSharpAccessModifier.Private;
            }

            if (fieldInfo.IsFamily)
            {
                return CSharpAccessModifier.Protected;
            }

            if (fieldInfo.IsAssembly)
            {
                return CSharpAccessModifier.Internal;
            }

            if (fieldInfo.IsPublic)
            {
                return CSharpAccessModifier.Public;
            }

            if (fieldInfo.IsFamilyOrAssembly)
            {
                return CSharpAccessModifier.ProtectedInternal;
            }

            return CSharpAccessModifier.InvalidForCSharp;
        }

        internal static CSharpAccessModifier GetCSharpAccessModifier(this Type type)
        {
            if (type.GetTypeInfo().IsNestedPrivate)
            {
                return CSharpAccessModifier.Private;
            }

            if (type.GetTypeInfo().IsNestedFamily)
            {
                return CSharpAccessModifier.Protected;
            }

            if (type.GetTypeInfo().IsNestedAssembly || (type.GetTypeInfo().IsClass && type.GetTypeInfo().IsNotPublic))
            {
                return CSharpAccessModifier.Internal;
            }

            if (type.GetTypeInfo().IsPublic || type.GetTypeInfo().IsNestedPublic)
            {
                return CSharpAccessModifier.Public;
            }

            if (type.GetTypeInfo().IsNestedFamORAssem)
            {
                return CSharpAccessModifier.ProtectedInternal;
            }

            return CSharpAccessModifier.InvalidForCSharp;
        }
    }
}
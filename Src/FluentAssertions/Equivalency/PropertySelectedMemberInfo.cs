using System;
using System.Reflection;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Provides an ISelectedMemberInfo for PropertyInfo objects
    /// </summary>
    internal class PropertySelectedMemberInfo : MemberInfoSelectedMemberInfo
    {
        private readonly PropertyInfo propertyInfo;

        public PropertySelectedMemberInfo(PropertyInfo propertyInfo)
            : base(propertyInfo)
        {
            this.propertyInfo = propertyInfo;
        }

        public override Type MemberType => propertyInfo.PropertyType;

        public override CSharpAccessModifier GetGetAccessModifier() => propertyInfo.GetGetMethod(nonPublic: true).GetCSharpAccessModifier();

        public override CSharpAccessModifier GetSetAccessModifier() => propertyInfo.GetSetMethod(nonPublic: true).GetCSharpAccessModifier();

        public override object GetValue(object obj, object[] index)
        {
            return propertyInfo.GetValue(obj, index);
        }
    }
}

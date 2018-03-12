using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace System
{
    public static class Tools
    {


        public static string GetNameOf(Expression<Func<object>> exp)
        {
            MemberExpression body = exp.Body as MemberExpression;

            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression) exp.Body;
                body = ubody.Operand as MemberExpression;
            }

            return body.Member.Name;
        }

        public static bool IsNullObj(this object obj)
        {
            return (null == obj);
        }

        public static bool IsNotNullObj(this object obj)
        {
            return null != obj;
        }

    }
}

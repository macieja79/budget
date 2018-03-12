using System.Reflection;

namespace Metaproject.Reflection
{
    public interface IPropertyMapperVisitor
    {
        void Proceed(PropertyInfo source, PropertyInfo destination);
    }

    class PropertyMapperCopy : IPropertyMapperVisitor
    {
        public void Proceed(PropertyInfo source, PropertyInfo destination)
        {
           
        }
    }
}
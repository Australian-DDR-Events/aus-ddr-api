using System.Reflection;
using AusDdrApi.Persistence;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;

namespace AusDdrApi.Extensions
{
    public class UseDatabaseContextAttribute : ObjectFieldDescriptorAttribute
    {
        public override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor, MemberInfo member)
        {
            descriptor.UseDbContext<DatabaseContext>();
        }
    }
}
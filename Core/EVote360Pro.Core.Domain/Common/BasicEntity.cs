
namespace EVote360Pro.Core.Domain.Common
{
    public class BasicEntity<TKey>
    {
        public required TKey Id { get; set; }
        public bool Activo { get; set; }
    }
}

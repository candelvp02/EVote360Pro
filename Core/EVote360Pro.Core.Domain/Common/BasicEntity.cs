namespace EVote360Pro.Core.Domain.Common
{
    public class BasicEntity<TKey>
    {
        public TKey Id { get; set; } = default!;
        public bool Activo { get; set; }
    }
}
namespace WorkItems.API.Models
{
    public enum RelevanciaItem
    {
        Baja = 1,
        Alta = 2
    }

    public enum EstadoItem
    {
        Pendiente = 1,
        Completado = 2
    }

    public class ItemTrabajo
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public RelevanciaItem Relevancia { get; set; }
        public DateTime FechaEntrega { get; set; }
        public EstadoItem Estado { get; set; } = EstadoItem.Pendiente;
        
        public string? NombreUsuarioAsignado { get; set; }
    }
}

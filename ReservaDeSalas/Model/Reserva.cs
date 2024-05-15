namespace ReservaDeSalas.Model
{
    public class Reserva
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Sala { get; set; }
        public string Data { get; set; }
        public string Hora { get; set; }
    }
}

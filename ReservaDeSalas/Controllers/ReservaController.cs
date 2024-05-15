using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservaDeSalas.Data;
using ReservaDeSalas.Model;

namespace ReservaDeSalas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        [HttpGet]
        [Route("getReservaPorDataEHora")]
        public async Task<IActionResult> getReservaAsync([FromServices]AppDbContext context, string sala, string data, string hora)
        {
            var reserva = await context.Reservas.AsNoTracking().Where(x => x.Sala == sala && x.Data == data && x.Hora == hora).FirstOrDefaultAsync();
            
            if (reserva == null) 
                return NotFound("Reserva não encontrada");
            
            return Ok("A sala já possui reserva nessa data e hora");
        }

        [HttpGet]
        [Route("reservasSala")]
        public async Task<IActionResult> GetUsuariosPorSalaAsync([FromServices]AppDbContext context)
        {
            var reservas = await context.Reservas.AsNoTracking().ToListAsync();

            var salas = new List<string>();
            foreach (var reserva in reservas) 
            {
                salas.Add(reserva.Sala);
            }

            return Ok(salas);
        }

        [HttpGet]
        [Route("reservaPorUsuario")]
        public async Task<IActionResult> GetReservasPorUsuario([FromServices]AppDbContext context, string user)
        {
            var reservas = await context.Reservas.Where(x => x.Usuario == user).ToListAsync();

            return Ok(reservas);
        }


        [HttpPost]
        [Route("agendar")]
        public async Task<IActionResult> CadastrarReserva([FromServices]AppDbContext context, string usuario, string sala, string data, string hora)
        {
            var reserva = await context.Reservas.AsNoTracking().Where(x => x.Sala == sala && x.Data == data && x.Hora == hora).FirstOrDefaultAsync();

            if (reserva != null)
                return BadRequest("Sala não está disponível nessa data e hora.");

            var novaReserva = new Reserva();
            novaReserva.Sala = sala;
            novaReserva.Data = data;
            novaReserva.Hora = hora;
            novaReserva.Usuario = usuario;

            await context.AddAsync(novaReserva);
            await context.SaveChangesAsync();

            return Ok("Reserva Concluida com sucesso");
        }

        [HttpPost]
        [Route("Remover")]
        public async Task<IActionResult> RemoverReserva([FromServices]AppDbContext context, string usuario, string sala, string data, string hora)
        {
            var reserva = await context.Reservas.AsNoTracking().Where(x => x.Sala == sala && x.Data == data && x.Hora == hora).FirstOrDefaultAsync();

            if (reserva != null)
            {
                if (reserva.Usuario == usuario)
                {
                    context.Reservas.Remove(reserva);
                    await context.SaveChangesAsync();
                    return Ok("Remoção de reserva concluída com sucesso.");
                }

                return BadRequest("A sala está reservada para outro usuário");
            }

            return NotFound("Não existe reserva para esta sala na data e hora fornecida");
        }
    }
}

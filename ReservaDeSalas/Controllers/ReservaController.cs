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
        [Route("consultar-reserva")]
        public async Task<IActionResult> getReservaAsync([FromServices]AppDbContext context, [FromBody] Reserva reserva)
        {
            var reservaDb = await context.Reservas.AsNoTracking().Where(x => x.Sala == reserva.Sala && x.Data == reserva.Data && x.Hora == reserva.Hora).FirstOrDefaultAsync();
            
            if (reservaDb == null) 
                return NotFound("Reserva não encontrada");
            
            return Ok("A sala já possui reserva nessa data e hora");
        }

        [HttpGet]
        [Route("reservas-sala")]
        public async Task<IActionResult> GetUsuariosPorSalaAsync([FromServices]AppDbContext context, string sala)
        {
            var reservas = await context.Reservas.AsNoTracking().Where(x => x.Sala == sala).ToListAsync();

            var user = new List<string>();
            foreach (var reserva in reservas) 
            {
                user.Add(reserva.Usuario);
            }

            return Ok(user);
        }

        [HttpGet]
        [Route("reservas-usuario")]
        public async Task<IActionResult> GetReservasPorUsuario([FromServices]AppDbContext context, string user)
        {
            var reservas = await context.Reservas.Where(x => x.Usuario == user).ToListAsync();

            return Ok(reservas);
        }


        [HttpPost]
        [Route("agendar")]
        public async Task<IActionResult> CadastrarReserva([FromServices]AppDbContext context, [FromBody] Reserva reserva)
        {
            var reservaDB = await context.Reservas.AsNoTracking().Where(x => x.Sala == reserva.Sala && x.Data == reserva.Data && x.Hora == reserva.Hora).FirstOrDefaultAsync();

            if (reservaDB != null)
                return BadRequest("Sala não está disponível nessa data e hora.");

            var novaReserva = new Reserva();
            novaReserva.Sala = reserva.Sala;
            novaReserva.Data = reserva.Data;
            novaReserva.Hora = reserva.Hora;
            novaReserva.Usuario = reserva.Usuario;

            await context.AddAsync(novaReserva);
            await context.SaveChangesAsync();

            return Ok("Reserva Concluida com sucesso");
        }

        [HttpDelete]
        [Route("Remover")]
        public async Task<IActionResult> RemoverReserva([FromServices]AppDbContext context, [FromBody] Reserva reserva)
        {
            var reservaDb = await context.Reservas.AsNoTracking().Where(x => x.Sala == reserva.Sala && x.Data == reserva.Data && x.Hora == reserva.Hora).FirstOrDefaultAsync();

            if (reservaDb != null)
            {
                if (reservaDb.Usuario == reserva.Usuario)
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

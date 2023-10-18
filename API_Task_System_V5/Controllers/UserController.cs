using API_Task_System_V5.Models;
using API_Task_System_V5.Token;
using Domain.Interfaces;
using Entities.Enums;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Task_System_V5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUsuario _iUsuario;

        private readonly UserManager<ApplicationUser> _userManager;
        
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(IUsuario iUsuario, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _iUsuario = iUsuario;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/CriarToken")]
        public async Task<IActionResult> CriarToken([FromBody] Login login)
        {
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Senha))
                return Unauthorized();

            var resultado = await _iUsuario.ExisteUsuario(login.Email, login.Senha);

            if (resultado)
            {
                var idUsuario = await _iUsuario.RetornaIdUsuario(login.Email);
                var token = new TokenJWTBuilder()
                    .AddSecurityKey(JwtSecurityKey.Create("Secret_Key-12345678"))
                .AddSubject("Empresa - Projeto DDD")
                .AddIssuer("Teste.Securiry.Bearer")
                .AddAudience("Teste.Securiry.Bearer")
                .AddClaim("idUsuario", idUsuario)
                .AddExpiry(5)
                .Builder();

                return Ok(token.value);
            }
            else
            {
                return Unauthorized();
            }

        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/AdicionarUsuario")]
        public async Task<IActionResult> AdicionarUsuario([FromBody] Login login)
        {
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Senha))
                return Ok("Falta alguns dados");

            await _iUsuario.AdicionarUsuario(login.Email, login.Senha, login.Idade, login.Celular);
            return Ok("Usuário adicionado com sucesso");

        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/CriarTokenIdentity")]
        public async Task<IActionResult> CriarTokenIdentity([FromBody] Login login)
        {
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Senha))
                return Ok("Falta alguns dados");

            var resultado = await _signInManager.PasswordSignInAsync(login.Email, login.Senha, false, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                var idUsuario = await _iUsuario.RetornaIdUsuario(login.Email);
                var token = new TokenJWTBuilder()
                    .AddSecurityKey(JwtSecurityKey.Create("Secret_Key-12345678"))
                .AddSubject("Empresa - Projeto DDD")
                .AddIssuer("Teste.Securiry.Bearer")
                .AddAudience("Teste.Securiry.Bearer")
                .AddClaim("idUsuario", idUsuario)
                .AddExpiry(5)
                .Builder();

                return Ok(token.value);
            }
            else
            {
                return Unauthorized();
            }
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/CriarUsuarioIdentity")]
        public async Task<IActionResult> CriarUsuarioIdentity([FromBody] Login login)
        {
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Senha))
                return Ok("Falta alguns dados");

            var user = new ApplicationUser
            {
                UserName = login.Email,
                Email = login.Email,
                Celular = login.Celular,
                UserType = UserType.Common
            };

            var resultado = await _userManager.CreateAsync(user, login.Senha);

            if (resultado.Errors.Any())
                return Ok(resultado.Errors);

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            // retorno email 
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var resultado2 = await _userManager.ConfirmEmailAsync(user, code);

            if (resultado2.Succeeded)
                return Ok("Usuário Adicionado com sucesso");
            else
                return Ok("Erro ao confirmar usuário");

        }


    }
}
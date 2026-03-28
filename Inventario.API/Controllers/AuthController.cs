using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly Supabase.Client _supabaseClient;

    public AuthController(Supabase.Client supabaseClient)
    {
        _supabaseClient = supabaseClient;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO login)
    {
        try
        {
            // O Supabase faz a validação por nós
            var session = await _supabaseClient.Auth.SignIn(login.Email, login.Password);

            if (session != null && !string.IsNullOrEmpty(session.AccessToken))
            {
                // Retornamos o token que o Supabase gerou
                return Ok(new { token = session.AccessToken, user = session.User?.Email });
            }

            return Unauthorized("Falha na autenticação.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro ao realizar login: {ex.Message}");
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] LoginDTO login)
    {
        try
        {
            var session = await _supabaseClient.Auth.SignUp(login.Email, login.Password);
            return Ok("Usuário criado com sucesso! Verifique seu e-mail (se a confirmação estiver ativa).");
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro ao criar usuário: {ex.Message}");
        }
    }
}
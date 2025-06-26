using ReceitasMaster.Models;

namespace ReceitasMaster {
    public class Program {
        public static string Conetor = "";
        public static string SmtpIP = "";

        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSession(s => s.IdleTimeout = TimeSpan.FromMinutes(20));
            builder.Services.AddMvc();

            var config = builder.Configuration.GetSection("Configuracao").Get<Configuracao>();
            Conetor = config.Conexao;
            SmtpIP = config.SmtpIP;

            var app = builder.Build();

            app.UseSession();
            app.UseStaticFiles();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Receita}/{action=Index}/{id?}"
            );

            app.Run();
        }
    }
}
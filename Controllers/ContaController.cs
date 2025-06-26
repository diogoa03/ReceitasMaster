using ReceitasMaster.Models;
using Microsoft.AspNetCore.Mvc;

namespace ReceitasMaster.Controllers {
    public class ContaController : Controller {

        [HttpGet]
        public IActionResult Login() {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Conta contaEnviada) {
            HelperConta helper = new HelperConta();
            HttpContext.Session.SetString("contaAcesso", helper.serializeConta(helper.authUser(contaEnviada.Email, contaEnviada.Senha)));
            return RedirectToAction("Index", "Receita");
        }

        public IActionResult Logout() {
            HelperConta helper = new HelperConta();
            HttpContext.Session.SetString("contaAcesso", helper.serializeConta(helper.setGuest()));
            return RedirectToAction("Index", "Receita");
        }
    }
}
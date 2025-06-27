using ReceitasMaster.Models;
using Microsoft.AspNetCore.Mvc;
using ReceitasMaster.Models.ReceitasMaster.Models;

namespace ReceitasMaster.Controllers {
    public class ContaController : GenericBaseController {

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
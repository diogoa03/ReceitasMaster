using ReceitasMaster.Models;
using Microsoft.AspNetCore.Mvc;

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

        // Gestão de utilizadores - apenas para editores
        public IActionResult Gestao() {
            if (_conta.NivelAcesso == 2) { // Apenas editores
                HelperConta helper = new HelperConta();
                List<Conta> lista = helper.list();
                ViewBag.Conta = _conta;
                return View(lista);
            }
            return RedirectToAction("Index", "Receita");
        }

        public IActionResult AlterarStatus(string id, bool ativo) {
            if (_conta.NivelAcesso == 2) { // Apenas editores
                HelperConta helper = new HelperConta();
                helper.updateStatus(id, ativo);
            }
            return RedirectToAction("Gestao", "Conta");
        }
    }
}
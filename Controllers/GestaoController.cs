using Microsoft.AspNetCore.Mvc;
using ReceitasMaster.Models;

namespace ReceitasMaster.Controllers
{
    public class GestaoController : GenericBaseController {

        public IActionResult Index() {
            // Apenas editores podem aceder à gestão
            if (_conta.NivelAcesso != 2) {
                return RedirectToAction("Index", "Receita");
            }

            HelperGestaoContas helper = new HelperGestaoContas();
            List<Conta> listaContas = helper.list();

            ViewBag.Conta = _conta;
            return View(listaContas);
        }

        public IActionResult ToggleStatus(string id) {
            // Apenas editores podem alterar status
            if (_conta.NivelAcesso != 2) {
                return RedirectToAction("Index", "Receita");
            }

            HelperGestaoContas helper = new HelperGestaoContas();
            Conta? conta = helper.get(id);

            if (conta != null) {
                // Não permitir desativar o próprio editor ou contas de nível 2
                if (conta.GuidConta != _conta.GuidConta && conta.NivelAcesso < 2) {
                    helper.updateStatus(id, !conta.Ativa);
                }
            }

            return RedirectToAction("Index", "Gestao");
        }
    }
}
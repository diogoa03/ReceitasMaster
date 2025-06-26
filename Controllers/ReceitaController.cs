using Microsoft.AspNetCore.Mvc;
using ReceitasMaster.Models;

namespace ReceitasMaster.Controllers {
    public class ReceitaController : GenericBaseController {

        public IActionResult Index(string? filtro) {
            bool? ativas = null;

            switch (filtro) {
                case "ativas":
                    ativas = true;
                    break;
                case "inativas":
                    ativas = false;
                    break;
                case "todas":
                default:
                    ativas = null;
                    break;
            }

            HelperReceita helper = new HelperReceita();
            List<Receita> lista = helper.list(ativas);

            ViewBag.TotalReceitas = helper.getTotalReceitas();
            ViewBag.ReceitasAtivas = helper.getReceitasAtivas();
            ViewBag.Conta = _conta;
            return View(lista);
        }

        public IActionResult Detalhe(string id) {
            HelperReceita helper = new HelperReceita();
            Receita? receita = helper.get(id);

            if (receita != null) {
                ViewBag.Conta = _conta;
                return View(receita);
            }
            else {
                return RedirectToAction("Index", "Receita");
            }
        }

        [HttpGet]
        public IActionResult Criar() {
            if (_conta.NivelAcesso > 0) {
                HelperReceita helper = new HelperReceita();
                ViewBag.Categorias = helper.getCategorias();
                return View();
            }
            return RedirectToAction("Index", "Receita");
        }

        [HttpPost]
        public IActionResult Criar(Receita receita) {
            if (_conta.NivelAcesso > 0) {
                if (ModelState.IsValid) {
                    receita.GuidConta = _conta.GuidConta.ToString();
                    HelperReceita helper = new HelperReceita();
                    helper.save(receita);
                    return RedirectToAction("Index", "Receita");
                }
                return View(receita);
            }
            return RedirectToAction("Index", "Receita");
        }

        [HttpGet]
        public IActionResult Editar(string id) {
            if (_conta.NivelAcesso > 0) {
                HelperReceita helper = new HelperReceita();
                Receita? receita2Edit = helper.get(id);

                if (receita2Edit != null) {
                    // Autor só pode editar suas próprias receitas
                    if (_conta.NivelAcesso == 1 && receita2Edit.GuidConta != _conta.GuidConta.ToString()) {
                        return RedirectToAction("Index", "Receita");
                    }

                    ViewBag.Categorias = helper.getCategorias();
                    return View(receita2Edit);
                }
                else {
                    return RedirectToAction("Index", "Receita");
                }
            }
            else {
                return RedirectToAction("Index", "Receita");
            }
        }

        [HttpPost]
        public IActionResult Editar(string id, Receita receitaPostada) {
            if (_conta.NivelAcesso > 0) {
                if (ModelState.IsValid) {
                    HelperReceita helper = new HelperReceita();
                    Receita? receitaExistente = helper.get(id);

                    // Verificar permissões
                    if (receitaExistente != null) {
                        if (_conta.NivelAcesso == 1 && receitaExistente.GuidConta != _conta.GuidConta.ToString()) {
                            return RedirectToAction("Index", "Receita");
                        }

                        receitaPostada.GuidConta = receitaExistente.GuidConta; // Manter o autor original
                        helper.save(receitaPostada, id);
                    }

                    return RedirectToAction("Index", "Receita");
                }
                else {
                    return View(receitaPostada);
                }
            }
            else {
                return RedirectToAction("Index", "Receita");
            }
        }

        public IActionResult Eliminar(string id) {
            if (_conta.NivelAcesso == 2) { // Apenas editores podem eliminar
                HelperReceita helper = new HelperReceita();
                helper.delete(id);
            }
            return RedirectToAction("Index", "Receita");
        }
    }
}
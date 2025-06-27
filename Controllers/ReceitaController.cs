using Microsoft.AspNetCore.Mvc;
using ReceitasMaster.Models;

namespace ReceitasMaster.Controllers {
    public class ReceitaController : GenericBaseController {

        public IActionResult Index(string? id) {
            Receita.EstadoReceita estadoListagem = Receita.EstadoReceita.Ativa;

            switch (id) {
                case "0":
                    estadoListagem = Receita.EstadoReceita.Inativa;
                    break;
                case "1":
                    estadoListagem = Receita.EstadoReceita.Ativa;
                    break;
                case "2":
                    estadoListagem = Receita.EstadoReceita.Todas;
                    break;
                default:
                    estadoListagem = Receita.EstadoReceita.Ativa;
                    break;
            }

            HelperReceita helper = new HelperReceita();

            // A BD agora controla o acesso automaticamente
            List<Receita> lista = helper.list(estadoListagem, _conta.NivelAcesso);

            // Contadores que respeitam o nível de acesso
            ViewBag.TotalReceitas = helper.getTotalReceitas(_conta.NivelAcesso);
            ViewBag.ReceitasAtivas = helper.getReceitasAtivas();
            ViewBag.ReceitasInativas = helper.getReceitasInativas(_conta.NivelAcesso);
            ViewBag.ReceitasRapidas = helper.getReceitasRapidas();
            ViewBag.ReceitasDemoradas = helper.getReceitasDemoradas();
            ViewBag.Conta = _conta;
            return View(lista);
        }

        public IActionResult Detalhe(string id) {
            HelperReceita helper = new HelperReceita();

            // A BD controla automaticamente se o utilizador pode ver a receita
            Receita? receita = helper.get(id, _conta.NivelAcesso);

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
                return View();
            }
            return RedirectToAction("Index", "Receita");
        }

        [HttpPost]
        public IActionResult Criar(Receita receita) {
            if (_conta.NivelAcesso > 0) {
                if (ModelState.IsValid) {
                    receita.GuidConta = _conta.Email;
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

                // Usar nível de acesso máximo para edição (editores podem editar inativas)
                Receita? receita2Edit = helper.get(id, 2);

                if (receita2Edit != null) {
                    // Autor só pode editar suas próprias receitas
                    if (_conta.NivelAcesso == 1 && receita2Edit.GuidConta != _conta.Email) {
                        return RedirectToAction("Index", "Receita");
                    }
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
                    Receita? receitaExistente = helper.get(id, 2); 

                    if (receitaExistente != null) {
                        if (_conta.NivelAcesso == 1 && receitaExistente.GuidConta != _conta.Email) {
                            return RedirectToAction("Index", "Receita");
                        }
                        receitaPostada.GuidConta = receitaExistente.GuidConta;
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
            if (_conta.NivelAcesso == 2) {
                HelperReceita helper = new HelperReceita();
                helper.delete(id);
            }
            return RedirectToAction("Index", "Receita");
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReceitasMaster.Models;

namespace ReceitasMaster.Controllers {
    public class GenericBaseController : Controller {
        protected Conta? _conta;

        public override void OnActionExecuting(ActionExecutingContext context) {
            base.OnActionExecuting(context);
            HelperConta helperConta = new HelperConta();

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("contaAcesso"))) {
                HttpContext.Session.SetString("contaAcesso", helperConta.serializeConta(helperConta.setGuest()));
            }

            _conta = helperConta.deserializeConta(HttpContext.Session.GetString("contaAcesso") ?? string.Empty);
        }
    }
}
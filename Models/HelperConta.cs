using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json;

namespace ReceitasMaster.Models {
    namespace ReceitasMaster.Models {
        public class HelperConta {

            public Conta setGuest() {
                return new Conta {
                    GuidConta = Guid.NewGuid(),
                    Nome = "Visitante",
                    Email = "visitante@receitasmaster.pt",
                    NivelAcesso = 0,
                    Senha = "",
                    Ativa = true
                };
            }

            public Conta authUser(string email, string senha) {
                if (email == "editor@receitasmaster.pt" && senha == "editor123") {
                    return new Conta {
                        GuidConta = Guid.NewGuid(),
                        Nome = "Editor Principal",
                        Email = "editor@receitasmaster.pt",
                        NivelAcesso = 2,
                        Senha = "editor123",
                        Ativa = true
                    };
                }
                if (email == "autor@receitasmaster.pt" && senha == "autor123") {
                    return new Conta {
                        GuidConta = Guid.NewGuid(),
                        Nome = "Autor Teste",
                        Email = "autor@receitasmaster.pt",
                        NivelAcesso = 1,
                        Senha = "autor123",
                        Ativa = true
                    };
                }
                return setGuest();
            }

            public string serializeConta(Conta conta) {
                return JsonSerializer.Serialize(conta);
            }

            public Conta? deserializeConta(string json) {
                Conta? c;
                try {
                    c = JsonSerializer.Deserialize<Conta>(json);
                }
                catch {
                    c = null;
                }
                return c;
            }
        }
    }
}
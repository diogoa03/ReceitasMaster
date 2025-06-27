using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json;

namespace ReceitasMaster.Models {
    public class HelperConta : HelperBase {

        public Conta setGuest() {
            // Buscar o GUID real do visitante da base de dados
            Conta? visitante = getContaByEmail("visitante@receitasmaster.pt");
            if (visitante != null) {
                return visitante;
            }

            // Fallback se não encontrar
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
            // Buscar utilizador real da base de dados
            Conta? conta = getContaByEmailSenha(email, senha);
            if (conta != null) {
                return conta;
            }

            // Se não encontrar, retorna visitante
            return setGuest();
        }

        private Conta? getContaByEmail(string email) {
            try {
                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter();
                SqlCommand comando = new SqlCommand();
                SqlConnection conexao = new SqlConnection(ConetorHerdado);

                comando.CommandType = CommandType.Text;
                comando.Connection = conexao;
                comando.CommandText = "SELECT guidConta, nome, email, nivelAcesso, ativa FROM tConta WHERE email = @Email";
                comando.Parameters.AddWithValue("@Email", email);

                adapter.SelectCommand = comando;
                adapter.Fill(dt);

                if (dt.Rows.Count == 1) {
                    DataRow linha = dt.Rows[0];
                    Conta conta = new Conta();
                    conta.GuidConta = (Guid)linha["guidConta"];
                    conta.Nome = linha["nome"].ToString();
                    conta.Email = linha["email"].ToString();
                    conta.NivelAcesso = Convert.ToInt32(linha["nivelAcesso"]);
                    conta.Ativa = Convert.ToBoolean(linha["ativa"]);
                    return conta;
                }
            }
            catch {
                // Se houver erro, retorna null
            }
            return null;
        }

        private Conta? getContaByEmailSenha(string email, string senha) {
            try {
                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter();
                SqlCommand comando = new SqlCommand();
                SqlConnection conexao = new SqlConnection(ConetorHerdado);

                comando.CommandType = CommandType.Text;
                comando.Connection = conexao;
                comando.CommandText = "SELECT guidConta, nome, email, nivelAcesso, ativa FROM tConta WHERE email = @Email AND senha = @Senha AND ativa = 1";
                comando.Parameters.AddWithValue("@Email", email);
                comando.Parameters.AddWithValue("@Senha", senha);

                adapter.SelectCommand = comando;
                adapter.Fill(dt);

                if (dt.Rows.Count == 1) {
                    DataRow linha = dt.Rows[0];
                    Conta conta = new Conta();
                    conta.GuidConta = (Guid)linha["guidConta"];
                    conta.Nome = linha["nome"].ToString();
                    conta.Email = linha["email"].ToString();
                    conta.NivelAcesso = Convert.ToInt32(linha["nivelAcesso"]);
                    conta.Ativa = Convert.ToBoolean(linha["ativa"]);
                    return conta;
                }
            }
            catch {
                // Se houver erro, retorna null
            }
            return null;
        }

        public List<Conta> list() {
            DataTable dt = new DataTable();
            List<Conta> saida = new List<Conta>();
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand comando = new SqlCommand();
            SqlConnection conexao = new SqlConnection(ConetorHerdado);
            comando.CommandType = CommandType.StoredProcedure;
            comando.Connection = conexao;
            comando.CommandText = "QConta_List";

            adapter.SelectCommand = comando;
            adapter.Fill(dt);

            foreach (DataRow linha in dt.Rows) {
                Conta conta = new Conta();
                conta.GuidConta = (Guid)linha["guidConta"];
                conta.Nome = linha["nome"].ToString();
                conta.Email = linha["email"].ToString();
                conta.NivelAcesso = Convert.ToInt32(linha["nivelAcesso"]);
                conta.Ativa = Convert.ToBoolean(linha["ativa"]);
                saida.Add(conta);
            }
            return saida;
        }

        public bool updateStatus(string guidConta, bool ativa) {
            try {
                SqlCommand comando = new SqlCommand();
                SqlConnection conexao = new SqlConnection(ConetorHerdado);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Connection = conexao;
                comando.CommandText = "QConta_UpdateStatus";
                comando.Parameters.AddWithValue("@GuidConta", guidConta);
                comando.Parameters.AddWithValue("@Ativa", ativa);
                conexao.Open();
                comando.ExecuteNonQuery();
                conexao.Close();
                conexao.Dispose();
                return true;
            }
            catch {
                return false;
            }
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
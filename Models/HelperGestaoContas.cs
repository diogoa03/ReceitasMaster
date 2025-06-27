using Microsoft.Data.SqlClient;
using System.Data;


namespace ReceitasMaster.Models {
    public class HelperGestaoContas : HelperBase {
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
                Conta conta = new Conta {
                    GuidConta = (Guid)linha["guidConta"],
                    Nome = linha["nome"].ToString(),
                    Email = linha["email"].ToString(),
                    NivelAcesso = Convert.ToInt32(linha["nivelAcesso"]),
                    Ativa = Convert.ToBoolean(linha["ativa"]),
                    DthrRegisto = Convert.ToDateTime(linha["dthrRegisto"])
                };
                saida.Add(conta);
            }
            return saida;
        }

        public Conta? get(string guidConta) {
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand comando = new SqlCommand();
            SqlConnection conexao = new SqlConnection(ConetorHerdado);
            comando.CommandType = CommandType.StoredProcedure;
            comando.Connection = conexao;
            comando.CommandText = "QConta_Get";
            comando.Parameters.AddWithValue("@GuidConta", guidConta);

            adapter.SelectCommand = comando;
            adapter.Fill(dt);

            if (dt.Rows.Count == 1) {
                DataRow linha = dt.Rows[0];
                return new Conta {
                    GuidConta = (Guid)linha["guidConta"],
                    Nome = linha["nome"].ToString(),
                    Email = linha["email"].ToString(),
                    NivelAcesso = Convert.ToInt32(linha["nivelAcesso"]),
                    Senha = linha["senha"].ToString(),
                    Ativa = Convert.ToBoolean(linha["ativa"]),
                    DthrRegisto = Convert.ToDateTime(linha["dthrRegisto"])
                };
            }
            return null;
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
    }
}

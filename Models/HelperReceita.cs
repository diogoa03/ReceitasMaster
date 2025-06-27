using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using ReceitasMaster.Models;
using System.Data;

namespace ReceitasMaster.Models {
    public class HelperReceita : HelperBase {

        public List<Receita> list(Receita.EstadoReceita estado) {
            DataTable dt = new DataTable();
            List<Receita> saida = new List<Receita>();
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand comando = new SqlCommand();
            SqlConnection conexao = new SqlConnection(ConetorHerdado);
            comando.CommandType = CommandType.StoredProcedure;
            comando.Connection = conexao;
            comando.CommandText = "QReceita_List";

            if (estado != Receita.EstadoReceita.Todas) {
                comando.Parameters.AddWithValue("@Ativas", estado == Receita.EstadoReceita.Ativa);
            }

            adapter.SelectCommand = comando;
            adapter.Fill(dt);

            foreach (DataRow linha in dt.Rows) {
                Receita receita = new Receita(linha["guidReceita"].ToString());
                receita.Titulo = linha["titulo"].ToString();
                receita.Descricao = linha["descricao"].ToString();
                receita.Instrucoes = linha["instrucoes"].ToString();
                receita.Ativa = Convert.ToBoolean(linha["ativa"]);
                receita.DataCriacao = Convert.ToDateTime(linha["dataCriacao"]);
                receita.GuidConta = linha["guidConta"].ToString();
                saida.Add(receita);
            }
            return saida;
        }

        public void delete(string guidReceita2Del) {
            SqlCommand comando = new SqlCommand();
            SqlConnection conexao = new SqlConnection(ConetorHerdado);
            comando.CommandType = CommandType.StoredProcedure;
            comando.Connection = conexao;
            comando.CommandText = "QReceita_Delete";
            comando.Parameters.AddWithValue("@GuidReceita", guidReceita2Del);
            conexao.Open();
            comando.ExecuteNonQuery();
            conexao.Close();
            conexao.Dispose();
        }

        public Receita? get(string guidReceita) {
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand comando = new SqlCommand();
            SqlConnection conexao = new SqlConnection(ConetorHerdado);
            comando.CommandType = CommandType.StoredProcedure;
            comando.Connection = conexao;
            comando.CommandText = "QReceita_Get";
            comando.Parameters.AddWithValue("@GuidReceita", guidReceita);

            adapter.SelectCommand = comando;
            adapter.Fill(dt);

            if (dt.Rows.Count == 1) {
                DataRow linha = dt.Rows[0];
                Receita receita = new Receita(linha["guidReceita"].ToString());
                receita.Titulo = linha["titulo"].ToString();
                receita.Descricao = linha["descricao"].ToString();
                receita.Instrucoes = linha["instrucoes"].ToString();
                receita.Ativa = Convert.ToBoolean(linha["ativa"]);
                receita.DataCriacao = Convert.ToDateTime(linha["dataCriacao"]);
                receita.GuidConta = linha["guidConta"].ToString();
                return receita;
            }
            return null;
        }

        public Boolean save(Receita receitaSent, string guidReceita = "") {
            Boolean result = false;
            SqlCommand comando = new SqlCommand();
            SqlConnection conexao = new SqlConnection(ConetorHerdado);
            comando.CommandType = CommandType.StoredProcedure;
            comando.Connection = conexao;

            if (string.IsNullOrEmpty(guidReceita)) {
                // INSERT - Nova receita
                comando.CommandText = "QReceita_Insert";
                comando.Parameters.AddWithValue("@Titulo", receitaSent.Titulo);
                comando.Parameters.AddWithValue("@Descricao", receitaSent.Descricao);
                comando.Parameters.AddWithValue("@Instrucoes", receitaSent.Instrucoes);
                comando.Parameters.AddWithValue("@Ativa", receitaSent.Ativa);
                comando.Parameters.AddWithValue("@GuidConta", receitaSent.GuidConta);
            }
            else {
                // UPDATE - Receita existente
                comando.CommandText = "QReceita_Update";
                comando.Parameters.AddWithValue("@GuidReceita", guidReceita);
                comando.Parameters.AddWithValue("@Titulo", receitaSent.Titulo);
                comando.Parameters.AddWithValue("@Descricao", receitaSent.Descricao);
                comando.Parameters.AddWithValue("@Instrucoes", receitaSent.Instrucoes);
                comando.Parameters.AddWithValue("@Ativa", receitaSent.Ativa);
            }

            conexao.Open();
            comando.ExecuteNonQuery();
            conexao.Close();
            conexao.Dispose();
            result = true;

            return result;
        }

        public int getTotalReceitas() {
            int total = 0;
            SqlCommand comando = new SqlCommand();
            SqlConnection conexao = new SqlConnection(ConetorHerdado);
            comando.CommandType = CommandType.StoredProcedure;
            comando.Connection = conexao;
            comando.CommandText = "QReceita_GetTotal";
            conexao.Open();
            total = Convert.ToInt32(comando.ExecuteScalar());
            conexao.Close();
            conexao.Dispose();
            return total;
        }

        public int getReceitasAtivas() {
            int total = 0;
            SqlCommand comando = new SqlCommand();
            SqlConnection conexao = new SqlConnection(ConetorHerdado);
            comando.CommandType = CommandType.StoredProcedure;
            comando.Connection = conexao;
            comando.CommandText = "QReceita_GetAtivas";
            conexao.Open();
            total = Convert.ToInt32(comando.ExecuteScalar());
            conexao.Close();
            conexao.Dispose();
            return total;
        }
    }
}
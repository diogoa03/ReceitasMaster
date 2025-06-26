using Microsoft.Data.SqlClient;
using System.Data;

namespace ReceitasMaster.Models {
    public class HelperReceita : HelperBase {

        public List<Receita> list(bool? ativas = null) {
            DataTable dt = new DataTable();
            List<Receita> saida = new List<Receita>();
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand comando = new SqlCommand();
            SqlConnection conexao = new SqlConnection(ConetorHerdado);
            comando.CommandType = CommandType.StoredProcedure;
            comando.Connection = conexao;
            comando.CommandText = "QReceita_List";

            if (ativas.HasValue) {
                comando.Parameters.AddWithValue("@Ativas", ativas.Value);
            }

            adapter.SelectCommand = comando;
            adapter.Fill(dt);

            foreach (DataRow linha in dt.Rows) {
                Receita receita = new Receita(linha["GuidReceita"].ToString());
                receita.Titulo = linha["titulo"].ToString();
                receita.Descricao = linha["descricao"].ToString();
                receita.TempoPreparo = Convert.ToInt32(linha["tempoPreparo"]);
                receita.Porcoes = Convert.ToInt32(linha["porcoes"]);
                receita.Instrucoes = linha["instrucoes"].ToString();
                receita.Ativa = Convert.ToBoolean(linha["ativa"]);
                receita.DataCriacao = Convert.ToDateTime(linha["dataCriacao"]);
                receita.NivelDificuldade = (Receita.Dificuldade)Convert.ToByte(linha["nivelDificuldade"]);
                receita.GuidCategoria = linha["guidCategoria"].ToString();
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
                receita.TempoPreparo = Convert.ToInt32(linha["tempoPreparo"]);
                receita.Porcoes = Convert.ToInt32(linha["porcoes"]);
                receita.Instrucoes = linha["instrucoes"].ToString();
                receita.Ativa = Convert.ToBoolean(linha["ativa"]);
                receita.DataCriacao = Convert.ToDateTime(linha["dataCriacao"]);
                receita.NivelDificuldade = (Receita.Dificuldade)Convert.ToByte(linha["nivelDificuldade"]);
                receita.GuidCategoria = linha["guidCategoria"].ToString();
                receita.GuidConta = linha["guidConta"].ToString();
                return receita;
            }
            return null;
        }

        public Boolean save(Receita receitaSent, string guidReceita = "") {
            Boolean result = false;
            Receita? receita2Save;
            string instrucaoSQL = "";

            if (string.IsNullOrEmpty(guidReceita)) {
                receita2Save = new Receita();
            }
            else {
                receita2Save = get(guidReceita);
            }

            if (receita2Save != null) {
                receita2Save.Titulo = receitaSent.Titulo;
                receita2Save.Descricao = receitaSent.Descricao;
                receita2Save.TempoPreparo = receitaSent.TempoPreparo;
                receita2Save.Porcoes = receitaSent.Porcoes;
                receita2Save.Instrucoes = receitaSent.Instrucoes;
                receita2Save.Ativa = receitaSent.Ativa;
                receita2Save.NivelDificuldade = receitaSent.NivelDificuldade;
                receita2Save.GuidCategoria = receitaSent.GuidCategoria;
                receita2Save.GuidConta = receitaSent.GuidConta;

                if (receita2Save.GuidReceita == Guid.Empty.ToString()) {
                    instrucaoSQL = "QReceita_Insert";
                }
                else {
                    instrucaoSQL = "QReceita_Update";
                }

                SqlCommand comando = new SqlCommand();
                SqlConnection conexao = new SqlConnection(ConetorHerdado);
                comando.CommandType = CommandType.StoredProcedure;
                comando.CommandText = instrucaoSQL;
                comando.Connection = conexao;

                comando.Parameters.AddWithValue("@Titulo", receita2Save.Titulo);
                comando.Parameters.AddWithValue("@Descricao", receita2Save.Descricao);
                comando.Parameters.AddWithValue("@TempoPreparo", receita2Save.TempoPreparo);
                comando.Parameters.AddWithValue("@Porcoes", receita2Save.Porcoes);
                comando.Parameters.AddWithValue("@Instrucoes", receita2Save.Instrucoes);
                comando.Parameters.AddWithValue("@Ativa", receita2Save.Ativa);
                comando.Parameters.AddWithValue("@NivelDificuldade", receita2Save.NivelDificuldade);
                comando.Parameters.AddWithValue("@GuidCategoria", receita2Save.GuidCategoria);
                comando.Parameters.AddWithValue("@GuidConta", receita2Save.GuidConta);
                comando.Parameters.AddWithValue("@GuidReceita", receita2Save.GuidReceita);

                conexao.Open();
                comando.ExecuteNonQuery();
                conexao.Close();
                conexao.Dispose();
                result = true;
            }
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

        public List<Categoria> getCategorias() {
            DataTable dt = new DataTable();
            List<Categoria> saida = new List<Categoria>();
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand comando = new SqlCommand();
            SqlConnection conexao = new SqlConnection(ConetorHerdado);
            comando.CommandType = CommandType.StoredProcedure;
            comando.Connection = conexao;
            comando.CommandText = "QCategoria_List";

            adapter.SelectCommand = comando;
            adapter.Fill(dt);

            foreach (DataRow linha in dt.Rows) {
                Categoria categoria = new Categoria(linha["GuidCategoria"].ToString());
                categoria.Nome = linha["nome"].ToString();
                categoria.Descricao = linha["descricao"].ToString();
                categoria.Ativa = Convert.ToBoolean(linha["ativa"]);
                saida.Add(categoria);
            }
            return saida;
        }
    }
}


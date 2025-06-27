namespace ReceitasMaster.Models {
    public class Receita {
        public enum EstadoReceita {
            Inativa,
            Ativa,
            Todas = 99
        }

        private Guid _guidReceita;

        public string GuidReceita {
            get { return _guidReceita.ToString(); }
        }

        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Instrucoes { get; set; }
        public bool Ativa { get; set; }
        public DateTime DataCriacao { get; set; }
        public string GuidConta { get; set; }

        public Receita() {
            _guidReceita = Guid.Empty;
            Titulo = "";
            Descricao = "";
            Instrucoes = "";
            Ativa = true;
            DataCriacao = DateTime.Now;
            GuidConta = "";
        }

        public Receita(string guidReceita) {
            Guid.TryParse(guidReceita, out _guidReceita);
            Titulo = "";
            Descricao = "";
            Instrucoes = "";
            Ativa = true;
            DataCriacao = DateTime.Now;
            GuidConta = "";
        }
    }
}

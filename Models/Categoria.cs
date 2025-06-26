namespace ReceitasMaster.Models {
    public class Categoria {
        private Guid _guidCategoria;

        public string GuidCategoria {
            get { return _guidCategoria.ToString(); }
        }

        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool Ativa { get; set; }

        public Categoria() {
            _guidCategoria = Guid.NewGuid();
            Nome = "";
            Descricao = "";
            Ativa = true;
        }

        public Categoria(string guidCategoria) {
            Guid.TryParse(guidCategoria, out _guidCategoria);
            Nome = "";
            Descricao = "";
            Ativa = true;
        }
    }
}


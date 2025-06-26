namespace ReceitasMaster.Models {
    public class Ingrediente {
        private Guid _guidIngrediente;

        public string GuidIngrediente {
            get { return _guidIngrediente.ToString(); }
        }

        public string Nome { get; set; }
        public string UnidadeMedida { get; set; }

        public Ingrediente() {
            _guidIngrediente = Guid.NewGuid();
            Nome = "";
            UnidadeMedida = "";
        }

        public Ingrediente(string guidIngrediente) {
            Guid.TryParse(guidIngrediente, out _guidIngrediente);
            Nome = "";
            UnidadeMedida = "";
        }
    }
}

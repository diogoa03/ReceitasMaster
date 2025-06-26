namespace ReceitasMaster.Models {
    public class ReceitaIngrediente {
        public string ReceitaId { get; set; }
        public string IngredienteId { get; set; }
        public decimal Quantidade { get; set; }
        public string UnidadeMedida { get; set; }
    }
}


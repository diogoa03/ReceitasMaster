namespace ReceitasMaster.Models {
    public class Receita {
        public enum Dificuldade {
            Facil,
            Media,
            Dificil
        }

        private Guid _guidReceita;

        public string GuidReceita {
            get { return _guidReceita.ToString(); }
        }

        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public int TempoPreparo { get; set; } // em minutos
        public int Porcoes { get; set; }
        public string Instrucoes { get; set; }
        public bool Ativa { get; set; }
        public DateTime DataCriacao { get; set; }
        public Dificuldade NivelDificuldade { get; set; }

        // Relacionamentos
        public string GuidCategoria { get; set; }
        public string GuidConta { get; set; } // Autor da receita

        public Receita() {
            _guidReceita = Guid.Empty;
            Titulo = "";
            Descricao = "";
            TempoPreparo = 0;
            Porcoes = 1;
            Instrucoes = "";
            Ativa = true;
            DataCriacao = DateTime.Now;
            NivelDificuldade = Dificuldade.Media;
        }

        public Receita(string guidReceita) {
            Guid.TryParse(guidReceita, out _guidReceita);
            Titulo = "";
            Descricao = "";
            TempoPreparo = 0;
            Porcoes = 1;
            Instrucoes = "";
            Ativa = true;
            DataCriacao = DateTime.Now;
            NivelDificuldade = Dificuldade.Media;
        }
    }
}

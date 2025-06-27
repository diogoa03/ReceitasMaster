namespace ReceitasMaster.Models {
    public class Conta {
        public Guid GuidConta { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public int NivelAcesso { get; set; } // 0=Visitante, 1=Autor, 2=Editor
        public string Senha { get; set; }
        public bool Ativa { get; set; }
        public DateTime DthrRegisto { get; set; }

        public Conta() {
            GuidConta = Guid.NewGuid();
            Nome = "";
            Email = "";
            NivelAcesso = 0;
            Senha = "";
            Ativa = true;
            DthrRegisto = DateTime.Now;
        }
    }   
}

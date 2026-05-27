namespace CodeBolosJacquin.API.Utils
{
    public class SenhaUtils
    {
        public static string HashSenha(string senha)
        {
            if (string.IsNullOrEmpty(senha))
                throw new ArgumentException("Senha não pode ser vazia");

            return BCrypt.Net.BCrypt.HashPassword(senha, workFactor: 10);
        }



        public static bool VerificarSenha(string senhaInformada, string hashArmazenado)
        {
            return BCrypt.Net.BCrypt.Verify(senhaInformada, hashArmazenado);
        }


        public static bool EstaHashada(string senha)
        {
            if (string.IsNullOrEmpty(senha))
                return false;

            return senha.Length == 60 && senha.StartsWith("$2");

        }
    }
}

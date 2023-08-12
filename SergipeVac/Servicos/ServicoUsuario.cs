using System.Security.Cryptography;

namespace SergipeVac.Servicos
{
    public class ServicoUsuario
    {
        public string CriptografarSenha(string senha)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(senha, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashComSalt = new byte[36];
            Array.Copy(salt, 0, hashComSalt, 0, 16);
            Array.Copy(hash, 0, hashComSalt, 16, 20);
            string senhaCriptografada = Convert.ToBase64String(hashComSalt);

            return senhaCriptografada;
        }
    }
}

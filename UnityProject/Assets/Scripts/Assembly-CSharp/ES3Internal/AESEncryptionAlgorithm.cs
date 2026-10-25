using System.IO;

namespace ES3Internal
{
	public class AESEncryptionAlgorithm : EncryptionAlgorithm
	{
		private const int ivSize = 16;

		private const int keySize = 16;

		private const int pwIterations = 100;

		public override byte[] Encrypt(byte[] bytes, string password, int bufferSize)
		{
			// Recovered from GameAssembly.dll RVA 0x591300
			using (var input = new MemoryStream(bytes))
			using (var output = new MemoryStream())
			{
			    Encrypt(input, output, password, bufferSize);
			    return output.ToArray();
			}
		}

		public override byte[] Decrypt(byte[] bytes, string password, int bufferSize)
		{
			// Recovered from GameAssembly.dll RVA 0x5914E0
			using (var input = new MemoryStream(bytes))
			using (var output = new MemoryStream())
			{
			    Decrypt(input, output, password, bufferSize);
			    return output.ToArray();
			}
		}

		public override void Encrypt(Stream input, Stream output, string password, int bufferSize)
		{
			// Recovered from GameAssembly.dll RVA 0x5916C0
			input.Position = 0;
			using (var aes = System.Security.Cryptography.Aes.Create())
			{
			    aes.Mode = System.Security.Cryptography.CipherMode.CBC;
			    aes.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
			    aes.GenerateIV();
			    aes.Key = new System.Security.Cryptography.Rfc2898DeriveBytes(password, aes.IV, 100).GetBytes(16);
			    output.Write(aes.IV, 0, 16);
			    using (var encryptor = aes.CreateEncryptor())
			    using (var crypto = new System.Security.Cryptography.CryptoStream(output, encryptor, System.Security.Cryptography.CryptoStreamMode.Write))
			        CopyStream(input, crypto, bufferSize);
			}
		}

		public override void Decrypt(Stream input, Stream output, string password, int bufferSize)
		{
			// Recovered from GameAssembly.dll RVA 0x591B60
			using (var aes = System.Security.Cryptography.Aes.Create())
			{
			    byte[] iv = new byte[16];
			    // Exact original: one Read, no retry/read-exact loop or reset of input position.
			    input.Read(iv, 0, 16);
			    aes.IV = iv;
			    aes.Key = new System.Security.Cryptography.Rfc2898DeriveBytes(password, aes.IV, 100).GetBytes(16);
			    using (var decryptor = aes.CreateDecryptor())
			    using (var crypto = new System.Security.Cryptography.CryptoStream(input, decryptor, System.Security.Cryptography.CryptoStreamMode.Read))
			        CopyStream(crypto, output, bufferSize);
			}
			output.Position = 0;
		}
	}
}

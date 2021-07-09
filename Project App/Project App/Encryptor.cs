using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encryption
{
	class Encryptor
	{
		private char Cipher(char ch, int key)
		{
			if (!char.IsLetter(ch))
			{
				return ch;
			}

			char d = char.IsUpper(ch) ? 'A' : 'a';
			return (char)((((ch + key) - d) % 26) + d);


		}
		public string Encrypt(int offset, string temp = "")
		{ 
			foreach (char a in rawInput)
            {
				temp += Cipher(a, offset).ToString();
            }
			output = temp;
			return output;
		}
		public string Decrypt(int offset,string temp = "")
		{ 
			return Encrypt(26 - offset, temp);
		}
		public string RawInput { get { return rawInput; } set { rawInput = value; } }
		public string Output { get { return output; } set { output = value; } }
		private string rawInput;
		private string output;
	}
}

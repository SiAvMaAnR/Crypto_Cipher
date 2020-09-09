using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography_1
{
	class FirstCipher
	{
		private static string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя0123456789.,?!*/+-=_()%;:#";//Набор сиволов

		private int NumberKey;//Числовой ключ сдвига
		private string LineKey;//Ключевое слово
		private string Text;//Ввод

		private string encrypt;
		private string decrypt;

		public FirstCipher(string LineKey, int NumberKey = 0, string Text = "")//Конструктор
		{
			this.NumberKey = NumberKey;
			this.LineKey = LineKey;
			this.Text = Text;
		}

		private string CipherDerivation()
		{
			char[] ReserveAlphabet = Alphabet.ToCharArray();//Создание резервного массива символов алфавита
			LineKey = new string(LineKey.Distinct().ToArray()).Replace(" ", "");//Удаление повторяющихся символов

			NumberKey %= ReserveAlphabet.Length;
			for (int i = 0, IndexOfKey = 0; i < ReserveAlphabet.Length; i++)
			{
				if (IndexOfKey != LineKey.Length)//Запись Слова в Алфавит, последний символ Слова завершает слияние
				{
					if (NumberKey == ReserveAlphabet.Length)
					{
						NumberKey = 0;
					}
					ReserveAlphabet[NumberKey] = LineKey[IndexOfKey];
					NumberKey++; IndexOfKey++;
				}
			}
			string ResidualAlphabet = Alphabet;
			for (int i = 0; i < LineKey.Length; i++)//Алфавит без Ключевого слова
			{
				ResidualAlphabet = ResidualAlphabet.Replace(LineKey[i].ToString(), "");
			}

			for (int i = 0, k = 0; i < ReserveAlphabet.Length && k < ResidualAlphabet.Length; i++, k++)
			{
				if (NumberKey == ReserveAlphabet.Length)
				{
					NumberKey = 0;
				}
				ReserveAlphabet[NumberKey++] = ResidualAlphabet[k];
			}
			return new string(ReserveAlphabet);
		}

		public string Encode()//Шифрование
		{
			string Cipher = CipherDerivation();
			for (int i = 0; i < Text.Length; i++)
			{
				int INDEX = Alphabet.IndexOf(Text[i]);
				encrypt += (INDEX != -1) ? Cipher[INDEX] : Text[i];
			}
			return encrypt;
		}

		public string Decode()//Дешифрование
		{
			string Cipher = CipherDerivation();
			for(int i=0;i<Text.Length;i++)
			{
				int INDEX = Cipher.IndexOf(Text[i]);
				decrypt += (INDEX != -1) ? Alphabet[INDEX] : Text[i];
			}
			return decrypt;
		}

	}
}
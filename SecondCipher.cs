using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography_1
{
	class SecondCipher
	{
		private static string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя0123456789.,?!*/+-=_()%;:#";//Набор сиволов
		private string NewAlphabet;

		private int[] ArrNumber = new int[Alphabet.Length];//Массив чисел, соответствующих алфавиту
		private int [] NewArrNumber = new int[Alphabet.Length];//Зашифрованный массив чисел

		private string encrypt = "";
		private string decrypt = "";

		private string Text;//Входной текст

		private int N = Alphabet.Length;//Количество элементов в алфавите
		private int A;//Первый ключ
		private int B;//Второй ключ

		public SecondCipher(int A,int B, string Text, out int Lenght)
		{
			Lenght = N;//Передача длины Алфавита
			this.A = A;
			this.B = B;
			this.Text=Text;
		}

		private void AlphabetNumber()//Заполнить численный алфавит ArrNumber значениями от 0 до конца алфавита
		{
			for (int i = 0; i < N; i++)
				ArrNumber[i] = i;
		}


		private void NewAlphabetNumber()//Зашифровать численный алфавит NewArrNumber
		{
			for (int i = 0; i < N; i++)
			{
				NewArrNumber[i] = (A * ArrNumber[i] + B) % N;
			}
		}

		private string CipherDerivation()
		{
			AlphabetNumber();//Заполнить численный алфавит ArrNumber значениями от 0 до конца алфавита
			NewAlphabetNumber();//Зашифровать численный алфавит NewArrNumber
			for (int i = 0; i < N; i++)
			{
				NewAlphabet += Alphabet[NewArrNumber[i]];
			}
			return NewAlphabet;
		}


		public string Encode()//Зашифрованный Алфавит
		{
			string Cipher = CipherDerivation();
			for (int i = 0; i < Text.Length; i++)
			{
				int INDEX = Alphabet.IndexOf(Text[i]);
				encrypt += (INDEX != -1) ? Cipher[INDEX] : Text[i];
			}
			return encrypt;
		}

		public string Decode()//Расшированный текст
		{
			string Cipher = CipherDerivation();
			for (int i = 0; i < Text.Length; i++)
			{
				int INDEX = Cipher.IndexOf(Text[i]);
				decrypt += (INDEX != -1) ? Alphabet[INDEX] : Text[i];
			}
			return decrypt;
		}

	}
}

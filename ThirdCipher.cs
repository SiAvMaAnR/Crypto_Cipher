using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography_1
{
	class ThirdCipher
	{
		//private static string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя0123456789.,?!*/+-=_()%;:#";//Набор сиволов
		//Перемешанный алфавит
		private static string Alphabet = "ъ=#VКsэGу6ЮлнzSОqhWЫxCФN.HдM8ИdcFjtQ+ITхЦв;awrтDУп:Бж5ЁХЛLчvёгE2Аu70аГ/3Aошeе)бПТЩЭЕZ9mXРЯpygкыi?%юй4Вl,сДьЪзН(ф-PBШOСb_Ж!рoщUЬR*JямnKЧЗ1YМцfkЙи";//Набор сиволов

		private static int columns = 12;//Столбцы
		private static int rows = 12;//Ряды

		private char[,] FirstSquare = new char[rows, columns];//Первый квадрат Уитстона
		private char[,] SecondSquare = new char[rows, columns];//Второй квадрат Уитстона 

		private string firstKey = "";//Первый ключ
		private string secondKey = "";//Второй ключ

		private string encrypt = "";//Зашифрованные данные
		private string decrypt = "";//Дешифрованные данные

		private string text = "";

		public ThirdCipher(string firstKey,string secondKey,string text)
		{
			//Удаление повторяющихся символов из первого ключа
			this.firstKey = new string(firstKey.Distinct().ToArray()).Replace(" ", "");
			//Удаление повторяющихся символов из второго ключа
			this.secondKey = new string(secondKey.Distinct().ToArray()).Replace(" ", "");
			this.text = text;
		}

		private string GetKeyLess(string _Key)//Вернуть алфавит без указанного ключа
		{
			string ResidualAlphabet = Alphabet;
			for (int i = 0; i < _Key.Length; i++)//Алфавит без Ключевого слова
			{
				ResidualAlphabet = ResidualAlphabet.Replace(_Key[i].ToString(), "");
			}
			return ResidualAlphabet;
		}

		//Первый квадрат
		private void SetFirstSquare()
		{
			string KeyLess = GetKeyLess(firstKey);
			int index = 0, k = 0;
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					FirstSquare[i, j] = (index < firstKey.Length) ? firstKey[index++] : KeyLess[k++];
				}
			}
		}
		//Второй квадрат
		private void SetSecondSquare()
		{
			string KeyLess = GetKeyLess(secondKey);
			int index = 0, k = 0;
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					SecondSquare[i, j] = (index < secondKey.Length) ? secondKey[index++] : KeyLess[k++];
				}
			}
		}

		//Создание двух квадратов и замена пробелов на подчеркивания
		private void CipherDerivation()
		{
			SetFirstSquare();
			SetSecondSquare();
			text = text.Replace(" ", "_");
			if (text.Length % 2 == 1)
				text += "_"; 
		}

		//Зашифровка
		public string Encode()
		{
			CipherDerivation();
			string Pair ="";
			for (int i = 0; i < text.Length; i++)
			{
				Pair += text[i];
				if (i % 2 == 1)
				{
					encrypt += EncodeElementPairCipher(Pair);
					Pair = "";
				}
			}
			return encrypt;
		}

		//Дешифровка путем замены расположения, меняем элементы пары местами
		public string Decode()
		{
			SetFirstSquare();
			SetSecondSquare(); 
			string Pair = "";
			for (int i = 0; i < text.Length; i++)
			{
				Pair += text[i];
				if (i % 2 == 1)
				{
					decrypt += DecodeElementPairCipher(Pair);
					Pair = "";
				}
			}
			return decrypt.Replace("_"," ").Trim();
		}



		//Поиск позиции элемента в таблице
		private void SearchIndexToArray(char [,] Square, char SearchChar, out int TableRows, out int TableColumns)
		{
			TableRows = -1;
			TableColumns = -1;
			int Rows = Square.GetUpperBound(0)+1;//Ряды
			int Columns = Square.GetUpperBound(1)+1;//Колонки

			for (int i = 0; i < Rows; i++)
			{
				for (int j = 0; j < Columns; j++)
				{
					if (Square[i, j]==SearchChar)
					{
						TableRows = i;
						TableColumns = j;
					}
				}
			}
		}


		//Замена пары символов на зашифрованную пару
		private string EncodeElementPairCipher(string StrPair)
		{
			string Pair = "";

			SearchIndexToArray(FirstSquare, StrPair[0], out int Rows_0, out int Columns_0);//Позиция 1 символа в 1 квадрате
			SearchIndexToArray(SecondSquare, StrPair[1], out int Rows_1, out int Columns_1);//Позиция 2 символа в 2 квадрате

			if (Rows_0 == Rows_1)//Если в 1 ряду 
			{
				Pair += (Rows_0 == -1 || Rows_1 == -1) ? StrPair : SecondSquare[Rows_0, Columns_0].ToString() + FirstSquare[Rows_1, Columns_1].ToString();
			}
			else//Если в разных рядах 
			{
				Pair += (Rows_0 == -1 || Rows_1 == -1) ? StrPair : SecondSquare[Rows_0, Columns_1].ToString() + FirstSquare[Rows_1, Columns_0].ToString();
			}
			return Pair;
		}


		//Замена пары символов на дешифрованную пару
		private string DecodeElementPairCipher(string StrPair)
		{
			string Pair = "";

			SearchIndexToArray(SecondSquare, StrPair[0], out int Rows_0, out int Columns_0);//Позиция 1 символа в 1 квадрате
			SearchIndexToArray(FirstSquare, StrPair[1], out int Rows_1, out int Columns_1);//Позиция 2 символа в 2 квадрате

			if (Rows_0 == Rows_1)//Если в 1 ряду 
			{
				Pair += (Rows_0 == -1 || Rows_1 == -1) ? StrPair : FirstSquare[Rows_0, Columns_0].ToString() + SecondSquare[Rows_1, Columns_1].ToString();
			}
			else//Если в разных рядах 
			{
				Pair += (Rows_0 == -1 || Rows_1 == -1) ? StrPair : FirstSquare[Rows_0, Columns_1].ToString() + SecondSquare[Rows_1, Columns_0].ToString();
			}
			return Pair;
		}
	}
}

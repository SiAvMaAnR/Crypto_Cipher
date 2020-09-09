using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cryptography_1
{
	public partial class Form1 : Form
	{
		private byte RadioButtonState;
		
		public Form1()
		{
			InitializeComponent();
		}

		private async void Form1_Load(object sender, EventArgs e)
		{
			for (Opacity = 0; Opacity < 1; Opacity += 0.02)
			{
				await Task.Delay(10);
			}
		}


		//Системные клавиши
		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Escape: Close();break;
			}
		}


		//Копировать текст в буфер обмена
		private void label1_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(label1.Text))
				if (MessageBox.Show("Скопировать данные в буфер обмена?", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
					Clipboard.SetText(label1.Text, TextDataFormat.UnicodeText);
		}


		//Сменить местамитекст ввода и вывода
		private void pictureBox1_Click(object sender, EventArgs e)
		{
			(textBox1.Text,label1.Text) = (label1.Text,textBox1.Text);
		}


		//Система цезаря с ключевым словом
		private void radioButton1_CheckedChanged(object sender, EventArgs e)
		{
			groupBox2.Text = "Введите ключ сдвига и ключевую фразу";
			RadioButton radioButton = (RadioButton)sender;
			if (radioButton.Checked)
			{
				RadioButtonState = 1;
			}
		}


		//Аффинная система подстановки Цезаря
		private void radioButton2_CheckedChanged(object sender, EventArgs e)
		{
			groupBox2.Text = "Введите два численных ключа";
			RadioButton radioButton = (RadioButton)sender;
			if (radioButton.Checked)
			{
				RadioButtonState = 2;
			}
		}


		//Двойной квадрат Уитстона
		private void radioButton3_CheckedChanged(object sender, EventArgs e)
		{
			groupBox2.Text = "Введите два ключа";
			RadioButton radioButton = (RadioButton)sender;
			if (radioButton.Checked)
			{
				RadioButtonState = 3;
			}
		}


		//Шифр двойной перестановки
		private void radioButton4_CheckedChanged(object sender, EventArgs e)
		{
			groupBox2.Text = "Введите два ключа";
			RadioButton radioButton = (RadioButton)sender;
			if (radioButton.Checked)
			{
				RadioButtonState = 4;
			}
		}


		//Шифрование 1 методом - Система Цезаря с ключевым словом
		private void FirstEncrypt()
		{

			try
			{
				int.TryParse(textBox2.Text, out int KeyNumber);
				if(textBox3.Text.Length>=60)
					throw new Exception("Длина ключевого слова слишком велика!");
				if (!(KeyNumber >= 0 && KeyNumber < 999999999))
					throw new Exception("Не корректный ключевой сдвиг!");
				FirstCipher firstCipher = new FirstCipher(textBox3.Text, KeyNumber, textBox1.Text);
				label1.Text = firstCipher.Encode();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}


		//Дешифрование 1 методом - Система Цезаря с ключевым словом
		private void FirstDecrypt()
		{
			try
			{
				int.TryParse(textBox2.Text, out int KeyNumber);
				if (textBox3.Text.Length >= 30)
					throw new Exception("Длина ключевого слова выходит за пределы!");
				if (!(KeyNumber >= 0 && KeyNumber < 999999999))
					throw new Exception("Не корректный ключевой сдвиг!");
				FirstCipher firstcipher = new FirstCipher(textBox3.Text, KeyNumber, textBox1.Text);
				label1.Text = firstcipher.Decode();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}


		//Шифрование 2 методом - Афинная система подстановок Цезаря
		private void SecondEncrypt()
		{
			try
			{
				int A = int.Parse(textBox2.Text);
				int B = int.Parse(textBox3.Text);
				SecondCipher secondCipher = new SecondCipher(A, B, textBox1.Text, out int LengthAlphabet);
				if (NOD(A, LengthAlphabet) != 1)
					throw new Exception($"Первый ключ должен быть взаимно простым с числом {LengthAlphabet}!");
				if (A < 0 || B < 0)
					throw new Exception($"Ключи должны быть положительными!");
				label1.Text = secondCipher.Encode();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}


		//Дешифрование 2 методом - Афинная система подстановок Цезаря
		private void SecondDecrypt()
		{
			try
			{
				int K = int.Parse(textBox2.Text);
				int A = int.Parse(textBox3.Text);
				SecondCipher secondCipher = new SecondCipher(K, A, textBox1.Text, out int LengthAlphabet);
				if (NOD(K, LengthAlphabet) != 1)
					throw new Exception($"Первый ключ должен быть взаимно простым с числом {LengthAlphabet}!");
				label1.Text = secondCipher.Decode();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}


		//Шифрование 3 методом - Двойной квадрат Уитстона
		private void ThirdEncrypt()
		{
			try
			{
				if (textBox1.Text.Contains('_'))
					throw new Exception("Для повышения безопасности запрещена возможность ввода \"_\"");
				ThirdCipher thirdCipher = new ThirdCipher(textBox2.Text, textBox3.Text, textBox1.Text);
				label1.Text = thirdCipher.Encode();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}


		//Дешифрование 3 методом - Двойной квадрат Уитстона
		private void ThirdDecrypt()
		{
			try
			{ 
				ThirdCipher thirdCipher = new ThirdCipher(textBox2.Text, textBox3.Text, textBox1.Text);
				label1.Text = thirdCipher.Decode();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}


		//Шифрование 4 методом - Метод двойной перестановки
		private void FourthEncrypt()
		{
			try
			{
				if(string.IsNullOrEmpty(textBox2.Text))
					throw new Exception("Поле первого ключа обязательно к заполнению");
				FourthCipher fourthCipher = new FourthCipher(textBox1.Text, textBox2.Text, textBox3.Text, out int Height, out string Alphabet);
				if (textBox3.Text.Length != Height)
					throw new Exception($"Длина второго ключа должна равняться {Height}");
				if (IsCorrectText(textBox2.Text, Alphabet)|| IsCorrectText(textBox3.Text, Alphabet))
					throw new Exception($"Ключи должны быть в виде числа или слова (без комбинаций и пробелов!)");
				if (textBox3.Text.Length > 10)
					throw new Exception($"Предельная длина 2 ключа, первый ключ должен быть длиннее");
				if (IsUniqueString(textBox2.Text) || IsUniqueString(textBox3.Text))
					throw new Exception($"Ключи должны состоять из уникальных символов!");

				label1.Text = fourthCipher.Encode();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}


		//Дешифрование 4 методом - Метод двойной перестановки
		private void FourthDecrypt()
		{
			try
			{
				if (string.IsNullOrEmpty(textBox2.Text))
					throw new Exception("Поле ключей обязательно к заполнению");
				FourthCipher fourthCipher = new FourthCipher(textBox1.Text, textBox2.Text, textBox3.Text, out int Height, out string Alphabet);
				if (textBox3.Text.Length != Height)
					throw new Exception($"Длина второго ключа должна равняться {Height}");
				if (IsCorrectText(textBox2.Text, Alphabet) || IsCorrectText(textBox3.Text, Alphabet))
					throw new Exception($"Ключи должны быть в виде числа или слова (без комбинаций и пробелов!)");
				if (textBox3.Text.Length > 10)
					throw new Exception($"Предельная длина 2 ключа, первый ключ должен быть длиннее");
				if (IsUniqueString(textBox2.Text) || IsUniqueString(textBox3.Text))
					throw new Exception($"Ключи должны состоять из уникальных символов!");

				label1.Text = fourthCipher.Decode();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}


		//Проверка на коректность ввода ключей 4 шифра
		private bool IsCorrectText(string str, string Alphabet)
		{
			if (int.TryParse(str.Trim(), out int number))
			{
				return false;
			}
			else
			{
				int ch = 0;
				for (int i = 0; i < str.Length; i++)
				{
					for (int j = 0; j < Alphabet.Length; j++)
					{
						if (str[i].ToString() == Alphabet[j].ToString())
						{
							ch++;
						}
					}
				}
				return (str.Length == ch) ? false : true;
			}
		}


		//Проверка строки на уникальность
		private bool IsUniqueString(string str)
		{
			for (int i = 0; i < str.Length; i++)
			{
				if (str.IndexOf(str[i]) != str.LastIndexOf(str[i]))
				{
					return true;
				}
			}
			return false;
		}

		//Зашифровать
		private void encrypt_Click(object sender, EventArgs e)
		{
			switch (RadioButtonState)
			{
				case 1: FirstEncrypt(); break;
				case 2: SecondEncrypt(); break;
				case 3: ThirdEncrypt(); break;
				case 4: FourthEncrypt(); break;
				default: MessageBox.Show("Выберите тип шифра!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);break;
			}
		}


		//Дешифровать
		private void decrypt_Click(object sender, EventArgs e)
		{
			switch (RadioButtonState)
			{
				case 1: FirstDecrypt(); break;
				case 2: SecondDecrypt(); break;
				case 3: ThirdDecrypt(); break;
				case 4: FourthDecrypt(); break;
				default: MessageBox.Show("Выберите тип шифра!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); break;
			}
		}


		//Наибольший Общий Делитель
		private int NOD(int A, int B)
		{
			if (A == B)
				return A;
			if (A > B)
				(A, B) = (B, A);
			return NOD(A, B - A);
		}
	}
}

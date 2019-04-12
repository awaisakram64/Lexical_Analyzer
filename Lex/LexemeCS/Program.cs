using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace LexemeCS
{
    class LexicalAnalysis
    {        
        string VariableType = "";
        int CurrentIndex = 0;
        List<string> VariableName = new List<string>();
        List<string> ErrorName = new List<string>();
        string VariableValue = "";
        bool Error = false;
        List<int> ErrorIndex = new List<int>();
        public List<string> Parse()
        {
            return ErrorName;
        }
        public string GenerateToken(string[] Lexeme)
        {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < Lexeme.Length; i++)
            {
                CurrentIndex = i;
                if (IsReserveWord(Lexeme[i]))
                {
                    VariableType = Lexeme[i];                    
                    str.Append("(" + Lexeme[i] + " ==> Reserve Word , index = " + i.ToString() + "+)\n");
                }
                else if (IsIdentifier(Lexeme[i]))
                {
                    VariableName.Add(Lexeme[i]);
                    str.Append("(" + Lexeme[i] + " ==> ID , index = " + i.ToString() + "+)\n");
                }
                else if (IsOperator(Lexeme[i]))
                {
                    CurrentIndex = i;
                    str.Append("(" + Lexeme[i] + " ==> Operator , index = " + i.ToString() + "+)\n");
                }
                else if (IsLiteral(Lexeme[i]))
                {
                    CurrentIndex = i;
                    str.Append("(" + Lexeme[i] + " ==> Litteral , index = " + i.ToString() + "+)\n");
                }
                else if (IsSepetator(Lexeme[i]))
                {
                    CurrentIndex = i;
                    str.Append("(" + Lexeme[i] + " ==> Seperator , index = " + i.ToString() + "+)\n");
                }
            }
            Program p = new Program(ErrorName);
            //p.error(ErrorIndex);
            return str.ToString();
        }
        public bool IsDigit(char symbol)
        {
            if (char.IsDigit(symbol))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsIdentifier(string Lexeme)// Variable name
        {
            char[] chararr = Lexeme.ToCharArray();

            if (IsDigit(chararr[0]))
                return false;
            else if (IsReserveWord(Lexeme))
                return false;
            else if (IsOperator(Lexeme))
                return false;
            else if (IsPunctuation(Lexeme))
                return false;
            else if (IsSepetator(Lexeme))
                return false;
            else
                return true;
        }

        public bool IsLetter(char symbol)
        {
            if (char.IsLetter(symbol))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsLiteral(string Lexeme) //assign value of Like(int i = 0 ; >>0 is literal)
        {
            switch (VariableType)
            {
                case "int":
                     if (IsNumber(Lexeme))
                    {    return true; }
                    else
                    { ErrorIndex.Add(CurrentIndex);
                    ErrorName.Add("Variable is not int at "+CurrentIndex);
                    return false; }
                case "string":
                    if (IsString(Lexeme))
                        return true;
                    else
                    {   ErrorIndex.Add(CurrentIndex);
                        ErrorName.Add("Variable is not String at " + CurrentIndex);
                        return false;
                    }
                case "char":
                    if (IsChar(Lexeme))
                        return true;
                    else
                    {
                        ErrorIndex.Add(CurrentIndex);
                        ErrorName.Add("Variable is not char at " + CurrentIndex);
                        return false;
                    }
                case "bool":
                    if (IsBool(Lexeme))
                        return true;
                    else
                    {
                        ErrorName.Add("Variable is not bool at " + CurrentIndex);
                        ErrorIndex.Add(CurrentIndex);
                        return false;
                    }
            }
            return false;
            //throw new NotImplementedException();
        }
        public bool IsChar(string Lexeme)
        {
            bool flag = true;
            if (Lexeme.StartsWith("'"))
                if (Lexeme.EndsWith("'"))
                    if (Lexeme.Length == 3)
                    {
                        flag = true;
                        return true;
                    }
                    else
                    {
                        flag = true;
                        return false;
                    }
            return flag;
        }
        public bool IsBool(string Lexeme)
        {
            bool flag = true;
            if (Lexeme == "true" || Lexeme == "false")
                flag = true;
            else
                flag = false;
            return flag;
        }
        public bool IsNumber(string Lexeme)
        {
            char[] arr = Lexeme.ToCharArray();
            bool flag = false;
            foreach (var item in arr)
            {
                if (char.IsDigit(item))
                    flag = true;
                else
                    flag = false;
                break;
            }
            if (flag == false)
                return false;
            else
                return true;
        }
        public bool IsString(string Lexeme)
        {
            if (Lexeme.StartsWith("\""))
                if (Lexeme.EndsWith("\""))
                    return true;
            return false;
        }
        public bool IsOperator(string Lexeme)
        {
            string operators = File.ReadAllText(Application.StartupPath + "\\operator.txt");
            string[] operat = operators.Split(',');
            if (Array.IndexOf(operat, Lexeme) > -1)
                return true;
            return false;
        }
        public bool IsSepetator(string Lexeme)
        {
            string operators = File.ReadAllText(Application.StartupPath + "\\separator.txt");
            string[] operat = operators.Split(',');
            if (Array.IndexOf(operat, Lexeme) > -1)
                return true;
            return false;
        }

        public bool IsPunctuation(string Lexeme)  // delimiter and puntuation are same
        {
            string Punctuation = File.ReadAllText(Application.StartupPath + "\\Punctuation.txt");
            string[] Punctua = Punctuation.Split(',');
            if (Array.IndexOf(Punctua, Lexeme) > -1)
                return true;
            return false;
        }
        /// <summary>
        /// Check for Keywords 
        /// </summary>
        /// <param name="Lexeme"></param>
        /// <returns></returns>
        public bool IsReserveWord(string Lexeme)
        {
            string Punctuation = File.ReadAllText(Application.StartupPath + "\\Reserve.txt");
            string[] Punctua = Punctuation.Split(',');
            if (Array.IndexOf(Punctua, Lexeme) > -1)
                return true;
            return false;
        }
    }
    interface Isignature
    {
        bool IsIdentifier(string Lexeme);
        bool IsLiteral(string Lexeme);
        bool IsNumber(string Lexeme);
        bool IsPunctuation(string Lexeme);
        bool IsOperator(string Lexeme);
        bool IsReserveWord(string Lexeme);
        bool IsLetter(char symbol);
        bool IsDigit(char symbol);
        bool IsSeparator(string Lexeme);
        String GenerateToken(string Lexeme);
    }

    class Program
    {
        public List<string> err = new List<string>();
        public Program(List<string> Error)
        {
            foreach (var item in Error)
            {
                err.Add(item);
            }
        }
        static void Main(string[] aaa)
        {
            string[] args = {"int","i","=","0",";" };
            //Console.WriteLine("(" + "int" + "=> Reserve Word " + " )"+"\n");
            //Console.ReadKey();
            LexicalAnalysis lex = new LexicalAnalysis();
            string text = "";
            if (args != null)
            {
                foreach (var item in args)
                {
                    text += item + " ";
                }
            }
            string Token = lex.GenerateToken(args);

            Console.WriteLine(Token);
            Console.WriteLine(text);
            Console.WriteLine("\nError Occur\n");
            foreach (var item in lex.Parse())
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }
    }
}

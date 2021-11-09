using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MyApp
{
    public class Base64Math
    {
        public class B64ReturnData  //Struct to encapsulate the data that is needed to be returned to and from these internal functions
        {
            private int returnCode;
            private byte[] returnArray;
            public int ReturnCode
            {
                get { return returnCode; }
                protected internal set {  returnCode = value; }  //Only want this methods in this class and derivations to be able to set values
            }
            public byte [] ReturnArray 
            { 
                get { return returnArray; }
                internal internal set { returnArray = value; }  
            }
        }

        [DllImport("Base64Math.dll")]
        private static extern int add64(byte[] input1, byte[] input2, byte[] returnBuffer, int lengthOfReturnBuffer);
        [DllImport("Base64Math.dll")]
        private static extern int sub64(byte[] input1, byte[] input2, byte[] returnBuffer, int lengthOfReturnBuffer);

        public B64ReturnData Add64(string firstNumber, string secondNumber)
        {
            const int lengthOfReturnBuffer = 9;
            byte[] firstNumberASCII = Encoding.ASCII.GetBytes(firstNumber);
            byte[] secondNumberASCII = Encoding.ASCII.GetBytes(secondNumber);
            byte[] returnBuffer = new byte[lengthOfReturnBuffer];
            B64ReturnData b64ReturnData = new B64ReturnData();

            int returnValue = add64(firstNumberASCII, secondNumberASCII, returnBuffer, lengthOfReturnBuffer);

            b64ReturnData.ReturnCode = returnValue;
            b64ReturnData.ReturnArray = returnBuffer;
            return b64ReturnData;
        }

        public B64ReturnData Sub64(string firstNumber, string secondNumber)
        {
            const int lengthOfReturnBuffer = 9;
            byte[] firstNumberASCII = Encoding.ASCII.GetBytes(firstNumber);
            byte[] secondNumberASCII = Encoding.ASCII.GetBytes(secondNumber);
            byte[] returnBuffer = new byte[lengthOfReturnBuffer];
            B64ReturnData b64ReturnData = new B64ReturnData();

            int returnValue = sub64(firstNumberASCII, secondNumberASCII, returnBuffer, lengthOfReturnBuffer);

            b64ReturnData.returnCode = returnValue;
            b64ReturnData.returnArray = returnBuffer;
            return b64ReturnData;
        }
        public string Read64BitNumber(string prompt)
        {
            //Note this does NOT check for valid input characters! The library functions will return a non-zero value to the caller in such cases.
            char[] inBuffer = new char[8];  //Max number of chars in buffer
            int i = 7;
            int x;
            char ch;
            Console.Write(prompt);
            while (i != 0)
            {
                x = (int)Console.ReadKey().Key; //getchar, with explicit cast (shock, horror! :o)
                try
                {
                    ch = Convert.ToChar(x); //Convert because C# is like that. Structured error handling galore!!

                    switch (x)
                    {
                        case 0x000A:    // If the user presses LF, zero extend the value in the buffer
                            while (i > -1)
                            {
                                inBuffer[i] = '0';
                                i--;
                            }
                            break;

                        case 0x0008:    // If the user presses backspace AND we are not at the beginning of the buffer, increment buffer counter.
                            if (i < 7)
                            {
                                i++;
                                Console.Write(" "); //Backspace should remove whatever char is there... this isnt BIOS anymore, someone might _use_ this...
                            }
                            break;

                        case 0x0020:    // If the user presses the space key, beep aggressively at the user
                            Console.Beep();
                            break;

                        case 0x0009:
                            Console.Beep(); // IF the user presses the beep key, beep aggressively at the user
                            break;

                        default:       // Otherwise, insert the char, unless it causes errors
                            inBuffer[i] = ch;
                            i--;
                            break;
                    }
                }
                catch (OverflowException e)
                {
                    //If an error, flush the buffer and return a 0 value.
                    for (i = 7; i > -1; i--)
                    {
                        inBuffer[i] = '0';
                        Console.WriteLine("\nInput Error!");
                    }
                }
            }

            return new string(inBuffer);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Base64Math base64Dispatcher = new Base64Math(); //Be a good adherent to Object Oriented Paradigms. 

            Console.WriteLine("Please enter your first 64 bit number ");
            string input1 = base64Dispatcher.Read64BitNumber(">");

            Console.WriteLine("Please enter your second 64 bit number ");
            string input2 = base64Dispatcher.Read64BitNumber(">");

            returnArrayAdd = base64Dispatcher.Add64(input1, input2);
        }
    }
}
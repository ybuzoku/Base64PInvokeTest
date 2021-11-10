using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base64MathSpace;

namespace Base64PInvokeTest
{
    public class Program
    {
        public static void Main()
        {
            Base64Math base64Dispatcher = new Base64Math(); //Be a good adherent to Object Oriented Paradigms. 
            bool state = true;


            Console.WriteLine("Base 64 arithmetic x86-64 DLL test application\n\nWritten by Yll Buzoku\n\n" +
                "Instructions for use:" +
                "\n1) When prompted, please type in the number you wish to compute with, in our proprietary Buzoku Base 64 encoding(C)." +
                "\n2) Numbers are processed in the Little Endian format." +
                "\n3) Numbers can be at most 8 digits long." +
                "\n4) If more than 8 digits are typed in, only the FIRST 8 digits will be processed, in Little Endian format" +
                "\n5) Base 64 numbers are postfixed with the £ symbol.\n");
            while (state)
            {
                Console.WriteLine("Please enter the first number ");
                string input1 = base64Dispatcher.Read64BitNumber(">");

                Console.WriteLine("Please enter the second number ");
                string input2 = base64Dispatcher.Read64BitNumber(">");

                B64ReturnData ret1 = base64Dispatcher.Add64(input1, input2);
                B64ReturnData ret2 = base64Dispatcher.Sub64(input1, input2);
                if (ret1.ReturnCode != 0 || ret2.ReturnCode != 0)
                {
                    Console.WriteLine("Bad input argument!");
                }
                else
                {
                    Console.WriteLine("\n |{0}£                |{1}£", input1, input1);
                    Console.WriteLine(" |{0}£ +              |{1}£ -", input2, input2);
                    Console.WriteLine("------------              ------------");
                    Console.WriteLine(" {0}£                {1}£", Encoding.ASCII.GetString(ret1.ReturnArray), Encoding.ASCII.GetString(ret2.ReturnArray));
                }

                bool state2 = true; //Reinitialise the second state machine
                while (state2)
                {
                    Console.WriteLine("\nDo you want to do another calculation? (y/n)");
                    char ch = char.ToUpper(Console.ReadKey(true).KeyChar);    //Get char in upper case form, the true means it does NOT echo the char onto the console
                    switch (ch)
                    {
                        case 'Y':
                            Console.WriteLine("\nAwesome!\n");
                            state2 = false; //Exit y/n loop
                            break;
                        case 'N':
                            Console.WriteLine("\nGoodbye!\nThank you for using the Base 64 arithmetic test app!\n");
                            state2 = false; //Exit y/n loop
                            state = false;  //Exit program main loop
                            break;
                        default:
                            Console.WriteLine("\nPlease type in either y for yes or n for no\n");
                            break;
                    }
                }
            }
        }
    }
}
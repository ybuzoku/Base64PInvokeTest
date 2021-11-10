using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Base64MathSpace
{
    public class B64ReturnData  //Struct to encapsulate the data that is needed to be returned to and from these internal functions
    {
        private readonly int _returnCode;
        private readonly byte[] _returnArray;
        public B64ReturnData(int returnCode, byte[] returnArray)
        {
            _returnCode = returnCode;
            _returnArray = returnArray;
        }
        public int ReturnCode
        {
            get { return _returnCode; }
        }
        public byte[] ReturnArray
        {
            get { return _returnArray; }
        }
    }

    public class Base64Math
    {
        [DllImport("..\\..\\..\\Base64Externals\\Base64Math.dll")]
        private unsafe static extern int add64(byte* input1Ptr, byte* input2Ptr, byte* returnBufferPtr, int lengthOfReturnBuffer);
        [DllImport("..\\..\\..\\Base64Externals\\Base64Math.dll")]
        private unsafe static extern int sub64(byte* input1Ptr, byte* input2Ptr, byte* returnBufferPtr, int lengthOfReturnBuffer);

        public B64ReturnData Add64(string firstNumber, string secondNumber)
        {
            const int lengthOfReturnBuffer = 9;
            byte[] firstNumberASCII = Encoding.ASCII.GetBytes(firstNumber); 
            byte[] secondNumberASCII = Encoding.ASCII.GetBytes(secondNumber);
            byte[] returnBuffer = new byte[lengthOfReturnBuffer];
            int returnValue;        //Mark as a managed variable to pass back outside 
            /* 
             * 
             * C# appears to reserve a paragraph (16 bytes) preceeding the data buffer of byte[] type arrays for its own metadata.
             * Therefore we must mark the following as unsafe and move the pointer the buffer for each array to the actual data buffer before calling the function.
             * It appears that C# is aware that we dont want the pointer to point to its metadata paragraph and so gives us pointers to the data buffers when 
             * doing the pointer assignments. By this I mean the following:
             * Suppose &firstNumberASCII = 0x00. Thus &firstNumberASCII[0]=0x10 and so byte* ptr1 = firstNumberASCII would be ptr1=0x10 and not 0x00 as expected.
             * 
             */
            unsafe
            {
                fixed (byte* ptr1 = firstNumberASCII, ptr2 = secondNumberASCII, returnPtr = returnBuffer)   //Tells GC NOT to touch these objects in memory
                {
                    returnValue = add64(ptr1, ptr2, returnPtr, lengthOfReturnBuffer);
                }
            }
            B64ReturnData b64ReturnData = new B64ReturnData(returnValue, returnBuffer);
            return b64ReturnData;
        }

        public B64ReturnData Sub64(string firstNumber, string secondNumber)
        {
            const int lengthOfReturnBuffer = 9;
            byte[] firstNumberASCII = Encoding.ASCII.GetBytes(firstNumber);
            byte[] secondNumberASCII = Encoding.ASCII.GetBytes(secondNumber);
            byte[] returnBuffer = new byte[lengthOfReturnBuffer];
            int returnValue;        

            unsafe
            {
                fixed (byte* ptr1 = firstNumberASCII, ptr2 = secondNumberASCII, returnPtr = returnBuffer)   //Tells GC to NOT touch these objects in memory
                {
                    returnValue = sub64(ptr1, ptr2, returnPtr, lengthOfReturnBuffer);
                }
            }
            B64ReturnData b64ReturnData = new B64ReturnData(returnValue, returnBuffer);
            return b64ReturnData;
        }

        //Note this does NOT check for valid input characters! The library functions will return a non-zero value to the caller in such cases.
        public string Read64BitNumber(string prompt)
        {
            Console.Write(prompt);  //Put the prompt on the screen

            string returnString;
            string? inString = Console.ReadLine();  //Cannot for the life of me remember why the ? suppresses the warning but I learnt that ages ago!
            if (string.IsNullOrEmpty(inString))
            {
                returnString = "00000000";
            }
            else if (inString.Length < 8)                   //If the user typed less than 8 chars, probably can include the first case in here
            {
                StringBuilder stringBuilder = new StringBuilder
                { 
                    Capacity = 8                            //Set the maximum length of the string to be built to be 8
                };  

                int overHang = 8 - inString.Length;
                for(int i = 0; i < overHang; i++)
                {
                    stringBuilder.Append('0');              //Add the necessary number of 0's to the returnString
                }
                stringBuilder.Append(inString);
                returnString = stringBuilder.ToString();
            }
            else if (inString.Length > 8)
            {
                returnString = inString[..8];               //Get the first 8 characters from the typed in string
            }
            else
            {
                returnString = inString;                    //Manual speed opt at the cost of code size
            }
            return returnString;
        }
    }
}

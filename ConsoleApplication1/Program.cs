using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Refernces...
            // Basic Layout Info From http://www.thonky.com/qr-code-tutorial/ Site Down Since June 1st...
            // Messge Converstion Info From http://raidenii.net/files/datasheets/misc/qr_code.pdf

            //Setup User Input Array
            char[] Alpha = {'0','1','2','3','4','5','6','7','8','9','0','A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z','?','$','%','*','+','-','.','/',':'};

            Console.WriteLine("QRCodeGennerator >> Write Message to Encode to Binary for QRCode :");
            string InputString = Console.ReadLine();

            char[] MessageCharPair = new char[InputString.Length];
            int[] MessageIntPair = new int[InputString.Length];
            string[] MessageBinaryPair = new string[((InputString.Length + 1) / 2)];

            //Make Message CAPS
            InputString = InputString.ToUpper();

            // Convert Message to Char Value
            for (int x = 0; x < InputString.Length; x++) MessageCharPair[x] = Convert.ToChar(InputString.Substring(x, 1));

            //Convert Characters From Message to Integer Values Based on Alpha Mode
            for (int x = 0; x < MessageCharPair.Length; x++)
            {
                for (int y = 0; y < Alpha.Length; y++) 
                    if (Alpha[y] == MessageCharPair[x])
                        MessageIntPair[x] = y;
            }

            string EightBitEndcodeMessage = "";

            //Convert Individual Charaters From the Message to Binary
            for (int x = 0; x < MessageIntPair.Length; x = x + 2) {
                // Convert First Half of Binary Values
                int tmp = MessageIntPair[x] * Alpha.Length;
                int remainder;
                string BinaryCode = string.Empty;
                while (tmp > 0) {
                    remainder = tmp % 2;
                    tmp /= 2;
                    BinaryCode = remainder.ToString() + BinaryCode;
                }
                if (x + 1 > MessageIntPair.GetLength(0)) {
                    //Convert Second Half of Binary Values
                    int tmp2 = MessageIntPair[x + 1];
                    int remainder2;
                    string BinaryCode2 = string.Empty;
                    while (tmp2 > 0) {
                        remainder2 = tmp2 % 2;
                        tmp2 /= 2;
                        BinaryCode2 = remainder2.ToString() + BinaryCode2;
                    }
                    BinaryCode = BinaryCode + BinaryCode2;
                }
                // Save Binary to the Array
                MessageBinaryPair[(x / 2)] = BinaryCode;
            }

            //Get all Char Data
            for (int x = 0; x < (MessageIntPair.Length / 2); x++) EightBitEndcodeMessage = EightBitEndcodeMessage + MessageBinaryPair[x];

            //Convert NumChar to Binary
            int CharRemainder;
            int NumChar = InputString.Length;
            string BinaryNumChar = string.Empty;
            while (NumChar > 0) {
                CharRemainder = NumChar % 2;
                NumChar /= 2;
                BinaryNumChar = CharRemainder.ToString() + BinaryNumChar;
            }

            //Make BinaryNumChar 9 Long
            while (BinaryNumChar.Length % 9 != 0) BinaryNumChar = "0" + BinaryNumChar;

            // Add Alpha Mode & # of Chars To the Binary
            EightBitEndcodeMessage = "0010" + BinaryNumChar + EightBitEndcodeMessage;

            // Add Padded "0"'s
            while (EightBitEndcodeMessage.Length % 8 != 0) EightBitEndcodeMessage = EightBitEndcodeMessage + "0";

            Console.WriteLine("QRCodeGennerator >> Length of Binary String : " + Convert.ToString(EightBitEndcodeMessage.Length));
            Console.WriteLine("QRCodeGennerator >> Message Converted to Binary :");
            Console.WriteLine("QRCodeGennerator >> " + EightBitEndcodeMessage);

            // Do Setup QRCode Space While Not Large Enough
            

            int Bytes = 0;

            while (Bytes < EightBitEndcodeMessage.Length)
            {
                // QR Code Setup
                string Output = "";
                string WChar = "█";
                string BChar = " ";
                
                bool TimeDot = true;
                bool FillUp = true;

                int Version = 1;

                int ix = 0;
                int iy = 0;

                int QRCodeX = ((Version * 4) + 17) + 2;
                int QRCodeY = ((Version * 4) + 17) + 2;

                int LargeLocatorX = 9;
                int LargeLocatorY = 9;

                int SmallLocatorX = 5;
                int SmallLocatorY = 5;

                int FormatInfo = 15;

                // Array(s) for QRCode
                string[,] QRCode = new string[QRCodeX, QRCodeY];
                string[,] LargeLocator = new string[LargeLocatorX, LargeLocatorY];
                string[,] SmallLocator = new string[SmallLocatorX, SmallLocatorY];
                string[] FormatInformation = new string[FormatInfo];

                //Setup the Format Array
                for (ix = 0; ix < FormatInfo; ix++)
                {
                    if (ix == 1 || ix == 2 || ix == 4 || ix == 5 || ix == 6 || ix == 8 || ix == 11 || ix == 13 || ix == 14) FormatInformation[ix] = BChar;
                    else FormatInformation[ix] = WChar;
                }

                //Setup the Large Locator(s) Array
                for (ix = 0; ix < LargeLocatorX; ix++)
                {
                    for (iy = 0; iy < LargeLocatorY; iy++)
                    {
                        if (ix == 0 || iy == 0 || ix == (LargeLocatorX - 1) || iy == (LargeLocatorY - 1) ||
                            ix == 2 && iy >= 2 && iy <= 6 || ix == 6 && iy >= 2 && iy <= 6 ||
                            iy == 2 && ix >= 2 && ix <= 6 || iy == 6 && ix >= 2 && ix <= 6)
                        {
                            LargeLocator[ix, iy] = WChar;
                        }
                        else
                        {
                            LargeLocator[ix, iy] = BChar;
                        }
                    }
                }

                /*//Draw Large Locator Result
                for (ix = 0; ix < LargeLocatorX; ix++) {
                    for (iy = 0; iy < LargeLocatorY; iy++) {
                        Output = Output + LargeLocator[ix, iy];
                    }
                    Console.WriteLine(Output);
                    Output = "";
                }
                */

                //Setup the Small Locator(s) Array
                for (ix = 0; ix < SmallLocatorX; ix++)
                {
                    for (iy = 0; iy < SmallLocatorY; iy++)
                    {
                        if (ix == 1 && iy >= 1 && iy <= 3 || ix == 3 && iy >= 1 && iy <= 3 ||
                            iy == 1 && ix >= 1 && ix <= 3 || iy == 3 && ix >= 1 && ix <= 3)
                        {
                            SmallLocator[ix, iy] = WChar;
                        }
                        else
                        {
                            SmallLocator[ix, iy] = BChar;
                        }
                    }
                }

                /*//Draw Small Locator Result
                for (ix = 0; ix < SmallLocatorX; ix++) {
                    for (iy = 0; iy < SmallLocatorY; iy++) {
                        Output = Output + SmallLocator[ix, iy];
                    }
                    Console.WriteLine(Output);
                    Output = "";
                }
                */

                //Setup the Bounds Addition [ Outside Area ]
                for (ix = 0; ix < QRCodeX; ix++)
                {
                    for (iy = 0; iy < QRCodeY; iy++)
                    {
                        if (ix == 0 || iy == 0 || ix == (QRCodeX - 1) || iy == (QRCodeY - 1))
                        {
                            QRCode[ix, iy] = WChar;
                        }
                    }
                }

                //The 3 Corner Locators Addition
                for (ix = 0; ix < LargeLocatorX; ix++)
                {
                    for (iy = 0; iy < LargeLocatorY; iy++)
                    {
                        //First Top Left Locator
                        QRCode[ix, iy] = LargeLocator[ix, iy];
                        //Second Top Right Locator
                        QRCode[ix, (iy + (QRCodeY - LargeLocatorY))] = LargeLocator[ix, iy];
                        //Third Bottom Left Locator
                        QRCode[(ix + (QRCodeX - LargeLocatorX)), iy] = LargeLocator[ix, iy];
                    }
                }

                //The Timing Strips Addition
                for (ix = 0; ix < QRCodeX; ix++)
                {
                    for (iy = 0; iy < QRCodeY; iy++)
                    {
                        if (ix == 7 && iy >= LargeLocatorY && iy <= (QRCodeY - LargeLocatorY) || iy == 7 && ix >= LargeLocatorX && ix <= (QRCodeX - LargeLocatorX))
                        {
                            if (QRCode[ix, iy] == null)
                            {
                                if (TimeDot) QRCode[ix, iy] = BChar;
                                else QRCode[ix, iy] = WChar;
                            }
                        }
                        TimeDot = !TimeDot;
                    }
                }

                /*Small Locators Addition
                for (ix = 0; ix < SmallLocatorX; ix++)
                {
                    for (iy = 0; iy < SmallLocatorY; iy++)
                    {
                        QRCode[(ix + (18 + 3)), (iy + (18 + 3))] = SmallLocator[ix, iy];
                    }
                }*/

                //Dark Module
                QRCode[(4 * Version) + 10, 9] = BChar;

                //Formating / Error Information Addition
                int Format1 = 0;
                int Format2 = 0;
                int Format3 = 0;
                int Format4 = 0;

                for (ix = 0; ix < FormatInfo; ix++)
                {
                    //Top Left, Right Format info [0-7]md
                    if (QRCode[ix + 1, LargeLocatorY] == null && ix <= ((FormatInformation.Length / 2) + 1))
                    {
                        QRCode[ix + 1, LargeLocatorY] = FormatInformation[Format1];
                        Format1++;
                    }
                    //Bottom Left, Right Format info [8-14]
                    if (QRCode[QRCodeX - (ix + 2), LargeLocatorY] == null && ix <= ((FormatInformation.Length / 2) - 1))
                    {
                        QRCode[QRCodeX - (ix + 2), LargeLocatorY] = FormatInformation[(FormatInfo - (Format2 + 1))];
                        Format2++;
                    }
                    //Top Right, Bottom Format info [7-0]
                    if (QRCode[LargeLocatorX, QRCodeX - (ix + 2)] == null && ix <= (FormatInformation.Length / 2))
                    {
                        QRCode[LargeLocatorX, QRCodeX - (ix + 2)] = FormatInformation[Format3];
                        Format3++;
                    }
                    //Top Left, Bottom Format info [14-8]
                    if (QRCode[LargeLocatorX, ix + 1] == null && ix <= ((FormatInformation.Length / 2) + 1))
                    {
                        QRCode[LargeLocatorX, ix + 1] = FormatInformation[(FormatInfo - (Format4 + 1))];
                        Format4++;
                    }
                }

                /*Output the Results to the Console for Debug
                for (ix = 0; ix < QRCodeX; ix++) {
                    for (iy = 0; iy < QRCodeY; iy++) {
                        Output = Output + QRCode[ix, iy];
                    }
                    Console.WriteLine(Output);
                    Output = "";
                }*/

                //Output the "Results" to the Console
                for (ix = 0; ix < QRCodeX; ix++)
                {
                    for (iy = 0; iy < QRCodeY; iy++)
                    {
                        if (QRCode[ix, iy] == null)
                        {
                            //Shape Picture?
                            Bytes++;
                        }
                        Output = Output + QRCode[ix, iy];
                    }
                    //Console.WriteLine(Output);
                    Output = "";
                }
                Console.WriteLine("QRCodeGennerator >> Bytes Available Before Writing Data : " + Bytes);

                //# of Message Data Space Available Before Adding Message Info
                string[] MessageInformation = new string[Bytes];

                //Real User Data PreBinary Converted to Message Array
                for (ix = 0; ix < Bytes; ix++)
                {
                    if (ix < EightBitEndcodeMessage.Length && EightBitEndcodeMessage.Substring(ix, 1) == "0") MessageInformation[ix] = WChar;
                    else MessageInformation[ix] = BChar;
                }

                /* Test Print of QRCode
                Console.WriteLine("███████████████████████");
                Console.WriteLine("█       █ ███ █       █");
                Console.WriteLine("█ █████ █  █ ██ █████ █");
                Console.WriteLine("█ █   █ █     █ █   █ █");
                Console.WriteLine("█ █   █ █ █ █ █ █   █ █");
                Console.WriteLine("█ █   █ ██    █ █   █ █");
                Console.WriteLine("█ █████ █ █ █ █ █████ █");
                Console.WriteLine("█       █ █ █ █       █");
                Console.WriteLine("███████████████████████");
                Console.WriteLine("█  ██   █████ ██ █    █");
                Console.WriteLine("██     █  █ █   ██ █ ██");
                Console.WriteLine("███ ███ █ █    ████  ██");
                Console.WriteLine("█  █ █ █ ██   ██ ████ █");
                Console.WriteLine("█   █        ██    ████");
                Console.WriteLine("█████████ ██  ██ ██  ██");
                Console.WriteLine("█       ████ ██    █ ██");
                Console.WriteLine("█ █████ █  ███  █ █████");
                Console.WriteLine("█ █   █ █ ███   █████ █");
                Console.WriteLine("█ █   █ ███ █   ██    █");
                Console.WriteLine("█ █   █ ██ ██  ████████");
                Console.WriteLine("█ █████ █ ██  ██ █ ████");
                Console.WriteLine("█       █    ██   █ █ █");
                Console.WriteLine("███████████████████████");
                */

                //Test Data Area for Data Algorithm... for (ix = 0; ix < Bytes; ix++) MessageInformation[ix] = WChar;

                /*Test Data For Algorithm [Link to Chris Hadfeild's Space Oddity on Youtube]
                for (ix = 0; ix < Bytes; ix++) {
                    if (ix == 0 || ix == 2 || ix == 5 || ix == 6 || ix == 8 || ix == 9 || ix == 12 || ix == 16 || ix == 17 ||
                        ix == 23 || ix == 24 || ix == 26 || ix == 30 || ix == 31 || ix == 34 || ix == 36 || ix == 39 ||

                        ix == 40 || ix == 46 || ix == 49 || ix == 51 || ix == 52 || ix == 54 || ix == 56 || ix == 57 || ix == 58 ||
                        ix == 60 || ix == 61 || ix == 63 || ix == 65 || ix == 66 || ix == 67 || ix == 69 || ix == 70 || ix == 72 ||
                        ix == 76 || ix == 77 || ix == 78 || ix == 79 || 
                    
                        ix == 83 || ix == 85 || ix == 87 || ix == 88 || ix == 89 || ix == 90 || ix == 92 || ix == 93 || ix == 94 || 
                        ix == 95 || ix == 97 || ix == 98 || ix == 100 || ix == 101 || ix == 102 || ix == 105 || ix == 108 || ix == 109 ||

                        ix == 115 || ix == 118 || ix == 119 || ix == 122 || ix == 123 || ix == 124 || ix == 127 ||
                        ix == 133 || ix == 134 || ix == 136 || ix == 137 || ix == 138 ||

                        ix == 141 || ix == 144 || ix == 145 || ix == 147 || ix == 148 || ix == 149 ||
                        ix == 150 || ix == 152 || ix == 154 || ix == 155 || ix == 156 || ix == 158 || ix == 160 ||
                        ix == 162 || ix == 163 || ix == 164 || ix == 168 || ix == 170 || ix == 171 || ix == 174 || 
                        ix == 175 || ix == 179 || ix == 181 || ix == 182 ||  ix == 183 || ix == 184 || ix == 186 || ix == 188 ||

                        ix == 194 || ix == 195 || ix == 198 || ix == 199 || ix == 200 || ix == 205 || ix == 206 || ix == 207 ||
                        ix == 210 || ix == 212 || ix == 213 || ix == 217 || ix == 219 || ix == 221 || ix == 223 || ix == 224 ||
                        ix == 229 || ix == 230 || ix == 231 || ix == 232 || ix == 234 || ix == 235 || ix == 236 || ix == 240 ||
                        ix == 241 || ix == 242 || ix == 245 ||
                    
                        ix == 248 || ix == 250 || ix == 252 || ix == 254 || ix == 255 || ix == 260 || ix == 266 || ix == 267 ||
                        ix == 268 || ix == 270 || ix == 271 || ix == 272 || ix == 273 || ix == 275 || ix == 276 || ix == 277 ||
                        ix == 282 || ix == 284 || ix == 288 || ix == 290 || ix == 293 || ix == 296 || ix == 298 || ix == 299 || 
                        ix == 301 ||
                    
                        ix == 304 || ix == 306 || ix == 307 || ix == 308 || ix == 310 || ix == 316 || ix == 319 || ix == 320 ||
                        ix == 321 || ix == 322 || ix == 325 || ix == 329 || ix == 330 || ix == 331 || ix == 333 || ix == 336 ||
                        ix == 337 || ix == 339 || ix == 340 || ix == 343 || ix == 344 || ix == 345 || ix == 346 || ix == 347 ||
                        ix == 350 || ix == 351 || ix == 355 || ix == 356 || ix == 357 ||

                        ix == 360 || ix == 361 || ix == 363 || ix == 365 || ix == 367 || ix == 377 || ix == 379 || ix == 380 || 
                        ix == 381 || ix == 382 || ix == 383 || ix == 386 || ix == 388 || ix == 392 || ix == 393 || ix == 394 ||
                        ix == 395 || ix == 396 || ix == 397 || ix == 399 || ix == 400 || ix == 402 || ix == 403 || ix == 405 ||
                        ix == 406 || ix == 407 || ix == 414 ||

                        ix == 415 || ix == 417 || ix == 418 || ix ==  423 || ix == 424 || ix == 425 || ix == 427 || ix == 430 ||
                        ix == 432 || ix == 435 || ix == 436 || ix == 437 || ix == 438 || ix == 440 || ix == 443 || ix == 444 ||
                        ix == 445 || ix == 446 || ix == 447 || ix == 448 || ix == 450 || ix == 451 || ix == 452 || ix == 455 ||
                        ix == 457 || ix == 460 || ix == 462 || ix == 463 || ix == 466 || ix == 467 || ix == 468  || ix == 470 ||

                        ix == 473 || ix == 475  || ix == 476 || ix == 478 || ix == 481 || ix == 482 || ix == 483 || ix == 486 || 
                        ix == 487 || ix == 491 || ix == 494 ||
                    
                        // Data Shift.... Algorithm Fix..
                        //ix == 498 || ix == 499  || ix == 500|| ix == 501

                        ix == 497 || ix == 498 || ix == 500 || ix == 501 || ix == 504 || ix == 506 ||

                        ix == 512 || ix == 522 || ix == 524 ||
                    
                        ix == 532 || ix == 533 || ix == 534 || ix == 538 || ix == 539 || ix == 540 || ix == 541 || ix == 543 ||
                        ix == 544 || ix == 545 || ix == 547 || ix == 548 || ix == 550 || ix == 552 ||

                        ix == 556 || ix == 558 || ix == 564 || ix == 567
                    
                        ) MessageInformation[ix] = WChar;
                    else MessageInformation[ix] = BChar;
                }*/

                //Write Message Algorithm
                int iBytes = 0;

                //For Every Byte in Message
                for (ix = 0; ix < (((QRCodeX - 2) / 2) + 1); ix++)
                {
                    for (iy = 0; iy < (QRCodeY - 2); iy++)
                    {
                        //if (QRCodeX - ((2 * ix) + 2) == 7) {
                        //iy = -1;
                        //ix = ix + 1;
                        //}
                        //else
                        //{
                        if ((ix % 2) == 0) FillUp = true;
                        else FillUp = false;

                        if (FillUp)
                        {
                            if (QRCode[QRCodeY - (iy + 2), QRCodeX - ((2 * ix) + 2)] == null)
                            {
                                QRCode[QRCodeY - (iy + 2), QRCodeX - ((2 * ix) + 2)] = MessageInformation[iBytes];
                                iBytes++;
                            }
                            if (QRCode[QRCodeY - (iy + 2), QRCodeX - ((2 * ix) + 3)] == null)
                            {
                                QRCode[QRCodeY - (iy + 2), QRCodeX - ((2 * ix) + 3)] = MessageInformation[iBytes];
                                iBytes++;
                            }
                        }
                        else if (!FillUp)
                        {
                            if (QRCode[(iy + 1), QRCodeX - ((2 * ix) + 2)] == null)
                            {
                                QRCode[(iy + 1), QRCodeX - ((2 * ix) + 2)] = MessageInformation[iBytes];
                                iBytes++;
                            }
                            if (QRCode[(iy + 1), QRCodeX - ((2 * ix) + 3)] == null)
                            {
                                QRCode[(iy + 1), QRCodeX - ((2 * ix) + 3)] = MessageInformation[iBytes];
                                iBytes++;
                            }
                        }
                        //}
                    }
                }

                //Reset
                int OutputBytes = 0;

                //Output the End Results to the Console
                for (ix = 0; ix < QRCodeX; ix++)
                {
                    for (iy = 0; iy < QRCodeY; iy++)
                    {
                        if (QRCode[ix, iy] == null)
                        {
                            QRCode[ix, iy] = BChar;
                            OutputBytes++;
                        }
                        Output = Output + QRCode[ix, iy];
                    }
                    Console.WriteLine(Output);
                    Output = "";
                }
                Console.WriteLine("QRCodeGennerator >> Bytes Available After Writing Data : " + OutputBytes);

                /* Test Picture Output
                Bitmap image = new Bitmap(QRCodeX * 2, QRCodeY * 2);
                image.SetResolution(QRCodeX / 2, QRCodeY / 2);
                image.SetPixel(,, Color.Black);
                image.SetPixel(,, Color.White);
                image
                */
            }

            Console.ReadLine();
        }
    }
}

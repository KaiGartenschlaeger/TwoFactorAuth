using Base32;
using OtpSharp;
using System;

namespace TwoFactorAuth
{
    class Program
    {
        // static key for tests
        private static readonly string SecretTestKey = "SVSPMQGHAIKFW6FBQI5NNXKOUBZM3L34";
        private static readonly byte[] SecretTestData = Base32Encoder.Decode(SecretTestKey);

        static void Main(string[] args)
        {
            // create new secret key
            byte[] secretKeyData = KeyGeneration.GenerateRandomKey(20);
            string secretKeyString = Base32Encoder.Encode(secretKeyData);

            Console.WriteLine("Add a new Key to you Auth App and enter the generated code.");
            Console.WriteLine();
            Console.WriteLine("Secret key: " + secretKeyString);
            Console.WriteLine();

            // var otpCode = otp.ComputeTotp(DateTime.UtcNow);
            // var otpSeconds = otp.RemainingSeconds();

            // check code
            Console.Write("Code: ");
            var enteredCode = Console.ReadLine();

            var otp = new Totp(secretKeyData);
            if (otp.VerifyTotp(enteredCode, out long timeStepMatched))
            {
                Console.WriteLine("The code is valid");
            }
            else
            {
                Console.WriteLine("The code is invalid");
            }

            Console.ReadKey(true);
        }
    }
}
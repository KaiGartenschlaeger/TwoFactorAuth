using Base32;
using OtpSharp;
using QRCoder;
using System;
using System.Drawing.Imaging;

namespace TwoFactorAuth
{
    class Program
    {
        // a static key for tests
        private static readonly string SecretTestKey = "SVSPMQGHAIKFW6FBQI5NNXKOUBZM3L34";
        private static readonly byte[] SecretTestData = Base32Encoder.Decode(SecretTestKey);

        static void Main(string[] args)
        {
            // create a new secret key
            byte[] secretKeyData = KeyGeneration.GenerateRandomKey(20);
            string secretKeyString = Base32Encoder.Encode(secretKeyData);

            Console.WriteLine("Add the following new secret key to you auth app and enter the generated code.");
            Console.WriteLine();
            Console.WriteLine("Secret key: " + secretKeyString);
            Console.WriteLine();

            // var otpCode = otp.ComputeTotp(DateTime.UtcNow);
            // var otpSeconds = otp.RemainingSeconds();

            // uncomment to create a qr code image and save it in bin folder under the filename qr-code.png.
            // string issuer = "Example";
            // string appName = "MyApp";
            // string user = "test@my-app.abc";
            // 
            // QRCodeGenerator qrGenerator = new QRCodeGenerator();
            // QRCodeData qrCodeData = qrGenerator.CreateQrCode(
            //     $"otpauth://totp/{appName}:{user}?secret={secretKeyString}&issuer={issuer}",
            //     QRCodeGenerator.ECCLevel.H);
            // QRCode qrCode = new QRCode(qrCodeData);
            // 
            // var qrCodeImage = qrCode.GetGraphic(20);
            // qrCodeImage.Save("qr-code.png", ImageFormat.Png);

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

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
            //
            // create a new secret key
            //
            var secretKeyData = KeyGeneration.GenerateRandomKey(20);
            var secretKeyString = Base32Encoder.Encode(secretKeyData);
            var period = 30;
            var digits = 6;

            Console.WriteLine("Add the following new secret key to you auth app and enter the generated code.");
            Console.WriteLine();
            Console.WriteLine("Secret key: " + secretKeyString);
            Console.WriteLine();

            var otp = new Totp(secretKeyData,
                step: period,
                mode: OtpHashMode.Sha256,
                totpSize: digits);

            // 
            // generate a qr code image
            //
            var otpQrGenerator = new PayloadGenerator.OneTimePassword
            {
                Label = "Example",
                Type = PayloadGenerator.OneTimePassword.OneTimePasswordAuthType.TOTP,
                Period = period,
                Algorithm = PayloadGenerator.OneTimePassword.OoneTimePasswordAuthAlgorithm.SHA256,
                Secret = secretKeyString,
                Issuer = "example@test.ab",
                Digits = digits
            };

            var qrCodepayload = otpQrGenerator.ToString();

            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(qrCodepayload, QRCodeGenerator.ECCLevel.H);
            var qrCode = new QRCode(qrCodeData);

            var qrCodeImage = qrCode.GetGraphic(20);
            qrCodeImage.Save("qr-code.png", ImageFormat.Png);

            //
            // validate entered code
            //
            Console.Write("Code: ");
            var enteredCode = Console.ReadLine();

            var otpCode = otp.ComputeTotp(DateTime.UtcNow);
            var otpSeconds = otp.RemainingSeconds();

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

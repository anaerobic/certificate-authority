using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace CertificateAuthority
{
    class Program
    {
        static void Main(string[] args)
        {
            const string cPath = "C:\\";

            string
                sslPath = Path.Combine(cPath, "ssl\\"),
                exePath = Path.Combine(Environment.CurrentDirectory, "openssl\\openssl.exe"),
                cmdExe = "\"" + exePath + "\"",
                rootPath = Path.Combine(sslPath, "root\\"),
                devicePath = Path.Combine(sslPath, "device\\"),
                configPath = Path.Combine(Environment.CurrentDirectory, "openssl\\openssl.cnf"),
                configOpenssl = " -config \"" + configPath + "\"",
                subAltDomsPath = Path.Combine(Environment.CurrentDirectory, "openssl\\subaltdoms.cnf"),
                configSubAltDoms = " -config \"" + subAltDomsPath + "\"",
                certName = "*.ltfinc.dev",
                password = "12345",
                rootName = "Team LEGO Root CA";

            if (!Directory.Exists(sslPath))
            {
                Directory.CreateDirectory(sslPath);
            }
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            using (var process = Process.Start(new ProcessStartInfo
            {
                Arguments = "\\c ",
                FileName = "cmd",
                RedirectStandardInput = true,
                UseShellExecute = false
            }))
            {
                if (!File.Exists(Path.Combine(rootPath, "rootCA.key")))
                {
                    process.StandardInput.WriteLine(string.Format(cmdExe + " genrsa -out {0}rootCA.key 2048 -des3", rootPath));

                    Thread.Sleep(3000);
                }

                if (!File.Exists(Path.Combine(rootPath, "rootCA.pem")))
                {
                    process.StandardInput.WriteLine(
                        cmdExe +
                        string.Format(" req -x509 -new -nodes -key {0}rootCA.key -days 1024 -out {0}rootCA.pem -sha256",
                            rootPath) +
                        configOpenssl);

                    new AnswerOpenSslForRoot(process).Answer();

                    Thread.Sleep(3000);
                }

                if (!File.Exists(Path.Combine(rootPath, "rootCA.pfx")))
                {
                    process.StandardInput.WriteLine(
                            cmdExe +
                            string.Format(
                                " pkcs12 -export -out {0}rootCA.pfx -in {0}rootCA.pem -inkey {0}rootCA.key -name \"{1}\" -passout pass:{2}",
                                rootPath,
                                rootName,
                                password));

                    Thread.Sleep(3000);
                }

                if (!Directory.Exists(devicePath))
                {
                    Directory.CreateDirectory(devicePath);
                }

                if (!File.Exists(Path.Combine(devicePath, "device.key")))
                {
                    process.StandardInput.WriteLine(cmdExe + string.Format(" genrsa -out {0}device.key 2048", devicePath));

                    Thread.Sleep(3000);
                }

                if (!File.Exists(Path.Combine(devicePath, "device.csr")))
                {
                    process.StandardInput.WriteLine(cmdExe +
                                                    string.Format(" req -key {0}device.key -out {0}device.csr -new -sha256",
                                                        devicePath) +
                                                    configSubAltDoms);

                    new AnswerOpenSslForDevice(process).Answer();

                    Thread.Sleep(3000);

                    process.StandardInput.WriteLine(cmdExe +
                                                    string.Format(" req -text -noout -in {0}device.csr", devicePath));

                    Thread.Sleep(3000);
                }

                if (!File.Exists(Path.Combine(devicePath, "device.crt")))
                {
                    process.StandardInput.WriteLine(
                        cmdExe +
                        string.Format(
                            " x509 -sha256 -req -in {1}device.csr -CA {0}rootCA.pem -CAkey {0}rootCA.key -CAcreateserial -out {1}device.crt -days 500 -extensions v3_req -extfile \"{2}\"",
                            rootPath, devicePath, subAltDomsPath));
                }

                if (!File.Exists(Path.Combine(devicePath, "device.pem")))
                {
                    process.StandardInput.WriteLine(
                        cmdExe +
                        string.Format(
                            " x509 -sha256 -in {0}device.crt -out {0}device.pem -outform PEM",
                            devicePath));

                    Thread.Sleep(3000);
                }

                if (!File.Exists(Path.Combine(devicePath, "device.pfx")))
                {
                    process.StandardInput.WriteLine(
                        cmdExe +
                        string.Format(
                            " pkcs12 -export -out {0}device.pfx -in {0}device.pem -inkey {0}device.key -name \"{1}\" -passout pass:{2}",
                            devicePath,
                            certName,
                            password));

                    Thread.Sleep(3000);
                }

                var x509 = new X509Certificate2(Path.Combine(devicePath, "device.crt"));

                var ps1 = CertificateAuthority.Resources.template
                    .Replace("{rootCertPath}", Path.Combine(rootPath, "rootCA.pfx"))
                    .Replace("{deviceCertPath}", Path.Combine(devicePath, "device.pfx"))
                    .Replace("{thumbprint}", x509.Thumbprint.ToLower());

                File.WriteAllText(Path.Combine(sslPath, "run.ps1"), ps1);
            }
        }
    }
}

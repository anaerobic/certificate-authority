using System;
using System.Diagnostics;

namespace CertificateAuthority
{
    class AnswerOpenSslForDevice : AnswerOpenSslTemplate
    {
        public AnswerOpenSslForDevice(Process process)
            : base(process)
        {
        }

        protected override void Commonz()
        {
            Process.StandardInput.WriteLine("*.ltfinc.dev"); //common
        }

        protected override void Afterz()
        {
            Process.StandardInput.WriteLine(""); //password
            Process.StandardInput.WriteLine(Environment.MachineName); //alternative company
        }
    }
}
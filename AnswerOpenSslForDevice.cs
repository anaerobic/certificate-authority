using System;
using System.Diagnostics;

namespace CertificateAuthority
{
    class AnswerOpenSslForDevice : AnswerOpenSslTemplate
    {
        private readonly string _commonName;

        public AnswerOpenSslForDevice(Process process,string commonName)
            : base(process)
        {
            _commonName = commonName;
        }

        protected override void Commonz()
        {
            Process.StandardInput.WriteLine(_commonName); //common
        }

        protected override void Afterz()
        {
            Process.StandardInput.WriteLine(""); //password
            Process.StandardInput.WriteLine(Environment.MachineName); //alternative company
        }
    }
}
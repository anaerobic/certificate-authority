using System;
using System.Diagnostics;

namespace CertificateAuthority
{
    class AnswerOpenSslForRoot : AnswerOpenSslTemplate
    {
        public AnswerOpenSslForRoot(Process process)
            : base(process)
        {
        }

        protected override void Commonz()
        {
            Process.StandardInput.WriteLine("Team LEGO Root CA"); //common
        }
    }
}
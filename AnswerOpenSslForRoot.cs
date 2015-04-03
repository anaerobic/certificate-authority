using System.Diagnostics;

namespace CertificateAuthority
{
    class AnswerOpenSslForRoot : AnswerOpenSslTemplate
    {
        private readonly string _commonName;

        public AnswerOpenSslForRoot(Process process, string commonName)
            : base(process)
        {
            _commonName = commonName;
        }

        protected override void Commonz()
        {
            Process.StandardInput.WriteLine(_commonName); //common
        }
    }
}
using System.Diagnostics;

namespace CertificateAuthority
{
    abstract class AnswerOpenSslTemplate
    {
        protected readonly Process Process;

        protected AnswerOpenSslTemplate(Process process)
        {
            Process = process;
        }

        public void Answer()
        {
            Process.StandardInput.WriteLine("US"); //country
            Process.StandardInput.WriteLine("CA"); //state
            Process.StandardInput.WriteLine("Aliso Viejo"); //locality
            Process.StandardInput.WriteLine("LTF, INC."); //org
            Process.StandardInput.WriteLine("Team LEGO"); //org unit

            Commonz();

            Process.StandardInput.WriteLine("mmartin2@lifetimefitness.com"); //email

            Afterz();
        }

        protected abstract void Commonz();

        protected virtual void Afterz() { }
    }
}
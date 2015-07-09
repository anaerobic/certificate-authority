# certificate-authority

## Usage
* Build CertificateAuthority.sln
* Run bin\Debug\CertificateAuthority.exe
  *  this will create a folder c:\ssl with a run.ps1 and two subdirectories, device and root.
  *  the ssl\root folder will contain your rootCA pfx certificate.
  *  the ssl\device folder will contain your device crt and pfx certificates.
* run the ssl\run.ps1 to add the certificates to their new homes in your cert store.
* you can spin up the OwinApp provided if you need a simple web server to test.
  (you may need to run netsh http add urlacl url=https://+:443/ user=everyone from an elevated command prompt)
* navigate to https://lacolhost.com (resolves to localhost) and witness a nice green lock in Chrome.

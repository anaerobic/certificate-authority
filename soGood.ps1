function Import-PfxCertificate {

	param([String]$certPath,[String]$certRootStore = "CurrentUser",[String]$certStore = "My",$pfxPass = $null)

	$pfx = new-object System.Security.Cryptography.X509Certificates.X509Certificate2

	if ($pfxPass -eq $null) {$pfxPass = read-host "Enter the $certPath password" -assecurestring}

	$pfx.import($certPath,$pfxPass,"Exportable,PersistKeySet,MachineKeySet")

	$store = new-object System.Security.Cryptography.X509Certificates.X509Store($certStore,$certRootStore)
	$store.open("MaxAllowed")
	$store.add($pfx)
	$store.close()
}

Import-PfxCertificate "{rootCertPath}" "LocalMachine" "Root" "12345"

Import-PfxCertificate "{deviceCertPath}" "LocalMachine" "My" "12345"

netsh http delete sslcert ipport=0.0.0.0:44333

$guid = [guid]::NewGuid()

$Command = "http add sslcert ipport=0.0.0.0:44333 certhash={thumbprint} appid={$guid}"
$Command | netsh


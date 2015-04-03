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

Import-PfxCertificate "{rootCertPath}" "LocalMachine" "Root" {rootPassword}

Import-PfxCertificate "{deviceCertPath}" "LocalMachine" "My" {devicePassword}

netsh http delete sslcert ipport={ipPort}

$guid = [guid]::NewGuid()

$Command = "http add sslcert ipport={ipPort} certhash={thumbprint} appid={$guid}"
$Command | netsh


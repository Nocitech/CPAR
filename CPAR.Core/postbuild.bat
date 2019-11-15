:: POSTBUILD Perform post build actions
:: This script performs the following tasks.
::   1. Compile xsd schema's from the CPAR.Core assembly
::   2. Uploads the schema's to nocitech.com/xsd/working so they are globally accessible
:: 
:: The script takes the following parametes
cd bin
cd Debug

"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\xsd.exe" %1 /outputdir:"C:\CPAR\xsd" /type:CPAR.Core.Protocol
copy C:\CPAR\xsd\schema0.xsd C:\CPAR\xsd\protocol.xsd
del C:\CPAR\xsd\schema0.xsd 

"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\xsd.exe" %1 /outputdir:"C:\CPAR\xsd" /type:CPAR.Core.Experiment
copy C:\CPAR\xsd\schema0.xsd C:\CPAR\xsd\experiment.xsd
del C:\CPAR\xsd\schema0.xsd 

"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\xsd.exe" %1 /outputdir:"C:\CPAR\xsd" /type:CPAR.Core.Exporter
copy C:\CPAR\xsd\schema0.xsd C:\CPAR\xsd\exporter.xsd
del C:\CPAR\xsd\schema0.xsd 

cd ..
cd ..


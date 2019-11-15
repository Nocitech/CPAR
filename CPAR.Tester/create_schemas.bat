
"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\xsd.exe" bin/Debug/CPAR.Tester.exe /outputdir:Schema /type:CPAR.Tester.TestScript

cd Schema
copy schema0.xsd testscript.xsd
del schema0.xsd
cd ..

pause
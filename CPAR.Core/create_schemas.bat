
cd bin
cd Debug
xsd.exe  CPAR.Core.dll /type:CPAR.Core.Protocol

copy schema0.xsd ..\..\Schemas\protocol.xsd
del schema0.xsd

xsd.exe  CPAR.Core.dll /type:CPAR.Core.Experiment

copy schema0.xsd ..\..\Schemas\experiment.xsd
del schema0.xsd

xsd.exe  CPAR.Core.dll /type:CPAR.Core.Exporter

copy schema0.xsd ..\..\Schemas\exporter.xsd
del schema0.xsd

cd ..
cd ..

pause
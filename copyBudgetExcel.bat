set src="Metaproject.Excel\bin\Debug\Metaproject.Excel.*"
set dstUI="Budget.UI\bin\Debug"
set dstTests="Budget.UnitTests\bin\Debug"

copy %src% %dstUI%
copy %src% %dstTests%

pause
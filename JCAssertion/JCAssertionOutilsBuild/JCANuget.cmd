
rem toujours laisser une ligne blanche au debut du fichier
@echo off

chcp 65001
set Msbuildexe="C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe"
set Nugetexe="D:\DevCenter\Tools\nuget.exe"
echo Go >%~dp0log.txt
 
xcopy "D:\DevCenter\Sources\JCAssertion\JCAssertion\JCAssertionnCore\bin\Release\*.*" "D:\DevCenter\build\TempBuild\bin\JcAssertion\lib\net45" /y >>%~dp0log.txt
if %errorlevel% NEQ 0 goto erreur

xcopy "D:\DevCenter\Sources\JCAssertion\JCAssertion\JCAExport\bin\Release\*.exe" "D:\DevCenter\build\TempBuild\bin\JcAssertion\content" /y >>%~dp0log.txt
if %errorlevel% NEQ 0 goto erreur

xcopy "D:\DevCenter\Sources\JCAssertion\JCAssertion\JCAssertion\bin\Release\*.exe" "D:\DevCenter\build\TempBuild\bin\JcAssertion\content" /y >>%~dp0log.txt
if %errorlevel% NEQ 0 goto erreur

xcopy "D:\DevCenter\Sources\JCAssertion\JCAssertion\JCAssertionOutilsBuild\JCAssertion.nuspec" "D:\DevCenter\build\TempBuild\bin\JCAssertion\" /f /y >>%~dp0log.txt
if %errorlevel% NEQ 0 goto erreur

echo creer la config
%Nugetexe% config -set verbosity=detailed >>%~dp0log.txt
if %errorlevel% NEQ 0 goto erreur

%Nugetexe% pack "D:\DevCenter\build\TempBuild\bin\JCAssertion\JCAssertion.nuspec" -OutputDirectory "D:\DevCenter\Nuget\MaGallerie"  -configfile "D:\DevCenter\build\TempBuild\bin\DocDocos\0.1.1\DocDocosNConfig.xml"2>>%~dp0log.txt
if %errorlevel% NEQ 0 goto erreur

goto fini
:erreurs
pause
:erreur
echo Fichier de command fini avec erreur >>%~dp0log.txt
notepad %~dp0log.txt
goto end
:fini
echo Fichier de command fini sans erreur >>%~dp0log.txt
:end
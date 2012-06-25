@echo off

REM you should have git added to your %PATH% variable!

setlocal EnableDelayedExpansion
echo make sure VS is closed before running!
SET /p desc=Enter a description of your changes: 
SET /p yn=Really commit changes? (Y/n) [n]: 
if (%yn%)==() ( SET yn=n )

set no=false

if "%yn%"=="n" ( SET no=true )
if "%yn%"=="N" ( SET no=true )
if "%yn%"=="no" ( SET no=true )
if "%yn%"=="No" ( SET no=true )

if "%no%"=="false" ( 
echo cleaning files..

rem remove build stuff 
rem we only want to commit source
for /D %%y in (*Resharper.*) do rmdir %%y /S /Q
for /D %%z in (*.*) do rmdir %%z\bin /S /Q
for /D %%x in (*.*) do rmdir %%x\obj /S /Q
del /Q *.suo
del /Q /S *.user

echo commiting changes..
git add -A

if ["%desc%"]==[] (
git commit
) else (
git commit -m "%desc%"
)

rem git status

"C:\Program Files (x86)\Git\bin\sh.exe" --login -i -c "git push origin"

rem git push -u origin master
)

PAUSE
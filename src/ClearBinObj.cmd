@echo off
set "projetoCaminho=.\api\"

for /d /r %projetoCaminho% %%d in (bin obj) do (
    if exist "%%d" (
        echo Excluindo pasta: %%d
        rd /s /q "%%d"
    )
)

pause
exit
set WORKSPACE=..

set GEN_CLIENT=%WORKSPACE%\Excel\Luban.ClientServer\Luban.ClientServer.exe
set CONF_ROOT=%WORKSPACE%\Excel\Config


%GEN_CLIENT%  -j cfg --^
 -d %CONF_ROOT%\Defines\__root__.xml ^
 --input_data_dir %CONF_ROOT%\Datas ^
 --output_code_dir %WORKSPACE%\Unity\Assets\Scripts\GenerateCode^
 --output_data_dir %WORKSPACE%\Unity\Assets\Resources\Cfg ^
 --gen_types code_cs_unity_json,data_json ^
 -s all 

pause
rem
rem  This batch file is called in the project's post-build event.
rem
rem  It copies the output .DLL (and .PDB) to LINQPad's drivers folder, so that LINQPad
rem  picks up the drivers immediately (without needing to click 'Add Driver').
rem

xcopy /i/y Madd0.AzureStorageDriver.dll "%AllUsersProfile%\LINQPad\Drivers\DataContext\3.5\Madd0.AzureStorageDriver (47842961fb3025d7)\"
xcopy /i/y Madd0.AzureStorageDriver.pdb "%AllUsersProfile%\LINQPad\Drivers\DataContext\3.5\Madd0.AzureStorageDriver (47842961fb3025d7)\"

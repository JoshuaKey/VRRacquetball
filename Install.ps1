
# "C:\Program Files\Unity\Hub\Editor\2018.3.11f1\Editor\Unity.exe" -batchmode -buildWindowsPlayer "./Builds/Build.exe" -stackTraceLogType Full -logFile "Log.txt"
# C:\Program Files\Unity\Hub\Editor\2018.3.11f1\Editor\Unity.exe
# -logFile <pathname>: Log File Path
# -batchmode: Do not open Unity Editor, run commands
# -stackTraceLogType Full: Logging
# -buildTarget Android: Build 
# -executeMethod <Classname.Method>: Calls a Static method 
# -quit: Quit unity Editor

adb uninstall [package name / com.Test.VRTest]

adb install [apk file / VRTest.apk]
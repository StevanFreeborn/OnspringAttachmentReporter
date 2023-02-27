Remove-Item -Path tests\TestResults -Recurse -Force

dotnet test --collect:"XPlat Code Coverage;Include=[OnspringAttachmentReporter]*"

dotnet "$HOME\.nuget\packages\reportgenerator\5.1.18\tools\net6.0\ReportGenerator.dll" `
-reports:tests\TestResults\*\coverage.cobertura.xml `
-targetdir:tests\TestResults\coveragereport `
-reporttypes:Html_Dark;
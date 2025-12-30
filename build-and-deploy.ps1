# Скрипт для сборки и подготовки к развертыванию
# Использование: .\build-and-deploy.ps1 -ApiBaseUrl "https://your-domain.com/api"

param(
    [string]$ApiBaseUrl = "https://localhost:7203",
    [string]$Configuration = "Release"
)

Write-Host "=== Сборка ZasNet приложения ===" -ForegroundColor Green

# Пути к проектам
$apiPath = "C:\Users\Rustem\source\repos\ZasNet\ZasNet.WebApi"
$clientPath = "C:\Users\Rustem\source\repos\ZasNetWebClient\ZasNetWebClient"
$outputPath = ".\deploy"

# Создание выходной директории
if (Test-Path $outputPath) {
    Remove-Item $outputPath -Recurse -Force
}
New-Item -ItemType Directory -Path $outputPath | Out-Null
New-Item -ItemType Directory -Path "$outputPath\api" | Out-Null
New-Item -ItemType Directory -Path "$outputPath\client" | Out-Null

Write-Host "`n1. Сборка API..." -ForegroundColor Yellow
Set-Location $apiPath
dotnet publish -c $Configuration -o "$PSScriptRoot\$outputPath\api" --no-self-contained

if ($LASTEXITCODE -ne 0) {
    Write-Host "Ошибка при сборке API!" -ForegroundColor Red
    exit 1
}

Write-Host "`n2. Сборка клиента..." -ForegroundColor Yellow
Set-Location $clientPath

# Установка переменной окружения для API URL
$env:ApiBaseUrl = $ApiBaseUrl
dotnet publish -c $Configuration -o "$PSScriptRoot\$outputPath\client-temp"

if ($LASTEXITCODE -ne 0) {
    Write-Host "Ошибка при сборке клиента!" -ForegroundColor Red
    exit 1
}

# Копирование только wwwroot из клиента
Write-Host "`n3. Копирование файлов клиента..." -ForegroundColor Yellow
Copy-Item -Path "$PSScriptRoot\$outputPath\client-temp\wwwroot\*" -Destination "$PSScriptRoot\$outputPath\client" -Recurse -Force

# Обновление appsettings.json для клиента с API URL
$clientAppSettingsPath = "$PSScriptRoot\$outputPath\client\appsettings.json"
if (Test-Path $clientAppSettingsPath) {
    $clientAppSettings = @{
        ApiBaseUrl = $ApiBaseUrl
    } | ConvertTo-Json
    Set-Content -Path $clientAppSettingsPath -Value $clientAppSettings
} else {
    $clientAppSettings = @{
        ApiBaseUrl = $ApiBaseUrl
    } | ConvertTo-Json
    Set-Content -Path $clientAppSettingsPath -Value $clientAppSettings
}

# Добавление/обновление скрипта в index.html для установки API URL
$indexHtmlPath = "$PSScriptRoot\$outputPath\client\index.html"
if (Test-Path $indexHtmlPath) {
    $indexContent = Get-Content $indexHtmlPath -Raw
    # Заменяем существующий API_BASE_URL или добавляем новый
    if ($indexContent -match "window\.API_BASE_URL\s*=") {
        $indexContent = $indexContent -replace "window\.API_BASE_URL\s*=\s*[^;]+;", "window.API_BASE_URL = '$ApiBaseUrl';"
    } else {
        $scriptTag = @"
    <script>
        // Установка API Base URL (может быть переопределена при развертывании)
        window.API_BASE_URL = window.API_BASE_URL || '$ApiBaseUrl';
    </script>
"@
        # Вставка скрипта перед закрывающим тегом </head>
        $indexContent = $indexContent -replace '</head>', "$scriptTag`n</head>"
    }
    Set-Content -Path $indexHtmlPath -Value $indexContent
}

# Удаление временной папки
Remove-Item "$PSScriptRoot\$outputPath\client-temp" -Recurse -Force

Write-Host "`n4. Создание инструкций по развертыванию..." -ForegroundColor Yellow
$deployInstructions = @"
# Инструкции по развертыванию

## API
Скопируйте содержимое папки 'api' на сервер в директорию /opt/zasnet/api

## Клиент
Скопируйте содержимое папки 'client' на сервер в директорию /var/www/zasnet/client

## Настройка API URL
API Base URL установлен: $ApiBaseUrl

## Следующие шаги
1. Настройте appsettings.json для API с правильными строками подключения
2. Примените миграции базы данных
3. Настройте Nginx для раздачи клиента и проксирования API
4. Настройте systemd service для API (см. DEPLOYMENT.md)
"@

Set-Content -Path "$outputPath\README.txt" -Value $deployInstructions

Write-Host "`n=== Сборка завершена успешно! ===" -ForegroundColor Green
Write-Host "Файлы готовы к развертыванию в папке: $PSScriptRoot\$outputPath" -ForegroundColor Cyan
Write-Host "API Base URL: $ApiBaseUrl" -ForegroundColor Cyan

Set-Location $PSScriptRoot


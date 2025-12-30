#!/bin/bash
# Скрипт для сборки и подготовки к развертыванию на Linux
# Использование: ./build-and-deploy.sh https://your-domain.com/api

set -e

API_BASE_URL=${1:-"https://localhost:7203"}
CONFIGURATION=${2:-"Release"}

echo "=== Сборка ZasNet приложения ==="

# Пути к проектам (настройте под вашу систему)
API_PATH="/path/to/ZasNet/ZasNet.WebApi"
CLIENT_PATH="/path/to/ZasNetWebClient/ZasNetWebClient"
OUTPUT_PATH="./deploy"

# Создание выходной директории
rm -rf "$OUTPUT_PATH"
mkdir -p "$OUTPUT_PATH/api"
mkdir -p "$OUTPUT_PATH/client"

echo ""
echo "1. Сборка API..."
cd "$API_PATH"
dotnet publish -c "$CONFIGURATION" -o "$OUTPUT_PATH/api" --no-self-contained

if [ $? -ne 0 ]; then
    echo "Ошибка при сборке API!"
    exit 1
fi

echo ""
echo "2. Сборка клиента..."
cd "$CLIENT_PATH"

# Установка переменной окружения для API URL
export ApiBaseUrl="$API_BASE_URL"
dotnet publish -c "$CONFIGURATION" -o "$OUTPUT_PATH/client-temp"

if [ $? -ne 0 ]; then
    echo "Ошибка при сборке клиента!"
    exit 1
fi

# Копирование только wwwroot из клиента
echo ""
echo "3. Копирование файлов клиента..."
cp -r "$OUTPUT_PATH/client-temp/wwwroot/"* "$OUTPUT_PATH/client/"

# Обновление appsettings.json для клиента с API URL
CLIENT_APPSETTINGS="$OUTPUT_PATH/client/appsettings.json"
if [ -f "$CLIENT_APPSETTINGS" ]; then
    echo "{\"ApiBaseUrl\": \"$API_BASE_URL\"}" > "$CLIENT_APPSETTINGS"
else
    echo "{\"ApiBaseUrl\": \"$API_BASE_URL\"}" > "$CLIENT_APPSETTINGS"
fi

# Добавление/обновление скрипта в index.html для установки API URL
INDEX_HTML="$OUTPUT_PATH/client/index.html"
if [ -f "$INDEX_HTML" ]; then
    # Заменяем существующий API_BASE_URL или добавляем новый
    if grep -q "window.API_BASE_URL" "$INDEX_HTML"; then
        sed -i "s|window\.API_BASE_URL\s*=.*|window.API_BASE_URL = '$API_BASE_URL';|" "$INDEX_HTML"
    else
        SCRIPT_TAG="<script>window.API_BASE_URL = window.API_BASE_URL || '$API_BASE_URL';</script>"
        sed -i "s|</head>|$SCRIPT_TAG\n</head>|" "$INDEX_HTML"
    fi
fi

# Удаление временной папки
rm -rf "$OUTPUT_PATH/client-temp"

echo ""
echo "4. Создание инструкций по развертыванию..."
cat > "$OUTPUT_PATH/README.txt" << EOF
# Инструкции по развертыванию

## API
Скопируйте содержимое папки 'api' на сервер в директорию /opt/zasnet/api

## Клиент
Скопируйте содержимое папки 'client' на сервер в директорию /var/www/zasnet/client

## Настройка API URL
API Base URL установлен: $API_BASE_URL

## Следующие шаги
1. Настройте appsettings.json для API с правильными строками подключения
2. Примените миграции базы данных
3. Настройте Nginx для раздачи клиента и проксирования API
4. Настройте systemd service для API (см. DEPLOYMENT.md)
EOF

echo ""
echo "=== Сборка завершена успешно! ==="
echo "Файлы готовы к развертыванию в папке: $OUTPUT_PATH"
echo "API Base URL: $API_BASE_URL"


# Инструкция по развертыванию ZasNet

Это руководство описывает процесс развертывания приложения ZasNet на хостинге.

## Структура проекта

- **ZasNetWebClient** - Blazor WebAssembly клиентское приложение
- **ZasNet.WebApi** - ASP.NET Core Web API серверное приложение
- **База данных** - PostgreSQL

## Требования

- .NET 8.0 SDK
- PostgreSQL 12+
- Docker и Docker Compose (опционально, для упрощения развертывания)
- Веб-сервер (Nginx, IIS, или другой) для раздачи статических файлов клиента

## Варианты развертывания

### Вариант 1: Развертывание с Docker Compose (Рекомендуется)

Этот вариант упрощает развертывание и управление всеми компонентами.

#### Шаг 1: Подготовка файлов

1. Скопируйте все файлы проекта на сервер:
   - Скопируйте папку `ZasNet` (серверный проект) на сервер
   - Скопируйте файлы `docker-compose.yml`, `.env.example` на сервер
2. Убедитесь, что у вас есть доступ к серверу по SSH
3. На сервере создайте структуру папок:
   ```bash
   mkdir -p /opt/zasnet
   cd /opt/zasnet
   ```

#### Шаг 2: Настройка переменных окружения

На сервере создайте файл `.env` в корне проекта (рядом с docker-compose.yml):

```env
# База данных
POSTGRES_DB=ZasNet
POSTGRES_USER=zasnet_user
POSTGRES_PASSWORD=your_secure_password_here

# API настройки
API_PORT=8080
API_ASPNETCORE_ENVIRONMENT=Production

# JWT Secret Key (используйте надежный ключ!)
JWT_SECRET_KEY=your_super_secret_key_here_min_32_chars

# Telegram Bot (опционально)
TELEGRAM_BOT_TOKEN=your_telegram_bot_token
TELEGRAM_CHANNEL_ID=your_channel_id
TELEGRAM_WEBHOOK_SECRET=your_webhook_secret

# API Base URL (для клиента)
API_BASE_URL=https://your-domain.com/api
```

#### Шаг 3: Настройка appsettings.json для API

Отредактируйте `ZasNet.WebApi/appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "sqlConnection": "host=postgres; Port=5432; Database=ZasNet;Username=zasnet_user;Password=your_secure_password_here"
  },
  "AllowedHosts": "*",
  "AuthSettings": {
    "secretKey": "your_super_secret_key_here_min_32_chars"
  },
  "TelegramSettings": {
    "BotToken": "your_telegram_bot_token",
    "ChannelId": "your_channel_id",
    "WebhookSecret": "your_webhook_secret",
    "IsEnabled": true,
    "ManagerUserIds": []
  }
}
```

#### Шаг 4: Сборка и запуск

```bash
# Сборка и запуск всех сервисов
docker-compose up -d --build

# Просмотр логов
docker-compose logs -f

# Остановка
docker-compose down
```

#### Шаг 5: Применение миграций базы данных

```bash
# Выполните миграции базы данных
docker-compose exec api dotnet ef database update --project ZasNet.Infrastruture
```

#### Шаг 6: Настройка Nginx (для раздачи клиента)

Создайте конфигурацию Nginx `/etc/nginx/sites-available/zasnet`:

```nginx
server {
    listen 80;
    server_name your-domain.com;

    # Редирект на HTTPS (опционально)
    # return 301 https://$server_name$request_uri;

    # Раздача статических файлов клиента
    location / {
        root /var/www/zasnet/client;
        try_files $uri $uri/ /index.html;
    }

    # Проксирование API запросов
    location /api {
        proxy_pass http://localhost:8080;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_cache_bypass $http_upgrade;
    }
}
```

Активируйте конфигурацию:

```bash
sudo ln -s /etc/nginx/sites-available/zasnet /etc/nginx/sites-enabled/
sudo nginx -t
sudo systemctl reload nginx
```

### Вариант 2: Ручное развертывание без Docker

#### Шаг 1: Установка PostgreSQL

```bash
# Ubuntu/Debian
sudo apt update
sudo apt install postgresql postgresql-contrib

# Создание базы данных
sudo -u postgres psql
CREATE DATABASE ZasNet;
CREATE USER zasnet_user WITH PASSWORD 'your_secure_password';
GRANT ALL PRIVILEGES ON DATABASE ZasNet TO zasnet_user;
\q
```

#### Шаг 2: Сборка и публикация API

```bash
cd C:\Users\Rustem\source\repos\ZasNet\ZasNet.WebApi
dotnet publish -c Release -o ./publish

# Скопируйте папку publish на сервер
```

#### Шаг 3: Настройка API на сервере

1. Создайте файл `appsettings.Production.json`:

```json
{
  "ConnectionStrings": {
    "sqlConnection": "host=localhost; Port=5432; Database=ZasNet;Username=zasnet_user;Password=your_secure_password"
  },
  "AuthSettings": {
    "secretKey": "your_super_secret_key_here_min_32_chars"
  },
  "TelegramSettings": {
    "BotToken": "your_telegram_bot_token",
    "ChannelId": "your_channel_id",
    "WebhookSecret": "your_webhook_secret",
    "IsEnabled": true
  }
}
```

2. Примените миграции:

```bash
cd /path/to/published/api
dotnet ef database update --project /path/to/ZasNet.Infrastruture
```

3. Создайте systemd service `/etc/systemd/system/zasnet-api.service`:

```ini
[Unit]
Description=ZasNet Web API
After=network.target postgresql.service

[Service]
Type=notify
WorkingDirectory=/opt/zasnet/api
ExecStart=/usr/bin/dotnet /opt/zasnet/api/ZasNet.WebApi.dll
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=zasnet-api
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://localhost:8080

[Install]
WantedBy=multi-user.target
```

4. Запустите сервис:

```bash
sudo systemctl daemon-reload
sudo systemctl enable zasnet-api
sudo systemctl start zasnet-api
sudo systemctl status zasnet-api
```

#### Шаг 4: Сборка и публикация клиента

```bash
cd C:\Users\Rustem\source\repos\ZasNetWebClient\ZasNetWebClient

# Создайте appsettings.json в wwwroot
# Или установите переменную окружения при сборке
$env:ApiBaseUrl="https://your-domain.com/api"
dotnet publish -c Release -o ./publish

# Скопируйте содержимое папки publish/wwwroot на сервер в /var/www/zasnet/client
```

#### Шаг 5: Настройка Nginx

См. конфигурацию Nginx в Варианте 1, Шаг 6.

## Настройка HTTPS (SSL/TLS)

### Использование Let's Encrypt (Certbot)

```bash
# Установка Certbot
sudo apt install certbot python3-certbot-nginx

# Получение сертификата
sudo certbot --nginx -d your-domain.com

# Автоматическое обновление
sudo certbot renew --dry-run
```

Обновите конфигурацию Nginx для использования HTTPS:

```nginx
server {
    listen 443 ssl http2;
    server_name your-domain.com;

    ssl_certificate /etc/letsencrypt/live/your-domain.com/fullchain.pem;
    ssl_certificate_key /etc/letsencrypt/live/your-domain.com/privkey.pem;

    # ... остальная конфигурация
}

server {
    listen 80;
    server_name your-domain.com;
    return 301 https://$server_name$request_uri;
}
```

## Настройка Telegram Webhook

Если вы используете Telegram бота, настройте webhook:

```bash
TOKEN="your_bot_token"
PUBLIC_URL="https://your-domain.com/api/telegram/update"
SECRET="your_webhook_secret"

curl -X POST "https://api.telegram.org/bot$TOKEN/setWebhook" \
  -d "url=$PUBLIC_URL" \
  -d "secret_token=$SECRET"

# Проверка webhook
curl "https://api.telegram.org/bot$TOKEN/getWebhookInfo"
```

## Мониторинг и логи

### Просмотр логов API

```bash
# Docker
docker-compose logs -f api

# Systemd
sudo journalctl -u zasnet-api -f

# Nginx
sudo tail -f /var/log/nginx/access.log
sudo tail -f /var/log/nginx/error.log
```

## Резервное копирование базы данных

```bash
# Создание бэкапа
docker-compose exec postgres pg_dump -U zasnet_user ZasNet > backup_$(date +%Y%m%d_%H%M%S).sql

# Восстановление
docker-compose exec -T postgres psql -U zasnet_user ZasNet < backup.sql
```

## Обновление приложения

### С Docker Compose

```bash
# Остановка
docker-compose down

# Обновление кода
git pull  # или скопируйте новые файлы

# Пересборка и запуск
docker-compose up -d --build

# Применение миграций (если есть)
docker-compose exec api dotnet ef database update --project ZasNet.Infrastruture
```

### Без Docker

```bash
# Остановка API
sudo systemctl stop zasnet-api

# Обновление кода и пересборка
# ... (см. шаги сборки выше)

# Запуск API
sudo systemctl start zasnet-api

# Обновление клиента
# Скопируйте новые файлы в /var/www/zasnet/client
```

## Устранение неполадок

### API не запускается

1. Проверьте логи: `sudo journalctl -u zasnet-api -n 50`
2. Проверьте подключение к базе данных
3. Проверьте порты: `sudo netstat -tulpn | grep 8080`
4. Проверьте права доступа к файлам

### Клиент не подключается к API

1. Проверьте CORS настройки в API
2. Проверьте, что API доступен: `curl http://localhost:8080/api/v1/health`
3. Проверьте конфигурацию Nginx прокси
4. Проверьте консоль браузера на ошибки CORS

### Проблемы с базой данных

1. Проверьте подключение: `psql -h localhost -U zasnet_user -d ZasNet`
2. Проверьте логи PostgreSQL: `sudo tail -f /var/log/postgresql/postgresql-*.log`
3. Убедитесь, что миграции применены

## Безопасность

1. **Измените все пароли по умолчанию**
2. **Используйте надежные секретные ключи** (минимум 32 символа)
3. **Настройте файрвол**:
   ```bash
   sudo ufw allow 22/tcp
   sudo ufw allow 80/tcp
   sudo ufw allow 443/tcp
   sudo ufw enable
   ```
4. **Регулярно обновляйте систему и зависимости**
5. **Используйте HTTPS** для всех соединений
6. **Ограничьте доступ к базе данных** только с localhost
7. **Настройте регулярные бэкапы**

## Производительность

1. **Включите кэширование** в Nginx для статических файлов
2. **Настройте gzip сжатие** в Nginx
3. **Используйте CDN** для статических ресурсов (опционально)
4. **Настройте connection pooling** для базы данных
5. **Мониторьте использование ресурсов**: `htop`, `docker stats`

## Контакты и поддержка

При возникновении проблем проверьте логи и документацию. Для дополнительной помощи обратитесь к разработчику.


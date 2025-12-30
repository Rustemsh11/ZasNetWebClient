# Быстрое руководство по развертыванию ZasNet

## Что нужно для развертывания

1. **Сервер** с установленным:
   - .NET 8.0 Runtime (для API)
   - PostgreSQL 12+
   - Nginx (для раздачи клиента)
   - Docker и Docker Compose (опционально, но рекомендуется)

2. **Домен** с настроенным DNS (опционально, можно использовать IP)

## Быстрый старт с Docker Compose

### 1. Подготовка

```bash
# Скопируйте все файлы проекта на сервер
# Создайте файл .env на основе .env.example
cp .env.example .env
nano .env  # Отредактируйте значения
```

### 2. Настройка .env файла

Обязательно измените:
- `POSTGRES_PASSWORD` - надежный пароль для БД
- `JWT_SECRET_KEY` - секретный ключ минимум 32 символа
- `API_BASE_URL` - URL вашего API (например: `https://your-domain.com/api`)

### 3. Запуск

```bash
# Сборка и запуск
docker-compose up -d --build

# Применение миграций БД
docker-compose exec api dotnet ef database update --project ZasNet.Infrastruture

# Просмотр логов
docker-compose logs -f
```

### 4. Настройка Nginx

```bash
# Скопируйте пример конфигурации
sudo cp nginx.conf.example /etc/nginx/sites-available/zasnet

# Отредактируйте домен
sudo nano /etc/nginx/sites-available/zasnet

# Активируйте
sudo ln -s /etc/nginx/sites-available/zasnet /etc/nginx/sites-enabled/
sudo nginx -t
sudo systemctl reload nginx
```

### 5. Развертывание клиента

```bash
# На Windows используйте PowerShell скрипт
.\build-and-deploy.ps1 -ApiBaseUrl "https://your-domain.com/api"

# На Linux используйте bash скрипт
./build-and-deploy.sh https://your-domain.com/api

# Скопируйте содержимое папки deploy/client на сервер
scp -r deploy/client/* user@server:/var/www/zasnet/client/
```

## Ручное развертывание (без Docker)

См. подробную инструкцию в файле `DEPLOYMENT.md`

## Важные моменты

1. **Безопасность**:
   - Измените все пароли по умолчанию
   - Используйте HTTPS в продакшене
   - Настройте файрвол

2. **База данных**:
   - Создайте резервные копии регулярно
   - Примените миграции после обновления

3. **Мониторинг**:
   - Проверяйте логи регулярно
   - Настройте мониторинг дискового пространства

## Обновление приложения

```bash
# Остановка
docker-compose down

# Обновление кода (git pull или копирование файлов)

# Пересборка
docker-compose up -d --build

# Применение миграций (если есть новые)
docker-compose exec api dotnet ef database update --project ZasNet.Infrastruture
```

## Устранение проблем

### API не запускается
```bash
# Проверьте логи
docker-compose logs api

# Проверьте подключение к БД
docker-compose exec postgres psql -U zasnet_user -d ZasNet
```

### Клиент не подключается к API
- Проверьте `API_BASE_URL` в appsettings.json клиента
- Проверьте CORS настройки в API
- Проверьте конфигурацию Nginx прокси

### Проблемы с базой данных
```bash
# Проверьте статус PostgreSQL
docker-compose ps postgres

# Проверьте логи
docker-compose logs postgres
```

## Полная документация

Подробная инструкция доступна в файле `DEPLOYMENT.md`


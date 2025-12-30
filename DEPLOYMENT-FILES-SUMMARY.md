# Сводка файлов для развертывания

## Созданные файлы

### Документация
1. **DEPLOYMENT.md** - Полная подробная инструкция по развертыванию
2. **README-DEPLOY.md** - Краткое руководство для быстрого старта
3. **DEPLOYMENT-FILES-SUMMARY.md** - Этот файл

### Конфигурация Docker
4. **docker-compose.yml** - Конфигурация для развертывания с Docker
5. **.env.example** - Пример файла переменных окружения

### Скрипты сборки
6. **build-and-deploy.ps1** - PowerShell скрипт для сборки на Windows
7. **build-and-deploy.sh** - Bash скрипт для сборки на Linux

### Конфигурация веб-сервера
8. **nginx.conf.example** - Пример конфигурации Nginx

### Измененные файлы проекта
9. **ZasNetWebClient/Program.cs** - Обновлен для использования переменной окружения ApiBaseUrl
10. **ZasNetWebClient/wwwroot/index.html** - Добавлен скрипт для установки API_BASE_URL
11. **ZasNetWebClient/wwwroot/appsettings.json** - Создан файл конфигурации для API URL

## Как использовать

### Для развертывания с Docker:

1. Скопируйте на сервер:
   - Папку `ZasNet` (весь серверный проект)
   - Файлы `docker-compose.yml`, `.env.example`
   - Папку `ZasNetWebClient` (клиентский проект)

2. На сервере:
   ```bash
   # Создайте .env из примера
   cp .env.example .env
   nano .env  # Отредактируйте значения
   
   # Запустите
   docker-compose up -d --build
   ```

3. Соберите клиент:
   ```powershell
   # На Windows
   .\build-and-deploy.ps1 -ApiBaseUrl "https://your-domain.com/api"
   ```

4. Скопируйте клиент на сервер:
   ```bash
   scp -r deploy/client/* user@server:/var/www/zasnet/client/
   ```

5. Настройте Nginx (см. nginx.conf.example)

### Для ручного развертывания:

Следуйте инструкциям в `DEPLOYMENT.md`, раздел "Вариант 2: Ручное развертывание без Docker"

## Важные замечания

1. **Пути в скриптах**: Скрипты сборки используют абсолютные пути Windows. 
   При использовании на Linux измените пути в скриптах на соответствующие.

2. **API Base URL**: Убедитесь, что API Base URL указывает на правильный адрес вашего сервера.

3. **Безопасность**: 
   - Измените все пароли по умолчанию
   - Используйте надежные секретные ключи
   - Настройте HTTPS

4. **База данных**: Не забудьте применить миграции после первого запуска:
   ```bash
   docker-compose exec api dotnet ef database update --project ZasNet.Infrastruture
   ```

## Следующие шаги

1. Прочитайте `README-DEPLOY.md` для быстрого старта
2. Изучите `DEPLOYMENT.md` для подробной информации
3. Настройте переменные окружения в `.env`
4. Соберите и разверните приложение


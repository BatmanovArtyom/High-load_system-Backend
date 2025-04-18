# RateLimiter System

Высоконагруженная система для управления ограничением скорости запросов (rate limiting) и пользователями, построенная на микросервисной архитектуре.

## Компоненты системы

### 1. RateLimiter.Writer
- **Назначение**: CRUD-операции с правилами ограничений
- **Технологии**: 
  - gRPC сервер
  - MongoDB (хранение правил)
  - Логирование изменений через Change Streams
- **Функции**:
  - Создание/обновление/удаление лимитов
  - Валидация и консистентность данных

### 2. RateLimiter.Reader
- **Назначение**: Применение ограничений и мониторинг
- **Технологии**:
  - In-memory кэш (ConcurrentDictionary)
  - Фоновая загрузка данных батчами
  - Репликация изменений из MongoDB в реальном времени
- **Функции**:
  - Проверка лимитов в режиме реального времени
  - Синхронизация с Writer через MongoDB Change Streams

### 3. UserService
- **Назначение**: Управление пользователями и событиями
- **Технологии**:
  - PostgreSQL (хранение пользователей)
  - Kafka (обработка событий)
  - FluentValidation
- **Функции**:
  - Регистрация/управление пользователями
  - Планировщик событий с RPM-контролем
  - Валидация данных

### Требования
- Docker
- .NET 8 SDK
- MongoDB 6+
- PostgreSQL 15
- Kafka 3.3

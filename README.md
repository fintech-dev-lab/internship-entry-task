# REST API для игры в крестики-нолики NxN

## Стек технологий

- **ASP.NET Core 9 Web API**
- **SQL Server 2019** (в Docker)
- **Entity Framework Core**
- **Docker + Docker Compose**
- **AutoMapper, xUnit, Moq**
- **C# 12 / .NET 9**


## Как собрать и запустить проект

```bash
docker-compose up --build -d
curl http://localhost:8080/health   # должен вернуть 200 OK
dotnet test                          # запуск всех модульных и интеграционных тестов
````

### Порты

| Сервис | Адрес                                          |
| ------ | ---------------------------------------------- |
| API    | [http://localhost:8080](http://localhost:8080) |
| БД     | localhost:1433 (sa/q7\_E+3OjIi)                |

---

## Архитектура

Решение разбито по слоям и проектам:

```
TicTacToe/
├── TicTacToe.Api           # Входная точка API
├── TicTacToe.Contracts     # DTO и запросы/ответы API
├── TicTacToe.Core          # Сущности и интерфейсы (Domain Model)
├── TicTacToe.Services      # Сервисы и репозитории
├── TicTacToe.UnitTests     # Юнит-тесты (с Moq)
├── TicTacToe.IntegrationTests # Интеграционные тесты (с InMemory)
└── docker-compose.yml      # Docker-сборка всего проекта
```

Приняты следующие **архитектурные решения**:

* **Слои разделены**: нет зависимости от Web API в бизнес-логике.
* **Entity Framework Core** используется с миграциями.
* **Сериализация поля Board** реализована через `string[][]` + `BoardJson`.
* **Настройки игры (размер поля, длина победной линии)** передаются через `IOptions<GameSettings>`.


## API Описание

| Метод  | Путь             | Описание                |
| ------ | ---------------- | ----------------------- |
| `POST` | `/api/Game`      | Создать новую игру      |
| `POST` | `/api/Game/MakeMove` | Совершить ход           |
| `GET`  | `/api/Game/{uuid}` | Получить состояние игры |


## Переменные окружения

* `GameSettings__BoardSize` — размер поля
* `GameSettings__WinLength` — длина победной линии
* `ConnectionStrings__DefaultConnection` — строка подключения к SQL Server

---

## Тестирование

* **Unit-тесты** покрывают бизнес-логику в сервисах.
* **Integration-тесты** проверяют поведение репозиториев и базу.
* Запуск: `dotnet test`


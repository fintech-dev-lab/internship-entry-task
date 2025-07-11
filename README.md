Реализация REST API для игры "Крестики-нолики NxN" (N >= 3) для двух игроков.
Особенность игры — каждый третий ход с вероятностью 10% может быть заменён на символ противника.

Что реализовано:
 Инициализация новой игры через REST API

 Совершение ходов игроками

 Фиксация победы или ничьей

 Параметры поля и победы настраиваются через переменные окружения

 REST API принимает/возвращает JSON

 Приложение запускается в Docker с PostgreSQL через docker-compose

 Swagger доступен по адресу /swagger

 /health возвращает 200 OK при запуске

 Что не реализовано:
 Идемпотентность хода по POST /moves

 Юнит- и интеграционные тесты

 CI-сборка

 Обработка невалидного JSON по RFC 7807

Запуск проекта
Требования: Docker, docker-compose, .NET 9 SDK (если запускать локально)

docker-compose up --build

API слушает: http://localhost:8080

Swagger: http://localhost:8080/swagger
Переменные окружения
Настраиваются через docker-compose.yml:

переменные окружения:
GameSettings__BoardSize=3
GameSettings__WinLength=3

Архитектура:
ASP.NET Core 9 Web API (minimal hosting model)

PostgreSQL как основное хранилище

EF Core для доступа к БД

Разделение на Application, Infrastructure, Domain

HealthCheck через /health

Можно было добавить (но не захотел):
Fluent validation
automapper
Logger
Configurations (для бд)
Dto-шки для геймсервиса
midleware

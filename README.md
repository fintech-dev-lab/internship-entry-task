# TicTacToe
#### Docker compose run
Для запуска проекта, запустить команду в директории с файлом docker-compose.yml
```
docker-compose up 
```
#### Swagger API
Документация по API доступна по адрессу `http://localhost:8080/swagger/index.html`  
Или по адресу `http://localhost:5093/swagger/index.html` в режиме разработки

#### Использованые технологии
- Entity Framework Core
- PostgreSQL
- FluentValidation
- AutoMapper
- Swagger 
- xUnit 

#### Тесты
В проекте имеются unit и integration, которые проверяют логику работы GameControllera и TicTacToe утилиты

#### Дополнительные тонкости
- **Concurrency & идемпотентность**
   API реализует Concurrency & идемпотентность для запроса `POST /moves`

- **Persistence crash-safe**
   После перезапуска приложения сессия игроков сохряются

- **Негативные тесты**
   Все данные приходящие с фронтенда валидируются с помощью библиотеку FluentValidation

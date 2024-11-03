# Hospital API

Этот проект представляет собой REST API для управления записями пациентов (детей, рожденных в роддоме). Реализованы CRUD-методы для сущности `Patient` и возможности поиска по параметрам, соответствующие стандартам HL7 FHIR.

## Стек технологий
- **.NET 6**
- **Entity Framework Core**
- **Любая поддерживаемая SQL СУБД** (в примере используется SQL Server)
- **Docker**

## Функциональные возможности

### 1. Сущность `Patient`
Сущность `Patient` включает поля:
- `name` (содержит вложенные поля `use`, `family`, `given`)
- `gender` (возможные значения: `male`, `female`, `other`, `unknown`)
- `birthDate` (дата рождения)
- `active` (возможные значения: `true`, `false`)

#### Пример `Patient` в формате JSON:

```json
{
    "name": {
        "id": "d8ff176f-bd0a-4b8e-b329-871952e32e1f",
        "use": "official",
        "family": "Иванов",
        "given": ["Иван", "Иванович"]
    },
    "gender": "male",
    "birthDate": "2024-01-13T18:25:43",
    "active": true
}
```
Обязательные поля:
- `name.family`
- `birthDate`

### 2. CRUD-методы
Реализованы следующие операции для управления записями пациентов:

- Создание (`POST /api/patients`)
- Чтение (`GET /api/patients/{id}`)
- Обновление (`PUT /api/patients/{id}`)
- Удаление (`DELETE /api/patients/{id}`)

### 3. Поиск по birthDate
API поддерживает поиск записей по дате рождения с использованием параметра birthDate, соответствующий [FHIR](https://www.hl7.org/fhir/search.html#date "FHIR")-спецификации поиска по дате. Поддерживаются различные операторы для поиска, такие как `eq`, `ne`, `gt`, `lt`, `ge`, `le`, `sa`, `eb`, `ap`.

### 4. Swagger UI
Для удобства тестирования и документирования методов API в проекте подключен Swagger. Описание методов доступно по адресу `/swagger`.

### 5. Консольное приложение
Создано консольное приложение для добавления 100 сгенерированных объектов `Patient` через вызов API.

###  6. Docker-контейнеризация
Проект настроен для работы в контейнерах Docker. Включены:

- API контейнер для запуска Hospital API
- SQL Server контейнер для базы данных

**Примечание: На текущий момент в Docker-конфигурации не реализована связь между API и SQL Server из-за ошибки:**

------------

```vbnet
Microsoft.Data.SqlClient.SqlException: 'A network-related or instance-specific error occurred while establishing a connection to SQL Server. The server was not found or was not accessible. Verify that the instance name is correct and that SQL Server is configured to allow remote connections. (provider: Named Pipes Provider, error: 40 - Could not open a connection to SQL Server)'
```
**Связь между контейнерами API и SQL Server требует дополнительной настройки.**

## Postman Коллекция
Для демонстрации возможностей API создана Postman-коллекция, включающая:

- Добавление пациента
- Редактирование данных пациента
- Получение пациента по идентификатору
- Удаление пациента
- Поиск пациентов по дате рождения с различными параметрами

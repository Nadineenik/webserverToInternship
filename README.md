# UserManagementAPI
Web API на .NET 9 для CRUD-операций с пользователями (Users) с поддержкой Swagger.

---

## Содержание

1. [Установка и запуск](#installation)
2. [API эндпоинты](#endpoints)

   * Создание пользователя (Create)
   * Обновление профиля (Update Profile)
   * Смена пароля (Update Password)
   * Смена логина (Update Login)
   * Список активных пользователей (Get All Active)
   * Получить пользователя по логину (Get By Login)
   * Аутентификация (Authenticate)
   * Получить по возрасту (Get Older Than)
   * Удаление пользователя (Revoke/Delete)
   * Восстановление пользователя (Restore)
3. [Примеры запросов](#examples)
4. [Требования](#requirements)
5. [Лицензия](#license)

---

## <a name="installation"></a>1. Установка и запуск

```bash
# Клонировать репо
git clone https://github.com/Nadineenik/webserverToInternship
cd UserManagementAPI

# Установить зависимости
dotnet restore

# Запустить службу
dotnet run
```

Swagger UI будет доступен по адресу:

```
http://localhost:5088/swagger/index.html
```

---

## <a name="endpoints"></a>2. API эндпоинты

Все запросы требуют заголовок `X-User: {login_сессии}` для авторизации.

### 2.1 Create User

**POST** `/api/users`

* **Headers**:

  * `X-User`: admin
* **Body (JSON)**:

  ```json
  ```

{ "login": "string", "password": "string", "name": "string", "gender": 0|1|2, "birthday": "YYYY-MM-DD", "admin": true|false }

````

### 2.2 Update Profile  
**PUT** `/api/users/profile/{login}`
- **Headers**: `X-User`
- **Body**: `{ "name": "string", "gender": 0|1|2, "birthday": "YYYY-MM-DD" }`

### 2.3 Update Password  
**PUT** `/api/users/password/{login}`
- **Headers**: `X-User`
- **Body**: строка нового пароля

### 2.4 Update Login  
**PUT** `/api/users/login/{oldLogin}`
- **Headers**: `X-User`
- **Body**: строка нового логина

### 2.5 Get All Active  
**GET** `/api/users`  (Admin only)

### 2.6 Get By Login  
**GET** `/api/users/{login}`  (Admin only)

### 2.7 Authenticate  
**POST** `/api/users/auth`
- **Body**: `{ "login": "string", "password": "string" }`

### 2.8 Get Older Than  
**GET** `/api/users/olderThan/{age}`  (Admin only)

### 2.9 Revoke/Delete
**DELETE** `/api/users/{login}?hard=true|false`  (Admin only)

### 2.10 Restore
**POST** `/api/users/restore/{login}`  (Admin only)

---

## <a name="examples"></a>3. Примеры запросов

```bash
# Создать пользователя
curl -H "X-User: admin" -H "Content-Type: application/json" -d '{ "login": "u1", "password": "p1", "admin": false }' http://localhost:5088/api/users
````

```bash
# Получить всех активных
curl -H "X-User: admin" http://localhost:5088/api/users
```

---

## <a name="requirements"></a>4. Требования

* .NET 9 SDK
* Swashbuckle.AspNetCore

---

## <a name="license"></a>5. Лицензия

MIT © Надежда Н.

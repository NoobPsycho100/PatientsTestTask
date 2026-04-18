Запустить можно командой из корня проекта:

```bat
docker-compose up
```

При этом
1. будет собрана схема PatientsTestTask.DB.sqlproj в dacpac
2. будет запущен MS SQL Server 2025
3. из dacpac будет создана пустая база с именем PatientsTestTaskDB, при этом на сервере будет создан логин PatientsTestTaskUser / IRe4llYH4teStr0ng!estPassw0rd$ (к серверу можно подключиться из локального SSMS: host = localhost,1433 логин = sa / IH4teStr0ng!estPassw0rd$   )
4. будет запущен .net web api сервис из PatientsTestTask.Web.csproj

После запуска swagger будет доступен по http://localhost:5000/swagger/index.html

Коллекция для Postman - в файле `Patients test task api.postman_collection.json`

Проект PatientsTestTask.Utility.App.csproj позволяет добавить 100 случайных пациента.

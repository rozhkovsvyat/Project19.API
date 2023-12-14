# Project 19 : Phonebook API

<img align="right" width="100" height="100" src="https://github.com/rozhkovsvyat/Project19.API/assets/71471748/004210d4-8df8-4e8f-974c-c5dbcb5a0a18">
<img align="right" width="100" height="100" src="https://github.com/rozhkovsvyat/Project19.API/assets/71471748/4acc1bb8-d45b-44fe-9146-0eaf5c698709">

**#net7.0.10-aspnetcore**

API телефонной книги

> :link: [Использует общие библиотеки](https://github.com/rozhkovsvyat/Project19.Libs)

Предоставляет разграниченный доступ к коллекции контактов:
* **Администратор** -- любые операции над контактами и учетными записями пользователей
* **Пользователь** -- запрос контакта или коллекции, добавление контакта, смена пароля учетной записи
* **Анонимус** -- только запрос коллекции

---

### SERVICES

* **Phonebook** -- поставщик контактов / [PostgreSQL](https://www.nuget.org/packages/Npgsql.EntityFrameworkCore.PostgreSQL) + [Contacts.Db](https://www.nuget.org/packages/RozhkovSvyat.Project19.Models.Contacts.Db) + [HealthChecks](https://www.nuget.org/packages/Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore) + [Initializator](https://www.nuget.org/packages/RozhkovSvyat.Project19.Services.Initializator)
* **TestPhonebook** -- тестовый поставщик контактов / [Contacts.Test](https://www.nuget.org/packages/RozhkovSvyat.Project19.Models.Contacts.Test) + [Initializator](https://www.nuget.org/packages/RozhkovSvyat.Project19.Services.Initializator)
* **PhonebookIdentity** -- идентификация / [MongoDbCore](https://www.nuget.org/packages/AspNetCore.Identity.MongoDbCore) + [JwtBearer](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer) + [Identity.Mongo](https://www.nuget.org/packages/RozhkovSvyat.Project19.Models.Identity.Mongo) + [Initializator](https://www.nuget.org/packages/RozhkovSvyat.Project19.Services.Initializator)
> :bulb: Инициализация коллекций происходит при первом запуске API
* **PhonebookSimpleInitialization** -- добавление трех контактов / [Contacts.Factory](https://www.nuget.org/packages/RozhkovSvyat.Project19.Models.Contacts.Factory)
* **PhonebookRandomInitialization** -- добавление случайного числа контактов (3, 5 или 8) / [Contacts.Factory](https://www.nuget.org/packages/RozhkovSvyat.Project19.Models.Contacts.Factory)
* **PhonebookIdentitySimpleInitialization** -- добавление пользователя и администратора / [Identity.Factory](https://www.nuget.org/packages/RozhkovSvyat.Project19.Models.Identity.Factory)
* **PhonebookIdentityBagInitialization** -- добавление учетных записей из конфигурации / [Identity.Factory](https://www.nuget.org/packages/RozhkovSvyat.Project19.Models.Identity.Factory)

---

### CONTROLLERS

* **ContactsController** ../contacts/ -- управляет коллекцией контактов

> :memo: **<sub>_type_</sub>Method<sup>(args)**</sup>*<sup>-auth/</sup>**<sup>-аdmin</sup>
>
> <sub>_g_</sub>**Ping**</sub><sup>( )</sup> / <sub>_g_</sub>**Get**<sup>( )</sup> / <sub>_g_</sub>**Get**<sup>(int)</sup>* / <sub>_p_</sub>**Post**<sup>(contact)</sup>* / <sub>_pt_</sub>**Put**<sup>(int,contact)</sup>** / <sub>_d_</sub>**Delete**<sup>(int)</sup>**

* **IdentityRoleController** ../identityrole/ -- управляет пользовательскими ролями

> <sub>_g_</sub>**Get**<sup>( )</sup>** / <sub>_g_</sub>**Get**<sup>(str)</sup>** / <sub>_p_</sub>**Post**<sup>(roleform)</sup>** / <sub>_pt_</sub>**Put**<sup>(role)</sup>** / <sub>_d_</sub>**Delete**<sup>(str)</sup>**

* **IdentityController** ../identity/ -- управляет пользовательскими аккаунтами

> <sub>_g_</sub>**Ping**<sup>( )</sup> / <sub>_p_</sub>**LogOut**<sup>( )</sup> / <sub>_p_</sub>**Post**<sup>(str,signinform)</sup> / <sub>_p_</sub>**Post**<sup>(accform)</sup> / <sub>_pt_</sub>**Password**<sup>(str,passform)</sup>* / <sub>_g_</sub>**Get**<sup>( )</sup>** / <sub>_g_</sub>**Get**<sup>(str)</sup>** / <sub>_pt_</sub>**Put**<sup>(str,acc)</sup>** / <sub>_d_</sub>**Delete**<sup>(str)</sup>** / <sub>_g_</sub>**Login**<sup>(str)</sup>** / <sub>_d_</sub>**Remove**<sup>(str)</sup>** / <sub>_g_</sub>**Exclude**<sup>(str)</sup>** / <sub>_g_</sub>**Include**<sup>(str)</sup>** / <sub>_pt_</sub>**Include**<sup>(str,str)</sup>** / <sub>_pt_</sub>**IncludeAll**<sup>(str,[str])</sup>** / <sub>_pt_</sub>**Exclude**<sup>(str,str)</sup>** / <sub>_pt_</sub>**ExcludeAll**<sup>(str,[str])</sup>**


---

💣 **401** unauthorized
💣 **404** notfound
💣 **409** conflict
💣 **500** exception

# Project 19 : Phonebook API

<img align="right" width="100" height="100" src="https://github.com/rozhkovsvyat/Project19.API/assets/71471748/004210d4-8df8-4e8f-974c-c5dbcb5a0a18">
<img align="right" width="100" height="100" src="https://github.com/rozhkovsvyat/Project19.API/assets/71471748/4acc1bb8-d45b-44fe-9146-0eaf5c698709">

**#aspnetcore7.0.10**

Web-API проекта Phonebook

> :link: [Использует общие библиотеки](https://github.com/rozhkovsvyat/Project19.Libs)

---

### SERVICES

* **Phonebook** -- сервис поставщика контактов / [PostgreSQL](https://www.nuget.org/packages/Npgsql.EntityFrameworkCore.PostgreSQL) + [Contacts.Db](https://www.nuget.org/packages/RozhkovSvyat.Project19.Models.Contacts.Db/) + [HealthChecks](https://www.nuget.org/packages/Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore) + [Initializator](https://www.nuget.org/packages/RozhkovSvyat.Project19.Services.Initializator/)
* **TestPhonebook** -- тестовый сервис поставщика контактов / [Contacts.Test](https://www.nuget.org/packages/RozhkovSvyat.Project19.Models.Contacts.Test/) + [Initializator](https://www.nuget.org/packages/RozhkovSvyat.Project19.Services.Initializator/)
* **PhonebookSimpleInitialization** -- заполнение контактов простой фабрикой / [Contacts.Factory](https://www.nuget.org/packages/RozhkovSvyat.Project19.Models.Contacts.Factory/)
* **PhonebookRandomInitialization** -- заполнение контактов фабрикой случайного числа / [Contacts.Factory](https://www.nuget.org/packages/RozhkovSvyat.Project19.Models.Contacts.Factory/)
* **PhonebookIdentity** -- сервис идентификации / [MongoDbCore](https://www.nuget.org/packages/AspNetCore.Identity.MongoDbCore/) + [JwtBearer](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer/) + [Identity.Mongo](https://www.nuget.org/packages/RozhkovSvyat.Project19.Models.Identity.Mongo/) + [Initializator](https://www.nuget.org/packages/RozhkovSvyat.Project19.Services.Initializator/)
* **PhonebookIdentitySimpleInitialization** -- заполнение аккаунтов простой фабрикой / [Identity.Factory](https://www.nuget.org/packages/RozhkovSvyat.Project19.Models.Identity.Factory/)
* **PhonebookIdentityBagInitialization** -- заполнение аккаунтов конфигурируемой фабрикой / [Identity.Factory](https://www.nuget.org/packages/RozhkovSvyat.Project19.Models.Identity.Factory/)

---

### CONTROLLERS

* **ContactsController** ../Contacts/ -- управление контактами

  
> :bulb: **<sub>_type_</sub>Method<sup>(args)**</sup>*<sup>-auth/</sup>**<sup>-аdmin</sup>
>
> ../Contacts/ <sub>_g_</sub>**Ping**</sub><sup>( )</sup> / <sub>_g_</sub>**Get**<sup>( )</sup> / <sub>_g_</sub>**Get**<sup>(int)</sup>* / <sub>_p_</sub>**Post**<sup>(contact)</sup>* / <sub>_pt_</sub>**Put**<sup>(int,contact)</sup>** / <sub>_d_</sub>**Delete**<sup>(int)</sup>**

* **IdentityRoleController** ../IdentityRole/ -- управление пользовательскими ролями

> ../IdentityRole/ <sub>_g_</sub>**Get**<sup>( )</sup>** / <sub>_g_</sub>**Get**<sup>(int)</sup>** / <sub>_p_</sub>**Post**<sup>(createroleform)</sup>** / <sub>_pt_</sub>**Put**<sup>(role)</sup>** / <sub>_d_</sub>**Delete**<sup>(str)</sup>**

* **IdentityController** ../Identity/ -- управление пользовательскими аккаунтами

> ../Identity/ <sub>_g_</sub>**Ping**<sup>( )</sup> / <sub>_p_</sub>**LogOut**<sup>( )</sup> / <sub>_p_</sub>**Post**<sup>(str,signinform)</sup> / <sub>_p_</sub>**Post**<sup>(accform)</sup> / <sub>_pt_</sub>**Password**<sup>(str,passform)</sup>* / <sub>_g_</sub>**Get**<sup>( )</sup>** / <sub>_g_</sub>**Get**<sup>(int)</sup>** / <sub>_pt_</sub>**Put**<sup>(str,acc)</sup>** / <sub>_d_</sub>**Delete**<sup>(str)</sup>** / <sub>_g_</sub>**Login**<sup>(str)</sup>** / <sub>_d_</sub>**Remove**<sup>(str)</sup>** / <sub>_g_</sub>**Exclude**<sup>(str)</sup>** / <sub>_g_</sub>**Include**<sup>(str)</sup>** / <sub>_pt_</sub>**Include**<sup>(str,str)</sup>** / <sub>_pt_</sub>**IncludeAll**<sup>(str,[str])</sup>** / <sub>_pt_</sub>**Exclude**<sup>(str,str)</sup>** / <sub>_pt_</sub>**ExcludeAll**<sup>(str,[str])</sup>**

# Project 19 : Phonebook API

<img align="right" width="100" height="100" src="https://github.com/rozhkovsvyat/Project19.API/assets/71471748/705ea0d8-cfcc-4283-ad34-a8567e31eac4">

**#aspnetcore7.0.10**

Web-API проекта Phonebook

> :link: [Использует общие библиотеки](https://github.com/rozhkovsvyat/Project19.Libs)

---

### SERVICES

* **Phonebook** -- сервис контактов / [PostgreSQL](https://www.nuget.org/packages/Npgsql.EntityFrameworkCore.PostgreSQL) + [Contacts.Db](https://www.nuget.org/packages/RozhkovSvyat.Project19.Models.Contacts.Db/) + [Initializator](https://www.nuget.org/packages/RozhkovSvyat.Project19.Services.Initializator/)
* **TestPhonebook** -- тестовый сервис контактов / [Contacts.Test](https://www.nuget.org/packages/RozhkovSvyat.Project19.Models.Contacts.Test/) + [Initializator](https://www.nuget.org/packages/RozhkovSvyat.Project19.Services.Initializator/)
* **PhonebookSimpleInitialization** -- заполнение контактов простой фабрикой / [Contacts.Factory](https://www.nuget.org/packages/RozhkovSvyat.Project19.Models.Contacts.Factory/)
* **PhonebookRandomInitialization** -- заполнение контактов фабрикой случайного числа / [Contacts.Factory](https://www.nuget.org/packages/RozhkovSvyat.Project19.Models.Contacts.Factory/)
* **PhonebookIdentity** -- сервис идентификации / [MongoDbCore](https://www.nuget.org/packages/AspNetCore.Identity.MongoDbCore/) + [JwtBearer](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer/) + [Identity.Mongo](https://www.nuget.org/packages/RozhkovSvyat.Project19.Models.Identity.Mongo/) + [Initializator](https://www.nuget.org/packages/RozhkovSvyat.Project19.Services.Initializator/)
* **PhonebookIdentitySimpleInitialization** -- заполнение аккаунтов простой фабрикой / [Identity.Factory](https://www.nuget.org/packages/RozhkovSvyat.Project19.Models.Identity.Factory/)
* **PhonebookIdentityBagInitialization** -- заполнение аккаунтов конфигурируемой фабрикой / [Identity.Factory](https://www.nuget.org/packages/RozhkovSvyat.Project19.Models.Identity.Factory/)

---

### CONTROLLERS

* **Contacts** -- <sub>_g_</sub><sup>( )</sup> / <sub>_g_</sub>**Ping**</sub><sup>( )</sup> / <sub>_[auth]g_</sub><sup>(int)</sup> / <sub>_[auth]p_</sub><sup>(contact)</sup> / <sub>_[admin]pt_</sub><sup>(int,contact)</sup> / <sub>_[admin]d_</sub><sup>(int)</sup>

* **Identity** -- 	<sub>_g_</sub>**Ping**<sup>( )</sup> / <sub>_p_</sub>**LogOut**<sup>( )</sup> / <sub>_p_</sub><sup>(str,signinform)</sup> / <sub>_p_</sub><sup>(str,createaccountform)</sup> / <sub>_[auth]pt_</sub>**Password**<sup>(str,changepasswordform)</sup> / <sub>_[admin]g_</sub><sup>( )</sup> / <sub>_[admin]g_</sub><sup>(int)</sup> / <sub>_[admin]pt_</sub><sup>(str,account)</sup> / <sub>_[admin]d_</sub><sup>(str)</sup> / <sub>_[admin]g_</sub>**Login**<sup>(str)</sup> / <sub>_[admin]d_</sub>**Remove**<sup>(str)</sup> / <sub>_[admin]g_</sub>**Exclude**<sup>(str)</sup> / <sub>_[admin]g_</sub>**Include**<sup>(str)</sup> / <sub>_[admin]pt_</sub>**Include**<sup>(str,str)</sup> / <sub>_[admin]pt_</sub>**IncludeAll**<sup>(str,[str])</sup> / <sub>_[admin]pt_</sub>**Exclude**<sup>(str,str)</sup> / <sub>_[admin]pt_</sub>**ExcludeAll**<sup>(str,[str])</sup>

* **IdentityRole** -- <sub>_[admin]g_</sub><sup>( )</sup> / <sub>_[admin]g_</sub><sup>(int)</sup> / <sub>_[admin]p_</sub><sup>(createroleform)</sup> / <sub>_[admin]pt_</sub><sup>(role)</sup> / <sub>_[admin]d_</sub><sup>(str)</sup>

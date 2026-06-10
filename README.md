# EventHub .NET MAUI projekt

Autorid: Milan Petrovski ja Igor Aleksejev  
Rühm: TARPV24  
Õppeasutus: Tallinna Tööstushariduskeskus  
Teema: EventHub - ürituste rakendus

## Projekti kirjeldus

EventHub on .NET MAUI rakendus ürituste vaatamiseks ja haldamiseks. Rakenduse eesmärk on anda kasutajale lihtne võimalus sirvida üritusi, vaadata ürituse detaile, registreeruda üritusele ning hallata oma registreeringuid.

Rakendus kasutab XAML kasutajaliidest, C# code-behind loogikat, teenuste kihti ja lokaalset SQLite andmebaasi. Andmebaas luuakse esimesel käivitamisel automaatselt koos demoandmetega.

## Projekti tehniline lahendus

Lõplik tehniline lahendus on:

- .NET MAUI;
- Windows MAUI rakendus;
- Single Project struktuur;
- XAML kasutajaliides;
- C# code-behind;
- teenuste kiht;
- lokaalne SQLite andmebaas;
- `sqlite-net-pcl` ORM;
- rollipõhine ligipääs;
- lokaalne autentimine testkontodega.

## Rollid

Rakenduses on kolm põhirolli:

### Külaline

Külaline on kasutaja, kes ei ole sisse loginud.

Külaline saab:

- vaadata ürituste nimekirja;
- filtreerida üritusi kategooria järgi;
- avada ürituse detailvaate;
- liikuda sisselogimise või registreerimise vaatesse.

### Kasutaja

Kasutaja on registreeritud ja sisse loginud kasutaja.

Kasutaja saab:

- teha kõiki külalise tegevusi;
- luua konto;
- sisse logida;
- registreeruda üritusele;
- vaadata oma registreeringuid;
- tühistada registreeringuid;
- vaadata profiili;
- välja logida.

### Haldur

Haldur on administraatori õigustega kasutaja.

Haldur saab:

- teha kasutaja tegevusi;
- avada halduri paneeli;
- lisada üritusi;
- muuta üritusi;
- kustutada üritusi;
- lisada kategooriaid;
- muuta kategooriaid;
- kustutada kategooriaid.

## Funktsionaalsus

Rakenduse põhifunktsionaalsus:

- sisselogimine;
- konto loomine;
- külalisena jätkamine;
- ürituste nimekirja kuvamine;
- kategooria järgi filtreerimine;
- ürituse detailide kuvamine;
- üritusele registreerumine;
- registreeringu tühistamine;
- kasutaja registreeringute vaatamine;
- halduri CRUD üritustele;
- halduri CRUD kategooriatele.

## Testkontod

Administraator:

```text
E-post: admin@eventhub.ee
Parool: admin123
```

Tavakasutaja:

```text
E-post: user@eventhub.ee
Parool: user123
```

## Andmebaas

Rakendus kasutab lokaalset SQLite andmebaasi.

Põhitabelid:

- `AppUser`
- `EventCategory`
- `EventItem`
- `EventRegistration`

### AppUser

Kasutajate tabel.

Olulisemad väljad:

- `UserId` - primaarvõti;
- `FullName` - kasutaja nimi;
- `Email` - kasutaja e-post;
- `PasswordHash` - parooli hash;
- `Role` - kasutaja roll;
- `CreatedAt` - konto loomise aeg.

### EventCategory

Ürituste kategooriate tabel.

Olulisemad väljad:

- `CategoryId` - primaarvõti;
- `Name` - kategooria nimi;
- `Description` - kategooria kirjeldus.

### EventItem

Ürituste tabel.

Olulisemad väljad:

- `EventId` - primaarvõti;
- `CategoryId` - võõrvõti kategooriale;
- `Title` - ürituse pealkiri;
- `Description` - ürituse kirjeldus;
- `EventDate` - ürituse kuupäev ja kellaaeg;
- `Location` - asukoht;
- `ImageUrl` - pildi URL;
- `MaxParticipants` - maksimaalne osalejate arv;
- `IsPublished` - avaldamise staatus.

### EventRegistration

Registreeringute tabel.

Olulisemad väljad:

- `RegistrationId` - primaarvõti;
- `UserId` - võõrvõti kasutajale;
- `EventId` - võõrvõti üritusele;
- `RegisteredAt` - registreerimise aeg.

## Projekti struktuur

Tüüpiline projekti struktuur:

```text
EventHubMaui
│
├── Models
│   ├── AppUser.cs
│   ├── EventCategory.cs
│   ├── EventItem.cs
│   ├── EventRegistration.cs
│   ├── EventCard.cs
│   └── UserRole.cs
│
├── Services
│   ├── DatabaseService.cs
│   ├── AuthService.cs
│   ├── EventService.cs
│   └── PasswordHelper.cs
│
├── Pages
│   ├── WelcomePage.xaml
│   ├── EventsPage.xaml
│   ├── EventDetailsPage.xaml
│   ├── MyRegistrationsPage.xaml
│   ├── ProfilePage.xaml
│   ├── AdminDashboardPage.xaml
│   ├── AdminEventEditPage.xaml
│   └── AdminCategoriesPage.xaml
│
├── Platforms
├── Resources
├── App.xaml
├── App.xaml.cs
├── MauiProgram.cs
└── EventHubMaui.csproj
```

## Teenused

### DatabaseService

Vastutab SQLite andmebaasi loomise, tabelite loomise ja demoandmete lisamise eest.

### AuthService

Vastutab kasutaja autentimise, konto loomise, väljalogimise ja rolli kontrollimise eest.

### EventService

Vastutab ürituste, kategooriate ja registreeringute andmeloogika eest.

### PasswordHelper

Vastutab parooli hashimise eest.

## Kuidas käivitada

1. Ava projekt Visual Studios.
2. Ava lahendusfail `EventHubMaui.sln`.
3. Taasta NuGet paketid.
4. Vali käivitusplatvormiks `Windows Machine`.
5. Käivita projekt.
6. Esimesel käivitamisel luuakse lokaalne SQLite andmebaas ja demoandmed.

## Vajalikud paketid

Projekt kasutab NuGet paketti:

```text
sqlite-net-pcl
```

## Dokumentatsioon

Projektiga seotud nõuete dokumentatsioon on koostatud Wordi failina. Dokumentatsioon kirjeldab rolle, funktsionaalsust, andmebaasi, arhitektuuri, UML diagramme, vooskeeme, ProjectLibre ajaplaani ja viitamist.

Dokumentatsiooni tehniline kirjeldus vastab lõplikule rakendusele.

## ProjectLibre

ProjectLibre ajaplaan on koostatud XML-failina ja seda saab ProjectLibre programmis avada ning vajadusel salvestada `.pod` formaati.

Ajaplaan sisaldab järgmisi etappe:

- analüüs;
- disain;
- arendus;
- testimine;
- dokumentatsioon;
- esitluse ettevalmistus.
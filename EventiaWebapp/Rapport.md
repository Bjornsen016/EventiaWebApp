# Inlämning 2 ASP.NET - Eventia App

## Rapport Kim Björnsen Åklint <!-- omit in toc -->

- [Inloggningsuppgifter till seedade användare (För Björns enkelhet)](#inloggningsuppgifter-till-seedade-användare-för-björns-enkelhet)
- [Analys av programmet](#analys-av-programmet)
  - [Implementerade funktioner](#implementerade-funktioner)
  - [Behörighetshantering](#behörighetshantering)
  - [Struktur för vidarebyggnad](#struktur-för-vidarebyggnad)
- [Analys av mitt arbete](#analys-av-mitt-arbete)
- [Buggar jag vet om](#buggar-jag-vet-om)

## Inloggningsuppgifter till seedade användare (För Björns enkelhet)

| Role                   | Email             | Password       |
|------------------------|-------------------|----------------|
| Admin                  | admin@eventia.com | AdminP4ssword! |
| Organizer/Regular User | kims@email.com    | P4ssword!      |
| Regular User           | markus@email.com  | P4ssword!      |

## Analys av programmet

### Implementerade funktioner

- Registera ny användare
- In-/utloggning
- Roller (Admin, Organisatör, Användare)
- **Admin** kan göra följande:
  - Se en lista av alla Användare och Organisatörer
  - Bocka i om en användare ska vara Organisatör eller inte
- **Organisatör** kan gör följande:
  - Se en lista över alla sina events
  - Sortera den listan på datum eller antalet användare som signat upp
  - Se detaljer över vilka användare som har signat upp för varje event
  - Lägga till nya event
  - Editera sina events
- **Användare** kan göra följande:
  - Se en lista över de events som de har signat upp för
  - Registrera sig på ett nytt event
  - Avregistera sig på event som de signat upp för
- Alla användare kan se en lista över alla events (även ej inloggade)
- Alla **inloggade** användare kan se lite av sin kontoinformation och kan även ändra sitt lösenord

<div style="page-break-after: always;"></div>

### Behörighetshantering

Jag valde att inte sätta en "global" Authorization på min app, då jag ändå skulle sätta olika rollbehörigheter på nästan alla sidor. Detta kan såklart göra att man kanske missar att lägga till `[Authorize]` på controllers, razorpages eller metoder. Kan vara en svaghet, men är man noga vid kodning och codereviews så bör det inte vara ett problem.<br>
Jag la mest tid på att jobba fram en välfungerande backend och struktur, så frontenden är något jag skulle jobba mer på om jag fortsätter utvecklingen av den här hemsidan. Det är något jag känner att jag vill arbeta på mer för att bygga en bättre förståelse och struktur.

### Struktur för vidarebyggnad

Vid fortsatt utveckling av den här appen så är min uppfattning att den är strukturerad på ett sätt som gör det ganska enkelt att utöka funktionalitet. Jag har strukturerat upp appen på att sätt som jag tycker gör det lättöverskådligt och tror att det skulle vara ganska lätt att vidareutveckla appen även i ett team. <br>
Om jag skulle fortsätta skulle jag börja med att sätta mig ner och göra lite refactor, se om jag kan bryta ut lite kod till egna metoder/klasser/filer för lättare hantering.

## Analys av mitt arbete

Det var lite knepigt att går från min egenbyggda Auth till Identity men när jag väl kom in i flödet så funkade det galant. Blev klar ganska tidigt med grunden och la sedan mest tid på att hjälpa de andra förstå hur de kunde utvecklas. Där kunde jag kanske lagt mer tid på att vidareutveckla min app, men jag känner att jag får mer ut av att hjälpa andra med förståelse. Jag lär mig mer så att säga. <br>
Något jag kan jobba med själv är att arbeta mer strukturerat, tror det kan hjälpa mig i framtida arbete när jag jobbar i ett team. T.ex. att planera appen lite mer grundligt innan jag börjar koda för mycket. Och jag har tänkt att försöka jobba lite mer TDD snart, men är lite svårt när jag arbetar med helt nya grejer.

## Buggar jag vet om

- Tid visas fel när man lagt till en ny event, pga min UTC time-offset implementering. Ganska lätt att fixa, men upptäckt för sent

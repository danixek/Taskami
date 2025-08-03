# 📝 Taskami

**Taskami** je ukázkový projekt inspirovaný službou [Todoist](https://todoist.com) (a tak trochu i Redminem), sloužící jako technická demonstrace schopností práce s OpenAPI specifikací, architekturou klient–server v .NET a tvorbou multiplatformních UI.

---

## ⚙️ Technologie

Projekt je rozdělen do více částí:

| Projekt | Popis |
|--------|-------|
| `Taskami.API` | ASP.NET Core backend vygenerovaný z OpenAPI specifikace (Todoist) |
| `Taskami.Client` | Automaticky generovaný C# klient pro přístup k API (SDK) |
| `Taskami.WebUI` | Webová aplikace (např. Razor Pages / Blazor / jiné) pro práci s API |
| `Taskami.WPFUI` | Desktopová aplikace ve WPF jako alternativní frontend |

Mobilní verze (Flutter/Kotlin) je možnou budoucí rozšířenou alternativou, ale prozatím není v plánu.
Též zvažuji přepsat [Todoist Widget](https://github.com/danixek/todoist-widget) z Pythonu do C# nejen pro jednotnost jazyka, ale také pro rozšíření kompatibility s Taskami.

---

## 🎯 Cíle projektu

- ✅ Demonstrace práce s OpenAPI generátorem (server + client)
- ✅ Napojení různých UI (web, desktop) na společný backend
- ✅ Ukázka architektury víceprojektového řešení v .NET
- ✅ Přehledný a přenositelný kód vhodný pro nábor

---

## 🛡 Licence

- **Zdrojový kód** je poskytován pod licencí [MIT](./LICENSE-MIT.txt).

Tento projekt je zveřejněn jako **ukázka práce pro náborové účely**.  
Můžete jej volně prohlížet, analyzovat a inspirovat se.  
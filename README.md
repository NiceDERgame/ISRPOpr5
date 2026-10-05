# StudyPlanner — менеджер учебных дедлайнов (WPF, .NET 8, MVVM)

Сквозной учебный проект: учёт заданий по дисциплинам, приоритеты,
просроченные/ближайшие дедлайны, JSON-хранение, xUnit-тесты.

## Структура
- `src/StudyPlanner.App` — запуск WPF, composition root;
- `src/StudyPlanner.Presentation` — MainWindow, TaskEditWindow;
- `src/StudyPlanner.Application` — MainViewModel, TaskEditViewModel, IStorageService;
- `src/StudyPlanner.Domain` — Course, AssignmentItem, Priority, StudyData;
- `src/StudyPlanner.Deadlines` — DeadlineService;
- `src/StudyPlanner.Infrastructure.Json` — JsonStorageService;
- `tests/StudyPlanner.Tests` — xUnit;
- `docs/` — отчёты; `resources/` — ресурсы.

## Быстрый старт
```powershell
dotnet build StudyPlanner.Modular.sln
dotnet test tests/StudyPlanner.Tests/StudyPlanner.Tests.csproj
```

## Правила работы (ЛР №5)
1. Ветка `main` — стабильная; работа — в `feature/*`.
2. Один commit — одно изменение; `Add: ...`, `Fix: ...`, `Docs: ...`.
3. Перед commit: `git status`, после: `git log --oneline -5`.
4. Не коммитить `bin/`, `obj/`, `.vs/`, `TestResults/`, секреты.
5. Обмен по правилам ЛР №3: View→ViewModel через Binding/ICommand,
   Application→Domain типизированными объектами,
   сроки только через DeadlineService, хранение только через IStorageService (JSON).

# ThesisSchedulerGUI

### A Data-Driven Optimization Framework for Academic Course Scheduling and Graduation Planning

A desktop decision-support system that helps university students build personalized, conflict-free course schedules and plan their remaining courses toward graduation. Developed as a master's thesis project at **Yuan Ze University**, Department of Industrial Engineering and Management (2026).

## Project Overview

Academic planning requires balancing graduation credit requirements, prerequisites, course availability, timetable conflicts, and individual preferences. ThesisSchedulerGUI brings this information together in a **C# Windows Forms application** and uses **Python-based Mixed Integer Linear Programming (MILP)** with the **Gurobi optimizer** to produce recommendations.

The framework connects two optimization models:

1. **Current Semester Course Recommendation:** Selects a feasible set of courses for the current semester, considering academic regulations, credit limits, timetable conflicts, and student preferences.
2. **Future Graduation Planning:** Uses completed courses and current-semester recommendations to plan remaining courses across future semesters, with the goal of minimizing the remaining semesters required for graduation while satisfying academic requirements.

## Key Features

- Student profile input and validation (student ID, department, academic level)
- Graduation requirement import, parsing, review, and validation
- Completed-course record processing to identify fulfilled and outstanding requirements
- Department timetable loading and course availability processing
- Preferences for credit load, class days, time slots, and course or instructor choices
- Conflict-free current-semester timetable recommendations
- Semester-by-semester future graduation roadmap
- Graduation progress and credit distribution summaries
- JSON-based communication between the desktop interface and optimization engine

## Technology Stack

| Component | Technology |
| --- | --- |
| Desktop application | C# / Windows Forms (.NET Framework 4.8) |
| Optimization engine | Python |
| Optimization method | Mixed Integer Linear Programming (MILP) |
| Solver | Gurobi |
| Data exchange | JSON |
| Development environment | Visual Studio |

## System Workflow

```text
Student information + graduation requirements
                 +
Completed courses + semester timetable + preferences
                 |
                 v
       C# WinForms data processing
                 |
                 v
          Structured JSON input
                 |
                 v
    Python / Gurobi MILP Model 1
     (Current Semester Schedule)
                 |
                 v
    Python / Gurobi MILP Model 2
      (Future Graduation Plan)
                 |
                 v
         JSON optimization output
                 |
                 v
       C# WinForms visualization
```

## Project Structure

```text
ThesisSchedulerGUI/
├── Form1.cs ... Form4.cs          # Windows Forms workflow
├── Form*.Designer.cs             # Generated UI definitions
├── Form*.resx                    # Form resources
├── Program.cs                    # Application entry point
├── schedule_solver.py            # Current-semester optimization
├── future_graduation_solver.py   # Future graduation planning
├── FuturePlanningModels.cs       # Future planning data models
├── FuturePlanningSolverService.cs
├── SchedulerJsonModels.cs       # JSON data models
├── Properties/                  # Project resources and settings
├── Department Timetable/        # Local timetable data (if included)
├── ThesisSchedulerGUI.csproj
├── ThesisSchedulerGUI.sln
├── packages.config
└── App.config
```

> The tree summarizes major files from the project. Other helper classes and resource files may also be present.

## Requirements and Setup

1. Use **Windows** with **Visual Studio** and the **.NET Framework 4.8** development tools.
2. Clone or download this repository, then open `ThesisSchedulerGUI.sln` in Visual Studio.
3. Restore the project's **NuGet packages** when prompted.
4. Install a compatible **Python** environment, the **Gurobi Python package (`gurobipy`)**, and configure a valid Gurobi license.
5. Check any Python executable paths, input data paths, and configuration values expected by the application, and adjust them for your machine.
6. Build and run the WinForms application from Visual Studio.

> This is a research prototype. Exact runtime dependencies, paths, and sample-data availability may require local configuration; successful setup on a new machine has not been verified here.

## Research Context

**Thesis:** *A Data-Driven Optimization Framework for Academic Course Scheduling and Graduation Planning*  
**Researcher:** Phuong Dieu Thao  
**Advisor:** Dr. Yun-Chia Liang  
**Institution:** Yuan Ze University, Taiwan  
**Program:** M.S. in Industrial Engineering and Management  
**Year:** 2026

The thesis evaluates scenarios showing feasible current-semester schedules and personalized semester-by-semester graduation planning results.

## Notes

- Do not commit student records, passwords, API keys, private datasets, or machine-specific configuration values.
- Generated `bin/`, `obj/`, `.vs/`, and restored NuGet `packages/` directories are generally excluded from source control.
- Gurobi is a third-party optimizer and may require its own license for execution.

## Author

** Phuong Dieu Thao (Jerry) - 芳妙草 **  
Master's research project, Yuan Ze University

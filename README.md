# Student Records System

**Course:** CS0315 — Algorithms and Data Structures  
**Assessment:** Midterm Case Study (35%)  
**Professor:** Rrezart Prebreza  

## Authors

- Yiğit Ali Engin  
- Nihat Yaman Çelen  

## Overview

In-memory, console-based C# prototype for fictional student records.  
Main data structure: a from-scratch generic **Binary Search Tree** (`BinarySearchTree<T>`).

Supported operations:
- Add student  
- Search by StudentId  
- Update name / department / GPA (ID unchanged)  
- Delete student  
- List all students in ascending ID order (In-Order traversal)  

Duplicate StudentIds are rejected. Invalid input is validated before it reaches the tree.

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

Check installation:

```bash
dotnet --version
```

## Build and Run

1. Open a terminal in the project folder.
2. Restore and build:

```bash
dotnet build
```

3. Run:

```bash
dotnet run
```

Or double-click `run.cmd` on Windows.

## Menu

| Option | Action |
|--------|--------|
| 1 | Load sample fictional demo data |
| 2 | Add student |
| 3 | Search by ID |
| 4 | Update student |
| 5 | Delete student |
| 6 | List all students (In-Order) |
| 7 | Run automated test suite |
| 0 | Exit |

## Project Structure

| File | Description |
|------|-------------|
| `Student.cs` | Student model, validation, `IComparable` |
| `Node.cs` | Generic BST node |
| `BinarySearchTree.cs` | Insert, Search, Update, Delete, In-Order |
| `StudentService.cs` | Business logic and input checks |
| `TestRunner.cs` | Automated tests (expected / actual / PASS-FAIL) |
| `Program.cs` | Console menu |
| `run.cmd` | Windows helper to run the app |

## Sample Data

Demo data is fictional only (menu option **1**), for example:

- 1001 Can Öztürk — Physics  
- 1002 Mehmet Demir — Electrical Engineering  
- 1005 Ayşe Yılmaz — Computer Science  

## Testing

Menu option **7** runs the test suite and prints for each case:

- Test name  
- Expected result  
- Actual result  
- PASS / FAIL  

Use this console output as execution evidence in the report.

## Structural Rule (Invariant)

- Left subtree keys &lt; node key &lt; right subtree keys  
- Duplicate StudentId is not allowed  
- Update never changes StudentId, so tree links stay valid  

## Notes for Submission

- Full complexity comparison, diagrams, and operation traces belong in the **group report**.  
- This README covers build/run instructions required for the C# project submission.  
- Member contribution statement and AI-use statement should be included in the group report package.

# GurfeinD.DataProcessor.CLI
A modular .NET 8 command-line Data Processor that supports pluggable use-case
---

## Table of Contents
- [Description](#description)
- [Architecture](#architecture)
- [Use Cases](#use-cases)
- [Installation](#installation)
- [Usage](#usage)

---
## Description
- The Data Processor is generic command line tool that receives a use case name and input, it processes the input as per the use case and prints the result.
---
## Architecture
- The solution follows SOLID and Clean Architecture principles.
---
## Use Cases

### 1. MissingNumberArray
- **Name:** MissingNumberArray (default)
- **Description:** Given an array containing numbers taken from the range 0 to n, find the one number that is missing from the array.
- **Input:** An array of integers number where nums contains n distinct numbers from the range 0 to n.
- **Output:** Return the missing number.
- **Examples:**

```
Input: [3, 0, 1]

Output: 2

Input: [9, 6, 4, 2, 3, 5, 7, 0, 1]

Output: 8
```
---
## Installation

1. **Download the ZIP**  
   - From the GitHub Release page: `DataProcessor.zip`

2. **Extract the ZIP**  
   - Extract to any folder on your system.

3. **Run the program**  
   - Open a terminal/command prompt in the extracted folder and run:

   ```bash
   DataProcessor.exe
---
## Usage

DataProcessor [-u <UseCaseName>] <input>
- `-u <UseCaseKey>` → (optional) the use case to run; if omitted, the default use case is executed.  
- `<input>` → the raw input for the use case (comma-separated for arrays, etc.).



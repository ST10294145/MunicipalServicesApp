# Municipal Services App

## Table of Contents

1. [Project Overview](#project-overview)
2. [Features](#features)
3. [Technology Stack](#technology-stack)
4. [Installation & Setup](#installation--setup)
5. [Usage](#usage)
6. [Admin Functionality](#admin-functionality)
7. [Data Management](#data-management)

---

## Project Overview

The **Municipal Services App** is a WPF desktop application built in C# .NET, designed to streamline municipal issue reporting and management. Citizens can submit reports regarding municipal issues, while administrators can review these reports and provide feedback.

This app emphasizes simplicity, usability, and quick feedback loops without requiring permanent storage or complex databases.

---

## Features

**User Side**:

- Submit issues with the following details:
  - Title & Description
  - Reporter Name & Email
  - Province, City, and Street Address
  - Category of Issue
  - Attach files/images for reference
- View feedback from admin once provided.

**Admin Side**:

- Login via seeded credentials (`Username: "admin"/Password: "admin123"`)
- View all submitted reports in a tabular format
- Add feedback to individual issues
- Logout and return to main app without closing the application

**General Features**:

- Responsive DataGrid for report viewing, auto-resizes with window
- Open attached files directly from the report management window
- Simple UI design using WPF with clear, color-coded buttons

---

## Technology Stack

- **Language**: C#
- **Framework**: .NET Framework (WPF)
- **IDE**: Visual Studio 2022
- **Data Handling**: In-memory linked list (no permanent database)
- **File Handling**: OpenFileDialog for attachments

---

## Installation & Setup

1. Clone the repository:

   ```bash
   git clone <https://github.com/ST10294145/MunicipalServicesApp.git>
   ```

2. Open the solution in **Visual Studio**.
3. Ensure the project targets **.NET Framework 4.7.2** or higher.
4. Build the solution.
5. Run the application.

---

## Usage

### User Workflow

1. Launch the app → Main Window opens.
2. Click **Report Issues**.
3. Fill in all required fields: Title, Description, Reporter, Location (Province, City, Street), Category.
4. Optionally attach a file/image.
5. Click **Submit** → The report is added to the system.

### Admin Workflow

1. Click **Admin Login** in the main window (bottom-right corner).
2. Enter credentials:
    - Username: `admin`
    - Password: `admin123`
3. View submitted reports in the **Report Management** window.
4. Double-click a report to open its attached file or provide feedback.
5. Click **Logout** to return to the main window.

---

## Admin Functionality

- Admin can view all reports in a **DataGrid** with columns:
    - Issue ID, Title, Category, Province, City, Street Address, Status, Reported On, SLA Deadline, Attachment, Feedback
- Feedback is tied directly to the issue and is visible to the reporting user immediately.
- Admin can open attachments by double-clicking the row in the DataGrid.

---

## Data Management

- **In-Memory Storage**: The app uses an `IssueLinkedList` to store all issues during runtime.
- **No database required**: All data is cleared when the application closes.
- **Seeded admin login**: Admin credentials are hardcoded for simplicity.
  
---

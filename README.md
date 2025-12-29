
<h1 align="center">📚 GaBasa: Library Management System</h1>

<p align="center">
   <img src="LMS_GaBasa/LMS.Presentation/Assets/logos/gabasa-logo-zoom.png" width="400" /> 
   <br/>
   *Built with care, so every book finds a reader — and every reader finds a story.*
</p>

A Windows Desktop Application built with **C# .NET Framework WinForms** for managing library operations including cataloging, circulation, member management, and reporting. 

Take a look at our **Wiki** here on github to understand more about GaBasa: The Library Management System.

This project follows a **3-layer architecture**:

- 🖥️ **LMS.Presentation** → WinForms UI  
- ⚙️ **LMS.BusinessLogic** → Business Logic Layer (Managers)  
- 💾 **LMS.DataAccess** → Data Access Layer (Repositories, DB connection)  

---

## ✨ Features

- 👥 User Management (Librarian / Admin, Library Staff, Member)  
- 📖 Book Cataloging & Inventory  
- 🔄 Borrowing, Returns, Renewals  
- 💰 Fines and Penalty Management  
- 🔍 Advanced Search & Discovery  
- 📊 Reports & Analytics Dashboard  

---

## 💻 Prerequisites

- 🪟 Windows 10 or later  
- 🛠️ **Visual Studio 2022** (Community)  
- ⚡ **.NET Framework 4.8**  
- 🗄️ **SQL Server** for database

---

## 🤝 Contributors
Thanks to everyone who contributed to GaBasa!

| Name        | Role             | GitHub                                         |
| ----------- | ---------------- | ---------------------------------------------- |
| Ken Madayag | Lead Developer   | [@SixxCodes](https://github.com/SixxCodes)     |
| Merry Guisihan | Database Manager | [@Mauitypings](https://github.com/Mauitypings) |
| Vien Ugay | UI Design | [@viensed](https://github.com/viensed) |
| Jerard Lavilla | UI Design | [@lavillajerard](https://github.com/lavillajerard) |

---

## 🔗 Links
- 📄 <a href="https://docs.google.com/document/d/19aFYf08i4C4z6e4DFtAghInzjfmnRpLYf-IXhrbw5I0/edit?tab=t.0">Documentation</a>
- 🎨 <a href="https://www.figma.com/design/OdcDOrOT7lrfCN9eDQYJZP/IT13---LMS?node-id=0-1&p=f&t=Hq0fi7MlIlmH2Gtk-0">Prototype (Lo-Fi)</a>
- 🗂️ <a href="https://drive.google.com/drive/folders/1gb0dMvQgkr17ntrtM6xtuifRdsj52C16?usp=drive_link">Assets</a>

---

## 🚀 Getting Started

### 1️⃣ Clone the repository
```bash
D: (if you want to put it on drive D, ignore if on drive C)
cd (where you want to place project folder, ex: D:Programs/LMS_Gabasa)
git clone https://github.com/SixxCodes/GaBasa.git
```

### 2️⃣ Open in Visual Studio
- Open LMS_GaBasa.sln in Visual Studio 2022 or on File Explorer.
- Ensure all three projects **(Presentation, BusinessLogic, DataAccess)** are loaded.

### 3️⃣ Set Startup Project
- Right-click LMS.Presentation → Set as Startup Project.

### 4️⃣ Restore NuGet Packages / Install Dependencies
This project uses the following NuGet packages:

- 🖌️ **ReaLTaiizor** (for modern UI components)
   - [ReaLTaiizor GitHub](https://github.com/roy-t/ReaLTaiizor) – check this for tutorials, usage examples, and themes. (Make sure you're installing the correct package! Look at the package title, description, and authors to make sure.)
- 🔒 **BCrypt.Net-Next** (for password hashing)

**Note**:
- Visual Studio should restore them automatically when you build the solution.  
- You can also install them manually via NuGet Package Manager.
- Additional packages will automatically be installed as dependencies required by **BCrypt.Net-Next**.

To get a clearer view of all dependencies and their relationships:

1. In Visual Studio, go to **Architecture → Generate Dependency Graph → For Solution**.
2. Explore the diagram to see how projects and packages relate.

Or go to **Insights** of this repository and click the **Dependency Graph** tab to view all packages installed in this project.

### 5️⃣ Build and Run
- Press F5 to build and run the application.

--- 

## ✨ Thank you for exploring GaBasa! ✨
Developed with best practices, collaboration, and a passion for building meaningful systems.

# Database Setup

Quyidagi qadamlarni bajarish orqali ma'lumotlar bazasini sozlang:

## 1. Connection String sozlash

`appsettings.json` faylida quyidagi kodni qo'shing:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=QuizAppDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

## 2. Migration yaratish

Package Manager Console yoki terminalda quyidagi buyruqni bajaring:

```bash
add-migration Database
```

## 3. Ma'lumotlar bazasini yangilash

Migration yaratilgandan so'ng, quyidagi buyruq bilan ma'lumotlar bazasini yangilang:

```bash
update-database
```

---

**Eslatma:** Ushbu qadamlarni bajarishdan oldin SQL Server LocalDB o'rnatilganligiga ishonch hosil qiling.

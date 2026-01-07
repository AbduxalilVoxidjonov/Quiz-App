1. appsettings.json Added this code: 
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=QuizAppDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  },
}
2. add-migration Database
3. update-database
   

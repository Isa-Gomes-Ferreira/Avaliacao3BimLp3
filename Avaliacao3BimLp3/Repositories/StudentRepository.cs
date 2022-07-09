
using LabManager.Database;
using LabManager.Models;
using Microsoft.Data.Sqlite;
using Dapper;

namespace LabManager.Repositories;

class StudentRepository
{
    private DatabaseConfig databaseConfig;

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public StudentRepository(DatabaseConfig databaseConfig)
    {
        this.databaseConfig = databaseConfig;
    }

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public Student Save(Student student)
    {
        using var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();

        connection.Execute("INSERT INTO Students VALUES(@Registration, @Name, @City, @Former)",
        student);

        return student;
    }

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public void Delete(string registration)
    {
        using var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();

        connection.Execute("DELETE FROM Students WHERE registration = @Registration;", new {Registration = registration});
    }

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public void MarkAsFormed(string registration)
    {
        using var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();

        connection.Execute("UPDATE Students SET former = 1 WHERE registration = @Registration;",
        new {Registration = registration});
    }

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public IEnumerable<Student> GetAll()
    {
        using var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();

        var students = connection.Query<Student>("SELECT * FROM Students");

        return students;
    }

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public IEnumerable<Student> GetAllStudentByCity(string city)
    {
        using var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();
        
        var students = connection.Query<Student>
        ("SELECT * FROM Students WHERE city LIKE @City", new {City = "%" + city + "%"});

        return students;
    }

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public IEnumerable<CountStudentGroupByAttribute> CountByCities()
    {
        using var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();
        
        var qntByCities = connection.Query<CountStudentGroupByAttribute>
        ("SELECT * FROM Students WHERE city LIKE @City","SELECT COUNT(Registration), City FROM Students GROUP BY City ORDER BY COUNT(Registration) DESC;");

        return qntByCities;
    }

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public bool StudentExists(string registration)
    {
        using var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();

        var result = Convert.ToBoolean(connection.ExecuteScalar(
            "SELECT count(registration) FROM Students WHERE registration = $Registration;", new {Registration = registration}));

        return result;
    }
}
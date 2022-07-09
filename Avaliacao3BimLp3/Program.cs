
using LabManager.Database;
using LabManager.Models;
using LabManager.Repositories;
using Microsoft.Data.Sqlite;

var databaseConfig = new DatabaseConfig();
var databaseSetup = new DatabaseSetup(databaseConfig);

var studentRepository = new StudentRepository(databaseConfig);

// Routing
var modelName = args[0];
var modelAction = args[1];

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

if (modelName == "Student")
{

    if (modelAction == "New")
    {
        string registration = args[2];
        string name = args[3];
        string city = args[4];

        if (studentRepository.StudentExists(registration) == true )
        {
            Console.WriteLine($"Estudante com Id {registration} já existe!");
        }

        else
        {
            var student = new Student(registration, name, city, false);
            studentRepository.Save(student);

            Console.WriteLine($"Estudante {name} cadastrado com sucesso!");
        }
    }

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    if (modelAction == "Delete")
    {
        string registration = args[2];

        if (studentRepository.StudentExists(registration) == false )
        {
            Console.WriteLine($"Estudante {registration} não encontrado!");
        } 
        else
        {
            studentRepository.Delete(registration);
            Console.WriteLine($"Estudante {registration} removido com sucesso!");
        }
    }

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    if (modelAction == "MarkAsFormed")
    {
        string registration = args[2];

        if (studentRepository.StudentExists(registration) == false)
        {
            Console.WriteLine($"Estudante {registration} não encontrado!");
        }
        else
        {
            studentRepository.MarkAsFormed(registration);
            Console.WriteLine($"Estudante {registration} definido como formado!");
        }
    }

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    if (modelAction == "ListByCity")
    {
        string city = args[2];

        foreach (var student in studentRepository.GetAllStudentByCity(city))
        {
            string situation = (student.Former)
                   ? "formado" : "não formado";
            Console.WriteLine("{0}, {1}, {2}, {3}", student.Registration, student.Name, student.City, situation);
        }
    }
    else
    {
        Console.WriteLine("Nenhum estudante cadastrado!");
    }

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    
   /* if (modelAction == "CountByCities")
    {
   
    } */

}
